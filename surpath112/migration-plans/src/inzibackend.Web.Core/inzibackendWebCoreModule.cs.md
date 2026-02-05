# Modified
## Filename
inzibackendWebCoreModule.cs
## Relative Path
inzibackend.Web.Core\inzibackendWebCoreModule.cs
## Language
C#
## Summary
The modified file sets up configuration for a web core module in an ASP.NET Zero application. It includes setting default connection strings, enabling language management, creating controllers, configuring token authentication (JWTBearer), and replacing configuration services.
## Changes
Added a commented out line to use Redis cache instead of in-memory cache,Modified the Initialize method to call IocManager.RegisterAssemblyByConvention once instead of twice
## Purpose
The file is part of an ASP.NET Zero application's web core module configuration, handling dependency injection and service setup.
