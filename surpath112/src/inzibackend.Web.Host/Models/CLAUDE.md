# Models Documentation

## Overview
The Models folder contains view models and data transfer objects used by the Web Host application's UI components. These models are specifically for the minimal UI provided by the API host, primarily for IdentityServer login scenarios.

## Contents

### Files
No files exist directly in the Models folder.

### Key Components
All model classes are organized in the Ui subfolder for UI-specific view models.

### Dependencies
- System.ComponentModel.DataAnnotations (validation)
- Abp.Auditing (audit control)
- inzibackend.Sessions.Dto (session DTOs)

## Subfolders

### Ui
Contains view models for the minimal UI provided by the Web Host application.

#### LoginModel.cs
- **Purpose**: View model for the login form
- **Properties**:
  - `UserNameOrEmailAddress` (Required): User identifier for login
  - `Password` (Required, DisableAuditing): User password - auditing disabled for security
  - `RememberMe`: Persistent authentication option
  - `TenancyName`: Optional tenant identifier for multi-tenant login
- **Validation**: Uses DataAnnotations for required field validation
- **Security**: Password field excluded from audit logs via DisableAuditing attribute

#### HomePageModel.cs
- **Purpose**: View model for the authenticated home page
- **Properties**:
  - `IsMultiTenancyEnabled`: Configuration flag for multi-tenancy
  - `LoginInformation`: Current user session information
- **Methods**:
  - `GetShownLoginName()`: Formats display name with tenant prefix in multi-tenant mode
    - Single tenant: "Username"
    - Host user: ".\\Username"
    - Tenant user: "TenantName\\Username"
- **HTML Generation**: Includes span elements with IDs for JavaScript manipulation

## Architecture Notes

### Model Design Patterns
1. **View Models**: Specific to UI needs, not domain entities
2. **Validation**: Client and server-side validation via DataAnnotations
3. **Security**: Sensitive fields marked with DisableAuditing
4. **Multi-Tenancy**: Models aware of tenant context

### Separation of Concerns
- These models are only for the minimal UI in Web.Host
- Main application DTOs are in Application.Shared project
- Domain entities are in Core project

### HTML in Models
- GetShownLoginName() generates HTML - unusual pattern
- Suggests these views are server-rendered Razor views
- IDs included for JavaScript interaction

## Business Logic

### Login Flow
1. LoginModel captures credentials and tenant
2. Optional tenant name for multi-tenant scenarios
3. Remember me option for persistent sessions
4. Password never logged for security

### User Display
1. Shows username with tenant context
2. Visual distinction between host and tenant users
3. Formatted for display in UI header

## Usage Across Codebase

### Direct Consumers

#### UiController
- Uses LoginModel for login action
- Uses HomePageModel for index view
- Validates model state before processing

#### Razor Views
- Views/Ui/Login.cshtml uses LoginModel
- Views/Ui/Index.cshtml uses HomePageModel
- HTML helper methods called in views

### Data Flow
1. User submits login form → LoginModel
2. Controller validates and processes
3. Success → HomePageModel with session info
4. View renders with formatted username

### Security Considerations
- Password field never included in logs
- Tenant validation before authentication
- Session information sanitized for display