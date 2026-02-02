# Modified
## Filename
ResetPasswordInput.cs
## Relative Path
inzibackend.Application.Shared\Authorization\Accounts\Dto\ResetPasswordInput.cs
## Language
C#
## Summary
The modified class implements IShouldNormalize with properties for UserId, ResetCode, and decrypted values. The unmodified version includes ExpireDate as an additional property.
## Changes
Removed ExpireDate from c encryption; added decryption of userId and resetCode only.
## Purpose
Data normalization during password reset process.
