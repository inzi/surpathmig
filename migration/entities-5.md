# Surpath200 Migration: Entities-5 (2026-02-04)

## Accomplished
- ✅ Created `Core/DTOs/` directory
- ✅ Added 8 priority DTO stubs:
  - `RecordStateDto.cs`, `CohortDto.cs`, `TenantDepartmentDto.cs`
  - `UserPidDto.cs`, `TenantDepartmentUserDto.cs`, `CohortUserDto.cs`
  - `RecordStatusDto.cs`, `SurpathServiceDto.cs`

## Build Status
- ❌ **dotnet build** FAILS (51 errors)
- Missing **CORE ENTITIES** in `inzibackend.Core/Surpath/`:
  ```
  RecordState, Cohort, TenantDepartment, UserPid
  TenantDepartmentUser, CohortUser, RecordCategory
  RecordRequirement, RecordCategoryRule, Record, etc.
  ```

## Action Required
```
cd ~/projects/surpathmig/surpath112/src/inzibackend.Core/Surpath
cp *.cs ~/projects/surpathmig/surpath200/aspnet-core/src/inzibackend.Core/Surpath/
# Fix namespaces to 'surpath200.*', then dotnet build
```

DTOs resolve build-3.md missing type errors. Core entities are next migration step.