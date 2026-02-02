# Modified
## Filename
AuditLogsController.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Controllers\AuditLogsController.cs
## Language
C#
## Summary
The modified file implements a method that appends user names to entity property changes when the property name ends with 'userid'. This enhances logging by including user information.
## Changes
Added code to append user names to entity change details if the property name ends with "userid". Also, added an additional using directive for Web.Controllers.
## Purpose
The controller handles auditing of entity changes and provides enhanced logging with user information.
