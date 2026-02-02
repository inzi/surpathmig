# Cohort Migration Between Departments Feature Implementation Plan

## Overview

This plan outlines the implementation of a comprehensive cohort migration system that allows administrators to:
1. Move cohorts from one department to another (existing or new department)
2. Handle requirement category mapping during migration
3. Preserve compliance states throughout the migration process
4. Provide step-by-step guided workflow with validation and confirmations
5. Maintain data integrity and audit trails

## Problem Statement

Currently, when cohorts need to be moved between departments (e.g., LVN-RN Bridge cohorts from ADN department to LVN-RN Bridge department), there is no systematic way to handle:
- Department-specific requirements and categories
- User upload associations with requirement categories
- Compliance state preservation
- Data integrity during complex migrations

This creates manual processes that are error-prone and can result in data inconsistencies and compliance calculation issues.

## Solution Architecture

### 1. Core Components

#### A. Cohort Migration Service
- **File**: `src/inzibackend.Application/Surpath/CohortMigrationAppService.cs`
- **Interface**: `src/inzibackend.Application/Surpath/ICohortMigrationAppService.cs`
- **Purpose**: Handle all cohort migration operations with business logic validation

#### B. Cohort Migration DTOs
- **File**: `src/inzibackend.Application.Shared/Surpath/Dtos/CohortMigrationDtos.cs`
- **Purpose**: Define data transfer objects for cohort migration operations

#### C. Cohort Migration UI Components
- **Files**: 
  - `src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/_CohortMigrationWizardModal.cshtml`
  - `src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/_DepartmentSelectionStep.cshtml`
  - `src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/_RequirementMappingStep.cshtml`
  - `src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/_MigrationConfirmationStep.cshtml`
- **Purpose**: Provide step-by-step user interface for cohort migration

#### D. JavaScript Controllers
- **Files**:
  - `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Cohorts/cohortMigration.js`
  - Updates to existing `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Cohorts/index.js`

## Detailed Implementation Plan

### Phase 1: Backend Infrastructure

#### 1.1 Create Cohort Migration DTOs

**File**: `src/inzibackend.Application.Shared/Surpath/Dtos/CohortMigrationDtos.cs`

```csharp
// Main Migration DTOs
public class CohortMigrationDto
{
    public Guid CohortId { get; set; }
    public Guid? TargetDepartmentId { get; set; } // null if creating new department
    public string NewDepartmentName { get; set; } // required if TargetDepartmentId is null
    public List<RequirementCategoryMappingDto> CategoryMappings { get; set; }
    public bool ConfirmMigration { get; set; }
}

public class CohortMigrationResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Guid? NewDepartmentId { get; set; }
    public int AffectedUsersCount { get; set; }
    public int MigratedRecordStatesCount { get; set; }
    public List<string> Warnings { get; set; }
    public List<string> Errors { get; set; }
}

// Requirement Category Mapping DTOs
public class RequirementCategoryMappingDto
{
    public Guid SourceCategoryId { get; set; }
    public string SourceCategoryName { get; set; }
    public string SourceRequirementName { get; set; }
    public MappingAction Action { get; set; } // Map, Copy, Skip
    public Guid? TargetCategoryId { get; set; } // for Map action
    public string NewRequirementName { get; set; } // for Copy action
    public string NewCategoryName { get; set; } // for Copy action
    public int AffectedRecordStatesCount { get; set; }
    public int AffectedUsersCount { get; set; }
}

public enum MappingAction
{
    MapToExisting, // Map to existing category in target department
    CopyToNew,     // Copy category to target department
    Skip           // Skip this category (will lose data)
}

// Validation and Analysis DTOs
public class CohortMigrationAnalysisDto
{
    public Guid CohortId { get; set; }
    public string CohortName { get; set; }
    public Guid SourceDepartmentId { get; set; }
    public string SourceDepartmentName { get; set; }
    public int TotalUsersCount { get; set; }
    public List<RequirementCategoryAnalysisDto> RequirementCategories { get; set; }
    public List<string> Warnings { get; set; }
    public bool CanMigrate { get; set; }
}

public class RequirementCategoryAnalysisDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Guid RequirementId { get; set; }
    public string RequirementName { get; set; }
    public bool IsDepartmentSpecific { get; set; }
    public int RecordStatesCount { get; set; }
    public int AffectedUsersCount { get; set; }
    public List<TargetCategoryOptionDto> TargetOptions { get; set; }
}

public class TargetCategoryOptionDto
{
    public Guid? CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Guid? RequirementId { get; set; }
    public string RequirementName { get; set; }
    public bool IsExactMatch { get; set; }
    public bool IsSimilarMatch { get; set; }
}

// Department DTOs
public class DepartmentLookupDto
{
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int CohortsCount { get; set; }
    public int RequirementsCount { get; set; }
}

public class CreateDepartmentDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? TenantId { get; set; }
}
```

#### 1.2 Create Cohort Migration Service Interface

**File**: `src/inzibackend.Application/Surpath/ICohortMigrationAppService.cs`

```csharp
public interface ICohortMigrationAppService : IApplicationService
{
    // Analysis and Validation
    Task<CohortMigrationAnalysisDto> AnalyzeCohortMigration(Guid cohortId, Guid? targetDepartmentId);
    Task<List<TargetCategoryOptionDto>> GetTargetCategoryOptions(Guid sourceCategoryId, Guid targetDepartmentId);
    Task<bool> ValidateMigrationMappings(List<RequirementCategoryMappingDto> mappings);
    
    // Migration Operations
    Task<CohortMigrationResultDto> MigrateCohort(CohortMigrationDto input);
    Task<CohortMigrationResultDto> MigrateCohortToNewDepartment(CohortMigrationDto input);
    Task<CohortMigrationResultDto> MigrateCohortToExistingDepartment(CohortMigrationDto input);
    
    // Department Management
    Task<List<DepartmentLookupDto>> GetAvailableTargetDepartments(Guid excludeDepartmentId);
    Task<Guid> CreateDepartment(CreateDepartmentDto input);
    
    // Utility Methods
    Task<int> GetCohortUsersCount(Guid cohortId);
    Task<List<RequirementCategoryAnalysisDto>> GetCohortRequirementCategories(Guid cohortId);
    Task<bool> CanMigrateCohort(Guid cohortId);
    
    // Rollback and Recovery
    Task<bool> CanRollbackMigration(Guid cohortId);
    Task<CohortMigrationResultDto> RollbackMigration(Guid cohortId);
}
```

#### 1.3 Implement Cohort Migration Service

**File**: `src/inzibackend.Application/Surpath/CohortMigrationAppService.cs`

Key methods to implement:

1. **AnalyzeCohortMigration**: Analyze impact and provide mapping suggestions
2. **MigrateCohort**: Main orchestration method for migration
3. **HandleRequirementCategoryMapping**: Process category mappings (map/copy/skip)
4. **PreserveCohortUserCompliance**: Ensure compliance states are maintained
5. **CreateMigrationAuditTrail**: Track all migration operations
6. **ValidateDataIntegrity**: Ensure referential integrity throughout migration
7. **Transaction Management**: Ensure all operations are atomic

#### 1.4 Update Permissions

**File**: `src/inzibackend.Core/Authorization/AppPermissions.cs`

```csharp
// Add new permissions
public const string Pages_Cohorts_Migrate = "Pages.Cohorts.Migrate";
public const string Pages_Cohorts_MigrateBetweenDepartments = "Pages.Cohorts.MigrateBetweenDepartments";
public const string Pages_Cohorts_CreateDepartment = "Pages.Cohorts.CreateDepartment";
public const string Pages_Cohorts_ViewMigrationHistory = "Pages.Cohorts.ViewMigrationHistory";
public const string Pages_Cohorts_RollbackMigration = "Pages.Cohorts.RollbackMigration";
```

**File**: `src/inzibackend.Core/Authorization/AppAuthorizationProvider.cs`

Add permission definitions and hierarchy.

### Phase 2: Frontend Implementation

#### 2.1 Update Cohorts Index Page

**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/Index.cshtml`

Add new action button:
- "Migrate Cohort" button for each cohort row in the DataTable

#### 2.2 Create Cohort Migration Wizard Modal

**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/_CohortMigrationWizardModal.cshtml`

Features:
- Multi-step wizard interface
- Progress indicator
- Step navigation (Next/Previous/Cancel)
- Validation at each step

#### 2.3 Create Department Selection Step

**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/_DepartmentSelectionStep.cshtml`

Features:
- Radio button selection between existing department and new department
- Dropdown for existing departments
- Form fields for new department creation
- Department information display (cohorts count, requirements count)

#### 2.4 Create Requirement Mapping Step

**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/_RequirementMappingStep.cshtml`

Features:
- DataTable showing all requirement categories that need mapping
- For each category:
  - Source category information
  - Affected users/records count
  - Mapping action selection (Map/Copy/Skip)
  - Target category dropdown (for Map action)
  - New requirement/category names (for Copy action)
- Smart suggestions for similar categories
- Bulk actions for common mappings

#### 2.5 Create Migration Confirmation Step

**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/_MigrationConfirmationStep.cshtml`

Features:
- Summary of migration plan
- Impact analysis (users affected, records to be migrated)
- Warnings and confirmations
- Final confirmation checkbox
- Execute migration button

#### 2.6 JavaScript Implementation

**File**: `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Cohorts/cohortMigration.js`

Key functions:
- `showCohortMigrationWizard(cohortId)`
- `loadDepartmentSelectionStep()`
- `loadRequirementMappingStep(targetDepartmentId)`
- `loadMigrationConfirmationStep(mappings)`
- `validateCurrentStep()`
- `executeNextStep()`
- `executePreviousStep()`
- `executeMigration(migrationData)`
- `showMigrationProgress()`
- `handleMigrationComplete(result)`

### Phase 3: Controller Updates

#### 3.1 Update Cohorts Controller

**File**: `src/inzibackend.Web.Mvc/Areas/App/Controllers/CohortsController.cs`

Add new action methods:
- `CohortMigrationWizardModal(Guid cohortId)`
- `DepartmentSelectionStep(Guid cohortId)`
- `RequirementMappingStep(Guid cohortId, Guid? targetDepartmentId)`
- `MigrationConfirmationStep(CohortMigrationDto input)`
- `AnalyzeCohortMigration(Guid cohortId, Guid? targetDepartmentId)`
- `GetTargetCategoryOptions(Guid sourceCategoryId, Guid targetDepartmentId)`
- `MigrateCohort(CohortMigrationDto input)`
- `CreateDepartment(CreateDepartmentDto input)`

### Phase 4: Database Considerations

#### 4.1 Migration Audit Table

**File**: `sql/CohortMove/CreateMigrationAuditTable.sql`

```sql
CREATE TABLE CohortMigrationAudit (
    Id CHAR(36) PRIMARY KEY,
    CohortId CHAR(36) NOT NULL,
    SourceDepartmentId CHAR(36) NOT NULL,
    TargetDepartmentId CHAR(36) NOT NULL,
    MigrationData JSON, -- Store complete migration details
    UserId BIGINT NOT NULL,
    TenantId INT,
    Status VARCHAR(50), -- 'InProgress', 'Completed', 'Failed', 'RolledBack'
    StartTime DATETIME NOT NULL,
    EndTime DATETIME,
    ErrorMessage TEXT,
    CreationTime DATETIME NOT NULL,
    INDEX idx_cohort_migration_cohort (CohortId),
    INDEX idx_cohort_migration_department (SourceDepartmentId, TargetDepartmentId),
    INDEX idx_cohort_migration_status (Status),
    INDEX idx_cohort_migration_tenant (TenantId)
);
```

#### 4.2 Data Integrity Constraints

Ensure the following constraints are maintained:
- Cohort can only belong to one department
- All cohort users maintain their record states
- Compliance calculations remain accurate
- Audit trail for all migration operations
- Rollback capability for failed migrations

### Phase 5: Enhanced Migration Script

#### 5.1 Update Existing Migration Script

**File**: `sql/CohortMove/EnhancedCohortMigration.sql`

This script will use the new cohort migration functionality:

```sql
-- Use the new cohort migration service for proper data migration
-- This replaces the manual SQL approach with service-based migration

-- Step 1: Analyze migration requirements
CALL AnalyzeCohortMigration(@cohort_id, @target_department_id);

-- Step 2: Execute migration with proper mappings
CALL MigrateCohort(@migration_dto);

-- Step 3: Verify migration results
CALL ValidateMigrationResults(@cohort_id);
```

### Phase 6: Testing Strategy

#### 6.1 Unit Tests

**Files**:
- `test/inzibackend.Tests/Surpath/CohortMigrationAppService_Tests.cs`
- `test/inzibackend.Tests/Surpath/CohortMigrationValidation_Tests.cs`
- `test/inzibackend.Tests/Surpath/RequirementCategoryMapping_Tests.cs`

Test scenarios:
- Migrate cohort to existing department
- Migrate cohort to new department
- Requirement category mapping (all actions)
- Compliance state preservation
- Error handling and rollback
- Multi-tenant migration scenarios

#### 6.2 Integration Tests

**File**: `test/inzibackend.Tests/Surpath/CohortMigration_Integration_Tests.cs`

Test scenarios:
- End-to-end cohort migration workflow
- Database transaction integrity
- Compliance calculation after migration
- Audit trail verification
- Rollback functionality

#### 6.3 UI Tests

**File**: `ui-tests/tests/mvc/CohortMigration_Tests.js`

Test scenarios:
- Cohort migration wizard workflow
- Department selection step
- Requirement mapping step
- Migration confirmation and execution
- Error handling and validation

### Phase 7: Documentation Updates

#### 7.1 User Documentation

**File**: `docs/cohort-migration-user-guide.md`

Content:
- How to migrate cohorts between departments
- Understanding requirement category mapping
- Best practices for migration planning
- Troubleshooting common issues

#### 7.2 Technical Documentation

**File**: `docs/cohort-migration-technical-guide.md`

Content:
- API documentation for cohort migration service
- Database schema implications
- Integration with compliance system
- Migration audit and rollback procedures

## Implementation Timeline

### Step 1: Backend Infrastructure (2-3 weeks)
- Create DTOs and interfaces
- Implement core CohortMigrationAppService
- Add permissions and authorization
- Unit tests for core functionality
- Database schema updates

### Step 2: Frontend Development (2-3 weeks)
- Create wizard modal components
- Implement step-by-step workflow
- JavaScript functionality for wizard
- Update controller actions
- Basic UI testing

### Step 3: Integration and Testing (1-2 weeks)
- Integration tests
- End-to-end testing
- Performance testing
- Bug fixes and refinements

### Step 4: Documentation and Deployment (1 week)
- User and technical documentation
- Migration script updates
- Deployment preparation
- Final testing and validation

## Risk Mitigation

### Data Integrity Risks
- **Risk**: Cohort migration could break existing record states or compliance
- **Mitigation**: Comprehensive validation, transaction rollback, audit trails

### Performance Risks
- **Risk**: Large cohort migrations could impact system performance
- **Mitigation**: Background job processing, progress tracking, chunked operations

### User Experience Risks
- **Risk**: Complex wizard could confuse administrators
- **Mitigation**: Clear step-by-step guidance, validation, preview functionality

### Compliance Risks
- **Risk**: Migration could affect compliance calculations
- **Mitigation**: Compliance state preservation, automatic recalculation, validation

## Success Criteria

1. **Functional**: Administrators can successfully migrate cohorts between departments
2. **Data Integrity**: All operations maintain referential integrity and compliance states
3. **User Experience**: Intuitive wizard interface with clear guidance and validation
4. **Performance**: Migrations complete within acceptable time limits
5. **Auditability**: Complete audit trail for all migration operations
6. **Rollback**: Ability to rollback failed or incorrect migrations

## Future Enhancements

1. **Bulk Migration**: Migrate multiple cohorts simultaneously
2. **Migration Templates**: Pre-defined mapping templates for common scenarios
3. **Advanced Analytics**: Migration impact analysis and reporting
4. **API Integration**: REST API for external systems to trigger migrations
5. **Automated Mapping**: AI-powered suggestion for requirement category mapping

## Notes for Implementation

### Check ragdocs, memory, and rules
- Review existing documentation for similar patterns
- Check for established conventions in the codebase
- Follow existing architectural patterns

### Update rules as learning new patterns
- Document new implementation strategies discovered
- Update coding standards based on learnings
- Create reusable patterns for future features

### Check other code for ASP.NET Zero implementation patterns
- Review existing modal implementations
- Study DataTable patterns used in the application
- Follow established service and controller patterns

### Remember ASP.NET Zero modal patterns
- Use established modal manager patterns
- Follow existing wizard implementations if any
- Maintain consistency with existing UI components

### Follow existing DataTable patterns
- Use the specific ASP.NET Zero DataTable implementation
- Follow established column definition patterns
- Maintain consistency with existing table implementations

### Remember abp.services.app pattern
- Application services are available via abp.services.app.<serviceName>
- Direct controller calls are typically for modals and UI operations
- Follow established patterns for service communication

This plan provides a comprehensive approach to implementing cohort migration functionality that will solve the current manual migration issues and provide a robust, user-friendly system for future cohort management needs. 