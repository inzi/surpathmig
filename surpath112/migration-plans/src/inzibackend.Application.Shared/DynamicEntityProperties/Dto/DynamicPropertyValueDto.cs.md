# Modified
## Filename
DynamicPropertyValueDto.cs
## Relative Path
inzibackend.Application.Shared\DynamicEntityProperties\Dto\DynamicPropertyValueDto.cs
## Language
C#
## Summary
The modified file adds explicit backing field type declarations (public virtual int? TenantId { get; set; } and public virtual int DynamicPropertyId { get; set; }) for the properties in addition to the existing virtual string property.
## Changes
Explicit backing field type declarations were added for TenantId and DynamicPropertyId. The Value property already has a backing field declaration.
## Purpose
To enhance code clarity and maintainability by explicitly defining the types of each property.
