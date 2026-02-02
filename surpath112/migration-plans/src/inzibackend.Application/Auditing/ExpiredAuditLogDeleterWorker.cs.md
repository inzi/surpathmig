# Modified
## Filename
ExpiredAuditLogDeleterWorker.cs
## Relative Path
inzibackend.Application\Auditing\ExpiredAuditLogDeleterWorker.cs
## Language
C#
## Summary
An ExpiredAuditLogDeleterWorker class that periodically deletes expired audit logs. It uses a PeriodicBackgroundWorkerBase for background processing and implements ISingletonDependency for singleton behavior.
## Changes
The configuration property name used to enable/disable the worker has been updated from "App:AuditLog:AutoDeleteExpiredLogs:IsEnabled" to "App:AuditLog:AutoDeleteExpiredLogs" in the modified version.
## Purpose
Manages and enables/disables the deletion of expired audit logs as configured.
