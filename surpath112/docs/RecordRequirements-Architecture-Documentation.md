# RecordRequirements Architecture Documentation

## Overview
RecordRequirements is a core component of the Surpath compliance management system that defines what documents or records users must provide to meet compliance standards. This document provides a comprehensive analysis of how RecordRequirements are implemented and used throughout the codebase.

## Architecture Overview

### Entity Relationship Diagram
```
RecordRequirement (1) -----> (*) RecordCategory
                                      |
                                      v
                                 RecordCategoryRule
                                      |
                                      v
                                 RecordState <---- Record
                                      |
                                      v
                                 RecordStatus
```

## Core Components

### 1. Domain Entity
**Location**: `/src/inzibackend.Core/Surpath/RecordRequirement.cs`

```csharp
public class RecordRequirement : FullAuditedEntity<Guid>, IMayHaveTenant
{
    public string Name { get; set; }              // Required field
    public string Description { get; set; }
    public string Metadata { get; set; }
    public bool IsSurpathOnly { get; set; }
    public Guid? TenantDepartmentId { get; set; }
    public Guid? CohortId { get; set; }
    public Guid? SurpathServiceId { get; set; }
    public Guid? TenantSurpathServiceId { get; set; }
    public int? TenantId { get; set; }
}
```

**Key Features**:
- Inherits from `FullAuditedEntity<Guid>` providing audit fields (CreationTime, CreatorUserId, etc.)
- Implements `IMayHaveTenant` for multi-tenancy support
- Can be scoped to specific departments, cohorts, or services
- Supports metadata storage for additional configuration

### 2. Database Schema
**Table**: `RecordRequirements`

**Relationships**:
- One-to-Many with `RecordCategory` (via `RecordRequirementId` FK)
- Optional FK to `TenantDepartment`
- Optional FK to `Cohort`
- Optional FK to `SurpathService` (Note: FK constraint removed in migration)
- Optional FK to `TenantSurpathService` (Note: FK constraint removed in migration)

### 3. Application Service Layer
**Location**: `/src/inzibackend.Application/Surpath/RecordRequirement/RecordRequirementsAppService.cs`

**Main Operations**:
```csharp
public interface IRecordRequirementsAppService
{
    Task<PagedResultDto<GetRecordRequirementForViewDto>> GetAll(GetAllRecordRequirementsInput input);
    Task<GetRecordRequirementForViewDto> GetRecordRequirementForView(Guid id);
    Task<GetRecordRequirementForEditOutput> GetRecordRequirementForEdit(EntityDto<Guid> input);
    Task CreateOrEdit(CreateOrEditRecordRequirementDto input);
    Task Delete(EntityDto<Guid> input);
}
```

**Security**:
- Protected by permission: `AppPermissions.Pages_Administration_RecordRequirements`
- All operations require appropriate authorization

### 4. Data Transfer Objects (DTOs)
**Location**: `/src/inzibackend.Application.Shared/Surpath/RecordRequirement/Dtos/`

**Key DTOs**:
- `RecordRequirementDto`: Main DTO with navigation properties
- `CreateOrEditRecordRequirementDto`: Used for create/update operations
- `GetRecordRequirementForViewDto`: View model for display
- `GetAllRecordRequirementsInput`: Query parameters for filtering

### 5. Web Layer
**Controller**: `/src/inzibackend.Web.Mvc/Areas/App/Controllers/RecordRequirementsController.cs`

**Views**:
- Index: Lists all record requirements with DataTable
- CreateOrEditModal: Form for managing requirements
- ViewRecordRequirementModal: Read-only view
- CategoryModal: Manage associated categories
- MoveCategoryModal: Reorder categories

**JavaScript**:
- Uses jQuery DataTables for listing
- AJAX calls for CRUD operations
- Modal dialogs for editing

## Business Logic Flow

### 1. Requirement Definition
Administrators define RecordRequirements specifying:
- What documents are needed
- Which department/cohort they apply to
- Associated service (if any)
- Whether it's Surpath-only

### 2. Category Association
Each requirement has one or more RecordCategories that:
- Define specific document types
- Set expiration rules via RecordCategoryRule
- Determine compliance criteria

### 3. Document Upload Process
When users upload documents:
1. Document is created as a `Record` entity
2. Associated with appropriate `RecordCategory`
3. Given initial `RecordState`
4. Status tracked via `RecordStatus`

### 4. Compliance Evaluation
**Location**: `/src/inzibackend.Application/Surpath/ComplianceManager/SurpathComplianceEvaluator.cs`

The compliance evaluator:
1. Queries all applicable RecordRequirements for a user
2. Joins with RecordCategory and RecordCategoryRule
3. Checks if user has valid (non-archived) records
4. Evaluates expiration rules
5. Calculates overall compliance percentage

**Key Query Logic**:
```csharp
var query = from rr in _recordRequirementRepository.GetAll()
            join rc in _recordCategoryRepository.GetAll() on rr.Id equals rc.RecordRequirementId
            join rcr in _recordCategoryRuleRepository.GetAll() on rc.Id equals rcr.RecordCategoryId
            join rs in _recordStateRepository.GetAll() on rc.Id equals rs.RecordCategoryId
            join rst in _recordStatusRepository.GetAll() on rs.RecordStatusId equals rst.Id
            where !rs.IsArchived && rs.UserId == userId
            select new { ... };
```

### 5. Archiving Strategy
- When new documents are uploaded, previous versions are marked as `IsArchived = true`
- Only non-archived records count toward compliance
- Maintains full audit trail of all document versions

## Integration Points

### 1. Multi-Tenancy
- Requirements can be tenant-specific via `TenantId`
- Queries automatically filter by current tenant
- Cross-tenant requirements possible with `IsSurpathOnly` flag

### 2. Department/Cohort Scoping
- Requirements can target specific organizational units
- Inherited through user's department/cohort assignment
- Flexible scoping rules

### 3. Service Integration
- Can link requirements to specific services
- Allows service-specific compliance tracking
- Note: Direct FK constraints removed for flexibility

## Best Practices

### 1. Creating Requirements
- Always provide clear, descriptive names
- Use Description field for detailed instructions
- Consider scope (tenant, department, cohort) carefully
- Use Metadata for additional configuration

### 2. Category Management
- Create specific categories for different document types
- Set appropriate expiration rules
- Consider document renewal cycles

### 3. Compliance Tracking
- Regular evaluation of user compliance
- Monitor archived vs. active documents
- Track expiration dates proactively

### 4. Performance Considerations
- Index on frequently queried fields (UserId, IsArchived)
- Consider pagination for large requirement sets
- Cache commonly accessed requirement data

## Common Use Cases

### 1. Employee Onboarding
Create requirements for:
- Identity documents
- Educational certificates
- Background checks
- Department-specific documents

### 2. Annual Compliance
Requirements for:
- License renewals
- Training certificates
- Health clearances
- Policy acknowledgments

### 3. Service-Specific Requirements
Requirements tied to specific services:
- Service agreements
- Specialized certifications
- Access authorizations

## Migration History
- Initial implementation with basic fields
- Added service relationships
- Removed direct FK constraints for flexibility (migration: `20230212120321_remove-fk-record-requirement`)
- Enhanced with archiving support

## Future Considerations
1. **Bulk Operations**: Consider adding bulk requirement creation/update
2. **Templates**: Requirement templates for common scenarios
3. **Notifications**: Automated alerts for expiring documents
4. **Reporting**: Enhanced compliance reporting capabilities
5. **API Exposure**: RESTful API for external integrations

## Conclusion
RecordRequirements provide a flexible, scalable foundation for document compliance management. The architecture supports multi-tenancy, organizational scoping, and comprehensive audit trails while maintaining performance and usability.