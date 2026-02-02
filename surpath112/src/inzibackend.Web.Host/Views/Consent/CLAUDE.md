# Views/Consent Documentation

## Overview
This folder contains Razor views for the OAuth/IdentityServer consent screen. These views are displayed when an external application requests permission to access user data or perform actions on behalf of the user.

## Contents

### Files

#### Index.cshtml
- **Purpose**: Main OAuth consent form view
- **Model**: `inzibackend.Web.Models.Consent.ConsentViewModel`
- **Features**:
  - Displays requesting client information
  - Shows requested identity scopes (personal information permissions)
  - Shows requested API scopes (application access permissions)
  - Provides Allow/Deny buttons for user decision
  - Optional "Remember my decision" checkbox
- **Layout**:
  - Client logo displayed if provided
  - Heading explaining permission request
  - Two panels for identity and API scopes
  - Consent buttons at bottom
- **Styling**:
  - Uses Login.css for consistent appearance
  - Custom inline styles for button layout
  - Bootstrap panel components
  - Glyphicon icons for visual context
- **Configuration**:
  - `ViewBag.DisableTenantChange = true`: Prevents tenant switching during consent flow
  - Hidden ReturnUrl field for post-consent redirect

#### _ScopeListItem.cshtml
- **Purpose**: Partial view for individual scope/permission display
- **Model**: Individual scope object (from IdentityScopes or ApiScopes collection)
- **Features**:
  - Reusable component for each permission
  - Displays scope name and description
  - Shows checkbox or read-only indicator
  - Consistent formatting across all scopes
- **Usage**: Rendered via `@await Html.PartialAsync("_ScopeListItem", scope)` in Index.cshtml

### Key Components

#### OAuth Consent Flow
- User authenticates with application
- External client requests specific permissions
- Consent view displays requested scopes
- User approves or denies access
- Decision recorded and user redirected

#### Scope Display
- **Identity Scopes**: Personal information (profile, email, phone, etc.)
- **API Scopes**: Application functionality access
- Separate panels for clarity
- Visual distinction with icons

#### User Decision Capture
- Allow button (autofocus for quick approval)
- Deny button (explicit rejection)
- Remember consent checkbox (optional, controlled by client)
- Form post with button value ("yes" or "no")

### Dependencies
- **IdentityServer4**: OAuth/OpenID Connect framework
- **ABP Framework**: Localization and base page
- **Bootstrap**: Panel and form styling
- **Glyphicons**: Icons for visual enhancement
- **Login.css**: Shared authentication UI styles

## Architecture Notes

### View Inheritance
- Inherits from `inzibackendRazorPage<ConsentViewModel>`
- Base page provides localization (`L()` method)
- Strongly-typed model binding

### Consent Flow Integration
1. Client application initiates OAuth flow
2. User redirected to consent endpoint
3. ConsentController prepares ConsentViewModel
4. Index.cshtml renders consent form
5. User submits decision
6. Controller processes and redirects back to client

### Partial View Pattern
- `_ScopeListItem.cshtml` promotes reusability
- Consistent scope rendering
- Easier to maintain and update
- Follows ASP.NET Core conventions (underscore prefix)

### Security Considerations
- Tenant switching disabled during flow
- ReturnUrl validated to prevent open redirect
- CSRF protection via form tokens
- User must be authenticated before seeing consent

## Business Logic

### Consent Decision Logic
1. User reviews requested permissions
2. Client information displayed for transparency
3. Scopes grouped by type (identity vs API)
4. User makes informed decision
5. Optional persistent consent for convenience

### Remember Consent Feature
- Controlled by `AllowRememberConsent` flag
- Stores user decision for future requests
- Same client + scopes = automatic approval
- User can revoke in profile settings

### Scope Presentation
- Identity scopes: "Personal Informations" panel with user icon
- API scopes: "Application Access" panel with tasks icon
- Each scope rendered via partial view
- Only non-empty collections shown

### Multi-Language Support
- All text localized via `L()` helper
- Keys: "ClientIsRequestingYourPermission", "PersonalInformations", "ApplicationAccess", "RememberMyDecision", "Allow", "DoNotAllow"
- Supports all languages configured in application

## Usage Across Codebase

### Controllers Using Consent Views

#### ConsentController
```csharp
[HttpGet]
public async Task<IActionResult> Index(string returnUrl)
{
    var model = await BuildConsentViewModel(returnUrl);
    if (model == null)
    {
        // Invalid request
        return RedirectToAction("Error");
    }
    return View(model);
}

[HttpPost]
public async Task<IActionResult> Index(string button, ConsentInputModel model)
{
    if (button == "yes")
    {
        // Grant consent
        await _interaction.GrantConsentAsync(model.ReturnUrl,
            model.ScopesConsented,
            model.RememberConsent);
    }
    else
    {
        // Deny consent
        await _interaction.DenyAuthorizationAsync(model.ReturnUrl);
    }

    return Redirect(model.ReturnUrl);
}
```

### IdentityServer Integration
- Configured in Startup.cs
- Consent endpoint: `/Consent`
- Part of OAuth/OpenID Connect flow
- Integrates with IdentityServer middleware

### Client Configuration
```csharp
new Client
{
    ClientId = "external_app",
    RequireConsent = true,  // Forces consent screen
    AllowRememberConsent = true,  // Enables remember checkbox
    // ...
}
```

### Scope Definitions
```csharp
new ApiScope("api.read", "Read access to API"),
new ApiScope("api.write", "Write access to API"),
new IdentityResource[] {
    new IdentityResources.OpenId(),
    new IdentityResources.Profile(),
    new IdentityResources.Email()
}
```

## Development Notes

### Customizing Consent Screen

#### Modifying Layout
1. Edit Index.cshtml structure
2. Update CSS in Login.css or add consent-specific styles
3. Maintain accessibility (keyboard navigation, screen readers)
4. Test with various scope counts

#### Adding Scope Information
```cshtml
@foreach (var scope in Model.IdentityScopes)
{
    <div class="scope-item">
        <strong>@scope.Name</strong>
        <p>@scope.Description</p>
        @if (scope.Emphasize)
        {
            <span class="badge">Important</span>
        }
    </div>
}
```

#### Styling Buttons
```css
.consent-buttons button {
    display: inline-block;
    width: 49%;
    margin: 5px 0;
}

.consent-buttons button[value="yes"] {
    background-color: #4CAF50;
}

.consent-buttons button[value="no"] {
    background-color: #f44336;
}
```

### Modifying Partial View

#### _ScopeListItem.cshtml Customization
```cshtml
@model ScopeViewModel
<li class="list-group-item">
    <div class="scope-details">
        <input type="checkbox"
               name="ScopesConsented"
               value="@Model.Name"
               checked="@Model.Checked"
               disabled="@Model.Required" />
        <strong>@Model.DisplayName</strong>
        @if (!string.IsNullOrEmpty(Model.Description))
        {
            <p class="help-text">@Model.Description</p>
        }
        @if (Model.Required)
        {
            <span class="required-badge">Required</span>
        }
    </div>
</li>
```

### Testing Consent Flow
1. Configure test client with RequireConsent = true
2. Initiate OAuth flow from external app
3. Verify consent screen displays
4. Test Allow button → permissions granted
5. Test Deny button → access denied
6. Test Remember Consent → subsequent requests auto-approved
7. Verify localization with different languages
8. Test with various scope combinations

### Common Scenarios
- **First-time access**: User sees full consent screen
- **Remembered consent**: User bypasses screen
- **Updated scopes**: User sees consent again if new scopes requested
- **Revoked consent**: User must re-consent

### Troubleshooting
- **Consent not showing**: Check `RequireConsent` in client config
- **Invalid return URL**: Validate redirect URI in client configuration
- **Scopes not displaying**: Verify scope definitions and inclusion in request
- **Remember consent not working**: Check `AllowRememberConsent` setting
- **Styling issues**: Verify Login.css loaded correctly

### Security Best Practices
- Always validate return URL against configured redirect URIs
- Display client information clearly (name, logo)
- Explain each scope in user-friendly language
- Allow users to deny specific scopes (if supported)
- Provide link to manage consented applications
- Log consent decisions for audit purposes
- Implement consent expiration policy
- Allow users to revoke consent anytime