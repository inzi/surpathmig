# OUs Documentation

## Overview
Entity models for linking Tenant Departments with Organization Units, enabling hierarchical access control and data visibility management.

## Contents

### Files

#### TenantDepartmentOrganizationUnit.cs
- **Purpose**: Junction entity linking tenant departments to organization units
- **Key Properties**:
  - TenantDepartmentId: Reference to the tenant department
  - OrganizationUnitId: Reference to the ABP organization unit
  - TenantId: Multi-tenant support
- **Features**:
  - Full auditing capabilities
  - Many-to-many relationship support
  - Tenant isolation

### Key Components

- **TenantDepartmentOrganizationUnit**: Bridge entity for OU-Department relationships

### Dependencies

- **External Libraries**:
  - ABP Framework (Entities, Auditing)

- **Internal Dependencies**:
  - TenantDepartment entity
  - ABP OrganizationUnit

## Architecture Notes

- **Pattern**: Junction/Bridge table pattern for many-to-many relationships
- **Database**: Maps to TenantDepartmentOrganizationUnits table
- **Auditing**: Full audit trail for security tracking

## Business Logic

### Relationship Management
- Enables departments to belong to multiple OUs
- Allows OUs to contain multiple departments
- Provides foundation for hierarchical security

### Access Control
- Used by OUSecurityManager for visibility filtering
- Determines which departments users can access
- Supports complex organizational structures

## Usage Across Codebase

This entity is used by:
- OUSecurityManager for implementing row-level security
- Department assignment workflows
- Organizational hierarchy management
- Access control and visibility filtering