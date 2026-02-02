# Project Concert Test Utility

A test utility for Project Concert integration that's built alongside the HL7ParserService project.

## Building the Utility

The utility is a separate executable that shares the HL7ParserService's configuration and dependencies.

### Option 1: PowerShell Script (Recommended)
```powershell
.\BuildProjectConcertTest.ps1
```

### Option 2: Batch File
```cmd
BuildProjectConcertTest.bat
```

### Option 3: Manual Build
```cmd
# First build HL7ParserService
msbuild HL7ParserService.csproj /p:Configuration=Debug

# Then compile the test utility
csc /out:bin\Debug\ProjectConcertTest.exe /target:exe /reference:bin\Debug\HL7ParserService.exe /reference:bin\Debug\MySql.Data.dll /reference:bin\Debug\Serilog.dll /reference:bin\Debug\SurPath.Backend.dll /reference:bin\Debug\SurPath.Business.dll /reference:bin\Debug\SurPath.Data.dll /reference:bin\Debug\SurPath.Entity.dll /reference:bin\Debug\SurPath.Enum.dll /reference:System.Configuration.dll /reference:System.Data.dll ProjectConcertTestProgram.cs
```

## Usage

After building, run the utility from the `bin\Debug` directory:

```cmd
cd bin\Debug

# Test a specific donor
ProjectConcertTest.exe 12345

# Test a donor and reset status afterwards
ProjectConcertTest.exe 12345 --reset-after

# Run for all existing status=4 donors without changing any
ProjectConcertTest.exe --generate-only

# Check a donor without changing their status
ProjectConcertTest.exe 12345 --no-reset
```

## Configuration

The utility uses the same `App.config` as HL7ParserService. When you build it, the config file is automatically copied as `ProjectConcertTest.exe.config`.

Key settings used:
- `ConnectionString` - Database connection
- `LabReportFilePath` - Where lab reports are generated
- `MROReportFileInboundPath` - Where MRO reports are generated
- `StageFilesFolder` - Where template files are located

## How It Works

1. **Validates the donor** - Checks if they exist and are linked to a Project Concert partner
2. **Updates test status** - Sets the donor_test_info.test_status to 4 (if not using --no-reset)
3. **Runs HL7Stage.Gen()** - Generates test files ONLY for the specified donor
4. **Creates files**:
   - Lab reports in the configured LabReportFilePath
   - MRO reports in the configured MROReportFileInboundPath
5. **Optionally resets** - Can reset the status back to 6 after generation

## Production Safety

- Only processes the specific donor ID you provide
- Does not affect any other donors in the system
- Shows warnings if a donor is not properly integrated
- Asks for confirmation before proceeding with non-integrated donors

## Troubleshooting

If the build fails:
1. Ensure HL7ParserService builds successfully first
2. Check that all referenced DLLs exist in bin\Debug
3. Verify you have the .NET Framework SDK installed
4. Try using the full path to csc.exe if it's not found

If the utility fails to run:
1. Check the connection string in App.config
2. Verify the file paths for lab/MRO reports exist
3. Check the logs for specific error messages
4. Ensure the donor exists and has test info