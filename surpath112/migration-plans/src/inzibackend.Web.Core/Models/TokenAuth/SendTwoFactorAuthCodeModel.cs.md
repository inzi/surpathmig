# Modified
## Filename
SendTwoFactorAuthCodeModel.cs
## Relative Path
inzibackend.Web.Core\Models\TokenAuth\SendTwoFactorAuthCodeModel.cs
## Language
C#
## Summary
The modified file introduces a Range attribute on the UserId property, enforcing values between 1 and long.MaxValue.
## Changes
Added [Range(1, long.MaxValue)] to the UserId property in SendTwoFactorAuthCodeModel.
## Purpose
Configuration for validating user IDs during two-factor authentication.
