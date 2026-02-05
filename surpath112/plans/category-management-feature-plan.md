# Category Management Feature Implementation Plan

## Overview

This plan outlines the implementation of a comprehensive category management system that allows administrators to:
1. Move categories from one requirement to another (existing or new)
2. Copy categories from one requirement to another
3. Handle requirement deletion when the last category is moved
4. Provide warnings and confirmations for destructive operations

## Problem Statement

Currently, when migrating cohorts between departments, categories cannot be easily reorganized. For example, the "General Immunizations" requirement in ADN has multiple categories, but in LVN-RN Bridge, each immunization is its own requirement with a single category. This creates data inconsistencies and compliance calculation issues.

## Solution Architecture

### 1. Core Components

#### A. Category Management Service
- **File**: `src/inzibackend.Application/Surpath/CategoryManagementAppService.cs`
- **Interface**: `src/inzibackend.Application/Surpath/ICategoryManagementAppService.cs`
- **Purpose**: Handle all category move/copy operations with business logic validation

#### B. Category Management DTOs
- **File**: `src/inzibackend.Application.Shared/Surpath/Dtos/CategoryManagementDtos.cs`
- **Purpose**: Define data transfer objects for category operations

#### C. Category Management UI Components
- **Files**: 
  - `src/inzibackend.Web.Mvc/Areas/App/Views/RecordRequirements/_CategoryManagementModal.cshtml`
  - `src/inzibackend.Web.Mvc/Areas/App/Views/RecordRequirements/_MoveCategoryModal.cshtml`
  - `src/inzibackend.Web.Mvc/Areas/App/Views/RecordRequirements/_CopyCategoryModal.cshtml`
- **Purpose**: Provide user interfaces for category management operations

#### D. JavaScript Controllers
- **Files**:
  - `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/RecordRequirements/categoryManagement.js`
  - Updates to existing `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/RecordRequirements/index.js`

## Detailed Implementation Plan

### Phase 1: Backend Infrastructure

#### 1.1 Create Category Management DTOs

**File**: `src/inzibackend.Application.Shared/Surpath/Dtos/CategoryManagementDtos.cs`

```csharp
// Move Category DTOs
public class MoveCategoryDto
{
    public Guid CategoryId { get; set; }
    public Guid? TargetRequirementId { get; set; } // null if creating new requirement
    public string NewRequirementName { get; set; } // required if TargetRequirementId is null
    public string NewRequirementDescription { get; set; }
    public bool ConfirmRequirementDeletion { get; set; } // for when source requirement becomes empty
}

public class MoveCategoryResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public bool RequirementDeleted { get; set; }
    public Guid? NewRequirementId { get; set; }
    public List<string> Warnings { get; set; }
}

// Copy Category DTOs
public class CopyCategoryDto
{
    public List<Guid> CategoryIds { get; set; }
    public Guid TargetRequirementId { get; set; }
    public bool CopyRecordStates { get; set; } // option to copy existing user record states
}

public class CopyCategoryResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public List<Guid> NewCategoryIds { get; set; }
    public int RecordStatesCopied { get; set; }
}

// Validation DTOs
public class CategoryMoveValidationDto
{
    public bool CanMove { get; set; }
    public bool WillDeleteSourceRequirement { get; set; }
    public int AffectedRecordStatesCount { get; set; }
    public int AffectedUsersCount { get; set; }
    public List<string> Warnings { get; set; }
    public List<string> Errors { get; set; }
}

// Lookup DTOs
public class RequirementCategoryLookupDto
{
    public Guid RequirementId { get; set; }
    public string RequirementName { get; set; }
    public List<CategoryLookupDto> Categories { get; set; }
}

public class CategoryLookupDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int RecordStatesCount { get; set; }
    public bool IsOnlyCategory { get; set; }
}
```

#### 1.2 Create Category Management Service Interface

**File**: `src/inzibackend.Application/Surpath/ICategoryManagementAppService.cs`

```csharp
public interface ICategoryManagementAppService : IApplicationService
{
    // Validation methods
    Task<CategoryMoveValidationDto> ValidateCategoryMove(Guid categoryId, Guid? targetRequirementId);
    
    // Move operations
    Task<MoveCategoryResultDto> MoveCategory(MoveCategoryDto input);
    Task<MoveCategoryResultDto> MoveCategoryToNewRequirement(MoveCategoryDto input);
    Task<MoveCategoryResultDto> MoveCategoryToExistingRequirement(MoveCategoryDto input);
    
    // Copy operations
    Task<CopyCategoryResultDto> CopyCategories(CopyCategoryDto input);
    
    // Lookup methods
    Task<List<RequirementCategoryLookupDto>> GetRequirementsWithCategories(int? tenantId = null);
    Task<List<RecordRequirementDto>> GetAvailableTargetRequirements(Guid excludeRequirementId);
    
    // Utility methods
    Task<bool> CanDeleteRequirement(Guid requirementId);
    Task<int> GetAffectedRecordStatesCount(Guid categoryId);
    Task<int> GetAffectedUsersCount(Guid categoryId);
}
```

#### 1.3 Implement Category Management Service

**File**: `src/inzibackend.Application/Surpath/CategoryManagementAppService.cs`

Key methods to implement:

1. **ValidateCategoryMove**: Check if move is possible and what the impact will be
2. **MoveCategory**: Main orchestration method that routes to specific move operations
3. **MoveCategoryToNewRequirement**: Create new requirement and move category
4. **MoveCategoryToExistingRequirement**: Move category to existing requirement
5. **CopyCategories**: Duplicate categories and optionally copy record states
6. **Transaction Management**: Ensure all operations are atomic
7. **Audit Logging**: Track all category management operations

#### 1.4 Update Permissions

**File**: `src/inzibackend.Core/Authorization/AppPermissions.cs`

```csharp
// Add new permissions
public const string Pages_RecordRequirements_ManageCategories = "Pages.RecordRequirements.ManageCategories";
public const string Pages_RecordRequirements_MoveCategories = "Pages.RecordRequirements.MoveCategories";
public const string Pages_RecordRequirements_CopyCategories = "Pages.RecordRequirements.CopyCategories";
public const string Pages_RecordRequirements_DeleteRequirements = "Pages.RecordRequirements.DeleteRequirements";
```

**File**: `src/inzibackend.Core/Authorization/AppAuthorizationProvider.cs`

Add permission definitions and hierarchy.

### Phase 2: Frontend Implementation

#### 2.1 Update Record Requirements Index Page

**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/RecordRequirements/Index.cshtml`

Add new action buttons:
- "Manage Categories" button for each requirement
- "Copy Categories" button in the create/edit requirement modal

#### 2.2 Create Category Management Modal

**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/RecordRequirements/_CategoryManagementModal.cshtml`

Features:
- List all categories for the selected requirement
- Move category buttons with dropdown for target requirement
- "Create New Requirement" option
- Copy category functionality
- Validation warnings and confirmations

#### 2.3 Create Move Category Modal

**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/RecordRequirements/_MoveCategoryModal.cshtml`

Features:
- Category selection (source)
- Target requirement dropdown
- "Create New Requirement" option with name input
- Impact summary (affected users, record states)
- Confirmation checkboxes for destructive operations

#### 2.4 Create Copy Category Modal

**File**: `src/inzibackend.Web.Mvc/Areas/App/Views/RecordRequirements/_CopyCategoryModal.cshtml`

Features:
- Source requirement selection
- Multiple category selection (if source has multiple categories)
- Target requirement selection
- Option to copy existing record states
- Preview of what will be copied

#### 2.5 JavaScript Implementation

**File**: `src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/RecordRequirements/categoryManagement.js`

Key functions:
- `showCategoryManagementModal(requirementId)`
- `showMoveCategoryModal(categoryId)`
- `showCopyCategoryModal()`
- `validateCategoryMove(categoryId, targetRequirementId)`
- `executeCategoryMove(moveData)`
- `executeCategoryCopy(copyData)`
- `refreshRequirementsList()`

### Phase 3: Controller Updates

#### 3.1 Update RecordRequirements Controller

**File**: `src/inzibackend.Web.Mvc/Areas/App/Controllers/RecordRequirementsController.cs`

Add new action methods:
- `CategoryManagementModal(Guid requirementId)`
- `MoveCategoryModal(Guid categoryId)`
- `CopyCategoryModal()`
- `ValidateCategoryMove(Guid categoryId, Guid? targetRequirementId)`
- `MoveCategory(MoveCategoryDto input)`
- `CopyCategories(CopyCategoryDto input)`

### Phase 4: Database Considerations

#### 4.1 Potential Schema Updates

While the existing schema should support this functionality, consider adding:

**Optional Enhancement**: Add audit table for category management operations
```sql
CREATE TABLE CategoryManagementAudit (
    Id CHAR(36) PRIMARY KEY,
    OperationType VARCHAR(50), -- 'MOVE', 'COPY', 'DELETE'
    SourceRequirementId CHAR(36),
    TargetRequirementId CHAR(36),
    CategoryId CHAR(36),
    UserId BIGINT,
    TenantId INT,
    OperationData JSON, -- Store full operation details
    CreationTime DATETIME,
    INDEX idx_category_audit_category (CategoryId),
    INDEX idx_category_audit_requirement (SourceRequirementId, TargetRequirementId)
);
```

#### 4.2 Data Integrity Constraints

Ensure the following constraints are maintained:
- Every requirement must have at least one category
- Category names must be unique within a requirement
- Record states must reference valid categories
- Audit trail for all category operations

### Phase 5: Migration Script Enhancement

#### 5.1 Create Category Migration Helper

**File**: `sql/CohortMove/CategoryMigrationHelper.sql`

This script will use the new category management functionality to properly handle the ADN to LVN-RN Bridge migration:

```sql
-- Use the new category management functionality to properly migrate categories
-- Instead of updating recordstates directly, use the category management service

-- Step 1: Move "Hep B (Titer)" categories to new requirement
CALL MoveCategoryToNewRequirement(@old_hepb_titer_cor_cat, 'Hepatitis B (Titer)');
CALL MoveCategoryToExistingRequirement(@old_hepb_titer_wax_cat, @new_hepb_requirement_id);

-- Step 2: Move other categories as needed
-- This ensures proper data integrity and compliance calculation
```

### Phase 6: Testing Strategy

#### 6.1 Unit Tests

**Files**:
- `test/inzibackend.Tests/Surpath/CategoryManagementAppService_Tests.cs`
- `test/inzibackend.Tests/Surpath/CategoryMoveValidation_Tests.cs`

Test scenarios:
- Move category to existing requirement
- Move category to new requirement
- Copy categories with and without record states
- Validation of destructive operations
- Error handling for invalid operations

#### 6.2 Integration Tests

**File**: `test/inzibackend.Tests/Surpath/CategoryManagement_Integration_Tests.cs`

Test scenarios:
- End-to-end category move operations
- Database transaction integrity
- Compliance calculation after category moves
- Multi-tenant category operations

#### 6.3 UI Tests

**File**: `ui-tests/tests/mvc/CategoryManagement_Tests.js`

Test scenarios:
- Category management modal functionality
- Move category workflow
- Copy category workflow
- Validation and error handling

### Phase 7: Documentation Updates

#### 7.1 User Documentation

**File**: `docs/category-management-user-guide.md`

Content:
- How to move categories between requirements
- How to copy categories
- Understanding the impact of category operations
- Best practices for requirement organization

#### 7.2 Technical Documentation

**File**: `docs/category-management-technical-guide.md`

Content:
- API documentation for category management service
- Database schema implications
- Integration with compliance system
- Troubleshooting guide

## Implementation Timeline

### Step 1: Backend Infrastructure
- Create DTOs and interfaces
- Implement core CategoryManagementAppService
- Add permissions and authorization
- Unit tests for core functionality

### Step 2: Frontend Development
- Create modal components
- Implement JavaScript functionality
- Update controller actions
- Basic UI testing

### Step 3: Integration and Testing
- Integration tests
- End-to-end testing
- Performance testing
- Bug fixes and refinements

### Step 4: Documentation and Deployment
- User and technical documentation
- Migration script updates
- Deployment preparation
- Final testing and validation

## Risk Mitigation

### Data Integrity Risks
- **Risk**: Category moves could break existing record states
- **Mitigation**: Comprehensive validation before operations, transaction rollback on errors

### Performance Risks
- **Risk**: Large-scale category operations could impact system performance
- **Mitigation**: Batch processing, background jobs for large operations

### User Experience Risks
- **Risk**: Complex UI could confuse administrators
- **Mitigation**: Progressive disclosure, clear warnings, undo functionality where possible

### Compliance Risks
- **Risk**: Category changes could affect compliance calculations
- **Mitigation**: Automatic compliance recalculation after category operations

## Success Criteria

1. **Functional**: Administrators can successfully move and copy categories between requirements
2. **Data Integrity**: All operations maintain referential integrity and audit trails
3. **Performance**: Operations complete within acceptable time limits (< 30 seconds for typical operations)
4. **User Experience**: Intuitive interface with clear feedback and error handling
5. **Compliance**: Compliance calculations remain accurate after category operations

## Future Enhancements

1. **Bulk Operations**: Move/copy multiple categories simultaneously
2. **Category Templates**: Pre-defined category sets for common requirements
3. **Category Versioning**: Track historical changes to categories
4. **Advanced Validation**: Machine learning-based suggestions for category organization
5. **API Integration**: REST API for external systems to manage categories

This plan provides a comprehensive approach to implementing category management functionality that will solve the current migration issues and provide long-term flexibility for requirement organization. 