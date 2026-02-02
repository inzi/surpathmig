@echo off
echo Building Project Concert Test Utility...

REM First build the main HL7ParserService project to ensure all dependencies are available
msbuild HL7ParserService.csproj /p:Configuration=Debug /p:Platform=AnyCPU

REM Get the path to csc.exe (C# compiler)
set CSC="%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\Roslyn\csc.exe"
if not exist %CSC% set CSC="%ProgramFiles%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\Roslyn\csc.exe"
if not exist %CSC% set CSC="%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\Roslyn\csc.exe"
if not exist %CSC% set CSC="%ProgramFiles%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\Roslyn\csc.exe"
if not exist %CSC% set CSC="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"

echo Using compiler: %CSC%

REM Compile the test utility
%CSC% /out:bin\Debug\ProjectConcertTest.exe ^
      /target:exe ^
      /platform:anycpu ^
      /reference:bin\Debug\HL7ParserService.exe ^
      /reference:bin\Debug\MySql.Data.dll ^
      /reference:bin\Debug\Serilog.dll ^
      /reference:bin\Debug\SurPath.Backend.dll ^
      /reference:bin\Debug\SurPath.Business.dll ^
      /reference:bin\Debug\SurPath.Data.dll ^
      /reference:bin\Debug\SurPath.Entity.dll ^
      /reference:bin\Debug\SurPath.Enum.dll ^
      /reference:System.Configuration.dll ^
      /reference:System.Data.dll ^
      ProjectConcertTestProgram.cs

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build successful! 
    echo Executable created at: bin\Debug\ProjectConcertTest.exe
    echo.
    echo The utility will use the same App.config as HL7ParserService
) else (
    echo.
    echo Build failed!
)

pause