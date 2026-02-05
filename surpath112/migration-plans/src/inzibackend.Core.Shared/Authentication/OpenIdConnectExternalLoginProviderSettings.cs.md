# Modified
## Filename
OpenIdConnectExternalLoginProviderSettings.cs
## Relative Path
inzibackend.Core.Shared\Authentication\OpenIdConnectExternalLoginProviderSettings.cs
## Language
C#
## Summary
The modified class implements OpenIdConnectExternalLoginProviderSettings with key properties for client ID, secret, authority URL, login URL, issuer validation, and an IsValid() method that checks if client ID or authority is valid.
## Changes
The unmodified code includes a ResponseType property which was removed in the modification. The modified code also lacks the ResponseType validation check within the isValid() method.
## Purpose
This class configures settings for external login provider, ensuring proper authentication setup.
