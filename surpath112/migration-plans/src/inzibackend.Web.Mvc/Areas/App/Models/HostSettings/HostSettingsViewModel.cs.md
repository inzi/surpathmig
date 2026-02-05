# Modified
## Filename
HostSettingsViewModel.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Models\HostSettings\HostSettingsViewModel.cs
## Language
C#
## Summary
The modified HostSettingsViewModel introduces a new method GetOpenIdConnectResponseTypes() which retrieves OpenId Connect response types from the ExternalLoginProviderSettings.
## Changes
Added the method GetOpenIdConnectResponseTypes(), which returns a list of strings representing OpenId Connect response types by splitting and trimming the values from Settings.ExternalLoginProviderSettings.OpenIdConnect.ResponseType.
## Purpose
The method retrieves configuration parameters for OpenId Connect authentication, aiding in setting up social login functionality.
