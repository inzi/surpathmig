# Views/Ui Documentation

## Overview
This folder contains Razor views for the minimal authentication UI in the Web Host application. These views provide a simple, functional interface for login and authenticated home page display, primarily used when accessing the API host directly or for IdentityServer flows.

## Contents

### Files

#### Login.cshtml
- **Purpose**: Login form view for user authentication
- **Model**: `inzibackend.Web.Models.Ui.LoginModel`
- **Features**:
  - Username or email address input
  - Password input (secure, no autocomplete)
  - Optional tenant name field (multi-tenancy)
  - Remember me checkbox for persistent sessions
  - Return URL handling for post-login redirect
  - Version display in footer
- **Security**:
  - Anti-forgery token automatically set via `IAbpAntiForgeryManager.SetCookie()`
  - CSRF protection built-in
  - Secure password field
- **Conditional Display**:
  - Tenant name field only shown if `MultiTenancyConfig.IsEnabled`
  - Adapts UI based on configuration
- **Styling**:
  - Loads Login.css for form styling
  - `.login-form` container
  - `.login-form-row` for each field
  - `.login-form-button` for submit button
- **Localization**:
  - All labels and placeholders localized via `L()` helper
  - Keys: "LogIn", "TenancyName", "UserNameOrEmail", "Password", "RememberMe"
- **Footer Info**:
  - Application version from `AppVersionHelper.Version`
  - Release date in yyyyMMdd format

#### Index.cshtml
- **Purpose**: Authenticated home page for logged-in users
- **Model**: `inzibackend.Web.Models.Ui.HomePageModel`
- **Features**:
  - Displays logged-in user with tenant context
  - Links to developer/admin tools:
    - Swagger UI (API documentation)
    - Hangfire Dashboard (background jobs)
    - GraphQL Playground (GraphQL API testing)
  - Logout link
- **Security**:
  - Anti-forgery cookie with strict SameSite and Secure flags
  - Permission checks for Hangfire dashboard
- **Conditional Links**:
  - Swagger UI: Shown if `WebConsts.SwaggerUiEnabled`
  - Hangfire: Shown if enabled AND user has `AppPermissions.Pages_Administration_HangfireDashboard`
  - GraphQL: Shown if `WebConsts.GraphQL.Enabled` AND `WebConsts.GraphQL.PlaygroundEnabled`
- **User Display**:
  - Shows "You are already logged in with user: {username}"
  - Username formatted with tenant prefix via `Model.GetShownLoginName()`
  - HTML rendered raw for tenant context (.\\ or TenantName\\)
- **Styling**:
  - Loads Index.css for page layout
  - `.main-content` container
  - `.user-name` for user display
  - `.content-list` for admin tool links
  - `.logout` for logout link

### Key Components

#### Authentication Flow
1. Unauthenticated user accesses protected resource
2. Redirected to Login.cshtml with return URL
3. User submits credentials (and optional tenant name)
4. Controller validates and authenticates
5. Success → Redirect to return URL or Index.cshtml
6. Failure → Return to Login.cshtml with error

#### Home Page Features
- Quick access to developer tools
- User context display
- Simple navigation
- Clean logout option

### Dependencies
- **ABP Framework**:
  - `IAbpAntiForgeryManager`: CSRF token management
  - `IMultiTenancyConfig`: Multi-tenancy configuration
  - Localization support
- **Model Classes**:
  - `LoginModel`: Login form data
  - `HomePageModel`: Authenticated user data
- **Helpers**:
  - `AppVersionHelper`: Application version information
  - `WebConsts`: Feature flags and endpoints
- **Authorization**:
  - `IsGranted()`: Permission checking
  - `AppPermissions`: Permission constants
- **Styling**:
  - Login.css: Form styles
  - Index.css: Home page styles

## Architecture Notes

### View Inheritance
- Both views inherit from `inzibackendRazorPage<TModel>`
- Strongly-typed models for compile-time safety
- Built-in localization support via base class

### Security Integration
- CSRF tokens automatically managed
- Secure cookie flags (SameSite=Strict, Secure)
- Permission-based conditional rendering
- Tenant isolation maintained throughout

### Configuration-Driven UI
- Features enabled/disabled via configuration
- No code changes needed for feature toggles
- Consistent with application-wide settings

### Minimal UI Philosophy
- These views are not the main application UI
- Primary purpose: Authentication for API access
- Secondary purpose: Quick access to dev tools
- Actual application is separate SPA (Angular/React)

## Business Logic

### Login Process
1. **Tenant Resolution** (if multi-tenant):
   - Optional tenant name entered
   - Resolved before authentication
   - Sets tenant context for session

2. **Authentication**:
   - Username or email validated
   - Password checked against database
   - Two-factor authentication if enabled
   - External login providers (if configured)

3. **Session Creation**:
   - Authentication cookie set
   - Remember me extends cookie lifetime
   - Session information stored
   - User redirected to original request or home

### Home Page Logic
1. **User Context Display**:
   - Shows current username
   - Includes tenant prefix (multi-tenant)
   - Visual distinction for host vs tenant users

2. **Tool Access**:
   - Links shown based on configuration
   - Permission checks for sensitive tools
   - Direct navigation to admin interfaces

3. **Logout**:
   - Clears authentication cookie
   - Ends session
   - Redirects to login page

### Multi-Tenancy Support
- Host users: Login without tenant name
- Tenant users: Specify tenant name
- Display shows tenant context clearly
- Tenant-specific configuration applied

## Usage Across Codebase

### Controllers

#### UiController
```csharp
public class UiController : inzibackendControllerBase
{
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Resolve tenant if provided
        if (!string.IsNullOrEmpty(model.TenancyName))
        {
            await _tenantCache.GetOrNullAsync(model.TenancyName);
        }

        // Authenticate
        var result = await _signInManager.PasswordSignInAsync(
            model.UserNameOrEmailAddress,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return Redirect(returnUrl ?? "/ui");
        }

        ModelState.AddModelError("", "Invalid login attempt");
        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var model = new HomePageModel
        {
            IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
            LoginInformation = await _sessionAppService.GetCurrentLoginInformations()
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}
```

### Integration Points

#### Startup.cs
```csharp
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Ui}/{action=Index}/{id?}");
});
```

#### Feature Configuration (appsettings.json)
```json
{
  "WebConsts": {
    "SwaggerUiEnabled": true,
    "SwaggerUiEndPoint": "/swagger",
    "HangfireDashboardEnabled": true,
    "HangfireDashboardEndPoint": "/hangfire",
    "GraphQL": {
      "Enabled": false,
      "PlaygroundEnabled": false,
      "PlaygroundEndPoint": "/ui/playground"
    }
  }
}
```

## Development Notes

### Customizing Login Page

#### Adding Fields
```cshtml
<div class="login-form-row">
    <label for="NewField">@L("NewFieldLabel")</label>
    <input type="text" id="NewField" name="NewField" placeholder="@L("NewFieldPlaceholder")">
</div>
```

#### Adding External Login Providers
```cshtml
<div class="external-login-providers">
    <h3>@L("OrLoginWith")</h3>
    <a href="/account/external-login/google" class="btn btn-google">
        <i class="fa fa-google"></i> Google
    </a>
    <a href="/account/external-login/microsoft" class="btn btn-microsoft">
        <i class="fa fa-microsoft"></i> Microsoft
    </a>
</div>
```

### Customizing Home Page

#### Adding More Links
```cshtml
<ul class="content-list">
    @if (WebConsts.CustomFeatureEnabled)
    {
        <li><a href="@WebConsts.CustomFeatureEndPoint">Custom Feature</a></li>
    }
    <!-- Existing links -->
</ul>
```

#### Enhanced User Display
```cshtml
<div class="user-info-card">
    <div class="user-avatar">
        <img src="@Model.LoginInformation.User.ProfilePictureUrl" alt="Profile">
    </div>
    <div class="user-details">
        <h3>@Model.LoginInformation.User.Name</h3>
        <p>@Model.LoginInformation.User.EmailAddress</p>
        @if (Model.IsMultiTenancyEnabled && Model.LoginInformation.Tenant != null)
        {
            <span class="tenant-badge">@Model.LoginInformation.Tenant.TenancyName</span>
        }
    </div>
</div>
```

### Testing Views
1. **Login Flow**:
   - Test with valid credentials
   - Test with invalid credentials
   - Test remember me functionality
   - Test return URL handling
   - Test multi-tenant login

2. **Home Page**:
   - Verify user display with tenant context
   - Check conditional link visibility
   - Test permission-based access
   - Verify logout functionality

3. **Security**:
   - Confirm CSRF protection
   - Test cookie security flags
   - Verify tenant isolation
   - Check permission enforcement

### Best Practices
- Keep views simple and focused
- Use localization for all text
- Implement proper error handling
- Follow accessibility guidelines
- Test across browsers and devices
- Maintain consistent styling
- Document configuration options
- Provide clear user feedback

### Accessibility Checklist
- [ ] Form labels associated with inputs
- [ ] Keyboard navigation works correctly
- [ ] Focus indicators visible
- [ ] Error messages announced to screen readers
- [ ] Sufficient color contrast
- [ ] Semantic HTML elements used
- [ ] ARIA labels where needed

### Security Checklist
- [ ] CSRF tokens present and validated
- [ ] Password field not autocomplete
- [ ] HTTPS enforced
- [ ] Cookie security flags set
- [ ] Input validation on server side
- [ ] Rate limiting on login attempts
- [ ] Audit logging for authentication events
- [ ] Tenant isolation validated