# Modified
## Filename
inzibackendCoreModule.cs
## Relative Path
inzibackend.Core\inzibackendCoreModule.cs
## Language
C#
## Summary
The modified file adds configuration for real-time SMS and email notifications by registering notifiers in the PreInitialize method.
## Changes
Added Configuration.Notifications.Notifiers.Add<SmsRealTimeNotifier>(); and Configuration.Notifications.Notifiers.Add<EmailRealTimeNotifier>(); to register notification providers in the PreInitialize method.
## Purpose
Configure notification services (SMS and Email) for real-time notifications during development
