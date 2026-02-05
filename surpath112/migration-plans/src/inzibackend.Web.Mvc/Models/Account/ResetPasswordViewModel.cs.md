# Modified
## Filename
ResetPasswordViewModel.cs
## Relative Path
inzibackend.Web.Mvc\Models\Account\ResetPasswordViewModel.cs
## Language
C#
## Summary
The modified code includes an additional null check for the 'tenantId' query parameter before attempting to convert it to an integer.
## Changes
Added a null check for 'tenantId' in the query parameters. This prevents potential errors if the 'tenantId' is not present in the query string.
## Purpose
The file implements logic for handling password reset functionality, including decryption and tenant ID resolution.
