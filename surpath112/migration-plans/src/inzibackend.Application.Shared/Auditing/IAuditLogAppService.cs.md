# Modified
## Filename
IAuditLogAppService.cs
## Relative Path
inzibackend.Application.Shared\Auditing\IAuditLogAppService.cs
## Language
C#
## Summary
The modified file defines an interface IAuditLogAppService with methods for retrieving audit logs, exporting to Excel, tracking entity changes, generating history object types, and fetching property change lists. The unmodified version is identical except it does not include the GetEntityPropertyChanges method.
## Changes
Added a new method: Task<List<EntityPropertyChangeDto>> GetEntityPropertyChanges(long entityChangeId);
## Purpose
The file serves as an interface for auditing log management, including data retrieval and export functionalities.
