# Modified
## Filename
inzibackendSmtpEmailSenderConfiguration.cs
## Relative Path
inzibackend.Core\Net\Emailing\inzibackendSmtpEmailSenderConfiguration.cs
## Language
C#
## Summary
The modified file contains an override for the Password property in the SmtpEmailSenderConfiguration class. The password is decrypted using SimpleStringCipher.Instance.Decrypt and retrieved from the setting manager.
## Changes
No changes were made to the code between the modified and unmodified versions. Both files contain identical code except for formatting differences.
## Purpose
The file configures an email sender that uses an encrypted password, ensuring secure communication by decrypting the password before use.
