# PowerShell script to copy App.config from HL7ParserService
Write-Host "Copying App.config from HL7ParserService..." -ForegroundColor Green

$source = "..\HL7ParserService\App.config"
$destination = "App.config"

if (Test-Path $source) {
    # Backup current config
    if (Test-Path $destination) {
        Copy-Item $destination "$destination.backup" -Force
        Write-Host "Backed up current App.config to App.config.backup" -ForegroundColor Yellow
    }
    
    # Copy the config
    Copy-Item $source $destination -Force
    Write-Host "Copied App.config from HL7ParserService" -ForegroundColor Green
    
    # Update connection string if needed
    Write-Host "`nIMPORTANT: Review the App.config and update:" -ForegroundColor Yellow
    Write-Host "1. ConnectionString - ensure it points to your test database" -ForegroundColor Yellow
    Write-Host "2. Production - set to 'false' for testing or 'true' for production" -ForegroundColor Yellow
    Write-Host "3. File paths - ensure they exist and are accessible" -ForegroundColor Yellow
} else {
    Write-Host "ERROR: Could not find HL7ParserService\App.config" -ForegroundColor Red
    Write-Host "Make sure you're running this from the ProjectConcertTestCLI directory" -ForegroundColor Red
}

Write-Host "`nPress any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")