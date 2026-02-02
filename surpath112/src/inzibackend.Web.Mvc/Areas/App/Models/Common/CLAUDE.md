# Common Models Documentation

## Overview
This folder contains common view models, interfaces, and tree item models that are shared across multiple features in the App area. These models provide standardized structures for permissions, features, and organization units management with hierarchical tree representations.

## Contents

### Files

#### IPermissionsEditViewModel.cs
- **Purpose**: Interface defining contract for permission editing view models
- **Key Properties**:
  - `Permissions`: List of available permissions as FlatPermissionDto
  - `GrantedPermissionNames`: List of currently granted permission names
- **Usage**: Implemented by view models that handle permission editing (roles, users)

#### IFeatureEditViewModel.cs
- **Purpose**: Interface for feature management view models
- **Key Properties**:
  - `FeatureValues`: Current feature values as name-value pairs
  - `Features`: List of available features as FlatFeatureDto
- **Usage**: Used in edition and tenant feature management

#### IOrganizationUnitsEditViewModel.cs
- **Purpose**: Interface for organization unit assignment view models
- **Key Properties**:
  - `AllOrganizationUnits`: Complete list of organization units
  - `MemberedOrganizationUnits`: List of units the entity belongs to
- **Usage**: Used for managing user and role organization unit assignments

#### PermissionTreeItemModel.cs
- **Purpose**: Model for representing permissions in a hierarchical tree structure
- **Key Properties**:
  - `EditModel`: The IPermissionsEditViewModel containing permission data
  - `ParentName`: Name of the parent permission for hierarchy
- **Constructor Overloads**: Default and parameterized for flexibility
- **Usage**: Building permission trees in UI for visual hierarchy

#### OrganizationUnitTreeItemModel.cs
- **Purpose**: Model for representing organization units in tree structure
- **Key Properties**:
  - `EditModel`: The IOrganizationUnitsEditViewModel with unit data
  - `ParentId`: ID of parent organization unit (nullable for root)
- **Constructor Overloads**: Default and parameterized initialization
- **Usage**: Displaying organizational hierarchy in tree views

#### FeatureTreeItemModel.cs
- **Purpose**: Model for hierarchical feature representation
- **Key Properties**:
  - `EditModel`: The IFeatureEditViewModel with feature data
  - `ParentName`: Name of parent feature for tree structure
- **Constructor Overloads**: Supports both default and initialized construction
- **Usage**: Building feature trees for edition management

### Key Components

#### Interfaces
- Permission management contracts
- Feature management contracts
- Organization unit management contracts

#### Tree Models
- Hierarchical data representation
- Parent-child relationship tracking
- Composition of edit models with hierarchy

### Dependencies
- `inzibackend.Authorization.Permissions.Dto`: Permission DTOs
- `inzibackend.Editions.Dto`: Edition and feature DTOs
- `inzibackend.Organizations.Dto`: Organization unit DTOs
- `Abp.Application.Services.Dto`: ABP framework DTOs

## Subfolders

### Modals
Contains specialized view models for modal dialogs including permission trees, entity history, lookups, and modal headers. See [Modals/CLAUDE.md](Modals/CLAUDE.md) for detailed documentation.

## Architecture Notes

### Design Patterns
- **Interface Segregation**: Clean contracts for specific editing scenarios
- **Composite Pattern**: Tree models compose edit models with hierarchy
- **View Model Pattern**: All models follow MVVM for ASP.NET MVC
- **Dependency Injection Ready**: Interfaces enable DI and testing

### Conventions
- Interfaces prefixed with "I"
- Tree models suffixed with "TreeItemModel"
- Edit view model interfaces suffixed with "EditViewModel"
- Consistent property naming across similar models

## Business Logic

### Permission Management
- Flat permission lists transformed to hierarchical trees
- Support for granted vs available permission distinction
- Parent-child permission relationships

### Feature Management
- Feature values separate from feature definitions
- Hierarchical feature organization
- Name-value pair pattern for flexibility

### Organization Units
- Membership tracking for users and roles
- Hierarchical organizational structure
- Nullable parent for root units

## Usage Across Codebase

These common models are used extensively by:
- **Controllers**: Role, User, Edition, Organization controllers
- **Views**: Permission trees, feature editors, organization selectors
- **JavaScript**: Tree rendering and manipulation
- **Services**: Permission and feature checking logic

### Direct Consumers
- `/Areas/App/Controllers/RolesController.cs`
- `/Areas/App/Controllers/UsersController.cs`
- `/Areas/App/Controllers/EditionsController.cs`
- `/Areas/App/Views/Roles/`
- `/Areas/App/Views/Users/`
- `/wwwroot/view-resources/Areas/App/Views/Roles/`
- `/wwwroot/view-resources/Areas/App/Views/Users/`

## Related Components
- Application Layer: `inzibackend.Application` services
- Domain Layer: Permission and feature definitions
- Client-Side: JavaScript tree components and handlers