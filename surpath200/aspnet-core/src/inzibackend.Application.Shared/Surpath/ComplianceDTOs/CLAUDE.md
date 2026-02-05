# Surpath/ComplianceDTOs Documentation

## Overview
Data Transfer Objects specifically designed for compliance tracking and requirement management in the Surpath system. These DTOs handle the complex relationships between requirements, categories, cohorts, departments, and compliance states.

## Contents

### Files

#### Requirement Management
- **RequirementCreationDto.cs** - Comprehensive DTO for creating and editing compliance requirements
  - Contains the requirement details (CreateOrEditRecordRequirementDto)
  - Associated record categories for multi-step requirements
  - Lookup data for cohorts, departments, and category rules
  - IsEditMode property for distinguishing create vs edit operations

- **CreateEditRecordRequirementDto.cs** - Core DTO for requirement CRUD operations with requirement-specific fields

#### Compliance Views
- **GetRecordStateCompliancetForViewDto.cs** - Complex view model for displaying compliance status
  - Combines requirement, category, and state information
  - Includes tenant department and cohort context
  - Supports hierarchical display with parent/child row indicators
  - Contains service availability information (SurpathService, TenantSurpathService)

- **GetRecordStateForQueueViewDto.cs** - Specialized view for compliance queue display

#### Utility DTOs
- **BalanceStatus.cs** - Enumeration or status tracking for balance-related compliance states

- **ComplianceGetAllForLookupTableInput.cs** - Input parameters for fetching compliance lookup data

- **UsernameAvailableDto.cs** - DTO for checking username availability in compliance contexts

### Key Components
- Hierarchical requirement structure with categories and steps
- Multi-tenant support with department and cohort scoping
- Service availability tracking for compliance features
- Parent-child relationships for complex compliance views

### Dependencies
- Abp.Application.Services.Dto - ABP framework DTOs
- System.Collections.Generic - Collection support
- inzibackend.Surpath.Dtos - Core Surpath DTOs

## Architecture Notes
- DTOs support complex compliance workflows with multiple steps/categories
- Designed for multi-tenant SaaS with department and cohort isolation
- Supports both creation and viewing perspectives
- Hierarchical data representation with parent/child indicators
- Service availability checks integrated into compliance views

## Business Logic
- **Requirements**: Can be scoped to specific departments, cohorts, or services
- **Categories**: Requirements can have multiple categories representing steps in compliance
- **Record States**: Track the actual compliance status for each requirement
- **Service Availability**: Compliance features depend on enabled Surpath services
- **Queue Management**: Special views for processing compliance queues
- **Hierarchical Display**: Support for grouped/nested compliance views

## Usage Across Codebase
These DTOs are consumed by:
- Compliance management services in the Application layer
- Compliance tracking controllers in Web.Mvc
- Queue processing components
- Reporting and dashboard features
- Student/cohort user compliance tracking