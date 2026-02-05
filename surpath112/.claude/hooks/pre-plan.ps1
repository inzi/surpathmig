# Pre-Plan Hook - Check Conventions
# This hook reminds Claude to check relevant conventions before planning

# Determine project root (script is in .claude/hooks, so go up 2 levels)
$scriptPath = $PSScriptRoot
$projectRoot = Split-Path (Split-Path $scriptPath -Parent) -Parent
$conventionsPath = Join-Path $projectRoot "conventions"

if (Test-Path $conventionsPath) {
    $conventionFiles = Get-ChildItem -Path $conventionsPath -Filter "*.md" -File

    if ($conventionFiles.Count -gt 0) {
        Write-Host "`n=== Available Conventions ===" -ForegroundColor Cyan
        Write-Host "Before planning, review relevant conventions in: $conventionsPath`n" -ForegroundColor Yellow

        foreach ($file in $conventionFiles) {
            Write-Host "  - $($file.Name)" -ForegroundColor Green
        }

        Write-Host "`nReminder: Follow ASP.NET Zero MVC + jQuery patterns from conventions/" -ForegroundColor Yellow
        Write-Host "========================================`n" -ForegroundColor Cyan
    }
}
