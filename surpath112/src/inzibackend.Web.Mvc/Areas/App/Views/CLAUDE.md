# Views Documentation

## Overview
This folder contains all Razor views for the App area of the MVC application. These views provide the user interface for the authenticated application area, including dashboards, entity management screens, modals, and shared layouts. The views follow ASP.NET Core MVC conventions with Razor syntax and are organized by controller/feature.

## Contents

### Configuration Files

#### _ViewStart.cshtml
- **Purpose**: Sets the default layout for all views in the App area
- **Key Features**:
  - Dynamic theme selection based on UiCustomizationSettings
  - Layout path determined by theme configuration
- **Usage**: Automatically applied to all views in this area

#### _ViewImports.cshtml
- **Purpose**: Global imports and tag helpers for all views
- **Imports**:
  - Abp.Localization for multi-language support
  - Base Razor page class (inzibackendRazorPage)
  - Microsoft and custom tag helpers
- **Inheritance**: All views inherit from inzibackendRazorPage<TModel>

### View Folders by Feature

#### AuditLogs/
- **Purpose**: Views for audit log display and filtering
- **Key Views**: Index, details, filtering modals
- **Features**: Activity tracking, compliance auditing

#### Cohorts/
- **Purpose**: Student cohort management views
- **Key Views**: Index, CreateOrEditModal, lookup modals
- **Features**: Group management, user assignment

#### CohortUsers/
- **Purpose**: Views for managing users within cohorts
- **Key Views**: Index, CreateOrEditModal, user lookup tables
- **Features**: User-cohort associations, bulk operations

#### Compliance/
- **Purpose**: Core compliance tracking and management views
- **Key Views**:
  - `_CreateNewRecordModal.cshtml`: New compliance record creation
  - `_CreateNewRecordForCategoryModal.cshtml`: Category-specific record creation
  - `_CreateEditRequirementModal.cshtml`: Requirement management
  - `_CohortLookupTableRegModal.cshtml`: Cohort selection
  - `_RecordCategoryLookupTableModal.cshtml`: Category selection
  - `_TenantDepartmentLookupTableRegModal.cshtml`: Department selection
- **Features**: Document upload, requirement tracking, status management

#### Records/
- **Purpose**: Compliance record management views
- **Key Views**: Index, CreateOrEditModal, ViewModal
- **Features**: Document tracking, status updates, history

#### RecordStates/
- **Purpose**: Record workflow state management views
- **Key Views**: Index, state transition modals
- **Features**: Workflow visualization, state changes

#### RecordRequirements/
- **Purpose**: Requirement definition and management views
- **Key Views**: Index, CreateOrEditModal, rule configuration
- **Features**: Requirement templates, validation rules

#### RecordCategories/
- **Purpose**: Category management for records
- **Key Views**: Index, CreateOrEditModal, hierarchy views
- **Features**: Category trees, assignment rules

#### TenantDepartments/
- **Purpose**: Department management within tenants
- **Key Views**: Index, CreateOrEditModal, hierarchy views
- **Features**: Department structure, user assignment

#### Users/
- **Purpose**: User management and administration views
- **Key Views**:
  - Index: User listing
  - CreateOrEditModal: User creation/editing
  - PermissionsModal: Permission assignment
  - LoginAttemptsModal: Security tracking
- **Features**: User CRUD, role assignment, permissions

#### Roles/
- **Purpose**: Role and permission management views
- **Key Views**:
  - Index: Role listing
  - CreateOrEditModal: Role management
  - PermissionsModal: Permission tree
- **Features**: Role hierarchy, permission assignment

#### Settings/
- **Purpose**: Application settings management views
- **Key Views**: Index, tenant settings, host settings
- **Features**: Configuration management, feature toggles

#### TenantDashboard/
- **Purpose**: Tenant-specific dashboard views
- **Key Views**: Index, widget components
- **Features**: Compliance metrics, charts, statistics

#### HostDashboard/
- **Purpose**: Host/system administrator dashboard
- **Key Views**: Index, system metrics
- **Features**: System-wide statistics, tenant overview

#### CustomizableDashboard/
- **Purpose**: Customizable dashboard framework
- **Key Views**: Index, widget management
- **Features**: Drag-drop widgets, personalization

#### Layout/
- **Purpose**: Shared layout templates by theme
- **Subfolders**:
  - `Default/`: Default theme layout
  - `Theme0/` through `Theme13/`: Various theme layouts
- **Key Files**:
  - `_Layout.cshtml`: Main layout template
  - `_Header.cshtml`: Header navigation
  - `_Sidebar.cshtml`: Side navigation menu
  - `_Footer.cshtml`: Footer content

#### Common/
- **Purpose**: Shared views and components
- **Subfolders**:
  - `Modals/`: Common modal dialogs
  - `Empty/`: Empty state templates
- **Key Components**:
  - Permission selection trees
  - Entity history viewers
  - Lookup modals

#### DemoUiComponents/
- **Purpose**: UI component demonstrations
- **Features**: Component showcase, usage examples

#### Notifications/
- **Purpose**: Notification management views
- **Key Views**: Index, settings modal
- **Features**: Alert preferences, notification history

#### Languages/
- **Purpose**: Language and localization management
- **Key Views**: Index, text editor, language selection
- **Features**: Translation management, locale settings

#### Editions/
- **Purpose**: Edition/subscription tier management
- **Key Views**: Index, CreateOrEditModal, feature assignment
- **Features**: Feature sets, pricing configuration

#### Tenants/
- **Purpose**: Multi-tenant management views
- **Key Views**: Index, CreateOrEditModal, feature modal
- **Features**: Tenant CRUD, feature management

### Shared Components

#### Shared/
- **Purpose**: Views shared across multiple controllers
- **Key Components**:
  - Error pages
  - Partial views
  - View components
- **Features**: Reusable UI elements

### Key Components

#### Modal Patterns
- CreateOrEditModal: Standard CRUD modal pattern
- LookupTableModal: Entity selection pattern
- PermissionsModal: Permission tree pattern

#### View Models
- Strongly typed models for each view
- DTOs from application layer
- Custom view models in Models folder

#### JavaScript Integration
- Corresponding JS files in wwwroot/view-resources
- AJAX-based modal loading
- DataTables integration

## Architecture Notes

### Design Patterns
- **Partial Views**: Reusable view components
- **Modal Dialogs**: Consistent modal patterns
- **Master-Detail**: List and detail view patterns
- **Theme Support**: Multiple theme layouts

### Conventions
- Views match controller action names
- Modals prefixed with underscore
- Shared views in Shared folder
- Theme-specific layouts in Layout folder

### Localization
- Uses IStringLocalizer for translations
- L() helper method for localized strings
- Resource files for multiple languages

## Business Logic

### View Responsibilities
- Presentation only - no business logic
- Data binding and display
- Form validation UI
- User interaction handling

### Data Flow
- Controllers prepare view models
- Views render HTML
- JavaScript handles interactions
- AJAX calls to controller actions

## Usage Across Codebase

### Direct Consumers
- Controllers return these views
- JavaScript files manipulate view elements
- CSS styles view appearance
- Tag helpers enhance functionality

### Related Components
- Controllers: `/Areas/App/Controllers/`
- Models: `/Areas/App/Models/`
- JavaScript: `/wwwroot/view-resources/Areas/App/Views/`
- CSS: `/wwwroot/view-resources/Areas/App/Views/`
- Application Services: `inzibackend.Application`

## View Helpers and Extensions
- Custom tag helpers for common patterns
- HTML helpers for form elements
- Localization helpers for translations
- Permission checking helpers for UI visibility