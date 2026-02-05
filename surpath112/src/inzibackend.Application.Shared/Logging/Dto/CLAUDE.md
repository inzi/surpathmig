# Logging DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for application logging and log file management. These DTOs enable administrators to view, download, and analyze application log files for troubleshooting, monitoring, and debugging purposes.

## Contents

### Files

- **GetLatestWebLogsOutput.cs** - Recent log entries:
  - LatestWebLogLines - Most recent log lines
  - Log file content from web server
  - Used for quick log viewing without download

### Key Components

#### Log Viewing
- Read recent log entries
- Filter by log level (Debug, Info, Warning, Error, Fatal)
- Search log content
- Download log files

#### Log Files
Typical log types:
- Application logs - General application events
- Error logs - Exception and error tracking
- Audit logs - Separate from general logs (see Auditing)
- Performance logs - Slow query and performance issues

### Dependencies
- Logging framework (likely Serilog or NLog)
- File system access for log files

## Architecture Notes

### Log File Management
- Logs typically stored in Logs/ folder
- Rolling log files (daily, size-based)
- Retention policies for log cleanup
- Structured logging with JSON support

### Security Considerations
- Log access restricted to administrators
- Sensitive data sanitized before logging
- Log files may contain PII (handle carefully)
- Secure log transmission (HTTPS only)

### Performance
- Latest lines only (doesn't load entire file)
- Paging for large log files
- Background log reading (non-blocking)

## Business Logic

### Log Viewing Workflow
1. Admin navigates to log viewer
2. System returns GetLatestWebLogsOutput with recent entries
3. Admin searches/filters logs
4. Downloads full log file if needed
5. Reviews errors for troubleshooting

### Use Cases
- **Troubleshooting**: Review errors and exceptions
- **Monitoring**: Check application health
- **Security**: Audit unauthorized access attempts
- **Performance**: Identify slow operations
- **Compliance**: Maintain log records

### Log Levels
- **Debug**: Detailed debugging information
- **Info**: General informational messages
- **Warning**: Warning messages for concerning situations
- **Error**: Error messages for failures
- **Fatal**: Critical failures requiring immediate attention

## Usage Across Codebase
These DTOs are consumed by:
- **IWebLogAppService** - Log retrieval operations
- **Admin Dashboard** - Log viewer interface
- **Monitoring Tools** - Log analysis and alerting
- **Troubleshooting Tools** - Error investigation
- **Download Services** - Log file download

## Cross-Reference Impact
Changes to these DTOs affect:
- Log viewer interfaces
- Log download functionality
- Monitoring dashboards
- Troubleshooting tools
- Log analysis features