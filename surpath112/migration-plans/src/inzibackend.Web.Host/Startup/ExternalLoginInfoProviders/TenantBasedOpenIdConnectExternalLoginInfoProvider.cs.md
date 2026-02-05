# Modified
## Filename
TenantBasedOpenIdConnectExternalLoginInfoProvider.cs
## Relative Path
inzibackend.Web.Host\Startup\ExternalLoginInfoProviders\TenantBasedOpenIdConnectExternalLoginInfoProvider.cs
## Language
C#
## Summary
The modified file includes an additional configuration parameter 'ResponseType' in the CreateExternalLoginInfo method, which adds a response URL mapping.
## Changes
Added "ResponseUrl": settings.ResponseType to the dictionary in the CreateExternalLoginInfo method.
## Purpose
To configure tenant-specific OpenId Connect authentication settings including response URL.
