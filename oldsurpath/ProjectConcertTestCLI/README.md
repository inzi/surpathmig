# Project Concert Test CLI

A command-line tool for testing Project Concert integration by generating test lab results using the HL7Stage class.

## Features

- Sets a specific donor's test status to 4 (Pre-Registration)
- Verifies if the donor is linked to Project Concert integration
- Runs HL7Stage to generate test lab and MRO report files
- Optionally resets the donor status after generation
- Can run for all existing status=4 donors without modifying any

## Prerequisites

1. Update the connection string in `App.config`
2. Ensure the file paths in `App.config` match your environment
3. Add references to the required projects in the .csproj file (update the GUIDs)
4. Build the solution

## Usage

### Test a specific donor:
```bash
ProjectConcertTestCLI.exe 12345
```

### Test a donor and reset status afterwards:
```bash
ProjectConcertTestCLI.exe 12345 --reset-after
```

### Run for all existing status=4 donors without changing any:
```bash
ProjectConcertTestCLI.exe --generate-only
```

### Check a donor without changing their status:
```bash
ProjectConcertTestCLI.exe 12345 --no-reset
```

## What it does

1. **Validates the donor** - Checks if they exist and are linked to a Project Concert partner
2. **Updates test status** - Sets the donor_test_info.test_status to 4
3. **Runs HL7Stage.Gen()** - Generates test files with random results
4. **Creates files**:
   - Lab reports in: `C:\SurPathReports\SurPathReports\CRL\`
   - MRO reports in: `C:\SurPathReports\SurPathReports\MRO\Inbound\`
5. **Optionally resets** - Can reset the status back to 6 after generation

## Important Notes

- The donor's client must be mapped in `backend_integration_partner_client_map` for HL7Stage to process them
- Files are generated with random positive/negative results (50/50 chance)
- The tool shows warnings if a donor is not properly integrated
- Always test in a non-production environment first

## Building

1. Add this project to the Surpath solution
2. Update the project references in the .csproj file with correct GUIDs
3. Restore NuGet packages: `nuget restore`
4. Build: `msbuild ProjectConcertTestCLI.csproj`

## Safety Features

- Shows donor information before making changes
- Warns if donor is not linked to Project Concert
- Asks for confirmation before proceeding with non-integrated donors
- Can reset status after testing with `--reset-after`
- Logs all actions with timestamps