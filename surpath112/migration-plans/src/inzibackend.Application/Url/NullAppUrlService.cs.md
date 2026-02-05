# Modified
## Filename
NullAppUrlService.cs
## Relative Path
inzibackend.Application\Url\NullAppUrlService.cs
## Language
C#
## Summary
Modified NullAppUrlService class with additional method overloads for creating email activation and password reset URLs using different parameter types.
## Changes
Added two new methods: CreateEmailActivationUrlFormat(int? tenantId) and CreateEmailActivationUrlFormat(string tenancyName), as well as their password reset counterparts. These methods handle URL formatting based on the provided parameters.
## Purpose
The class provides URL generation capabilities for email activation and password reset processes, supporting different parameter types to accommodate various user scenarios.
