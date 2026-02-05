# Modified
## Filename
AppConfigurations.cs
## Relative Path
inzibackend.Core\Configuration\AppConfigurations.cs
## Language
C#
## Summary
The modified code includes an additional method call to configure an Azure Key Vault for the built configuration. The unmodified version does not include this step.
## Changes
Added a new line in BuildConfiguration: new AppAzureKeyVaultConfigurer().Configure(builder, builtConfig);
## Purpose
Managing application settings with environment-specific configurations and user secrets using dependency injection and caching.
