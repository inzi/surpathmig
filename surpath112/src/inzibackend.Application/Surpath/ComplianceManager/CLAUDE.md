# ComplianceManager Documentation

## Overview
This folder contains the core compliance evaluation and management services for the Surpath system. It provides the central engine for calculating, tracking, and managing student compliance with various requirements including drug testing, documentation, background checks, and training. The services handle both individual and bulk compliance calculations across cohorts and departments.

## Contents

### Files

#### ISurpathComplianceAppService.cs
- **Purpose**: Interface defining the contract for compliance management operations
- **Key Methods**:
  - `GetTenantDeptCompliance()`: Calculate compliance totals for departments
  - `GetCohortCompliance()`: Calculate compliance totals for cohorts
  - `AddNoteToRecord()`: Add notes to compliance records with notifications
  - `CreateNewRecord()`: Create new compliance record submissions
  - `CreateNewRecordState()`: Create new record state entries
  - `GetDepartmentsForRegistration()`: Get available departments for user registration
  - `CreateEditRequirement()`: Create or edit compliance requirements
  - `GetRecordCategoriesForRequirementForEdit()`: Get categories for requirement editing
- **Registration Features**:
  - `UsernameAvailable()`: Check username availability
  - `EmailAvailable()`: Check email availability

#### SurpathComplianceAppService.cs
- **Purpose**: Main implementation of compliance management operations
- **Key Features**:
  - Multi-tenant compliance tracking
  - Department and cohort-level compliance aggregation
  - Record creation with file upload support
  - Note management with authorization levels
  - IP address tracking for audit purposes
  - Integration with binary storage for documents
  - Requirement management with category associations
- **Dependencies Injected**:
  - Multiple repositories for entities (Cohort, CohortUser, RecordState, etc.)
  - File storage managers (ITempFileCacheManager, IBinaryObjectManager)
  - Feature checker for tenant-specific features
  - Logger for audit and debugging

#### ISurpathComplianceEvaluator.cs
- **Purpose**: Interface for compliance calculation and evaluation logic
- **Key Methods**:
  - `GetComplianceValuesForCohortUser()`: Calculate compliance for specific cohort user
  - `GetComplianceValuesForUser()`: Calculate compliance for user across all requirements
  - `GetComplianceStatesForUser()`: Get detailed compliance states
  - `GetComplianceInfo()`: Get comprehensive compliance information
  - `GetDetailedComplianceValuesForUser()`: Get detailed compliance breakdown
  - `GetBulkComplianceValuesForUsers()`: Bulk calculation for multiple users
  - `GetHierarchicalRequirementCategories()`: Get requirement hierarchy
  - `RecalculateUserCompliance()`: Force recalculation after changes
- **Special Features**:
  - Hierarchical requirement inheritance
  - Bulk processing capabilities
  - Post-migration recalculation

#### SurpathComplianceEvaluator.cs
- **Purpose**: Implementation of compliance calculation algorithms
- **Key Features**:
  - Complex compliance calculation logic
  - Requirement inheritance from department to cohort
  - Status aggregation across multiple categories
  - Expiration date tracking
  - Conditional requirement evaluation
  - Performance-optimized bulk operations

### Key Components

**Core Services:**
- `SurpathComplianceAppService`: Orchestrates compliance management
- `SurpathComplianceEvaluator`: Calculates compliance scores and states

**Data Structures:**
- `ComplianceValues`: Compliance metrics and scores
- `ComplianceInfo`: Comprehensive compliance information
- `ComplianceCohortTotalsForViewDto`: Aggregated compliance totals
- `GetRecordStateCompliancetForViewDto`: Individual compliance states
- `HierarchicalRequirementCategoryDto`: Hierarchical requirement structure

### Dependencies
- **External:**
  - ABP Framework (authorization, domain, repositories, features)
  - Entity Framework Core (data access)
  - Castle.Core.Logging (logging)
  - Microsoft.AspNetCore.Http (HTTP context)

- **Internal:**
  - inzibackend.Surpath domain entities
  - inzibackend.Surpath.Compliance namespace
  - inzibackend.Surpath.Dtos (data transfer objects)
  - inzibackend.Storage (file storage)
  - inzibackend.MultiTenancy (tenant management)
  - inzibackend.Authorization (user/role management)

## Architecture Notes
- **Pattern**: Service-oriented architecture with separation of concerns
- **Calculation Strategy**: Lazy evaluation with caching for performance
- **Inheritance Model**: Requirements cascade from department → cohort → user
- **State Management**: Immutable compliance states with audit trails
- **Bulk Processing**: Optimized queries for multiple user calculations
- **Caching**: Results cached to avoid recalculation

## Business Logic

### Compliance Calculation Flow
1. **User Context**: Identify user's cohort and department assignments
2. **Requirement Collection**: Gather all applicable requirements
   - Direct cohort requirements
   - Inherited department requirements
   - Service-specific requirements
3. **State Evaluation**: For each requirement, evaluate:
   - Document submission status
   - Approval status
   - Expiration dates
   - Conditional dependencies
4. **Score Calculation**: Aggregate compliance scores:
   - Complete requirements
   - Pending requirements
   - Expired requirements
   - Not started requirements
5. **Result Compilation**: Generate compliance report with details

### Requirement Hierarchy
```
TenantDepartment
    └── Cohort
        └── CohortUser
            └── RecordStates (actual compliance)
```

### Status Priorities
1. **Expired**: Highest priority (critical non-compliance)
2. **Rejected**: Requires resubmission
3. **Pending**: Awaiting review
4. **Approved**: Compliant
5. **Not Started**: No submission yet

### Key Business Rules
- Users inherit requirements from their cohort
- Cohorts inherit base requirements from departments
- Requirements can have effective and expiration dates
- Some requirements are conditional based on other requirements
- Service availability affects requirement visibility
- Compliance is calculated in real-time but can be cached
- Bulk operations optimize for database performance

## Usage Across Codebase

### Primary Consumers
- Dashboard widgets showing compliance status
- Student portal compliance tracking
- Administrator compliance reports
- Background jobs for expiration monitoring
- API endpoints for mobile apps
- Notification services for compliance alerts

### Integration Points
- RecordStatesAppService for individual record management
- CohortsAppService for cohort-level operations
- Background jobs for scheduled compliance checks
- Notification system for compliance alerts
- Report generation for compliance certificates

## Security Considerations
- All operations require authentication
- Note visibility controlled by authorization levels
- Tenant isolation enforced throughout
- Audit trail for all compliance changes
- IP address tracking for submissions
- File upload validation and sanitization
- Role-based access to compliance data

## Performance Considerations
- Bulk operations minimize database round-trips
- Hierarchical queries optimized with includes
- Caching strategy for frequently accessed data
- Lazy loading disabled for predictable performance
- Background processing for heavy calculations
- Indexed queries for compliance aggregation

## Compliance Tracking Features
- **Real-time Status**: Live compliance status updates
- **Historical Tracking**: Complete audit trail of changes
- **Expiration Management**: Proactive expiration monitoring
- **Bulk Processing**: Efficient cohort-wide operations
- **Flexible Requirements**: Conditional and dependent requirements
- **Multi-level Inheritance**: Department → Cohort → User hierarchy
- **Document Management**: Integrated file upload and storage
- **Notification System**: Automated alerts for compliance changes