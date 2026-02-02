# Models Documentation

## Overview
This folder contains view models for the root-level pages and features of the MVC application, including authentication, tenant registration, installation, and payment processing. These models serve as data transfer objects between controllers and views.

## Subfolders

### Account/
**Purpose**: Authentication and account management view models

**Contains**:
- Login models (username/password, external auth)
- Registration models
- Password reset models
- Email confirmation models
- Two-factor authentication models
- Account linking models

**See**: [Account/CLAUDE.md](Account/CLAUDE.md)

### TenantRegistration/
**Purpose**: Tenant self-registration view models

**Contains**:
- Tenant registration form model
- Edition selection model
- Registration result/confirmation model

**Features**:
- Multi-tenant SaaS onboarding
- Subscription plan selection
- Email verification
- Manual approval workflows

**See**: [TenantRegistration/CLAUDE.md](TenantRegistration/CLAUDE.md)

### Install/
**Purpose**: Application installation wizard view models

**Contains**:
- Database configuration model
- Admin account setup model
- System settings model

**Usage**: First-time application setup

**See**: [Install/CLAUDE.md](Install/CLAUDE.md)

### Payment/
**Purpose**: General payment and subscription management view models

**Contains**:
- Payment information display model
- Payment creation model
- Payment result model
- Edition upgrade/downgrade model
- Payment gateway selection model

**Usage**: Shared across Stripe and PayPal implementations

**See**: [Payment/CLAUDE.md](Payment/CLAUDE.md)

### Stripe/
**Purpose**: Stripe-specific payment integration view models

**Contains**:
- Stripe checkout view model
- Stripe session configuration
- Stripe-specific payment properties

**Usage**: Stripe payment gateway implementation

**See**: [Stripe/CLAUDE.md](Stripe/CLAUDE.md)

### Paypal/
**Purpose**: PayPal-specific payment integration view models

**Contains**:
- PayPal purchase view model
- PayPal order configuration
- PayPal-specific payment properties

**Usage**: PayPal payment gateway implementation

**See**: [Paypal/CLAUDE.md](Paypal/CLAUDE.md)

## Architecture Notes

### View Model Pattern
View models serve multiple purposes:
- **Data Transfer**: Move data between controller and view
- **Validation**: Data annotations for input validation
- **Presentation Logic**: Properties specific to UI needs
- **Separation of Concerns**: Isolate view requirements from domain models

### Naming Convention
- Suffix with `ViewModel` or `Model`
- Organized by controller/feature area
- One model per view (typically)

### Validation
Models use data annotations:
```csharp
public class LoginModel {
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}
```

### AutoMapper Integration
View models often mapped from domain entities:
```csharp
var viewModel = ObjectMapper.Map<UserViewModel>(userEntity);
```

## Usage Across Codebase

### Controllers
Controllers accept view models from views:
```csharp
[HttpPost]
public async Task<IActionResult> Register(RegisterViewModel model) {
    if (!ModelState.IsValid) {
        return View(model);
    }

    await _accountService.RegisterAsync(model);
    return RedirectToAction("RegisterResult");
}
```

### Views
Views are strongly-typed to view models:
```razor
@model inzibackend.Web.Models.Account.RegisterViewModel

<form asp-action="Register" method="post">
    <div asp-validation-summary="All"></div>

    <input asp-for="EmailAddress" class="form-control" />
    <span asp-validation-for="EmailAddress"></span>

    <button type="submit">Register</button>
</form>
```

### Services
Application services receive DTOs, not view models:
```csharp
// Controller maps view model to DTO
var dto = ObjectMapper.Map<RegisterInput>(model);
await _accountAppService.RegisterAsync(dto);
```

## Design Patterns

### Composite View Models
For complex pages:
```csharp
public class DashboardViewModel {
    public UserInfoViewModel UserInfo { get; set; }
    public StatisticsViewModel Statistics { get; set; }
    public List<NotificationViewModel> Notifications { get; set; }
}
```

### Form Models vs. Display Models
- **Form Models**: Input properties, validation attributes
- **Display Models**: Read-only properties, formatting helpers

### Result Models
For post-action pages:
```csharp
public class PaymentResultViewModel {
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string TransactionId { get; set; }
}
```

## Validation Strategy

### Client-Side Validation
- jQuery Validation
- Data annotation attributes
- Custom validation rules

### Server-Side Validation
- ModelState validation
- Business rule validation
- Cross-field validation

### Custom Validators
```csharp
public class TenancyNameAttribute : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext context) {
        // Custom validation logic
    }
}
```

## Localization Support
View models support localized display names:
```csharp
[Display(Name = "EmailAddress", ResourceType = typeof(LocalizationSource))]
public string EmailAddress { get; set; }
```

## Security Considerations
- Never include sensitive data (passwords, tokens) in GET models
- Anti-forgery tokens on all POST forms
- Input sanitization via validation
- Output encoding in views

## Testing
- Unit test view model validation
- Test AutoMapper mappings
- Integration tests for controller-view-model interaction

## Dependencies
- ASP.NET Core MVC
- Data Annotations
- ABP Framework
- AutoMapper

## Related Documentation
- [Controllers/CLAUDE.md](../Controllers/CLAUDE.md): Controllers using these models
- [Views/CLAUDE.md](../Views/CLAUDE.md): Views rendering these models
- [inzibackend.Application.Shared/CLAUDE.md](../../../inzibackend.Application.Shared/CLAUDE.md): Application DTOs