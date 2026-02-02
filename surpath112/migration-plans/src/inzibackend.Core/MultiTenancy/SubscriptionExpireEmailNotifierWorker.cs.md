# Modified
## Filename
SubscriptionExpireEmailNotifierWorker.cs
## Relative Path
inzibackend.Core\MultiTenancy\SubscriptionExpireEmailNotifierWorker.cs
## Language
C#
## Summary
SubscriptionExpireEmailNotifierWorker is a background worker that notifies users when their subscriptions expire based on configuration settings. It retrieves expired tenants from the repository, checks if their subscription has expired, and sends an email notification.
## Changes
The constructor parameters for _userEmailer and _localStorageManager have been swapped in the modified version compared to the unmodified one.
## Purpose
To handle subscription expiration notifications efficiently as a background worker.
