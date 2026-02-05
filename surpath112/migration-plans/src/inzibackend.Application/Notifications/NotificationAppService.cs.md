# Modified
## Filename
NotificationAppService.cs
## Relative Path
inzibackend.Application\Notifications\NotificationAppService.cs
## Language
C#
## Summary
The modified file introduces a new GetNotificationSettings method that retrieves notification configurations. It removes several existing methods (ShouldUserUpdateApp, SetAllAvailableVersionNotificationAsRead, CreateMassNotification) and focuses on managing notification settings including subscribed notifications and their states.
## Changes
1. Added GetNotificationSettings method to retrieve notification configuration details.
2. Removed ShouldUserUpdateApp method which was checking for new version notifications.
3. Removed SetAllAvailableVersionNotificationAsRead method that updated notification states.
4. Removed CreateMassNotification method which enqueues mass notifications.
## Purpose
The modified file focuses on managing notification settings, including retrieving configuration details and handling notification subscriptions and states.
