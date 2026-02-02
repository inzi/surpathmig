# Views Documentation

## Overview
This folder contains Razor views for root-level (public) controllers of the MVC application, including authentication pages, tenant registration, payment processing, error pages, and application installation. These are non-authenticated pages accessible before login.

## Subfolders

### Account/
**Purpose**: Authentication and account management views

**Pages**:
- `Login.cshtml`: Login form with username/password and external providers
- `Register.cshtml`: New user registration form
- `ForgotPassword.cshtml`: Password reset request
- `ResetPassword.cshtml`: Password reset with token
- `EmailConfirmation.cshtml`: Email verification page
- `SendSecurityCode.cshtml`: 2FA provider selection
- `VerifySecurityCode.cshtml`: 2FA code entry
- `AccessDenied.cshtml`: Unauthorized access message
- `Lockout.cshtml`: Account lockout notification

**Features**:
- Form validation (client and server)
- External authentication provider buttons
- CAPTCHA integration
- Responsive design
- Multi-language support

### TenantRegistration/
**Purpose**: Tenant self-registration views

**Pages**:
- `SelectEdition.cshtml`: Subscription plan selection
- `Register.cshtml`: Tenant registration form
- `RegisterResult.cshtml`: Registration confirmation

**Features**:
- Edition comparison table
- Trial period information
- Pricing display
- Organization information collection
- Administrator account creation

### Install/
**Purpose**: Application installation wizard views

**Pages**:
- `Index.cshtml`: Installation wizard main page
- Steps for database setup, admin creation, configuration

**Usage**: First-time application setup only

### Payment/
**Purpose**: Payment gateway selection and result views

**Pages**:
- `GatewaySelection.cshtml`: Choose Stripe, PayPal, etc.
- `PaymentInfo.cshtml`: Current subscription information
- `Buy.cshtml`: Purchase initiation

**Features**:
- Multiple payment gateway support
- Subscription information display
- Secure payment processing

### PayPal/
**Purpose**: PayPal-specific payment views

**Pages**:
- `Purchase.cshtml`: PayPal checkout page
- `Callback.cshtml`: Payment result page

**Integration**: PayPal Smart Payment Buttons

### Stripe/
**Purpose**: Stripe-specific payment views

**Pages**:
- `Purchase.cshtml`: Stripe checkout page
- `Callback.cshtml`: Payment result page

**Integration**: Stripe Checkout or Stripe Elements

### LegalDocumentsPublic/
**Purpose**: Public legal document display

**Pages**:
- Terms of Service
- Privacy Policy
- Acceptable Use Policy
- Other legal agreements

**Features**:
- Public access (no authentication)
- Version tracking
- Acceptance tracking when user logs in

### Error/
**Purpose**: Error pages for various HTTP status codes

**Pages**:
- `Error.cshtml`: General error page
- `404.cshtml`: Page not found
- `500.cshtml`: Server error
- `403.cshtml`: Forbidden

**Features**:
- User-friendly error messages
- Link back to home or login
- Environment-specific details (dev vs. prod)

### Consent/
**Purpose**: OAuth consent pages

**Pages**:
- `Index.cshtml`: OAuth authorization consent page

**Usage**: When using IdentityServer4 for SSO

### Shared/
**Purpose**: Shared layouts, partial views, and components

**Contains**:
- `_Layout.cshtml`: Main layout for public pages
- `_AccountLayout.cshtml`: Authentication page layout
- `_ValidationScriptsPartial.cshtml`: Client validation scripts
- `_CookieConsentPartial.cshtml`: GDPR cookie consent
- View Components (AccountLanguages, AccountLogo, TenantChange)

## View Structure

### Layout Hierarchy
```
_Layout.cshtml (root layout)
├── _AccountLayout.cshtml (auth pages)
├── _PaymentLayout.cshtml (payment pages)
└── _ErrorLayout.cshtml (error pages)
```

### Sections
Common sections in views:
- `@section Styles { }`: Page-specific CSS
- `@section Scripts { }`: Page-specific JavaScript
- `@section Header { }`: Custom header content
- `@section Footer { }`: Custom footer content

### View Models
All views strongly-typed to view models:
```razor
@model inzibackend.Web.Models.Account.LoginViewModel
```

## Architecture Notes

### Razor View Engine
- Server-side rendering
- Strongly-typed models
- Tag helpers for HTML generation
- Partial views for reusability
- View components for complex logic

### Master-Detail Pattern
Layout pages provide structure:
```razor
<!DOCTYPE html>
<html>
<head>
    @RenderSection("Styles", required: false)
</head>
<body>
    <header>
        @await Component.InvokeAsync("AccountLogo")
    </header>

    <main>
        @RenderBody()
    </main>

    <footer>
        <!-- Footer content -->
    </footer>

    @RenderSection("Scripts", required: false)
</body>
</html>
```

Content views fill sections:
```razor
@section Styles {
    <link href="~/css/login.css" rel="stylesheet" />
}

<div class="login-form">
    <!-- Login form content -->
</div>

@section Scripts {
    <script src="~/js/login.js"></script>
}
```

### View Discovery
ASP.NET Core searches for views in order:
1. `/Views/{ControllerName}/{ActionName}.cshtml`
2. `/Views/Shared/{ActionName}.cshtml`
3. Area-specific views (if in area)

### Partial Views
Reusable view fragments:
```razor
@await Html.PartialAsync("_LoginProviders", Model.ExternalProviders)
```

## Client-Side Resources

### JavaScript
- jQuery for DOM manipulation
- jQuery Validation for forms
- Bootstrap for UI components
- Custom scripts per view

### CSS
- Bootstrap for base styles
- Custom stylesheets per theme
- View-specific styles
- Responsive design

### Bundling
Scripts and styles bundled via:
- `bundles.json`: Bundle definitions
- `gulpfile.js`: Build process
- Tag helpers for automatic minification

## Localization

### Resource Files
Localized strings in `.resx` files:
- `LocalizationSource.resx` (default language)
- `LocalizationSource.es.resx` (Spanish)
- `LocalizationSource.fr.resx` (French)

### Usage in Views
```razor
@L("Login")
@L("EmailAddress")
@L("Password")
```

### Language Selection
User can change language via `AccountLanguages` component.

## Security

### Anti-Forgery Tokens
All forms include CSRF protection:
```razor
<form asp-action="Login" method="post">
    @Html.AntiForgeryToken()
    <!-- Form fields -->
</form>
```

### Content Security Policy
- External resources whitelisted
- Inline scripts avoided (or hashed)
- XSS protection headers

### HTTPS Enforcement
All pages require HTTPS in production.

## Responsive Design
- Bootstrap grid system
- Mobile-first approach
- Media queries for breakpoints
- Touch-friendly controls

## Accessibility
- Semantic HTML5
- ARIA labels where needed
- Keyboard navigation support
- Screen reader compatible

## Performance Optimization
- Minified CSS/JS in production
- Image optimization
- Lazy loading for images
- Bundle and caching strategies

## Testing
- View rendering tests
- Form submission tests
- Validation tests
- Accessibility tests

## Dependencies
- ASP.NET Core MVC
- Razor view engine
- Bootstrap 5
- jQuery
- ABP Framework tag helpers

## Related Documentation
- [Controllers/CLAUDE.md](../Controllers/CLAUDE.md): Controllers rendering these views
- [Models/CLAUDE.md](../Models/CLAUDE.md): View models
- [Areas/App/Views/CLAUDE.md](../Areas/App/Views/CLAUDE.md): Authenticated app views
- [wwwroot/CLAUDE.md](../wwwroot/CLAUDE.md): Static resources (CSS, JS, images)