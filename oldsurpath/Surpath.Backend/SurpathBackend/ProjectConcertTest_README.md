# Project Concert Test Feature in SurPath.Backend

The SurPath.Backend service now includes Project Concert testing functionality, allowing you to generate test HL7 files for specific donors.

## Usage

After building the SurPath.Backend project, you can use the following commands:

### Test a specific donor
```cmd
Surpath.Backend.exe -t --donor-id 12345
```

### Test without changing donor status
```cmd
Surpath.Backend.exe -t --donor-id 12345 --no-reset
```

### Test and reset status afterwards
```cmd
Surpath.Backend.exe -t --donor-id 12345 --reset-after
```

### Run for all existing status=4 donors
```cmd
Surpath.Backend.exe -t
```

## Command-Line Options

- `-t, --project-concert-test`: Enable Project Concert test mode
- `--donor-id <id>`: Specify a donor ID to test
- `--no-reset`: Don't update the donor's test status to 4
- `--reset-after`: Reset the donor's test status back to 6 after generation

## How It Works

1. **Validates the donor**: Checks if the donor exists and their integration status
2. **Updates test status**: Sets donor_test_info.test_status to 4 (unless --no-reset)
3. **Generates test files**: Creates HL7 lab reports and MRO reports
4. **File locations**: 
   - Lab reports: Configured in `LabReportFilePath` setting
   - MRO reports: Configured in `MROReportFileInboundPath` setting

## Configuration

The feature uses these settings from App.config:
- `ConnectionString`: Database connection
- `Production`: Whether running in production mode
- `LabReportFilePath`: Where to save lab report files
- `MROReportFileInboundPath`: Where to save MRO report files

## Safety Features

- Prompts for confirmation if donor is not linked to Project Concert
- Only processes the specific donor ID provided
- Shows warnings for production mode
- Logs all operations for audit trail

## Building

Build the SurPath.Backend project in Visual Studio, then run from the bin\Debug directory.

## Notes

This is a simplified implementation that generates basic test HL7 files. For full HL7Stage functionality (with templates and more complex logic), you would need to reference the HL7ParserService project and use the actual HL7Stage class.