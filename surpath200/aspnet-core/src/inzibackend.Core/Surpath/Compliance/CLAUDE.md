# Compliance Documentation

## Overview
Core compliance management system for tracking and enforcing student/user compliance with institutional requirements. This module handles the complex business logic for determining applicable requirements based on user membership in cohorts and departments, managing Surpath service requirements, and providing access control for compliance-related documents.

## Contents

### Files

#### ComplianceInfo.cs
- **Purpose**: Data aggregation model for user compliance information
- **Key Functionality**:
  - Consolidates all compliance-related data for a user
  - Separates Surpath-only requirements from general requirements
  - Tracks user department and cohort memberships
  - Maintains lists of applicable rules and services

#### SurpathManager.cs
- **Purpose**: Domain service for managing user assignments, cohorts, and compliance access control
- **Key Functionality**:
  - User-to-department and user-to-cohort assignment management
  - Default cohort creation and management
  - User PID (Personal Identifier) management
  - File access control based on permissions and service types
  - Requirement retrieval based on cohort and department hierarchy
  - Tenant management utilities (name retrieval, donor pay status)
- **Security Features**:
  - Permission-based file access control
  - Service-specific download permissions (drug tests, background checks)
  - Impersonation support with proper permission checking

#### TenantRequirement.cs
- **Purpose**: Composite model linking requirements with their categories, rules, and services
- **Key Components**:
  - RecordRequirement: The base requirement definition
  - RecordCategory: Category classification
  - RecordCategoryRule: Business rules for the category
  - TenantSurpathService: Hierarchical service configuration

#### UserMembership.cs
- **Purpose**: Model representing a user's organizational affiliations
- **Key Data**:
  - Tenant department membership
  - Cohort membership
  - CohortUser relationship tracking
  - User identification

#### SurpathOnlyRequirements.cs
- **Purpose**: Static utility class for determining applicable Surpath service requirements
- **Key Functionality**:
  - Hierarchical requirement resolution (User → Cohort → Department → Tenant)
  - Feature-based requirement filtering (drug tests, background checks)
  - Support for legacy name-based matching and new FeatureIdentifier matching
  - User enrollment tracking to maintain service continuity
- **Business Logic**:
  - Priority order: User-specific > Cohort-specific > Department-specific > Tenant-wide
  - Ensures users stay enrolled in their existing services
  - Handles both individual compliance checking and bulk requirement retrieval

### Key Components

- **SurpathManager**: Central domain service for compliance operations
- **ComplianceInfo**: Comprehensive compliance data model
- **SurpathOnlyRequirements**: Static helper for Surpath service requirement resolution

### Dependencies

- **External Libraries**:
  - ABP Framework (Domain services, repositories, permissions)
  - Entity Framework Core
  - Microsoft.AspNetCore.Identity

- **Internal Dependencies**:
  - inzibackend.Authorization (Users, Permissions, Roles)
  - inzibackend.MultiTenancy
  - inzibackend.Features
  - Various Surpath entities (Cohort, CohortUser, RecordRequirement, etc.)

## Architecture Notes

- **Pattern**: Domain-Driven Design with domain services
- **Hierarchy**: Multi-level requirement resolution (User → Cohort → Department → Tenant)
- **Multi-tenancy**: Full support with proper tenant isolation
- **Unit of Work**: Explicit transaction management for cross-tenant operations
- **Caching**: Not implemented at this level (handled by ABP framework)

## Business Logic

### Requirement Resolution Hierarchy
1. **User-specific requirements** (highest priority)
2. **Cohort-specific requirements** (group level)
3. **Department-specific requirements** (organizational level)
4. **Tenant-wide requirements** (lowest priority, default)

### Service Features
- **Drug Testing** (AppFeatures.SurpathFeatureDrugTest)
- **Background Checks** (AppFeatures.SurpathFeatureBackgroundCheck)
- Each service has specific permissions for download access

### Default Cohort Management
- Every tenant has a default "Unassigned" cohort
- Users are automatically assigned if not manually placed
- Ensures no user exists without cohort membership

### Access Control
- File access validated through permission system
- Service-specific download permissions
- Support for impersonation with proper authorization
- Host administrators have elevated access

## Usage Across Codebase

This compliance system is used by:
- User registration and onboarding processes
- Compliance dashboard and reporting
- Document upload and verification workflows
- Service enrollment and management
- Administrative compliance review interfaces
- File download and access control systems