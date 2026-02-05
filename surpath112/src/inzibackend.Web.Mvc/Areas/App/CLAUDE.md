# App Area Documentation

## Overview
The App area is the main authenticated application area of the inzibackend MVC web application. It provides the complete user interface for the student compliance tracking system, including dashboards, entity management, administration, and multi-tenant support. This area follows the ASP.NET Core Areas pattern to organize related functionality.

## Contents

### Core Components

#### Controllers/
- **Purpose**: HTTP request handlers for all application features
- **Key Controllers**:
  - ComplianceController: Core compliance tracking
  - CohortsController: Student group management
  - UsersController: User administration
  - TenantsController: Multi-tenant management
  - HomeController: Application entry point
- **Features**: 72+ controllers handling all business operations
- **See**: [Controllers/CLAUDE.md](Controllers/CLAUDE.md) for detailed documentation

#### Models/
- **Purpose**: View models for strongly-typed views
- **Organization**: Organized by feature/entity
- **Key Model Groups**:
  - Common: Shared models and interfaces
  - Compliance: Compliance tracking models
  - Cohorts/CohortUsers: Group management
  - Records: Document and requirement tracking
- **See**: [Models/CLAUDE.md](Models/CLAUDE.md) for detailed documentation

#### Views/
- **Purpose**: Razor views for user interface
- **Organization**: Folder per controller/feature
- **Key View Groups**:
  - Layout: Theme-based layouts
  - Common: Shared components
  - Dashboard views
  - Entity management views
- **See**: [Views/CLAUDE.md](Views/CLAUDE.md) for detailed documentation

#### Startup/
- **Purpose**: Area configuration and initialization
- **Key Files**:
  - `AppNavigationProvider.cs`: Menu structure definition
  - `AppPageNames.cs`: Page name constants
  - `AppAreaRegistration.cs`: Area registration
- **Features**: Navigation setup, permission-based menu items

### Key Components

#### AppNavigationProvider.cs
- Defines the application menu structure
- Permission-based menu visibility
- Hierarchical menu organization
- Localized menu labels

#### AppPageNames.cs
- Constants for page identification
- Organized by namespace (Common, Host, Tenant)
- Used for navigation and permissions

## Architecture Notes

### Area Organization
- **MVC Pattern**: Standard ASP.NET Core MVC implementation
- **Feature Folders**: Organized by business feature
- **Separation of Concerns**: Clear separation between controllers, models, and views
- **Multi-tenancy**: Built-in tenant isolation and host features

### Security Model
- **Authorization**: ABP authorization framework
- **Permissions**: Granular permission system
- **Multi-tenant**: Tenant data isolation
- **Audit Logging**: Comprehensive activity tracking

### Theme Support
- Multiple theme layouts (Theme0-Theme13)
- Dynamic theme selection
- Responsive design support
- Customizable UI components

## Business Logic

### Core Workflows

#### Compliance Management
- Document upload and verification
- Requirement tracking
- Status management
- Multi-level approvals
- Deadline monitoring

#### Cohort Management
- Student grouping
- Department associations
- Rotation scheduling
- Bulk operations

#### User Management
- Role-based access control
- Department assignments
- Permission management
- Profile management

#### Multi-Tenancy
- Tenant isolation
- Feature management
- Subscription handling
- Billing integration

### Navigation Structure
```
Main Menu
├── Dashboard (Tenant/Host)
├── Compliance
│   ├── Records
│   ├── Requirements
│   └── Categories
├── Cohorts
│   ├── Manage Cohorts
│   └── Cohort Users
├── Departments
│   ├── Department Structure
│   └── Department Users
├── Administration
│   ├── Users
│   ├── Roles
│   ├── Settings
│   └── Audit Logs
└── System (Host Only)
    ├── Tenants
    ├── Editions
    └── Maintenance
```

## Dependencies

### Framework Dependencies
- ASP.NET Core MVC
- ABP Framework
- Entity Framework Core
- SignalR (real-time features)

### Application Layer
- `inzibackend.Application`: Business services
- `inzibackend.Application.Shared`: DTOs and interfaces
- `inzibackend.Core`: Domain entities
- `inzibackend.EntityFrameworkCore`: Data access

### Client-Side
- jQuery and jQuery plugins
- DataTables for grids
- Select2 for dropdowns
- Bootstrap for UI framework
- Custom JavaScript in wwwroot

## Usage Across Codebase

### Entry Points
- `/App/Home/Index`: Main application entry
- `/App/TenantDashboard`: Tenant dashboard
- `/App/HostDashboard`: Host dashboard

### Integration Points
- Application services via dependency injection
- SignalR hubs for real-time updates
- Background jobs for async processing
- External API endpoints

### Related Components
- `/wwwroot/view-resources/Areas/App/`: Client-side scripts and styles
- `/Controllers/`: Root-level controllers
- `/Views/Shared/`: Shared views and components
- `/Models/`: Root-level models

## Subfolders

### Controllers/
Contains all MVC controllers organized by feature. Each controller handles HTTP requests and coordinates with application services. See [Controllers/CLAUDE.md](Controllers/CLAUDE.md).

### Models/
View models organized by feature/entity providing data structures for views. Includes common models, entity-specific models, and dashboard models. See [Models/CLAUDE.md](Models/CLAUDE.md).

### Views/
Razor views organized by controller with shared layouts and components. Includes theme-specific layouts and reusable partial views. See [Views/CLAUDE.md](Views/CLAUDE.md).

### Startup/
Configuration classes for the App area including navigation providers and page name definitions.

## Configuration

### Area Registration
- Registered as "App" area
- Custom route configuration
- Default route: `App/{controller}/{action}/{id?}`

### Permissions
- Permission-based menu items
- Controller-level authorization
- Action-level permissions
- Feature-based access control

### Localization
- Multi-language support
- Resource files for translations
- Localized menu items and labels
- Culture-specific formatting