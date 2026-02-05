# Permissions DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the permission system. These DTOs represent the hierarchical permission structure used throughout the application for access control. Permissions define what actions users and roles can perform across different features and modules.

## Contents

### Files

- **FlatPermissionDto.cs** - Flat representation of a permission:
  - ParentName - Parent permission in hierarchy
  - Name - Unique permission identifier
  - DisplayName - User-friendly display text
  - Description - Permission purpose explanation
  - IsGrantedByDefault - Whether new roles get this permission automatically

- **FlatPermissionWithLevelDto.cs** - Extended permission with hierarchy level information for UI tree rendering

### Key Components

#### Permission Hierarchy
Permissions are organized hierarchically (e.g., "Pages.Administration.Users" has parent "Pages.Administration"). The flat representation simplifies transmission and storage while maintaining parent-child relationships through the ParentName property.

### Dependencies
- None - Pure DTOs with no external dependencies

## Architecture Notes

### Design Pattern
- **Flattened Hierarchy**: Converts tree structure to flat list for easier serialization
- **Self-Documenting**: DisplayName and Description provide user-facing information
- **Defaulting**: IsGrantedByDefault simplifies role creation by auto-granting common permissions

### Permission Naming Convention
Permissions follow a hierarchical dot-notation naming pattern:
- `Pages.Administration.Users` - Access to user management page
- `Pages.Administration.Users.Create` - Permission to create users
- `Pages.Surpath.Cohorts.Edit` - Permission to edit cohorts

## Business Logic

### Permission Use Cases
1. **Role Configuration**: Assigning permissions to roles during setup
2. **Permission Trees**: Rendering permission checkboxes in UI with proper hierarchy
3. **Access Control**: Runtime permission checking for features and pages
4. **Default Roles**: Creating roles with sensible default permissions

### Grant Semantics
- **IsGrantedByDefault = true**: New roles automatically receive this permission
- Parent permissions may imply child permissions depending on implementation
- Static roles (Admin) typically have all permissions

## Usage Across Codebase
These DTOs are consumed by:
- **IPermissionAppService** - Permission tree retrieval
- **IRoleAppService** - Role creation/editing with permissions
- **Permission Checkers** - Runtime authorization
- **UI Components** - Permission selection trees in role management
- **Permission Providers** - System permission definitions

## Cross-Reference Impact
Changes to these DTOs affect:
- Role management interfaces
- Permission assignment workflows
- Authorization attribute checking
- Permission tree rendering in admin UI