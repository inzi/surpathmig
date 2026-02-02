# Modals Documentation

## Overview
This folder contains view model classes for common modal dialogs used throughout the App area of the MVC application. These models provide data structures for reusable modal components that handle permissions, entity history, lookups, and modal headers.

## Contents

### Files

#### EntityHistoryModalViewModel.cs
- **Purpose**: View model for displaying entity change history in a modal dialog
- **Key Properties**:
  - `EntityTypeFullName`: The fully qualified name of the entity type
  - `EntityTypeDescription`: Human-readable description of the entity
  - `EntityId`: The unique identifier of the entity being tracked
- **Usage**: Used when viewing audit logs and change history for any tracked entity

#### LookupModalViewModel.cs
- **Purpose**: Simple view model for lookup/search modal dialogs
- **Key Properties**:
  - `Title`: The title to display in the lookup modal
- **Usage**: Base model for entity picker and search dialogs throughout the application

#### ModalHeaderViewModel.cs
- **Purpose**: Standard view model for modal dialog headers
- **Key Properties**:
  - `Title`: The title text for the modal header
- **Constructor**: Takes title as a parameter for easy initialization
- **Usage**: Provides consistent modal header structure across all modal dialogs

#### PermissionTreeModalViewModel.cs
- **Purpose**: View model for the permission tree selection modal
- **Key Properties**:
  - `Permissions`: List of all available permissions (FlatPermissionDto)
  - `GrantedPermissionNames`: List of currently granted permission names
- **Implements**: `IPermissionsEditViewModel` interface
- **Usage**: Used in role and user permission management screens for viewing and editing permissions

### Key Components

#### Interfaces
- `IPermissionsEditViewModel`: Contract for permission editing view models

#### Dependencies
- `inzibackend.Authorization.Permissions.Dto`: Permission data transfer objects
- Standard .NET namespaces for collections and async operations

## Architecture Notes

### Design Patterns
- **View Model Pattern**: All classes follow the MVVM pattern for ASP.NET MVC
- **Single Responsibility**: Each modal view model handles a specific type of dialog
- **Composition**: Models are designed to be composed into larger view models

### Naming Conventions
- All view models end with "ViewModel" suffix
- Properties use PascalCase following C# conventions
- Clear, descriptive names that indicate purpose

## Business Logic
- Permission management through tree structure visualization
- Entity auditing and history tracking capabilities
- Generic lookup functionality for entity selection
- Standardized modal header presentation

## Usage Across Codebase
These modal view models are used by:
- Controllers in the App area for modal dialog data
- Razor views for rendering modal content
- JavaScript/TypeScript code for client-side modal management
- Permission management features (roles, users)
- Audit log viewing features
- Entity selection and lookup features

## Related Components
- Views: `/Areas/App/Views/Common/Modals/`
- JavaScript: `/wwwroot/view-resources/Areas/App/Views/Common/`
- Controllers: Various controllers in `/Areas/App/Controllers/`