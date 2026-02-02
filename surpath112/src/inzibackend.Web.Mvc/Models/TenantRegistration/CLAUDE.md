# Models/TenantRegistration Documentation

## Overview
This folder contains view models for the tenant self-registration process, allowing new organizations to sign up for the SaaS application and select their subscription plan.

## Files

### TenantRegisterViewModel.cs
**Purpose**: View model for tenant registration form

**Properties**:
- `TenancyName`: Unique tenant identifier (subdomain/URL slug)
- `Name`: Full organization name
- `AdminEmailAddress`: Primary administrator email
- `AdminPassword`: Administrator password
- `AdminPasswordRepeat`: Password confirmation
- `EditionId`: Selected subscription plan
- `SubscriptionStartType`: Trial or paid
- `CaptchaResponse`: reCAPTCHA validation token
- `IsEmailConfirmationRequired`: Feature flag

**Validation**:
- Tenancy name must be unique and URL-safe
- Email must be valid and unique
- Password strength requirements
- Edition must be selectable (not disabled)
- CAPTCHA must be valid

### TenantRegisterResultViewModel.cs
**Purpose**: View model for registration confirmation page

**Properties**:
- `TenancyName`: Registered tenant identifier
- `Name`: Organization name
- `UserName`: Administrator username (usually email)
- `EmailAddress`: Administrator email
- `IsActive`: Whether tenant is immediately active
- `WaitingForEmailConfirmation`: Email verification required
- `IsEmailConfirmationRequired`: Feature enabled

**Display Logic**:
- If `IsActive && !WaitingForEmailConfirmation`: Show login link
- If `WaitingForEmailConfirmation`: Show email verification message
- If `!IsActive`: Show "awaiting approval" message (manual activation)

### EditionsSelectViewModel.cs
**Purpose**: View model for edition/plan selection during registration

**Properties**:
- `Editions`: List of available subscription plans
  - `Id`: Edition identifier
  - `DisplayName`: Plan name
  - `Description`: Plan features
  - `Price`: Monthly/annual pricing
  - `TrialDayCount`: Free trial duration (if applicable)
  - `IsFeature`: Feature availability flags
- `SelectedEditionId`: Currently selected plan
- `AllFeaturesAvailable`: All features included in plan

## Architecture Notes

### Registration Flow
1. User accesses tenant registration page
2. Views available editions via `EditionsSelectViewModel`
3. Selects desired plan
4. Fills `TenantRegisterViewModel` form
5. Submits registration
6. System creates:
   - Tenant record in database
   - Tenant database schema (if separate DB per tenant)
   - Administrator user account
   - Initial subscription record
7. Shows `TenantRegisterResultViewModel` with next steps
8. Optional: Email confirmation required
9. Optional: Manual activation by host required

### Edition Selection
- Different editions have different features
- Features controlled via ABP Feature system
- Examples:
  - Free: Limited users, basic features
  - Standard: More users, additional features
  - Premium: Unlimited users, all features
- Trial editions allow X days free before requiring payment

### Tenant Activation Strategies

#### Immediate Activation
- Tenant becomes active immediately after registration
- No email confirmation or manual approval required
- User can log in and start using the system

#### Email Confirmation Required
- Tenant created but not active
- Administrator must verify email before activation
- Confirmation email sent with link
- User clicks link to activate tenant

#### Manual Activation
- Tenant created but awaits host approval
- Host admin reviews registration
- Host admin manually activates tenant
- Email sent when activation complete

## Usage Across Codebase

### Controllers
- `TenantRegistrationController.SelectEdition()`: Show edition selection
- `TenantRegistrationController.Register(TenantRegisterViewModel)`: Process registration
- `TenantRegistrationController.RegisterResult(TenantRegisterResultViewModel)`: Show confirmation

### Views
- `/Views/TenantRegistration/SelectEdition.cshtml`: Edition selection page
- `/Views/TenantRegistration/Register.cshtml`: Registration form
- `/Views/TenantRegistration/RegisterResult.cshtml`: Confirmation page

### Services
- `ITenantRegistrationAppService.RegisterTenant()`: Backend registration logic
- `ITenantManager`: Tenant CRUD operations
- `IEditionManager`: Edition/plan management
- `IEmailSender`: Confirmation emails

## Business Logic

### Tenancy Name Validation
- Must be unique across all tenants
- URL-safe characters only (alphanumeric, hyphens)
- Minimum/maximum length constraints
- Reserved names not allowed (admin, api, www, etc.)
- Becomes part of tenant's URL: `{tenancyName}.yourdomain.com`

### Database Provisioning
**Shared Database Approach** (typical):
- All tenants share one database
- Data filtered by `TenantId` column
- Schema created on first registration

**Separate Database Approach** (enterprise):
- Each tenant gets dedicated database
- Connection string stored in tenant record
- Database created and migrated during registration
- Higher isolation, higher cost

### Subscription Management
- Free trial starts immediately (if offered)
- Payment collected at end of trial or immediately
- Subscription record tracks:
  - Edition ID
  - Start date
  - End date (trial expiry or paid expiry)
  - Status (trial, active, expired, cancelled)

### Security Considerations
- Rate limiting on registration endpoint (prevent abuse)
- CAPTCHA prevents automated bot registrations
- Email verification prevents fake accounts
- Tenancy name uniqueness prevents conflicts
- Strong password requirements protect accounts
- Admin approval prevents unwanted tenants (optional)

## Dependencies

### Internal
- `inzibackend.MultiTenancy`: Tenant management
- `inzibackend.Editions`: Plan management
- `inzibackend.Authorization.Users`: User management
- ABP Feature system: Feature toggles

### External
- reCAPTCHA: Bot protection
- Email service: Confirmation emails
- Payment gateway: Trial-to-paid conversion (Stripe/PayPal)

### Configuration
- `TenantManagement.AllowSelfRegistration`: Feature flag
- `TenantManagement.IsNewRegisteredTenantActiveByDefault`: Auto-activation
- `TenantManagement.UseCaptchaOnRegistration`: CAPTCHA requirement
- `EmailSettings.*`: Email configuration

## Testing

### Test Scenarios
- Valid registration (all fields correct)
- Duplicate tenancy name
- Invalid email address
- Weak password
- Missing required fields
- CAPTCHA validation
- Edition selection
- Email confirmation flow
- Manual activation flow
- Trial expiry handling

### Test Data
- Use test editions with short trial periods
- Test tenancy names: `testorg1`, `testorg2`, etc.
- Test email addresses with `+` notation: `user+test1@example.com`
- Test password patterns

## Related Documentation
- [Controllers/CLAUDE.md](../../Controllers/CLAUDE.md): TenantRegistration controller
- [Views/TenantRegistration/CLAUDE.md](../../Views/TenantRegistration/CLAUDE.md): Registration views
- [inzibackend.Core/MultiTenancy/CLAUDE.md](../../../../inzibackend.Core/MultiTenancy/CLAUDE.md): Tenant domain logic
- [inzibackend.Application.Shared/MultiTenancy/CLAUDE.md](../../../../inzibackend.Application.Shared/MultiTenancy/CLAUDE.md): Tenant services
- [Models/CLAUDE.md](../CLAUDE.md): Parent folder documentation