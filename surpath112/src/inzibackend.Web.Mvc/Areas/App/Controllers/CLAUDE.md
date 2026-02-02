# Controllers Documentation

## Overview
This folder contains all MVC controllers for the App area, which is the main authenticated application interface. These controllers handle HTTP requests, coordinate with application services, and return appropriate views or data for the student compliance tracking system. The controllers manage everything from user authentication to compliance tracking, department management, and system administration.

## Contents

### Core Controllers

#### HomeController.cs
- **Purpose**: Entry point controller that routes users based on permissions
- **Key Actions**:
  - `Index`: Redirects to appropriate dashboard based on user type (Host/Tenant)
- **Authorization**: Requires authentication (AbpMvcAuthorize)
- **Business Logic**: Multi-tenancy aware routing

#### MainController.cs
- **Purpose**: Main layout and navigation controller
- **Key Functionality**: Manages main application shell and menu structure
- **Authorization**: Authenticated access required

#### ComplianceController.cs
- **Purpose**: Core compliance tracking and management functionality
- **Key Features**:
  - File upload/download for compliance documents
  - Compliance status tracking
  - Record state management
  - Integration with RecordsAppService and SurpathComplianceAppService
- **Authorization**: Currently uses AbpAllowAnonymous (likely for testing)
- **File Handling**: Supports multiple file types (jpeg, jpg, png, pdf, txt, hl7)

### Entity Management Controllers

#### CohortsController.cs
- **Purpose**: Manages student cohorts/groups
- **Dependencies**: ICohortsAppService
- **Features**: CRUD operations for cohort management

#### CohortUsersController.cs
- **Purpose**: Manages users within cohorts
- **Dependencies**: ICohortUsersAppService
- **Features**: User-cohort association management

#### RecordsController.cs
- **Purpose**: Manages compliance records
- **Features**: Document tracking and management

#### RecordStatesController.cs
- **Purpose**: Tracks state changes for compliance records
- **Features**: Workflow and status management

#### RecordRequirementsController.cs
- **Purpose**: Defines compliance requirements
- **Features**: Requirement configuration and management

#### RecordCategoriesController.cs
- **Purpose**: Categorizes compliance records
- **Features**: Category management for organization

### Department & Organization Controllers

#### TenantDepartmentsController.cs
- **Purpose**: Manages departments within tenants
- **Features**: Department hierarchy and structure

#### DepartmentUsersController.cs
- **Purpose**: Associates users with departments
- **Features**: User-department relationships

#### OrganizationUnitsController.cs
- **Purpose**: Manages organizational hierarchy
- **Features**: Tree structure for organizations

### Medical & Testing Controllers

#### DrugTestCategoriesController.cs
- **Purpose**: Manages drug testing categories
- **Features**: Test category configuration

#### DrugsController.cs
- **Purpose**: Manages drug types and information
- **Features**: Drug database management

#### DrugPanelsController.cs
- **Purpose**: Configures drug testing panels
- **Features**: Panel composition and management

#### HospitalsController.cs
- **Purpose**: Manages hospital/facility information
- **Features**: Healthcare facility management

#### MedicalUnitsController.cs
- **Purpose**: Manages medical unit information
- **Features**: Medical department organization

### User & Authentication Controllers

#### UsersController.cs
- **Purpose**: User management and administration
- **Features**: User CRUD, role assignment, permissions

#### RolesController.cs
- **Purpose**: Role management and permissions
- **Features**: Role creation, permission assignment

#### ProfileController.cs
- **Purpose**: User profile management
- **Features**: Personal information, preferences

#### SessionController.cs
- **Purpose**: Session management
- **Features**: Login/logout, session tracking

### System Administration Controllers

#### SettingsController.cs
- **Purpose**: Application settings management
- **Features**: Tenant and host settings

#### HostSettingsController.cs
- **Purpose**: Host-specific settings
- **Features**: System-wide configuration

#### TenantsController.cs
- **Purpose**: Tenant management (multi-tenancy)
- **Features**: Tenant CRUD, feature management

#### EditionsController.cs
- **Purpose**: Edition/subscription tier management
- **Features**: Feature sets, pricing tiers

#### AuditLogsController.cs
- **Purpose**: Audit log viewing and management
- **Features**: Activity tracking, compliance auditing

#### MaintenanceController.cs
- **Purpose**: System maintenance operations
- **Features**: Cache management, system tasks

### Dashboard & Reporting Controllers

#### TenantDashboardController.cs
- **Purpose**: Tenant-specific dashboard
- **Features**: Compliance metrics, statistics

#### HostDashboardController.cs
- **Purpose**: Host/system dashboard
- **Features**: System-wide metrics

#### CustomizableDashboardControllerBase.cs
- **Purpose**: Base class for customizable dashboards
- **Features**: Widget management, personalization

### UI & Support Controllers

#### DemoUiComponentsController.cs
- **Purpose**: UI component demonstrations
- **Features**: Component showcase, examples

#### NotificationsController.cs
- **Purpose**: User notification management
- **Features**: Alert management, preferences

#### ChatController.cs
- **Purpose**: Real-time chat functionality
- **Features**: Messaging, support chat

#### CommonController.cs
- **Purpose**: Shared/common functionality
- **Features**: Utility actions, shared views

### Specialized Controllers

#### WebhookSubscriptionsController.cs
- **Purpose**: Webhook management
- **Features**: Event subscriptions, integrations

#### LanguagesController.cs
- **Purpose**: Language/localization management
- **Features**: Multi-language support

#### SubscriptionManagementController.cs
- **Purpose**: Subscription and billing
- **Features**: Payment processing, plan management

### Key Components

#### Base Classes
- `inzibackendControllerBase`: Common base controller with shared functionality
- `CustomizableDashboardControllerBase`: Base for dashboard controllers

#### Authorization
- Uses ABP authorization attributes
- Permission-based access control
- Multi-tenancy support

### Dependencies
- Application Services (IAppService interfaces)
- Domain managers (SurpathManager)
- ABP Framework components
- Entity Framework Core

## Architecture Notes

### Design Patterns
- **MVC Pattern**: Standard ASP.NET Core MVC implementation
- **Dependency Injection**: Constructor injection for services
- **Service Layer**: Controllers delegate to application services
- **Authorization Decorators**: Attribute-based security

### Conventions
- Controller names end with "Controller"
- Actions return IActionResult or Task<IActionResult>
- Area attribute marks App area membership
- Authorization attributes control access

## Business Logic

### Compliance Workflow
- Document upload and verification
- Requirement tracking and validation
- Status management and reporting
- Multi-level approval processes

### Multi-Tenancy
- Tenant isolation
- Host vs Tenant functionality
- Feature-based access control

### User Management
- Role-based permissions
- Department associations
- Cohort memberships

## Usage Across Codebase

### Direct Consumers
- Views in `/Areas/App/Views/`
- JavaScript in `/wwwroot/view-resources/Areas/App/`
- Application services in `inzibackend.Application`
- SignalR hubs for real-time features

### Related Components
- Models: `/Areas/App/Models/`
- Views: `/Areas/App/Views/`
- Client Scripts: `/wwwroot/view-resources/Areas/App/Views/`
- Application Layer: `inzibackend.Application`
- Domain Layer: `inzibackend.Core`