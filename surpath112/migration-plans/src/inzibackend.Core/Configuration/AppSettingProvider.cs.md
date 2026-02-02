# Modified
## Filename
AppSettingProvider.cs
## Relative Path
inzibackend.Core\Configuration\AppSettingProvider.cs
## Language
C#
## Summary
The provided C# code defines multiple `GetExternalLoginProviderSettings` methods that configure external authentication providers (e.g., Facebook, Google, Twitter) by reading configuration settings and creating `SettingDefinition` objects.
## Changes
The code contains redundant duplicated properties in the return arrays of each `GetExternalLoginProviderSettings` method. These duplicates should be removed to avoid confusion and potential errors.
## Purpose
The code is part of an application's authentication configuration setup, defining how different external login providers are configured with their respective settings.
