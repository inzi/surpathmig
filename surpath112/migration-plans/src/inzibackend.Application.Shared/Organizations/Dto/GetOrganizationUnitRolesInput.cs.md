# Modified
## Filename
GetOrganizationUnitRolesInput.cs
## Relative Path
inzibackend.Application.Shared\Organizations\Dto\GetOrganizationUnitRolesInput.cs
## Language
C#
## Summary
The modified file defines a PagedAndSortedInputDto class with a Normalize method that processes sorting parameters by replacing specific field names.
## Changes
Added line in the Normalize method: Sorting = DtoSortingHelper.ReplaceSorting(Sorting, s => ...);
## Purpose
Dependency injection and configuration for data normalization.
