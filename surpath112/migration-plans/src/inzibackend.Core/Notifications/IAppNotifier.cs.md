# Modified
## Filename
IAppNotifier.cs
## Relative Path
inzibackend.Core\Notifications\IAppNotifier.cs
## Language
C#
## Summary
The modified file introduces SendMassNotificationAsync which consolidates multiple send methods into a single method that accepts an array of targetNotifiers.
## Changes
Added SendMassNotificationAsync with parameters: string message, UserIdentifier[] userIds = null, NotificationSeverity severity = NotificationSeverity.Info, Type[] targetNotifiers = null. Removed duplicate SendMessageAsync tasks and merged their functionality.
## Purpose
To streamline notification handling by providing a single method that can handle multiple notifications efficiently.
