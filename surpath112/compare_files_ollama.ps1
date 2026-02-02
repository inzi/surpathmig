# PowerShell script to compare files and generate analysis using Ollama with DeepSeek R1

# Configuration
$ollamaModel = "deepseek-r1:7b" # DeepSeek R1 model
$ollamaApiUrl = "http://localhost:11434/api/generate" # Ollama API endpoint
$maxFileSize = 1000000 # 1MB limit for processing
$maxContentLength = 100000 # Leverage 128k context window
$timeoutSec = 300 # Timeout for API calls
$retryCount = 3 # Number of retries for API failures
$retryDelayBase = 1000 # Base delay for retries (ms)

# Function to move the last file from workfilelist.md to donefilelist.md
function Move-ToDone {
    $workfileContent = Get-Content -Path "workfilelist.md" -ErrorAction Stop
    if ($workfileContent) {
        $lastFile = $workfileContent | Select-Object -Last 1
        Add-Content -Path "donefilelist.md" -Value $lastFile -ErrorAction Stop
        $workfileContent[0..($workfileContent.Length - 2)] | Set-Content -Path "workfilelist.md" -ErrorAction Stop
    }
}

# Function to sanitize content (replace triple backticks and special characters)
function Sanitize-Content {
    param ([string]$content)
    $content = $content -replace '```', '[CODE_BLOCK]'
    $content = $content -replace '[\x00-\x1F\x7F]', ''
    $content = $content -replace '"', '\"'
    $content = $content -replace '\r\n', '\n'
    return $content
}

# Function to truncate XML content at tag boundaries
function Truncate-XmlContent {
    param (
        [string]$content,
        [int]$maxLength
    )
    if ($content.Length -le $maxLength) {
        return $content
    }
    $truncated = $content.Substring(0, $maxLength)
    $lastTagEnd = $truncated.LastIndexOf('>')
    if ($lastTagEnd -ge 0) {
        return $truncated.Substring(0, $lastTagEnd + 1) + "... [truncated]"
    }
    return $truncated + "... [truncated]"
}

# Function to extract JSON from response with <think> stream
function Extract-JsonFromResponse {
    param ([string]$content)
    # Match JSON between ```json and ```
    $regex = '```json\n(.*?)\n```'
    $match = [regex]::Match($content, $regex, [System.Text.RegularExpressions.RegexOptions]::Singleline)
    if ($match.Success) {
        return $match.Groups[1].Value
    }
    return $null
}

# Function to call Ollama API for analysis
function Invoke-OllamaAnalysis {
    param (
        [string]$modifiedContent,
        [string]$unmodifiedContent,
        [string]$status,
        [string]$language
    )

    Write-Output "Calling Ollama API at: $ollamaApiUrl with model: $ollamaModel for $relativePath"

    # Sanitize content
    $modifiedContent = Sanitize-Content -content $modifiedContent
    $unmodifiedContent = Sanitize-Content -content $unmodifiedContent

    # Truncate content, using XML-aware truncation for .csproj files
    if ($language -eq "XML") {
        $modifiedContent = Truncate-XmlContent -content $modifiedContent -maxLength $maxContentLength
        $unmodifiedContent = Truncate-XmlContent -content $unmodifiedContent -maxLength $maxContentLength
    } else {
        if ($modifiedContent.Length -gt $maxContentLength) {
            $modifiedContent = $modifiedContent.Substring(0, $maxContentLength) + "... [truncated]"
        }
        if ($unmodifiedContent.Length -gt $maxContentLength) {
            $unmodifiedContent = $unmodifiedContent.Substring(0, $maxContentLength) + "... [truncated]"
        }
    }

    # Log file content
    Add-Content -Path "content.log" -Value "File: $relativePath`nModified Content Length: $($modifiedContent.Length)`nModified Content:`n$modifiedContent`nUnmodified Content Length: $($unmodifiedContent.Length)`nUnmodified Content:`n$unmodifiedContent`n"

    $prompt = if ($status -eq "Modified") {
        @"
You are an expert code analyst. Analyze the provided modified and unmodified file contents and generate a valid JSON response with the following fields:

- **summary**: A concise description of the modified file's content (e.g., key classes, methods, or configurations) without code snippets. Focus on functionality and structure.
- **changes**: A precise summary of differences between the modified and unmodified content (e.g., added/removed lines, new methods, configuration changes).
- **purpose**: The file's role in an ASP.NET Zero solution (e.g., dependency injection, project configuration).

Return only the JSON object, without any thinking process, markdown, or backticks. Ensure the response is valid JSON, escaping special characters like quotes or newlines.

**Modified Content**:
$modifiedContent

**Unmodified Content**:
$unmodifiedContent

**Language**: $language

Respond in this JSON format:
{
  "summary": "...",
  "changes": "...",
  "purpose": "..."
}
"@
    } else {
        @"
You are an expert code analyst. Analyze the provided file content and generate a valid JSON response with the following fields:

- **summary**: A concise description of the file's content (e.g., key classes, methods, or configurations) without code snippets. Focus on functionality and structure.
- **changes**: Use "New file".
- **purpose**: The file's role in an ASP.NET Zero solution (e.g., dependency injection, project configuration).

Return only the JSON object, without any thinking process, markdown, or backticks. Ensure the response is valid JSON, escaping special characters like quotes or newlines.

**Content**:
$modifiedContent

**Language**: $language

Respond in this JSON format:
{
  "summary": "...",
  "changes": "New file",
  "purpose": "..."
}
"@
    }

    $body = @{
        model = $ollamaModel
        prompt = $prompt
        stream = $false
    } | ConvertTo-Json -Depth 10

    # Log request details
    Add-Content -Path "ollama_requests.log" -Value "Request for $relativePath : Model=$ollamaModel, PromptLength=$($prompt.Length), Language=$language, Body=$body"

    $attempt = 0
    while ($attempt -lt $retryCount) {
        try {
            $response = Invoke-WebRequest -Uri $ollamaApiUrl -Method Post -Body $body -ContentType "application/json" -TimeoutSec $timeoutSec -ErrorAction Stop
            $responseContent = $response.Content
            $headers = $response.Headers | Out-String
            Add-Content -Path "ollama_responses.log" -Value "Attempt $attempt for $relativePath : Status=$($response.StatusCode), Headers=$headers, Content=$responseContent"

            # Extract JSON from response
            $jsonContent = Extract-JsonFromResponse -content $responseContent
            if (-not $jsonContent) {
                throw "Failed to extract JSON from response: $responseContent"
            }

            $analysis = ConvertFrom-Json $jsonContent -ErrorAction Stop
            if (-not $analysis.summary -or -not $analysis.changes -or -not $analysis.purpose) {
                throw "Incomplete analysis response: $jsonContent"
            }
            return $analysis
        } catch {
            $attempt++
            $errorDetails = $_.Exception.Message
            $errorContent = "No response content"
            if ($_.Exception.Response) {
                $errorDetails += ", Status=$($_.Exception.Response.StatusCode)"
                try {
                    $errorContent = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream()).ReadToEnd()
                } catch {
                    $errorContent = "Failed to read error content: $_"
                }
            }
            Add-Content -Path "ollama_responses.log" -Value "Attempt $attempt error for $relativePath : $errorDetails, Content=$errorContent"
            Write-Output "Attempt $attempt failed for ${relativePath}: $errorDetails"
            if ($attempt -eq $retryCount) {
                Write-Error "Ollama analysis failed for ${relativePath} after $retryCount attempts: $errorDetails"
                Add-Content -Path "skipped_files.log" -Value "$relativePath : Analysis failed after $retryCount attempts - $errorDetails, Content=$errorContent"
                return $null
            }
            $delay = $retryDelayBase * [math]::Pow(2, $attempt - 1)
            Start-Sleep -Milliseconds $delay
        }
    }
}

# Main processing loop
while ($true) {
    # Ensure filelist.md exists
    if (-not (Test-Path "filelist.md")) {
        Write-Error "filelist.md not found!"
        exit 1
    }

    # Get the first line from filelist.md
    $firstLine = Get-Content -Path "filelist.md" -TotalCount 1

    # If filelist.md is empty, exit
    if (-not $firstLine) {
        Write-Output "Finished processing all files."
        exit 0
    }

    # Append the first line to workfilelist.md
    Add-Content -Path "workfilelist.md" -Value $firstLine -ErrorAction Stop

    # Remove the first line from filelist.md
    try {
        (Get-Content -Path "filelist.md" | Select-Object -Skip 1) | Set-Content -Path "filelist.md" -ErrorAction Stop
    } catch {
        Write-Error "Failed to remove first line from filelist.md: $_"
        exit 1
    }

    # Get the last line from workfilelist.md (the file to process)
    $relativePath = Get-Content -Path "workfilelist.md" | Select-Object -Last 1

    Write-Output "Processing file: $relativePath"

    # Validate the file
    $modifiedPath = ".\src\$relativePath"
    $unmodifiedPath = ".\aspnetzeromvc11.4\inzibackend\src\$relativePath"

    $modifiedExists = Test-Path $modifiedPath
    $unmodifiedExists = Test-Path $unmodifiedPath

    if (-not $modifiedExists) {
        Write-Output "File $relativePath not found in ./src, skipping."
        Add-Content -Path "skipped_files.log" -Value "$relativePath : Missing in src"
        continue
    }

    # Check file size
    $fileSize = (Get-Item $modifiedPath).Length
    if ($fileSize -gt $maxFileSize) {
        Write-Output "File $relativePath is too large ($fileSize bytes), skipping."
        Add-Content -Path "skipped_files.log" -Value "$relativePath : Too large ($fileSize bytes)"
        continue
    }

    # Compare files
    $status = ""
    $modifiedContent = if ($modifiedExists) { Get-Content -Path $modifiedPath -Raw } else { "" }
    $unmodifiedContent = if ($unmodifiedExists) { Get-Content -Path $unmodifiedPath -Raw } else { "" }

    if ($modifiedExists -and -not $unmodifiedExists) {
        $status = "New"
    } elseif ($modifiedExists -and $unmodifiedExists) {
        $comparison = Compare-Object $modifiedContent $unmodifiedContent
        if (-not $comparison) {
            Write-Output "File $relativePath is identical, skipping analysis."
            Move-ToDone
            continue
        } else {
            $status = "Modified"
        }
    }

    # Prepare metadata
    $filename = [System.IO.Path]::GetFileName($relativePath)
    $extension = [System.IO.Path]::GetExtension($relativePath).ToLower()
    $language = switch ($extension) {
        ".cs" { "C#" }
        ".csproj" { "XML" }
        ".js" { "JavaScript" }
        ".ts" { "TypeScript" }
        default { "Unknown" }
    }

    # Construct output path with src subfolder
    $outputDir = Join-Path "migration-plans" (Join-Path "src" ([System.IO.Path]::GetDirectoryName($relativePath)))
    $saveTo = Join-Path $outputDir "$filename.md"

    # Create output directory if it doesn't exist
    if (-not (Test-Path $outputDir)) {
        New-Item -Path $outputDir -ItemType Directory -Force -ErrorAction Stop
    }

    # Generate analysis with Ollama
    $analysis = Invoke-OllamaAnalysis -modifiedContent $modifiedContent -unmodifiedContent $unmodifiedContent -status $status -language $language

    if ($null -eq $analysis) {
        Write-Output "Analysis failed for $relativePath, leaving in workfilelist.md."
        continue
    }

    # Generate Markdown template
    $template = @"
# $status
## Filename
$filename
## Relative Path
$relativePath
## Language
$language
## Summary
$($analysis.summary)
## Changes
$($analysis.changes)
## Purpose
$($analysis.purpose)
"@

    # Save the analysis file
    try {
        Set-Content -Path $saveTo -Value $template -ErrorAction Stop
        Write-Output "Saved analysis to $saveTo"
        Move-ToDone
    } catch {
        Write-Error "Failed to save analysis for ${relativePath}: $_"
        Add-Content -Path "skipped_files.log" -Value "$relativePath : Save failed - $_"
        continue
    }
}