# Modified
## Filename
TenantBasedMicrosoftExternalLoginInfoProvider.cs
## Relative Path
inzibackend.Web.Host\Startup\ExternalLoginInfoProviders\TenantBasedMicrosoftExternalLoginInfoProvider.cs
## Language
C#
## Summary
The modified class implements dependency injection by using instance variables for ISettingManager and IAbpSession instead of passing them as parameters.
## Changes
The constructor now uses instance variables (_settingManager, _abpSession) instead of accepting them as parameters. This change likely improves dependency management within the ASP.NET Zero solution.
## Purpose
This class provides external login information for Microsoft authentication, utilizing dependency injection for better maintainability and testability.
