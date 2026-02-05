# Modified
## Filename
TenantBasedMicrosoftAccountOptions.cs
## Relative Path
inzibackend.Web.Mvc\Startup\AuthConfigurers\TenantBasedMicrosoftAccountOptions.cs
## Language
C#
## Summary
A class implementing TenantBasedSocialLoginOptionsBase for MicrosoftAccountOptions, managing external login provider settings with corrected method calls and constructor parameter.
## Changes
Constructor parameter name corrected from 'AppSettings.ExternalLoginProvider.Tenant.microsoft' to 'AppSettingsExternalLoginProvider.Tenant.microsoft'. Method calls in 'TenantHasSettings()' and 'GetHostSettings()' fixed by removing spaces around the dot.
## Purpose
Configuration for Microsoft Account external login provider settings in an ASP.NET Zero application.
