# Modified
## Filename
AppWebhookPublisher.cs
## Relative Path
inzibackend.Core\Webhooks\AppWebhookPublisher.cs
## Language
C#
## Summary
The modified file is a class implementing IAppWebhookPublisher interface with an additional variable declaration for separator used in constructing user details.
## Changes
Added line: var separator = DateTime.Now.Millisecond; which is not present in the unmodified version.
## Purpose
The class implements IAppWebhookPublisher interface and provides functionality to publish test webhooks with user details, likely used in dependency injection scenarios.
