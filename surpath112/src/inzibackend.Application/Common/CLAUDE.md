# Common Documentation

> Auto-generated documentation for src\inzibackend.Application\Common

## Overview
Shared/common functionality

## Contents

### Files
- **CommonLookupAppService.cs**: Business service implementation
  - Components: CommonLookupAppService, GetEditionsForCombobox, FindUsers, GetAllTenantDepartmentForLookupTable, CohortLookup

### Key Components
- CommonLookupAppService.cs: CommonLookupAppService
- CommonLookupAppService.cs: GetEditionsForCombobox
- CommonLookupAppService.cs: FindUsers
- CommonLookupAppService.cs: GetAllTenantDepartmentForLookupTable
- CommonLookupAppService.cs: CohortLookup
- CommonLookupAppService.cs: FindDepartments

### Dependencies

External libraries and frameworks:
- System.Linq
- System.Threading.Tasks
- Abp.Application.Services.Dto
- Abp.Authorization
- Abp.Collections.Extensions
- Abp.Extensions
- Abp.Linq.Extensions
- Abp.Runtime.Session
- Microsoft.EntityFrameworkCore
- inzibackend.Common.Dto

## Architecture Notes

- File types: .cs

## Business Logic

Key business rules and domain logic found in this folder:

- AppPermissions.Pages_Cohorts,
- using Abp.Domain.Uow;
- using Abp.Application.Services.Dto;
- private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLookUpRepository;

## Design Patterns

- Service layer pattern

---
*Generated on 2025-09-26*