# Modified
## Filename
TenantBasedGoogleExternalLoginInfoProvider.cs
## Relative Path
inzibackend.Web.Host\Startup\ExternalLoginInfoProviders\TenantBasedGoogleExternalLoginInfoProvider.cs
## Language
C#
## Summary
The modified class introduces a new constructor parameter for ICacheManager and corrects typo errors in method paths.
## Changes
Added ICacheManager as a constructor parameter. Corrected 'AppSettings.ExternalLoginProvider.Tenant.Google' to 'AppSettingsExternalLoginProvider.Tenant.Google'. Also corrected 'AppSettings.ExternalLoginProvider.Host.Google' to 'AppSettingsExternalLoginProvider.Host.Google'.
## Purpose
Provides configuration and dependency management for Google external authentication in an ASP.NET Zero application.
