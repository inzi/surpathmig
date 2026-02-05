@echo off
echo =============================================
echo Project Concert Test Utility
echo =============================================
echo.
echo IMPORTANT: You must first build the HL7ParserService project in Visual Studio!
echo.
echo If you haven't built it yet:
echo 1. Open Surpath-All.sln in Visual Studio
echo 2. Build the solution (Build menu - Build Solution)
echo 3. Then run this script again
echo.
pause

cd bin\Debug

if not exist HL7ParserService.exe (
    echo ERROR: HL7ParserService.exe not found in bin\Debug
    echo Please build the project in Visual Studio first!
    pause
    exit /b 1
)

if not exist SurPath.Backend.dll (
    echo ERROR: Required dependencies not found in bin\Debug
    echo Please build the entire solution in Visual Studio first!
    pause
    exit /b 1
)

echo.
echo To run the test utility, use one of these commands:
echo.
echo Test a specific donor:
echo   HL7ParserService.exe stage-test 12345
echo.
echo Test a donor and reset status after:
echo   HL7ParserService.exe stage-test 12345 --reset-after
echo.
echo Run for all existing status=4 donors:
echo   HL7ParserService.exe stage-test --generate-only
echo.
echo Examples:
HL7ParserService.exe stage-test --help

pause