# Authorization Documentation

## Overview
Central location for authorization-related constants and definitions used across the application's multi-tenant architecture. Provides role definitions and user validation constraints.

## Contents

### Files
No files directly in this folder.

### Key Components
- Role name definitions for multi-tenant architecture
- User data validation constants

### Dependencies
- No external dependencies
- Core authorization namespace for the application

## Subfolders

### Roles
Contains static role name definitions for the multi-tenant system:
- **StaticRoleNames.cs**: Defines Host Admin, Tenant Admin, and Tenant User roles
- Provides compile-time safe role constants
- Separates Host-level and Tenant-level role concerns

### Users
Contains user-related validation constants:
- **UserConsts.cs**: Defines phone number length constraints (10-24 characters)
- Provides centralized validation rules for user fields
- Supports international phone number formats

## Architecture Notes
- Clear separation between Host and Tenant authorization contexts
- Centralized constants promote consistency across the application
- Follows multi-tenant design patterns with role-based access control
- Constants are compile-time safe for better performance

## Business Logic
- **Host Admin**: System-wide administration capabilities
- **Tenant Admin**: Administration within a specific tenant
- **Tenant User**: Standard user within a tenant
- Phone number validation supports international formats

## Usage Across Codebase
Authorization constants are used throughout:
- Controller authorization attributes
- Service-layer permission checks
- User management operations
- Data validation in entities and DTOs
- Database constraints and migrations