# Surpath Documentation

## Overview
Core domain constants, enumerations, and business logic models for the Surpath compliance tracking system. This folder contains the foundational data structures and business rules for managing student/user compliance with institutional requirements including drug testing, background checks, immunizations, and document management.

## Contents

### Files

#### Core Enumerations

##### Record Management
- **EnumRecordState.cs**: Document approval states (NeedsApproval, Rejected, Approved)
- **EnumRecordType.cs**: Types of compliance records
- **EnumServiceRecordState.cs**: Service-related record states
- **EnumStatusComplianceImpact.cs**: Impact of status on compliance

##### Test Results
- **EnumOverAllTestResult.cs**: Overall test outcomes (None, Positive, Negative, Discard, Other)
- **EnumTestType.cs**: Types of tests performed
- **EnumReportType.cs**: Report categorization

##### Client and Vendor Types
- **EnumClientTypes.cs**: Client categorization
- **EnumClientMROTypes.cs**: Medical Review Officer types
- **EnumClientPaymentTypes.cs**: Payment method types
- **EnumVendorType.cs**: Vendor categorization

##### System Operations
- **EnumSurpathFileAction.cs**: File operation types
- **EnumPurchaseStatus.cs**: Purchase transaction states
- **EnumSlotShiftType.cs**: Scheduling shift types
- **EnumUnitOfMeasurement.cs**: Measurement units

##### Notifications
- **EnumNotificationExceptions.cs**: General notification exceptions
- **EnumNotificationClinicExceptions.cs**: Clinic-specific notification exceptions

##### Geographic
- **enumUSStates.cs**: US state abbreviations with full names

#### Constants Classes

##### Entity Constants (Empty placeholders)
- **CohortConsts.cs**: Cohort-related constants
- **CohortUserConsts.cs**: Cohort user constants
- **RecordConsts.cs**: Record constants
- **RecordCategoryConsts.cs**: Record category constants
- **RecordCategoryRuleConsts.cs**: Category rule constants
- **RecordNoteConsts.cs**: Record note constants
- **RecordRequirementConsts.cs**: Requirement constants
- **RecordStateConsts.cs**: State constants
- **RecordStatusConsts.cs**: Status constants

##### Service Constants
- **SurpathServiceConsts.cs**: Service definitions
- **TenantSurpathServiceConsts.cs**: Tenant-specific services
- **TenantDocumentConsts.cs**: Document constants
- **TenantDocumentCategoryConsts.cs**: Document categories
- **TenantDepartmentConsts.cs**: Department constants
- **TenantDepartmentUserConsts.cs**: Department user constants

##### Testing Constants
- **DrugConsts.cs**: Drug-related constants
- **DrugPanelConsts.cs**: Drug panel definitions
- **DrugTestCategoryConsts.cs**: Test categories
- **TestCategoryConsts.cs**: General test categories
- **PanelConsts.cs**: Panel definitions

##### User and Department
- **DepartmentUserConsts.cs**: Department user constants
- **UserPidConsts.cs**: User PID constants
- **UserPurchaseConsts.cs**: User purchase constants
- **PIDTypeConsts.cs**: PID type definitions
- **DeptCodeConsts.cs**: Department codes

##### Medical Facilities
- **HospitalConsts.cs**: Hospital constants
- **MedicalUnitConsts.cs**: Medical unit constants
- **RotationSlotConsts.cs**: Rotation scheduling

##### Financial
- **LedgerEntryConsts.cs**: Ledger entry constants
- **LedgerEntryDetailConsts.cs**: Ledger detail constants

##### Other
- **ConfirmationValueConsts.cs**: Confirmation values
- **WelcomemessageConsts.cs**: Welcome message constants
- **HierarchicalPricingNode.cs**: Pricing structure model

### Key Components
- Comprehensive enum definitions for all business states
- Constants for entity validation and constraints
- US state enumeration with descriptions
- Multi-level categorization (records, tests, services)
- Financial tracking structures

### Dependencies
- System.ComponentModel: For enum descriptions
- Core .NET types

## Subfolders

### AuthNet
Authorize.Net payment gateway integration:
- Payment submission models
- Transaction result DTOs
- Tokenized payment handling

### Compliance
Core compliance tracking:
- Multi-category compliance status (drug, background, immunization)
- Overall compliance calculation
- User and tenant association

### Extensions
Utility extensions and settings:
- Day of week utilities
- Enum description helpers
- GUID validation
- HTML helpers
- Application settings management

### LegalDocuments
Legal document management:
- Privacy Policy and Terms of Service
- File upload and external URL support
- CRUD operation DTOs

### ParserClasses
Document parsing utilities:
- CRL lab report parsing
- PDF to text conversion
- Test result extraction

### Purchase
Purchase and payment services:
- Authorize.Net configuration
- Purchase service interface
- Pre-authorization workflows

## Architecture Notes
- Domain-driven design with rich enumerations
- Placeholder constants classes for future expansion
- Clear separation of concerns by functional area
- Support for multi-tenant architecture
- Extensible categorization system

## Business Logic
- **Compliance Tracking**: Three-tier system (drug, background, immunization)
- **Record Lifecycle**: NeedsApproval → Approved/Rejected
- **Test Results**: Comprehensive outcome tracking
- **Payment Processing**: Pre-auth and capture model
- **Document Management**: Support for various document types
- **Multi-Tenant**: Tenant-specific services and configurations

## Usage Across Codebase
These domain components are used throughout:
- Entity definitions in Core layer
- Service implementations in Application layer
- API controllers for web services
- Data validation and constraints
- Business rule enforcement
- Reporting and analytics
- User interface dropdowns and displays
- Database schema definitions