# Modified
## Filename
IUserEmailer.cs
## Relative Path
inzibackend.Core\Authorization\Users\IUserEmailer.cs
## Language
C#
## Summary
The modified interface includes additional methods for sending email activation links with plain passwords, password reset links, sending unread chat emails, and notifications about compliance status changes.
## Changes
Added 'plainPassword' parameter to SendEmailActivationLinkAsync, added SendPasswordResetLinkAsync method, added TrySendMessageChatMessageMail method, and added SendEmailForComplianceRelatedNotification method.
## Purpose
To provide methods for user email notifications including activation links, password resets, chat messages, and compliance status alerts.
