# Modified
## Filename
inzibackendWebHostModule.cs
## Relative Path
inzibackend.Web.Host\Startup\inzibackendWebHostModule.cs
## Language
C#
## Summary
The modified file introduces additional authentication providers including OpenId Connect, WsFederation, Facebook, Twitter, Google, and Microsoft, each with their respective configuration options.
## Changes
Added `bool.Parse(_appConfiguration["Authentication:OpenId:ValidateIssuer"])` when creating the OpenIdConnect provider in the `ConfigureExternalAuthProviders` method.
## Purpose
The module configures external authentication providers for an ASP.NET Zero application, enabling social login features with various providers.
