# Modified
## Filename
ICommonLookupAppService.cs
## Relative Path
inzibackend.Application.Shared\Common\ICommonLookupAppService.cs
## Language
C#
## Summary
The modified file defines an interface with three methods: GetEditionsForCombobox, FindUsers, and GetAllTenantDepartmentForLookupTable. The new method GetAllTenantDepartmentForLookupTable retrieves paged results of CohortTenantDepartmentLookupTableDto based on input parameters.
## Changes
Added the GetAllTenantDepartmentForLookupTable method which takes a GetAllForLookupTableInput and returns a Task<PagedResultDto<CohortTenantDepartmentLookupTableDto>>. This method was not present in the unmodified version.
## Purpose
The interface likely serves as an extension of common lookup services, providing additional functionality for tenant department data retrieval.
