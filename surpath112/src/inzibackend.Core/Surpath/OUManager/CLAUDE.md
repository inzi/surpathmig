# OUManager Documentation

## Overview
Organization Unit (OU) security management system that provides data visibility filtering based on organizational hierarchy. This module implements row-level security for tenant departments, cohorts, and users based on their organizational unit memberships.

## Contents

### Files

#### IOUSecurityManager.cs
- **Purpose**: Interface defining organizational unit security operations
- **Key Methods**:
  - AddTenantDepartmentToOUAsync: Associate department with OU
  - GetOUOfTenantDepartmentAsync: Retrieve OU for a department
  - IsTenantDepartmentInOUAsync: Check department-OU membership
  - RemoveTenantDepartmentFromOUAsync: Disassociate department from OU
  - ApplyTenantDepartmentVisibilityFilter: Filter departments by user access
  - ApplyTenantDepartmentUserVisibilityFilter: Filter department users
  - ApplyTenantDepartmentCohortVisibilityFilter: Filter cohorts
  - ApplyTenantDepartmentCohortUserVisibilityFilter: Filter cohort users

#### OUSecurityManager.cs
- **Purpose**: Implementation of OU-based security filtering and management
- **Key Functionality**:
  - Manages tenant department to organization unit associations
  - Implements visibility filters based on user's OU membership
  - Provides hierarchical data access control
  - Integrates with ABP's organization unit system
- **Security Features**:
  - Row-level security through LINQ query filters
  - Hierarchical permission inheritance
  - User-based visibility restrictions

### Key Components

- **IOUSecurityManager**: Security interface for OU operations
- **OUSecurityManager**: Core implementation with visibility filtering

### Dependencies

- **External Libraries**:
  - ABP Framework (Organizations, Domain Services)
  - Entity Framework Core
  - System.Linq.Dynamic.Core
  - Microsoft.AspNetCore.Identity

- **Internal Dependencies**:
  - inzibackend.Surpath entities (TenantDepartment, Cohort, etc.)
  - inzibackend.Surpath.OUs (TenantDepartmentOrganizationUnit)
  - inzibackend.Authorization (Users, Roles)

## Architecture Notes

- **Pattern**: Domain Service with repository pattern
- **Security Model**: Hierarchical organization-based access control
- **Data Filtering**: LINQ-based query modification for row-level security
- **Multi-tenancy**: Integrated with tenant isolation

## Business Logic

### Organization Unit Hierarchy
- Organization Units form a tree structure
- Users inherit access from their OU membership
- Departments can be assigned to OUs for access control
- Visibility cascades down the hierarchy

### Visibility Filtering
1. Determines user's organization units
2. Finds departments associated with those OUs
3. Filters queries to show only accessible data
4. Applies to departments, users, cohorts, and cohort users

### Access Control Flow
- User → Organization Unit(s) → Tenant Department(s) → Related Data
- Ensures users only see data within their organizational scope

## Usage Across Codebase

This security manager is used by:
- Application services requiring department-based filtering
- Cohort and user management interfaces
- Reporting systems needing scoped data access
- Administrative tools for OU management
- Any query requiring organizational visibility restrictions

## Security Considerations

- Implements principle of least privilege
- Provides transparent security through query filtering
- Maintains tenant isolation
- Supports complex organizational hierarchies