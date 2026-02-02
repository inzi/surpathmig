# Auditing Documentation

## Overview
This folder contains service interfaces and DTOs for the audit logging system. The audit system tracks user actions, entity changes, and system events for security, compliance, and troubleshooting purposes. It provides comprehensive audit trails of all significant operations within the application.

## Contents

### Files

#### Service Interfaces
- **IAuditLogAppService.cs** - Audit log service interface:
  - GetAuditLogs - Query audit logs with filtering
  - GetEntityChanges - Query entity change history
  - GetEntityPropertyChanges - Query property-level changes
  - Export audit logs for compliance

- **IEntityPropertyValueResolver.cs** - Resolve entity property values:
  - Converts entity references to readable text
  - Resolves foreign keys to display names
  - Used in audit log display for better readability

- **IExpiredAndDeletedAuditLogBackupService.cs** - Audit log archival:
  - Backup expired audit logs
  - Clean up old logs per retention policy
  - Archive for long-term storage
  - Background service interface

### Subfolders

#### Dto
Contains all audit logging DTOs including query inputs, list outputs, and entity change tracking.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes

### Two-Level Auditing
- **Service-Level**: Tracks API calls, method executions, parameters
- **Entity-Level**: Tracks database changes (Create, Update, Delete)
- **Property-Level**: Tracks individual field modifications

### Audit Trail Components
- User identity tracking
- Impersonation tracking
- Client information (IP, browser)
- Execution timing and duration
- Exception capture
- Custom data fields

### Retention Management
- Configurable retention periods
- Automatic archival of old logs
- Backup before deletion
- Restore capability

## Business Logic

### Audit Logging Use Cases
1. **Security Investigations**: Track suspicious activities
2. **Compliance Reporting**: Generate audit reports for regulations
3. **Troubleshooting**: Review system behavior and errors
4. **Entity History**: Show complete change history for any record
5. **Performance Analysis**: Identify slow operations

### Audit Policy
- All authenticated actions logged
- Anonymous actions optionally logged
- Sensitive data excluded (passwords, PII)
- Configurable verbosity levels

## Usage Across Codebase
These interfaces are consumed by:
- **Admin dashboards** - Audit log viewers
- **Compliance reports** - Regulatory audit trails
- **Entity history features** - Show change history
- **Security monitoring** - Real-time alerts
- **Background jobs** - Audit log cleanup

## Cross-Reference Impact
Changes affect:
- Audit log viewing interfaces
- Entity change history displays
- Security monitoring systems
- Compliance reporting
- Log archival processes