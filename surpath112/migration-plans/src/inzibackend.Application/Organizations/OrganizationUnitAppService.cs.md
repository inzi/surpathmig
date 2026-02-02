# Modified
## Filename
OrganizationUnitAppService.cs
## Relative Path
inzibackend.Application\Organizations\OrganizationUnitAppService.cs
## Language
C#
## Summary
Implement getAll() method for OrganizationUnitDto using AutoMapper
## Changes
Add AutoMapper.MapListAsync to convert repository result to list of DTOs, remove manual mapping.
## Purpose
Use AutoMapper's MapListAsync for clean and maintainable conversion from repository to DTO
