# Modified
## Filename
PasswordExpirationBackgroundWorker.cs
## Relative Path
inzibackend.Application\Authorization\Users\Password\PasswordExpirationBackgroundWorker.cs
## Language
C#
## Summary
The class implements a BackgroundWorker that periodically (every day) calls a service to force users with expired passwords to change their passwords.
## Changes
The modified code introduces an extra space in the parameter list of the constructor for IUnitOfWorkManager, which is likely a typo or formatting error.
## Purpose
Background worker handling password expiration notifications.
