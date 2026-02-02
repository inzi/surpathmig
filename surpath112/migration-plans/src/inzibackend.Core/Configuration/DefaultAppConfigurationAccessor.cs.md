# Modified
## Filename
DefaultAppConfigurationAccessor.cs
## Relative Path
inzibackend.Core\Configuration\DefaultAppConfigurationAccessor.cs
## Language
C#
## Summary
The modified file defines a class DefaultAppConfigurationAccessor that implements IAppConfigurationAccessor interface. This class provides access to the Configuration property which is retrieved from AppConfigurations.Get(Directory.GetCurrentDirectory()). The class is marked as ISingletonDependency, indicating it should be used in a singleton context.
## Changes
No changes were made to the code content between modified and unmodified versions. The only difference is an added comment specifying that this service is replaced separately in Web and Test layers.
## Purpose
The file provides configuration access functionality within an ASP.NET Zero application, ensuring consistent configuration handling across services by utilizing singleton dependency injection.
