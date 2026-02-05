# Models/Ui Documentation

## Overview
This folder contains view models specifically for the minimal UI components of the Web Host application. These models support the authentication interface and authenticated home page, providing data structures for server-side rendered views.

## Contents

### Files

#### LoginModel.cs
- **Purpose**: View model for the login form
- **Namespace**: `inzibackend.Web.Models.Ui`
- **Properties**:
  - `UserNameOrEmailAddress` (string, Required): User identifier for authentication
  - `Password` (string, Required, DisableAuditing): User password with audit logging disabled for security
  - `RememberMe` (bool): Persistent authentication session flag
  - `TenancyName` (string, Optional): Tenant identifier for multi-tenant login
- **Validation**: Uses DataAnnotations for required field validation
- **Security Features**:
  - Password field excluded from audit logs via `[DisableAuditing]` attribute
  - Prevents sensitive data from appearing in logs
  - Required fields enforce data completeness

#### HomePageModel.cs
- **Purpose**: View model for the authenticated home/index page
- **Namespace**: `inzibackend.Web.Models.Ui`
- **Properties**:
  - `IsMultiTenancyEnabled` (bool): Configuration flag indicating multi-tenancy support
  - `LoginInformation` (GetCurrentLoginInformationsOutput): Current user session data including user and tenant details
- **Methods**:
  - `GetShownLoginName()`: Returns formatted username with tenant context
    - Single-tenant mode: Returns just username with span wrapper
    - Host user (multi-tenant): Returns ".\\Username" format
    - Tenant user (multi-tenant): Returns "TenantName\\Username" format
    - Includes HTML span elements with IDs and classes for JavaScript manipulation
- **Design Notes**:
  - Generates HTML directly (unconventional but functional)
  - Provides visual distinction between host and tenant users
  - Username wrapped in span with ID "HeaderCurrentUserName" for scripting

### Key Components

#### Authentication View Models
- Capture user credentials securely
- Support multi-tenant login scenarios
- Integrate with ABP authentication system

#### Session Display Models
- Present current user context
- Show tenant affiliation clearly
- Provide data for home page rendering

### Dependencies
- **System.ComponentModel.DataAnnotations**: Validation attributes
- **Abp.Auditing**: Audit control (DisableAuditing)
- **inzibackend.Sessions.Dto**: Session information DTOs
- **ABP Framework**: Authentication and session management

## Architecture Notes

### Model Design Patterns
- **View Models**: Tailored for specific UI needs, not domain entities
- **Validation**: Client and server-side via DataAnnotations
- **Security**: Sensitive data protected from logging
- **Multi-Tenancy Aware**: Models understand tenant context

### Separation of Concerns
- UI-specific models (not reusable DTOs)
- Located in Web.Host project
- Different from Application.Shared DTOs
- Different from Core domain entities

### HTML Generation in Models
- `GetShownLoginName()` returns HTML string
- Unusual pattern (typically done in views)
- Suggests tight coupling to specific view requirements
- Includes CSS classes and element IDs

### Security Considerations
- Password never logged due to `[DisableAuditing]`
- Tenant isolation enforced
- Session data sanitized for display
- HTML encoding responsibility on view

## Business Logic

### Login Flow
1. User submits login form
2. LoginModel captures credentials
3. Optional tenant name for multi-tenant login
4. Model validation ensures required fields present
5. Controller authenticates using model data
6. Success redirects to return URL or home page

### Multi-Tenancy Support
- Tenant name field optional (empty for host users)
- Tenant identified before authentication
- Different authentication paths for host vs tenant
- Session contains tenant context

### User Display Logic
- Host users shown with ".\\" prefix (Windows convention)
- Tenant users shown with "TenantName\\" prefix
- Single-tenant mode shows username only
- Visual cue for current context

### Remember Me Feature
- Extends authentication session
- Persistent cookie created
- User stays logged in across browser restarts
- Security consideration for shared devices

## Usage Across Codebase

### Direct Consumers

#### UiController
```csharp
[HttpGet]
public async Task<ActionResult> Login(string returnUrl = null)
{
    return View(new LoginModel());
}

[HttpPost]
public async Task<ActionResult> Login(LoginModel model, string returnUrl = null)
{
    if (!ModelState.IsValid)
        return View(model);
    // Authenticate using model.UserNameOrEmailAddress, model.Password
    // Handle model.TenancyName for tenant resolution
    // Apply model.RememberMe for session persistence
}

public async Task<ActionResult> Index()
{
    var model = new HomePageModel
    {
        IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
        LoginInformation = await _sessionAppService.GetCurrentLoginInformations()
    };
    return View(model);
}
```

#### Razor Views

##### Views/Ui/Login.cshtml
```cshtml
@model inzibackend.Web.Models.Ui.LoginModel
<!-- Form fields bound to model properties -->
<input asp-for="UserNameOrEmailAddress" />
<input asp-for="Password" type="password" />
<input asp-for="RememberMe" type="checkbox" />
@if (MultiTenancyConfig.IsEnabled)
{
    <input asp-for="TenancyName" />
}
```

##### Views/Ui/Index.cshtml
```cshtml
@model inzibackend.Web.Models.Ui.HomePageModel
<div class="user-name">
    @L("YouAreAlreadyLoggedInWithUser"): @Html.Raw(Model.GetShownLoginName())
</div>
```

### Data Flow

#### Login Process
1. GET /ui/login → Controller returns empty LoginModel → View renders form
2. User fills form → POST /ui/login → LoginModel populated
3. Model validation → Authentication attempt
4. Success → Redirect with authentication cookie
5. Failure → Return view with model and errors

#### Home Page Display
1. Authenticated request to /ui/
2. Controller gets current login information
3. Creates HomePageModel with session data
4. View renders username with tenant context
5. Links to admin tools (Swagger, Hangfire, etc.)

### Integration Points
- **Authentication Service**: Uses model data for login
- **Session Service**: Provides login information for home page
- **Multi-Tenancy Config**: Determines tenant field visibility
- **Localization**: View uses localization for labels

## Development Notes

### Modifying Login Model

#### Adding Fields
```csharp
public class LoginModel
{
    // Existing fields...

    [Required]
    public string NewField { get; set; }
}
```
- Update corresponding view
- Add validation rules as needed
- Update controller logic

#### Validation Customization
- Use DataAnnotations attributes
- Custom validators for complex rules
- Client-side validation via unobtrusive JavaScript

### Modifying Home Page Model

#### Changing Display Logic
```csharp
public string GetShownLoginName()
{
    // Custom formatting logic
    // Consider extracting HTML to view
}
```

#### Adding Properties
- Add session information as needed
- Update controller to populate
- Update view to display

### Best Practices
- Keep view models simple and focused
- Avoid business logic in view models
- Use validation attributes for data integrity
- Protect sensitive data from logging
- Consider extracting HTML generation to views
- Document unusual patterns clearly

### Security Checklist
- [ ] Password field marked with [DisableAuditing]
- [ ] No sensitive data in plain text properties
- [ ] Tenant isolation validated before authentication
- [ ] HTML encoding in views (not models)
- [ ] Remember me feature documented for users
- [ ] Session timeout configured appropriately

### Testing Considerations
- Test with and without tenant name
- Verify password not logged
- Test remember me functionality
- Validate multi-tenant display format
- Test error handling and validation
- Verify HTML encoding in output