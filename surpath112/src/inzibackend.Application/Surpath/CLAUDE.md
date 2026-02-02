# Surpath Documentation

## Overview
This folder contains the core business logic for the Surpath compliance tracking system. It implements comprehensive application services for managing student compliance with school policies including drug testing, documentation requirements, background checks, medical records, and training. The system tracks compliance across cohorts, departments, and various requirement categories.

## Contents

### Files

#### Core Entity Services
- **CohortsAppService.cs**: Manages student cohorts (groups of students with shared requirements)
- **CohortUsersAppService.cs**: Manages individual students within cohorts
- **RecordsAppService.cs**: Handles compliance record uploads and management
- **RecordStatesAppService.cs**: Tracks the state/status of compliance records
- **RecordCategoriesAppService.cs**: Manages categories of compliance requirements
- **RecordRequirementsAppService.cs**: Defines specific requirements within categories
- **RecordNotesAppService.cs**: Manages notes and annotations on records
- **RecordStatusesAppService.cs**: Handles record approval/rejection status

#### Department & Organization Services
- **TenantDepartmentsAppService.cs**: Manages departments within a tenant (school)
- **TenantDepartmentUsersAppService.cs**: Associates users with departments
- **DepartmentUsersAppService.cs**: Manages department user assignments
- **DeptCodesAppService.cs**: Handles department codes and identifiers

#### Drug Testing Services
- **DrugsAppService.cs**: Manages drug types and testing parameters
- **DrugPanelsAppService.cs**: Defines drug testing panels
- **DrugTestCategoriesAppService.cs**: Categories for drug testing requirements
- **PanelsAppService.cs**: Testing panel configurations
- **TestCategoriesAppService.cs**: Test category management

#### Document Management Services
- **TenantDocumentsAppService.cs**: Manages tenant-specific document templates
- **TenantDocumentCategoriesAppService.cs**: Document category management
- **CodeTypesAppService.cs**: Classification codes for documents
- **PIDTypesAppService.cs**: Personal identifier type management

#### Medical & Clinical Services
- **HospitalsAppService.cs**: Hospital/clinical site management
- **MedicalUnitsAppService.cs**: Medical unit/department management
- **RotationSlotsAppService.cs**: Clinical rotation scheduling

#### Financial & Service Management
- **LedgerEntriesAppService.cs**: Financial transaction tracking
- **LedgerEntryDetailsAppService.cs**: Detailed transaction records
- **UserPurchasesAppService.cs**: User purchase/payment tracking
- **SurpathServicesAppService.cs**: Available service definitions
- **TenantSurpathServicesAppService.cs**: Tenant-specific service configurations

#### Utility Services
- **CategoryManagementAppService.cs**: Centralized category management
- **CohortMigrationAppService.cs**: Cohort data migration utilities
- **UserTransferAppService.cs**: Transfer users between cohorts/departments
- **UserPidsAppService.cs**: User personal identifier management
- **ConfirmationValuesAppService.cs**: Confirmation value management
- **WelcomemessagesAppService.cs**: Welcome message configuration

### Key Components

**Core Domain Services:**
- Cohort and user management with compliance tracking
- Multi-level record management (categories, requirements, states, statuses)
- Department and organizational hierarchy
- Drug testing and panel configuration
- Document and template management

**Cross-Cutting Features:**
- Multi-tenancy support throughout all services
- Permission-based authorization using ABP framework
- Excel export capabilities for most entities
- Comprehensive filtering and pagination
- Audit logging and tracking

### Dependencies
- **External:**
  - ABP Framework (authorization, domain, repositories)
  - Entity Framework Core (data access)
  - NPOI (Excel operations)
  - Microsoft.Extensions.Configuration

- **Internal:**
  - inzibackend.Surpath domain entities
  - inzibackend.Surpath.Dtos (data transfer objects)
  - inzibackend.Storage (file storage)
  - inzibackend.Authorization (permissions)
  - Exporting subfolder (Excel exporters)
  - ComplianceManager subfolder (compliance evaluation)

## Subfolders

### ComplianceManager
Contains the core compliance evaluation engine including `SurpathComplianceAppService` which orchestrates compliance checking across all requirement types and `ISurpathComplianceEvaluator` interface for custom evaluation logic.

### Exporting
Comprehensive Excel export functionality for all major entities with interfaces and implementations for data export operations.

### Jobs
Background job services for automated compliance checking, expiration monitoring, and scheduled maintenance tasks.

### LegalDocuments
Legal document management and compliance tracking for agreements, waivers, and policy acknowledgments.

### Purchase
Purchase and payment processing services for compliance-related fees and services.

### Tools
Administrative tools including review queue management for compliance officers.

## Architecture Notes
- **Pattern**: Application Service pattern with DTOs for data transfer
- **Authorization**: Permission-based using ABP's `[AbpAuthorize]` attributes
- **Data Access**: Repository pattern via ABP's `IRepository<T>`
- **Multi-tenancy**: Tenant filtering applied throughout
- **Caching**: Uses ABP's caching for frequently accessed data
- **Background Jobs**: Async processing for long-running operations
- **File Storage**: Binary object storage for uploaded documents

## Business Logic

### Compliance Workflow
1. **Cohort Setup**: Students assigned to cohorts with specific requirements
2. **Requirement Definition**: Categories and requirements defined per cohort
3. **Document Upload**: Students upload compliance documents
4. **Review Process**: Administrators review and approve/reject submissions
5. **Status Tracking**: Real-time compliance status across all requirements
6. **Expiration Management**: Automated tracking of document expiration
7. **Notification**: Alerts for pending/expired requirements

### Key Business Rules
- Students must be in a cohort to have requirements
- Requirements can have effective and expiration dates
- Documents require approval before marking requirement complete
- Drug testing follows panel-based configuration
- Department association affects requirement visibility
- Service availability controlled at tenant level

### Data Relationships
- Cohorts → CohortUsers → Users
- RecordCategories → RecordRequirements → RecordStates
- TenantDepartments → Cohorts
- Records → RecordStates (compliance tracking)
- TenantDocumentCategories → TenantDocuments (templates)

## Usage Across Codebase

### Primary Consumers
- MVC Controllers for web interface
- API endpoints for mobile applications
- Background job processors
- Report generation services
- Dashboard and analytics modules

### Integration Points
- User management system for student data
- Document storage for file uploads
- Notification system for alerts
- Payment processing for services
- Reporting engine for compliance reports

## Security Considerations
- All services require authentication
- Permission-based access control per entity
- Tenant isolation ensures data segregation
- Document access controlled by ownership
- Sensitive data encryption for storage
- Audit trails for compliance changes
- Role-based review permissions

## Performance Considerations
- Lazy loading disabled with explicit includes
- Pagination on all list operations
- Background jobs for heavy processing
- Caching for reference data
- Optimized queries for compliance calculations
- Bulk operations for cohort management