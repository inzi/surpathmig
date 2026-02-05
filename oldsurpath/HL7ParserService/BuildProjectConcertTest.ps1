# PowerShell script to build Project Concert Test utility
Write-Host "Building Project Concert Test Utility..." -ForegroundColor Green

# First build the entire solution to ensure all dependencies are available
Write-Host "Building Surpath solution to ensure all dependencies..." -ForegroundColor Yellow
$solutionPath = "..\Surpath-All.sln"
if (Test-Path $solutionPath) {
    & msbuild $solutionPath /p:Configuration=Debug /p:Platform="Any CPU" /v:minimal /m
} else {
    Write-Host "Solution not found, trying to build just HL7ParserService..." -ForegroundColor Yellow
    & msbuild HL7ParserService.csproj /p:Configuration=Debug /p:Platform=AnyCPU /v:minimal
}

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build solution/project!" -ForegroundColor Red
    Write-Host "Try building the solution in Visual Studio first." -ForegroundColor Yellow
    exit 1
}

# Find C# compiler
$cscPaths = @(
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\Roslyn\csc.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\Roslyn\csc.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\Roslyn\csc.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\Roslyn\csc.exe",
    "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\csc.exe",
    "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
)

$csc = $null
foreach ($path in $cscPaths) {
    if (Test-Path $path) {
        $csc = $path
        break
    }
}

if ($null -eq $csc) {
    Write-Host "Could not find C# compiler!" -ForegroundColor Red
    exit 1
}

Write-Host "Using compiler: $csc" -ForegroundColor Gray

# Compile the test utility
Write-Host "Compiling ProjectConcertTest.exe..." -ForegroundColor Yellow

$references = @(
    "/reference:bin\Debug\HL7ParserService.exe",
    "/reference:bin\Debug\MySql.Data.dll",
    "/reference:bin\Debug\Serilog.dll",
    "/reference:bin\Debug\Serilog.Sinks.Console.dll",
    "/reference:bin\Debug\SurPath.Backend.dll",
    "/reference:bin\Debug\SurPath.Business.dll",
    "/reference:bin\Debug\SurPath.Data.dll",
    "/reference:bin\Debug\SurPath.Entity.dll",
    "/reference:bin\Debug\SurPath.Enum.dll",
    "/reference:bin\Debug\SurPath.MySQLHelper.dll",
    "/reference:bin\Debug\RTFLib.dll",
    "/reference:System.Configuration.dll",
    "/reference:System.Data.dll"
)

& $csc /out:bin\Debug\ProjectConcertTest.exe /target:exe /platform:anycpu $references ProjectConcertTestProgram.cs

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nBuild successful!" -ForegroundColor Green
    Write-Host "Executable created at: bin\Debug\ProjectConcertTest.exe" -ForegroundColor Cyan
    Write-Host "`nThe utility will use the same App.config as HL7ParserService" -ForegroundColor Yellow
    
    # Copy the config file
    if (Test-Path "bin\Debug\HL7ParserService.exe.config") {
        Copy-Item "bin\Debug\HL7ParserService.exe.config" "bin\Debug\ProjectConcertTest.exe.config" -Force
        Write-Host "Config file copied to: bin\Debug\ProjectConcertTest.exe.config" -ForegroundColor Gray
    }
} else {
    Write-Host "`nBuild failed!" -ForegroundColor Red
}

Write-Host "`nPress any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")