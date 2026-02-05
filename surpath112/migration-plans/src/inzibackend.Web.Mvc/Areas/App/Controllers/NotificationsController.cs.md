# Modified
## Filename
NotificationsController.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Controllers\NotificationsController.cs
## Language
C#
## Summary
The modified file introduces a simplified NotificationsController with a single INotificationAppService dependency, focusing on retrieving notification settings and creating mass notifications.
## Changes
1. Removed additional using statements related to Microsoft.AspNetCore.Mvc and inzibackend.Authorization.
2. Simplified the controller by removing IOrganizationUnitAppService dependency.
3. Added CreateMassNotificationModal() method for handling mass notifications.
4. Modified SettingsModal() to use GetAllNotifiers() instead of GetNotificationSettings().
## Purpose
The controller implements notification services within an ASP.NET Zero application, facilitating mass notifications and settings management.
