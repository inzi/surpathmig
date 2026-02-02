# Modified
## Filename
inzibackendWebMvcModule.cs
## Relative Path
inzibackend.Web.Mvc\Startup\inzibackendWebMvcModule.cs
## Language
C#
## Summary
The modified file introduces a new _audienceDiscriminatorEndpoint property with a backing field _audienceDiscriminatorEndpointPath. This addition enhances configuration options for multi-tenancy settings.
## Changes
Added the following lines:
1. Configuration.Navigation.Providers.Add<AudienceDiscriminatorProvider>();
2. Added the new property and its backing field: _audienceDiscriminatorEndpoint = _appConfiguration["AudienceDiscriminatorEndpoint"] ?? null;
## Purpose
The file enhances configuration flexibility for multi-tenancy settings by adding a new property that specifies the audience discriminator endpoint path.
