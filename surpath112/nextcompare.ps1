# PowerShell script to pop and compare files, outputting data for LLM analysis

param (
    [switch]$MoveLastToDone
)

# Function to move the last file from workfilelist.md to donefilelist.md
function Move-ToDone {
    $workfileContent = Get-Content -Path "workfilelist.md"
    if ($workfileContent) {
        $lastFile = $workfileContent | Select-Object -Last 1
        Add-Content -Path "donefilelist.md" -Value $lastFile
        $workfileContent[0..($workfileContent.Length - 2)] | Set-Content -Path "workfilelist.md"
    }
}

# If -MoveLastToDone flag is provided, execute Move-ToDone and exit
if ($MoveLastToDone) {
    Move-ToDone
    Write-Output "Moved last file from workfilelist.md to donefilelist.md"
    exit 0
}

# Ensure filelist.md exists
if (-not (Test-Path "filelist.md")) {
    Write-Error "filelist.md not found!"
    exit 1
}

# Get the first line from filelist.md
$firstLine = Get-Content -Path "filelist.md" -TotalCount 1

# If filelist.md is empty, exit
if (-not $firstLine) {
    Write-Output "finished"
    exit 0
}

# Append the first line to workfilelist.md
Add-Content -Path "workfilelist.md" -Value $firstLine

# Remove the first line from filelist.md
(Get-Content -Path "filelist.md" | Select-Object -Skip 1) | Set-Content -Path "filelist.md"

# Get the last line from workfilelist.md (the file to process)
$relativePath = Get-Content -Path "workfilelist.md" | Select-Object -Last 1

# Validate the file
$modifiedPath = ".\src\$relativePath"
$unmodifiedPath = ".\aspnetzeromvc11.4\inzibackend\src\$relativePath"

$modifiedExists = Test-Path $modifiedPath
$unmodifiedExists = Test-Path $unmodifiedPath

if (-not $modifiedExists) {
    Write-Output "File $relativePath not found in ./src, skipping."
    exit 0 # Leave in workfilelist.md
}

# Check file size
$fileSize = (Get-Item $modifiedPath).Length
if ($fileSize -gt 1000000) {
    Write-Output "File $relativePath is too large ($fileSize bytes), skipping."
    exit 0 # Leave in workfilelist.md
}

# Compare files
$status = ""
if ($modifiedExists -and -not $unmodifiedExists) {
    $status = "New"
} elseif ($modifiedExists -and $unmodifiedExists) {
    $unmodifiedContent = Get-Content -Path $unmodifiedPath -Raw
    $modifiedContent = Get-Content -Path $modifiedPath -Raw
    $comparison = Compare-Object $unmodifiedContent $modifiedContent
    if (-not $comparison) {
        Write-Output "identical"
        Move-ToDone
        continue # Process next file
    } else {
        $status = "Modified"
    }
}

# Prepare output for LLM
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
    New-Item -Path $outputDir -ItemType Directory -Force
}

# Generate partial template
$template = @"
# $status
## Filename
$filename
## Relative Path
$relativePath
## Language
$language
## Summary

## Changes

## Purpose
"@

# Prepare JSON output
$modifiedContent = if ($modifiedExists) { Get-Content -Path $modifiedPath -Raw } else { "" }
$unmodifiedContent = if ($unmodifiedExists) { Get-Content -Path $unmodifiedPath -Raw } else { "" }

$output = @{
    save_to = $saveTo
    template = $template
    modified_content = $modifiedContent
    unmodified_content = $unmodifiedContent
    relative_path = $relativePath
    status = $status
} | ConvertTo-Json -Compress

Write-Output $output
exit 0