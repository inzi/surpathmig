# Models Documentation

## Overview
This folder contains all view models for the App area of the MVC application. These models serve as data transfer objects between controllers and views, providing strongly-typed data structures for the user interface. The models are organized by feature/entity and follow the MVVM pattern for ASP.NET Core MVC.

## Contents

### Core Model Categories

#### Common/
- **Purpose**: Shared models and interfaces used across features
- **Key Components**:
  - Permission tree models
  - Organization unit models
  - Feature management interfaces
  - Modal view models
- **See**: [Common/CLAUDE.md](Common/CLAUDE.md) for detailed documentation

#### Compliance/
- **Purpose**: Models for compliance tracking and management
- **Key Models**:
  - Compliance record view models
  - Requirement management models
  - Document upload models
  - Status tracking models
- **Features**: Core compliance workflow support

#### Cohorts/
- **Purpose**: Student cohort/group management models
- **Key Models**:
  - CohortViewModel
  - CreateOrEditCohortViewModel
  - GetCohortForEditOutput
- **Features**: Group organization and management

#### CohortUsers/
- **Purpose**: Models for users within cohorts
- **Key Models**:
  - CohortUserViewModel
  - CohortUserAssignmentViewModel
- **Features**: User-cohort associations

### Entity Management Models

#### Records/
- **Purpose**: Compliance record management models
- **Features**: Document tracking, status updates

#### RecordStates/
- **Purpose**: Record workflow state models
- **Features**: State transitions, workflow tracking

#### RecordRequirements/
- **Purpose**: Requirement definition models
- **Features**: Requirement templates, rules

#### RecordCategories/
- **Purpose**: Category management models
- **Features**: Hierarchical categorization

#### RecordNotes/
- **Purpose**: Note and comment models
- **Features**: Record annotations

#### RecordStatuses/
- **Purpose**: Status tracking models
- **Features**: Status definitions

#### RecordCategoryRules/
- **Purpose**: Business rule models for categories
- **Features**: Automated rule processing

### Department & Organization Models

#### TenantDepartments/
- **Purpose**: Department structure models
- **Features**: Department hierarchy

#### DepartmentUsers/
- **Purpose**: Department user assignment models
- **Features**: User-department relationships

#### TenantDepartmentUsers/
- **Purpose**: Tenant-specific department user models
- **Features**: Multi-tenant department management

#### OrganizationUnits/
- **Purpose**: Organizational hierarchy models
- **Features**: Tree structures, unit management

#### DeptCodes/
- **Purpose**: Department code models
- **Features**: Code management and lookup

### Medical & Testing Models

#### DrugTestCategories/
- **Purpose**: Drug test category models
- **Features**: Test type definitions

#### Drugs/
- **Purpose**: Drug information models
- **Features**: Drug database structures

#### DrugPanels/
- **Purpose**: Drug panel configuration models
- **Features**: Panel composition

#### Panels/
- **Purpose**: General panel models
- **Features**: Panel management

#### TestCategories/
- **Purpose**: Test category models
- **Features**: Test classification

#### Hospitals/
- **Purpose**: Hospital/facility models
- **Features**: Healthcare facility data

#### MedicalUnits/
- **Purpose**: Medical unit models
- **Features**: Medical department structures

### User & Authentication Models

#### Users/
- **Purpose**: User management models
- **Key Models**:
  - UserViewModel
  - CreateOrEditUserViewModel
  - UserPermissionsEditViewModel
  - UserLoginAttemptsViewModel
- **Features**: User CRUD, permissions, security

#### Roles/
- **Purpose**: Role and permission models
- **Key Models**:
  - RoleViewModel
  - CreateOrEditRoleViewModel
  - RolePermissionsEditViewModel
- **Features**: Role management, permission trees

#### Profile/
- **Purpose**: User profile models
- **Features**: Personal information, preferences

### Administrative Models

#### Settings/
- **Purpose**: Application settings models
- **Features**: Configuration management

#### HostSettings/
- **Purpose**: Host-level settings models
- **Features**: System-wide configuration

#### Tenants/
- **Purpose**: Tenant management models
- **Features**: Multi-tenant support

#### Editions/
- **Purpose**: Edition/subscription models
- **Features**: Feature sets, pricing tiers

#### AuditLogs/
- **Purpose**: Audit log viewing models
- **Features**: Activity tracking

#### Maintenance/
- **Purpose**: System maintenance models
- **Features**: Cache, logs, system tasks

### Dashboard Models

#### TenantDashboard/
- **Purpose**: Tenant dashboard models
- **Features**: Metrics, widgets, charts

#### HostDashboard/
- **Purpose**: Host dashboard models
- **Features**: System metrics

#### CustomizableDashboard/
- **Purpose**: Customizable dashboard framework
- **Features**: Widget models, personalization

### Financial Models

#### Accounting/
- **Purpose**: Accounting and billing models
- **Features**: Financial tracking

#### LedgerEntries/
- **Purpose**: Ledger entry models
- **Features**: Transaction tracking

#### LedgerEntryDetails/
- **Purpose**: Ledger detail models
- **Features**: Transaction line items

#### Purchase/
- **Purpose**: Purchase management models
- **Features**: Purchasing workflows

#### UserPurchases/
- **Purpose**: User purchase models
- **Features**: User transaction history

### Communication Models

#### Webhooks/
- **Purpose**: Webhook configuration models
- **Features**: Event subscriptions

#### Welcomemessages/
- **Purpose**: Welcome message models
- **Features**: Onboarding messages

### System Models

#### Languages/
- **Purpose**: Language/localization models
- **Features**: Multi-language support

#### DynamicProperty/
- **Purpose**: Dynamic property models
- **Features**: Runtime property definition

#### DynamicEntityProperty/
- **Purpose**: Entity-specific dynamic properties
- **Features**: Custom fields

#### DynamicEntityPropertyValues/
- **Purpose**: Dynamic property value models
- **Features**: Custom field values

#### UiCustomization/
- **Purpose**: UI customization models
- **Features**: Theme settings

#### Layout/
- **Purpose**: Layout configuration models
- **Features**: UI layout settings

### Specialized Models

#### PIDTypes/
- **Purpose**: Patient/Person ID type models
- **Features**: Identifier management

#### UserPids/
- **Purpose**: User PID association models
- **Features**: User identifier tracking

#### CodeTypes/
- **Purpose**: Code type definition models
- **Features**: Code management

#### ConfirmationValues/
- **Purpose**: Confirmation value models
- **Features**: Verification tracking

#### RotationSlots/
- **Purpose**: Rotation scheduling models
- **Features**: Schedule management

#### SurpathServices/
- **Purpose**: Surpath service models
- **Features**: Service definitions

#### TenantSurpathServices/
- **Purpose**: Tenant-specific service models
- **Features**: Service assignments

#### TenantDocuments/
- **Purpose**: Tenant document models
- **Features**: Document management

#### TenantDocumentCategories/
- **Purpose**: Document category models
- **Features**: Document organization

#### LegalDocuments/
- **Purpose**: Legal document models
- **Features**: Compliance documents

### Master-Detail Models

#### MasterDetailChild_* Folders
- **Purpose**: Models for master-detail relationships
- **Patterns**:
  - Cohort_CohortUsers
  - LedgerEntry_LedgerEntryDetails
  - RecordRequirement_RecordCategories
  - RecordState_RecordNotes
  - TenantDepartment_TenantDepartmentUsers
  - TenantDocumentCategory_Records
- **Features**: Parent-child data relationships

## Architecture Notes

### Design Patterns
- **View Model Pattern**: All models are view-specific DTOs
- **Composition**: Complex models compose simpler ones
- **Inheritance**: Common base classes for similar models
- **Interfaces**: Contracts for common behaviors

### Naming Conventions
- ViewModels suffixed with "ViewModel"
- Edit models include "CreateOrEdit" prefix
- Output models suffixed with "Output"
- Input models suffixed with "Input"

### Model Responsibilities
- Data transfer between layers
- View-specific data shaping
- Validation attributes
- Display metadata

## Business Logic
Models contain:
- Data annotations for validation
- Display attributes for UI
- Computed properties for view logic
- No business logic (kept in services)

## Usage Across Codebase

### Direct Consumers
- Controllers create and populate models
- Views bind to model properties
- JavaScript accesses model data
- Validation uses model attributes

### Related Components
- Controllers: `/Areas/App/Controllers/`
- Views: `/Areas/App/Views/`
- Application DTOs: `inzibackend.Application.Shared`
- JavaScript: `/wwwroot/view-resources/Areas/App/`

## Subfolders
Each subfolder contains models specific to its feature area. Common patterns include:
- Index view models for lists
- CreateOrEdit models for forms
- Lookup models for entity selection
- Permission models for security
- Dashboard models for metrics

See individual CLAUDE.md files in subfolders for detailed documentation of specific model groups.