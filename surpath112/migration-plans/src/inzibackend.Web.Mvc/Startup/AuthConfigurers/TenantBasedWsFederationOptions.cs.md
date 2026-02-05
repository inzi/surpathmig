# Modified
## Filename
TenantBasedWsFederationOptions.cs
## Relative Path
inzibackend.Web.Mvc\Startup\AuthConfigurers\TenantBasedWsFederationOptions.cs
## Language
C#
## Summary
The modified code introduces additional properties and methods in the TenantBasedWsFederationOptions class. It includes setting AbpSession to NullAbpSession.Instance and assigns _settingManager. The class overrides methods such as GetTenantSettings, GetHostSettings, SetOptions with extra lines for ConfigurationManager assignment and HandleOnSecurityTokenValidated method call.
## Changes
Added properties: AbpSession = NullAbpSession.Instance; _settingManager = settingManager;. Modified overridden methods to include additional lines in SetOptions: options.ConfigurationManager assignment and HandleOnSecurityTokenValidated method call. Added GetHostSettings method.
## Purpose
Configuration setup for WsFederation authentication in an ASP.NET Zero application, ensuring proper settings are loaded for tenant-based scenarios.
