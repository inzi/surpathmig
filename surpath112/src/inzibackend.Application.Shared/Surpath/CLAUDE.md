# Surpath Documentation

## Overview
Core business domain service interfaces for the Surpath compliance tracking system. This folder contains all application service contracts that define the API surface for managing student compliance with school policies including drug testing, documentation, background checks, immunizations, and clinical requirements.

## Contents

### Files (Service Interfaces)

#### Core Entity Management
- **ICohortsAppService.cs** - Cohort management including compliance views and immunization reports
- **ICohortUsersAppService.cs** - Management of users within cohorts
- **ITenantDepartmentsAppService.cs** - Department management within multi-tenant context
- **IDepartmentUsersAppService.cs** - User-department association management
- **ITenantDepartmentUsersAppService.cs** - Tenant-specific department user management

#### Compliance & Records
- **IRecordRequirementsAppService.cs** - Compliance requirement definitions and rules
- **IRecordsAppService.cs** - Document/record management with file upload capabilities
- **IRecordCategoriesAppService.cs** - Category management for multi-step requirements
- **IRecordCategoryRulesAppService.cs** - Business rules for requirement categories
- **IRecordStatesAppService.cs** - State tracking for compliance records
- **IRecordStatusesAppService.cs** - Status management for records
- **IRecordNotesAppService.cs** - Note and comment management for records

#### Drug Testing
- **IDrugsAppService.cs** - Drug definitions for testing
- **IPanelsAppService.cs** - Drug testing panel configuration
- **IDrugPanelsAppService.cs** - Drug-to-panel associations
- **ITestCategoriesAppService.cs** - Test category management
- **IDrugTestCategoriesAppService.cs** - Drug test categorization
- **IConfirmationValuesAppService.cs** - Test result confirmation values

#### Medical/Clinical
- **IHospitalsAppService.cs** - Hospital and clinical site management
- **IMedicalUnitsAppService.cs** - Medical unit definitions
- **IRotationSlotsAppService.cs** - Clinical rotation scheduling

#### Document Management
- **ITenantDocumentsAppService.cs** - Tenant-specific document management
- **ITenantDocumentCategoriesAppService.cs** - Document category configuration
- **IPIDTypesAppService.cs** - Personal identifier type management
- **IUserPidsAppService.cs** - User personal identifier management

#### Financial
- **ILedgerEntriesAppService.cs** - Financial transaction management
- **ILedgerEntryDetailsAppService.cs** - Transaction detail management
- **IUserPurchasesAppService.cs** - User purchase tracking

#### Service Configuration
- **ISurpathServicesAppService.cs** - Available service definitions
- **ITenantSurpathServicesAppService.cs** - Tenant-specific service enablement

#### Utilities
- **ICodeTypesAppService.cs** - Code type management
- **IDeptCodesAppService.cs** - Department code configuration
- **IWelcomemessagesAppService.cs** - Welcome message management
- **ICategoryManagementAppService.cs** - General category management
- **ICohortMigrationAppService.cs** - Cohort data migration utilities
- **IUserTransferAppService.cs** - User transfer between cohorts/departments

### Key Components
All service interfaces follow consistent patterns:
- Extend `IApplicationService` from ABP framework
- Async Task-based operations
- Standard CRUD operations (GetAll, GetForView, GetForEdit, CreateOrEdit, Delete)
- Excel export capabilities (GetToExcel methods)
- Lookup table methods for UI dropdowns
- Specialized business operations per domain

### Dependencies
- Abp.Application.Services - ABP framework service base
- System.Threading.Tasks - Async operations
- Surpath.Dtos - All domain DTOs
- inzibackend.Dto - Common DTOs like FileDto
- Microsoft.AspNetCore.Http - File upload support

## Subfolders

### ComplianceDTOs
Specialized DTOs for compliance tracking and requirement management. Handles complex relationships between requirements, categories, cohorts, and compliance states.
[Full details in ComplianceDTOs/CLAUDE.md](ComplianceDTOs/CLAUDE.md)

### Dtos
Comprehensive collection (268 files) of domain DTOs covering all business entities, operations, lookups, and views.
[Full details in Dtos/CLAUDE.md](Dtos/CLAUDE.md)

### SlotDtos
DTOs specific to rotation slot scheduling and management.
[Full details in Dtos/SlotDtos/CLAUDE.md](Dtos/SlotDtos/CLAUDE.md)

### dashboardDTOs
Dashboard-specific view models and data structures for reporting and analytics.

### DTOBases
Base classes and interfaces for DTO inheritance hierarchy.

### exDtos
Extended DTOs for complex business scenarios and integrations.

## Architecture Notes
- Service-oriented architecture with clear separation of concerns
- Multi-tenant design with tenant isolation at service level
- File upload support integrated into record management
- Excel export capabilities for reporting
- Lookup table pattern for efficient UI data loading
- Compliance tracking as core domain concept
- Hierarchical requirement structure (Requirement → Category → Rule)

## Business Logic
- **Cohort Management**: Students are organized into cohorts for requirement tracking
- **Compliance Workflow**: Requirements are defined → Students upload documents → Admin review → Approval/Rejection → Expiration tracking
- **Drug Testing**: Comprehensive workflow from panel definition to result confirmation
- **Service Enablement**: Features controlled by Surpath services enabled per tenant
- **Multi-level Scoping**: Requirements can be scoped to tenant, department, or cohort
- **Document Lifecycle**: Upload → Categorization → Review → Status tracking → Expiration monitoring
- **Financial Tracking**: Ledger entries track all financial transactions

## Usage Across Codebase
These service interfaces are implemented by:
- Concrete service classes in inzibackend.Application
- Consumed by controllers in Web.Mvc layer
- Called by background jobs and schedulers
- Used by reporting and export services
- Referenced by integration endpoints
- Utilized by mobile applications via API