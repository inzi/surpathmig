# Surpath/Dtos Documentation

## Overview
Core business domain DTOs for the Surpath compliance tracking system. This comprehensive collection (268 files) defines the data structures for managing student compliance with school policies including drug testing, documentation, background checks, immunizations, and clinical requirements. The DTOs follow consistent patterns for CRUD operations, lookups, and complex business workflows.

## Contents

### Files (Key Categories)

#### Core Entities
- **CohortDto.cs / CreateOrEditCohortDto.cs** - Cohort management for grouping students
- **CohortUserDto.cs / CreateOrEditCohortUserDto.cs** - Association between cohorts and users
- **TenantDepartmentDto.cs / CreateOrEditTenantDepartmentDto.cs** - Department management within tenants
- **DepartmentUserDto.cs / CreateOrEditDepartmentUserDto.cs** - User-department associations

#### Compliance Requirements
- **RecordRequirementDto.cs / CreateOrEditRecordRequirementDto.cs** - Compliance requirement definitions
- **RecordCategoryDto.cs / CreateOrEditRecordCategoryDto.cs** - Categories/steps within requirements
- **RecordCategoryRuleDto.cs / CreateOrEditRecordCategoryRuleDto.cs** - Business rules for categories
- **RecordDto.cs / CreateOrEditRecordDto.cs** - Actual compliance records/documents
- **RecordStateDto.cs / GetRecordStateForViewDto.cs** - Current state of compliance records
- **RecordNoteDto.cs / CreateOrEditRecordNoteDto.cs** - Notes and comments on records

#### Drug Testing
- **DrugDto.cs / CreateOrEditDrugDto.cs** - Drug definitions for testing
- **PanelDto.cs / CreateOrEditPanelDto.cs** - Drug testing panels
- **DrugPanelDto.cs / CreateOrEditDrugPanelDto.cs** - Drug-to-panel associations
- **TestCategoryDto.cs / CreateOrEditDrugTestCategoryDto.cs** - Test categorization
- **ConfirmationValueDto.cs / CreateOrEditConfirmationValueDto.cs** - Test result confirmations
- **SpecimenStatusDto.cs** - Specimen tracking statuses

#### Medical/Clinical
- **HospitalDto.cs / CreateOrEditHospitalDto.cs** - Hospital/clinical site management
- **MedicalUnitDto.cs / CreateOrEditMedicalUnitDto.cs** - Medical unit definitions
- **RotationDto.cs / CreateOrEditRotationDto.cs** - Clinical rotation tracking
- **SlotDto.cs** (in SlotDtos subfolder) - Scheduling slots for rotations
- **ImmunizationDto.cs** - Immunization tracking
- **CohortImmunizationReportDto.cs** - Immunization reporting by cohort

#### Document Management
- **RecordTypeDto.cs / CreateOrEditRecordTypeDto.cs** - Types of compliance documents
- **PIDTypeDto.cs / CreateOrEditPIDTypeDto.cs** - Personal identifier types
- **UserPidDto.cs** - User personal identifiers
- **ExpirationCheckDto.cs** - Document expiration tracking

#### Financial
- **LedgerEntryDto.cs / CreateOrEditLedgerEntryDto.cs** - Financial transaction entries
- **LedgerEntryDetailDto.cs / CreateOrEditLedgerEntryDetailDto.cs** - Transaction details
- **InvoiceDto.cs** - Invoice generation
- **PaymentDto.cs** - Payment processing

#### Notifications & Communication
- **RecordNotificationDto.cs** - Notification settings for records
- **BulkRecordNotificationDto.cs** - Bulk notification operations
- **EmailTrackingDto.cs** - Email communication tracking
- **PortalMessageDto.cs** - In-app messaging

#### Reporting & Views
- **Dashboard DTOs** - Various dashboard view models
- **Report DTOs** - Compliance reporting structures
- **Queue DTOs** - Work queue management
- **Export DTOs** - Data export formats

#### Lookup Tables (40+ files)
Pattern: `[Entity1][Entity2]LookupTableDto.cs`
- Provide dropdown/select list data
- Support many-to-many relationships
- Enable efficient data fetching for UI

#### Service Management
- **SurpathServiceDto.cs / CreateOrEditSurpathServiceDto.cs** - Available services
- **TenantSurpathServiceDto.cs** - Tenant-specific service enablement
- **ServiceStatusDto.cs** - Service availability status

### Key Components
- Entity DTOs extending `EntityDto<Guid>` for domain objects
- CreateOrEdit DTOs for CRUD operations
- LookupTable DTOs for reference data
- View DTOs for complex display scenarios
- Input/Output DTOs for service operations

### Dependencies
- Abp.Application.Services.Dto - ABP framework DTO base classes
- System.Collections.Generic - Collection support
- System.ComponentModel.DataAnnotations - Validation attributes

## Subfolders

### SlotDtos
Contains DTOs specific to scheduling and slot management for clinical rotations.

## Architecture Notes
- Consistent naming conventions: `[Entity]Dto`, `CreateOrEdit[Entity]Dto`, `Get[Entity]ForViewDto`
- GUID-based entity identifiers for distributed systems
- Multi-tenant support with TenantId properties
- Audit fields (CreationTime, CreatorUserId, etc.) inherited from ABP
- Complex relationships managed through association DTOs
- Lookup tables optimize UI performance

## Business Logic
- **Cohort-User Relationship**: Students (CohortUsers) belong to cohorts for requirement tracking
- **Requirement Hierarchy**: Requirements → Categories → Rules → Records
- **Service Enablement**: Features are controlled by enabled Surpath services per tenant
- **Compliance States**: Records progress through defined states (pending, approved, rejected, expired)
- **Drug Testing Workflow**: Panels contain drugs, tests map to categories, results require confirmation
- **Document Lifecycle**: Upload → Review → Approve/Reject → Track Expiration
- **Multi-level Scoping**: Requirements can be scoped to tenant, department, or cohort level

## Usage Across Codebase
These DTOs are extensively used by:
- Application services in inzibackend.Application
- Web API controllers in Web.Mvc
- Entity Framework mappings in EntityFrameworkCore
- Client-side models in JavaScript/TypeScript
- Report generation services
- Background job processors
- Integration endpoints