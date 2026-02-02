# Entity Relationships Documentation

## Overview
This document provides a comprehensive overview of the entity relationships in the Surpath compliance management system. Understanding these relationships is crucial for implementing features like cohort migration and member transfers.

## Core Entities

### 1. Cohort
**Purpose**: Represents a group of users within a department who share common compliance requirements.

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `Name` (string): Cohort name
- `Description` (string): Cohort description
- `DefaultCohort` (bool): Indicates if this is the default cohort
- `TenantDepartmentId` (Guid?): Foreign key to TenantDepartment

**Navigation Properties**:
- `TenantDepartmentFk`: References the TenantDepartment entity

**Relationships**:
- Many-to-One with TenantDepartment (a cohort belongs to one department)
- One-to-Many with CohortUser (a cohort has many members)

### 2. CohortUser
**Purpose**: Junction table that represents the membership of a User in a Cohort.

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `CohortId` (Guid?): Foreign key to Cohort
- `UserId` (long): Foreign key to User

**Navigation Properties**:
- `CohortFk`: References the Cohort entity
- `UserFk`: References the User entity

**Relationships**:
- Many-to-One with Cohort
- Many-to-One with User
- Acts as a many-to-many relationship between Users and Cohorts

**Important Notes**:
- Users can belong to multiple cohorts simultaneously
- Each CohortUser record represents one membership relationship
- When transferring members, we UPDATE the CohortId rather than creating new records

### 3. User (AbpUser)
**Purpose**: Represents a system user with login credentials and personal information.

**Key Properties**:
- `Id` (long): Primary key
- Standard ABP user properties (UserName, EmailAddress, Name, Surname, etc.)

**Relationships**:
- One-to-Many with CohortUser (a user can be in multiple cohorts)
- One-to-Many with RecordState (a user has many compliance records)
- One-to-Many with TenantDepartmentUser (a user can be in multiple departments)

### 4. TenantDepartment
**Purpose**: Represents a department within a tenant organization.

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `Name` (string): Department name (required)
- `Description` (string): Department description
- `Active` (bool): Whether the department is active
- `MROType` (EnumClientMROTypes): Type of MRO (None=0, MPOS=1, MALL=2)
- `OrganizationUnitId` (long): Required link to ABP Organization Unit

**Relationships**:
- One-to-Many with Cohort (a department has many cohorts)
- One-to-Many with RecordRequirement (a department defines many requirements)
- One-to-Many with TenantDepartmentUser (a department has many users)

### 5. TenantDepartmentUser
**Purpose**: Junction table that links Users to TenantDepartments.

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `UserId` (long): Foreign key to User
- `TenantDepartmentId` (Guid): Foreign key to TenantDepartment

**Navigation Properties**:
- `UserFk`: References the User entity
- `TenantDepartmentFk`: References the TenantDepartment entity

**Relationships**:
- Many-to-One with User
- Many-to-One with TenantDepartment
- Acts as a many-to-many relationship between Users and Departments

### 6. RecordRequirement
**Purpose**: Defines a compliance requirement that must be met by users in a department.

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `Name` (string): Requirement name
- `Description` (string): Requirement description
- `TenantDepartmentId` (Guid?): Foreign key to TenantDepartment
- `CohortId` (Guid?): Foreign key to Cohort (for cohort-specific requirements)
- `IsSurpathOnly` (bool): Indicates if this is a Surpath-specific requirement

**Navigation Properties**:
- `TenantDepartmentFk`: References the TenantDepartment entity
- `CohortFk`: References the Cohort entity

**Relationships**:
- Many-to-One with TenantDepartment
- Many-to-One with Cohort (optional)
- One-to-Many with RecordCategory

### 7. RecordCategory
**Purpose**: Represents a specific category within a requirement (e.g., "TB Test" under "Health Screening").

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `Name` (string): Category name
- `Instructions` (string): Instructions for this category
- `RecordRequirementId` (Guid?): Foreign key to RecordRequirement
- `RecordCategoryRuleId` (Guid?): Foreign key to RecordCategoryRule

**Navigation Properties**:
- `RecordRequirementFk`: References the RecordRequirement entity
- `RecordCategoryRuleFk`: References the RecordCategoryRule entity

**Relationships**:
- Many-to-One with RecordRequirement
- Many-to-One with RecordCategoryRule (optional)
- One-to-Many with RecordState

### 8. RecordState
**Purpose**: Tracks the compliance state of a specific record/document for a user in a category.

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `State` (EnumRecordState): NeedsApproval, Rejected, or Approved
- `Notes` (string): Additional notes
- `RecordId` (Guid?): Foreign key to Record (the actual document)
- `RecordCategoryId` (Guid?): Foreign key to RecordCategory
- `UserId` (long?): Foreign key to User
- `RecordStatusId` (Guid): Foreign key to RecordStatus (required)

**Navigation Properties**:
- `RecordFk`: References the Record entity
- `RecordCategoryFk`: References the RecordCategory entity
- `UserFk`: References the User entity
- `RecordStatusFk`: References the RecordStatus entity

**Relationships**:
- Many-to-One with Record (optional - the uploaded document)
- Many-to-One with RecordCategory
- Many-to-One with User
- Many-to-One with RecordStatus
- One-to-Many with RecordNote

### 9. Record
**Purpose**: Represents an uploaded document or record file.

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `filename` (string): The file name
- `TenantDocumentCategoryId` (Guid?): Category ID
- `fileSize` (long): File size in bytes
- `ExpirationDate` (DateTime?): When the record expires
- Various other file-related properties

**Relationships**:
- One-to-Many with RecordState (one document can have multiple states)

### 10. RecordStatus
**Purpose**: Defines the status options for records with compliance impact.

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `StatusName` (string): Name of the status
- `ComplianceImpact` (EnumStatusComplianceImpact): Compliant, NotCompliant, or InformationOnly

**Relationships**:
- One-to-Many with RecordState

### 11. RecordNote
**Purpose**: Stores notes and audit information for record states.

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `Note` (string): The note content (required)
- `Created` (DateTime): When the note was created
- `AuthorizedOnly` (bool): If only authorized users can see this
- `HostOnly` (bool): If only host users can see this
- `SendNotification` (bool): Whether to send a notification
- `RecordStateId` (Guid?): Foreign key to RecordState
- `UserId` (long?): Foreign key to User who created the note
- `NotifyUserId` (long?): Foreign key to User to notify

**Navigation Properties**:
- `RecordStateFk`: References the RecordState entity
- `UserFk`: References the User entity (creator)
- `NotifyUserFk`: References the User entity (to notify)

**Relationships**:
- Many-to-One with RecordState
- Many-to-One with User (creator)
- Many-to-One with User (notify)

### 12. TenantSurpathService
**Purpose**: Represents a Surpath service configured for a specific tenant with hierarchical pricing and enablement.

**Key Properties**:
- `Id` (Guid): Primary key
- `TenantId` (int?): Multi-tenancy identifier
- `Name` (string): Service name (required)
- `Price` (double): Service price
- `Description` (string): Service description
- `IsEnabled` (bool): Whether this service is enabled
- `SurpathServiceId` (Guid?): Foreign key to base SurpathService
- `TenantDepartmentId` (Guid?): Foreign key to TenantDepartment (for department-level pricing)
- `CohortId` (Guid?): Foreign key to Cohort (for cohort-level pricing)
- `UserId` (long?): Foreign key to User (for user-specific pricing)
- `CohortUserId` (Guid?): Foreign key to CohortUser (for cohort-user-specific pricing)
- `RecordCategoryRuleId` (Guid?): Foreign key to RecordCategoryRule
- `OrganizationUnitId` (long?): Foreign key to OrganizationUnit

**Navigation Properties**:
- `SurpathServiceFk`: References the base SurpathService entity
- `TenantDepartmentFk`: References the TenantDepartment entity
- `CohortFk`: References the Cohort entity
- `UserFk`: References the User entity
- `CohortUserFk`: References the CohortUser entity
- `RecordCategoryRuleFk`: References the RecordCategoryRule entity
- `OrganizationUnitFk`: References the OrganizationUnit entity

**Relationships**:
- Many-to-One with SurpathService (base service definition)
- Many-to-One with TenantDepartment (optional - for department-level assignment)
- Many-to-One with Cohort (optional - for cohort-level assignment)
- Many-to-One with User (optional - for user-specific assignment)
- Many-to-One with CohortUser (optional - for cohort-user-specific assignment)
- Many-to-One with RecordCategoryRule (optional)
- Many-to-One with OrganizationUnit (optional)

**Hierarchical Assignment**:
TenantSurpathService supports hierarchical pricing and enablement:
- **Tenant-wide**: No TenantDepartmentId, CohortId, or UserId (applies to all users in tenant)
- **Department level**: Has TenantDepartmentId (applies to all users in department)
- **Cohort level**: Has CohortId (applies to all users in cohort)
- **User level**: Has UserId or CohortUserId (applies to specific user)

**Important Notes**:
- Services inherit through the hierarchy (tenant → department → cohort → user)
- The `IsEnabled` property controls whether the service is active at that level
- Pricing can be overridden at each level of the hierarchy
- Compliance requirements can be linked to specific TenantSurpathServices via RecordRequirement.TenantSurpathServiceId

## Entity Relationship Diagram (Textual)

```
TenantDepartment (1) ──── (*) Cohort
TenantDepartment (1) ──── (*) RecordRequirement
TenantDepartment (1) ──── (*) TenantDepartmentUser ──── (1) User
TenantDepartment (1) ──── (*) TenantSurpathService

Cohort (1) ──── (*) CohortUser ──── (1) User
Cohort (1) ──── (*) RecordRequirement (optional)
Cohort (1) ──── (*) TenantSurpathService

RecordRequirement (1) ──── (*) RecordCategory
RecordRequirement (*) ──── (0..1) TenantSurpathService
RecordCategory (1) ──── (*) RecordState
RecordCategory (1) ──── (0..1) RecordCategoryRule

RecordState (*) ──── (1) User
RecordState (*) ──── (1) RecordStatus
RecordState (*) ──── (0..1) Record
RecordState (1) ──── (*) RecordNote

RecordNote (*) ──── (1) User (creator)
RecordNote (*) ──── (0..1) User (notify)

TenantSurpathService (*) ──── (1) SurpathService
TenantSurpathService (*) ──── (0..1) TenantDepartment
TenantSurpathService (*) ──── (0..1) Cohort
TenantSurpathService (*) ──── (0..1) User
TenantSurpathService (*) ──── (0..1) CohortUser
TenantSurpathService (*) ──── (0..1) RecordCategoryRule
```

## Important Implementation Notes

### 1. Navigation Property Access
When accessing related entities, you must use the navigation property names exactly as defined:
- Use `cohort.TenantDepartmentFk` NOT `cohort.TenantDepartment`
- Use `recordCategory.RecordRequirementFk` NOT `recordCategory.RecordRequirement`
- Use `cohortUser.CohortFk` and `cohortUser.UserFk` for navigation

### 2. Collection Navigation
Entities do NOT have collection navigation properties defined in the entity classes. To get related collections, you must query the repository:
```csharp
// WRONG: cohort.CohortUsers (this property doesn't exist)
// RIGHT: 
var cohortUsers = await _cohortUserRepository.GetAll()
    .Where(cu => cu.CohortId == cohortId)
    .ToListAsync();
```

### 3. Include Statements
When using Entity Framework Include statements, use the actual navigation property names:
```csharp
var recordStates = await _recordStateRepository.GetAll()
    .Include(rs => rs.RecordCategoryFk)
    .ThenInclude(rc => rc.RecordRequirementFk)
    .ToListAsync();
```

### 4. Member Transfer Considerations
- Users can belong to multiple cohorts simultaneously
- When transferring members between cohorts, UPDATE existing CohortUser records
- Do NOT create new CohortUser records unless adding to an additional cohort
- Preserve all compliance data (RecordStates, RecordNotes) during transfers

### 5. Department Migration Considerations
- When migrating cohorts between departments, update the Cohort's TenantDepartmentId
- Update TenantDepartmentUser associations for all affected users
- Map RecordCategories between departments (may require user input)
- Recalculate compliance after migration

### 6. Data Integrity Rules
- RecordState requires a RecordStatusId (not nullable)
- RecordNote requires Note content (not nullable)
- TenantDepartment requires Name and OrganizationUnitId
- All entities support multi-tenancy through TenantId

## Common Query Patterns

### Get all users in a cohort:
```csharp
var users = await _cohortUserRepository.GetAll()
    .Include(cu => cu.UserFk)
    .Where(cu => cu.CohortId == cohortId)
    .Select(cu => cu.UserFk)
    .ToListAsync();
```

### Get all cohorts in a department:
```csharp
var cohorts = await _cohortRepository.GetAll()
    .Where(c => c.TenantDepartmentId == departmentId)
    .ToListAsync();
```

### Get all record states for a user:
```csharp
var recordStates = await _recordStateRepository.GetAll()
    .Include(rs => rs.RecordCategoryFk)
    .ThenInclude(rc => rc.RecordRequirementFk)
    .Where(rs => rs.UserId == userId)
    .ToListAsync();
```

### Check if user is in a department:
```csharp
var isInDepartment = await _tenantDepartmentUserRepository.GetAll()
    .AnyAsync(tdu => tdu.UserId == userId && tdu.TenantDepartmentId == departmentId);
```