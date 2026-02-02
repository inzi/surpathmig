# Modified
## Filename
AppNotificationProvider.cs
## Relative Path
inzibackend.Core\Notifications\AppNotificationProvider.cs
## Language
C#
## Summary
The modified file adds a new notification definition for RecordStateExpirationWarning with a specific permission dependency.
## Changes
Added a new NotificationDefinition for RecordStateExpirationWarning with displayName: L("RecordStateExpirationWarningDefinition") and permissionDependency: new SimplePermissionDependency(AppPermissionsPages_CohortUser).
## Purpose
The file is part of an ASP.NET Zero solution handling notification definitions for user, tenant, and record state expiration warnings.
