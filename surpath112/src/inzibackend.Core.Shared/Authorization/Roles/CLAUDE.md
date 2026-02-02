# Authorization/Roles Documentation

## Overview
Defines static role names used throughout the application for both Host and Tenant contexts in the multi-tenant architecture.

## Contents

### Files

#### StaticRoleNames.cs
- **Purpose**: Provides constant definitions for system role names
- **Key Functionality**:
  - Separates Host and Tenant role definitions
  - Defines Admin role for Host-level administration
  - Defines Admin and User roles for Tenant-level operations
- **Usage**: Used throughout the application for role-based authorization checks

### Key Components

#### StaticRoleNames Class
- Static class containing nested structures for role organization
- **Host**: Contains role names for host-level operations (Admin only)
- **Tenants**: Contains role names for tenant-level operations (Admin, User)

### Dependencies
- No external dependencies
- Part of the inzibackend.Authorization.Roles namespace

## Architecture Notes
- Follows the separation of concerns principle for multi-tenant applications
- Clear distinction between Host-level and Tenant-level roles
- Uses const strings for compile-time safety and performance

## Business Logic
- Host Admin: System-wide administrator with access to all tenants
- Tenant Admin: Administrator within a specific tenant
- Tenant User: Regular user within a specific tenant

## Usage Across Codebase
These role constants are referenced throughout:
- Authorization attributes in controllers
- Role-based permission checks in services
- User management and role assignment operations
- Security policies and access control implementations