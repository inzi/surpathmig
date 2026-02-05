# Modified
## Filename
WebhookSendAttemptAppService.cs
## Relative Path
inzibackend.Application\WebHooks\WebhookSendAttemptAppService.cs
## Language
C#
## Summary
The modified WebhookSendAttemptAppService class implements additional security controls by adding null checks for webhookEvent and webhookSubscription in its Resend method. It also enforces that TryOnce must be true before enqueuing the background job.
## Changes
Added null checks for webhookEvent and webhookSubscription, ensuring they are not null before enqueueing. Also added a check to ensure TryOnce is true before enqueuing the background job.
## Purpose
To enhance security by validating inputs and preventing potential runtime exceptions or multiple unnecessary job submissions.
