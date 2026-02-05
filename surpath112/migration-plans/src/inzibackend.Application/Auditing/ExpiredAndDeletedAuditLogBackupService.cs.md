# Modified
## Filename
ExpiredAndDeletedAuditLogBackupService.cs
## Relative Path
inzibackend.Application\Auditing\ExpiredAndDeletedAuditLogBackupService.cs
## Language
C#
## Summary
The modified file introduces explicit configuration checking for backup functionality, while retaining all other functionalities.
## Changes
Added explicit check for 'App:AuditLog:AutoDeleteExpiredLogs:ExcelBackup:IsEnabled' configuration setting in the constructor.
## Purpose
The class implements an Excel backup service for audit logs, using configuration settings to enable backups and specify output paths.
