# Modified
## Filename
SwaggerEnumParameterFilter.cs
## Relative Path
inzibackend.Web.Core\Swagger\SwaggerEnumParameterFilter.cs
## Language
C#
## Summary
The modified file implements a Swagger filter class that generates OpenAPI schemas for enum types in API parameters. It adds support for enum cases and ensures required parameters are marked appropriately.
## Changes
Added handling of enum parameter requirements by setting `parameter.Required` based on the type match, along with proper schema extension additions.
## Purpose
The file provides functionality to generate accurate OpenAPI schemas for enum types in API parameters, ensuring correct serialization and validation.
