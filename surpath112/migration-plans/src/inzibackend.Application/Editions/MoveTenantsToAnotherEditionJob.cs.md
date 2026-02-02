# Modified
## Filename
MoveTenantsToAnotherEditionJob.cs
## Relative Path
inzibackend.Application\Editions\MoveTenantsToAnotherEditionJob.cs
## Language
C#
## Summary
The modified file introduces additional logging for failed tenant moves and provides more detailed error handling. It includes specific edition names in user notifications.
## Changes
Added detailed logging using Logger.Error for failed tenant moves, added specific edition names in user notifications, and included more comprehensive error handling within ChangeEditionOfTenantAsync.
## Purpose
The class is used in dependency injection to move tenants between editions by updating their EditionId property.
