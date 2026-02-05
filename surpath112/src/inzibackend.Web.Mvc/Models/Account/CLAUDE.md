# Models/Account Documentation

## Overview
This folder contains view models for authentication-related pages including login, registration, password reset, email confirmation, two-factor authentication, and account switching.

## Files

### Login Models

#### LoginViewModel.cs
**Purpose**: Main view model for the login page

**Properties**:
- Login form data (`LoginFormViewModel`)
- Available external authentication providers
- Tenant information
- Remember me option
- Return URL

#### LoginFormViewModel.cs
**Purpose**: Data transfer object for login form submission

**Properties**:
- `UserNameOrEmailAddress`: User identifier
- `Password`: User password
- `RememberMe`: Persistence option
- `TenancyName`: Tenant identifier (optional)

#### LoginModel.cs
**Purpose**: Extended login model with additional context

**Properties**:
- All properties from `LoginFormViewModel`
- `ReturnUrl`: Redirect after successful login
- `SingleSignIn`: SSO indicators
- `IsExternalLogin`: External auth flags

### Registration Models

#### RegisterViewModel.cs
**Purpose**: View model for user registration page

**Properties**:
- `Name`: Full name
- `Surname`: Last name
- `UserName`: Desired username
- `EmailAddress`: Email for account
- `Password`: Chosen password
- `PasswordRepeat`: Confirmation
- `CaptchaResponse`: reCAPTCHA validation
- `IsEmailConfirmationRequired`: Feature flag

#### RegisterResultViewModel.cs
**Purpose**: Result page after registration

**Properties**:
- `IsActive`: Whether account is active
- `NameAndSurname`: Display name
- `UserName`: Registered username
- `EmailAddress`: Registered email
- `WaitingForEmailConfirmation`: Email verification pending

### Password Reset Models

#### SendPasswordResetLinkViewModel.cs
**Purpose**: Forgot password page model

**Properties**:
- `EmailAddress`: Where to send reset link
- `TenancyName`: Tenant context (if applicable)

#### ResetPasswordViewModel.cs
**Purpose**: Password reset page model

**Properties**:
- `UserId`: User to reset password for
- `ResetCode`: Token from email link
- `Password`: New password
- `PasswordRepeat`: Confirmation
- `TenancyName`: Tenant context
- `UserName`: Display purposes

### Email Confirmation

#### EmailConfirmationViewModel.cs
**Purpose**: Email verification page model

**Properties**:
- `UserId`: User confirming email
- `ConfirmationCode`: Token from email
- `IsConfirmed`: Verification status
- `ErrorMessage`: Any errors during confirmation

### Two-Factor Authentication

#### SendSecurityCodeViewModel.cs
**Purpose**: 2FA code delivery options

**Properties**:
- `Providers`: Available 2FA providers (SMS, Email, Authenticator)
- `RememberMe`: Remember this device
- `ReturnUrl`: Post-authentication redirect

#### VerifySecurityCodeViewModel.cs
**Purpose**: 2FA code verification

**Properties**:
- `Provider`: Selected 2FA provider
- `Code`: Verification code entered
- `RememberBrowser`: Don't ask again on this device
- `RememberMe`: Keep me logged in
- `ReturnUrl`: Success redirect

### Account Switching

#### SwitchToLinkedAccountModel.cs
**Purpose**: Switch between linked accounts

**Properties**:
- `TargetUserId`: Account to switch to
- `TargetTenantId`: Target tenant (null for host)
- `ImpersonationToken`: Secure token for switch
- `ReturnUrl`: Where to redirect after switch

### Shared Models

#### LanguagesViewModel.cs
**Purpose**: Available languages for account pages

**Properties**:
- `Languages`: List of active languages
- `CurrentLanguage`: Currently selected language
- Icons and display names for each

## Architecture Notes

### Validation
- Models use data annotations for validation
- Custom validators for business rules
- Client-side validation via jQuery Validate
- Server-side validation in controller actions

### Security
- Passwords never returned in view models
- Reset tokens validated server-side
- CAPTCHA integration for registration
- Anti-forgery tokens on all forms
- Rate limiting on authentication attempts

### Multi-Tenancy
- Optional `TenancyName` on login
- Tenant context maintained throughout auth flow
- Linked accounts can span tenants
- Host users can impersonate tenant users

## Usage Across Codebase

### Controllers
All models used by `AccountController.cs`:
- `Login(LoginModel)`: Authentication
- `Register(RegisterViewModel)`: New user creation
- `ForgotPassword(SendPasswordResetLinkViewModel)`: Password reset request
- `ResetPassword(ResetPasswordViewModel)`: Password reset execution
- `EmailConfirmation(EmailConfirmationViewModel)`: Email verification

### Views
Corresponding views in `/Views/Account/`:
- `Login.cshtml`: Uses `LoginViewModel`
- `Register.cshtml`: Uses `RegisterViewModel`
- `ForgotPassword.cshtml`: Uses `SendPasswordResetLinkViewModel`
- `ResetPassword.cshtml`: Uses `ResetPasswordViewModel`
- `EmailConfirmation.cshtml`: Uses `EmailConfirmationViewModel`
- `SendSecurityCode.cshtml`: Uses `SendSecurityCodeViewModel`
- `VerifySecurityCode.cshtml`: Uses `VerifySecurityCodeViewModel`

### Services
- `IAccountAppService`: Backend authentication logic
- `UserManager`: ASP.NET Core Identity user management
- `SignInManager`: Sign-in logic and 2FA
- `IEmailSender`: Email notifications
- `ISmsSender`: SMS for 2FA

## Business Logic

### Login Flow
1. User submits `LoginFormViewModel`
2. Controller validates credentials
3. If 2FA enabled, redirect to `SendSecurityCode`
4. Otherwise, create authentication ticket
5. Redirect to `ReturnUrl` or default page

### Registration Flow
1. User fills `RegisterViewModel`
2. CAPTCHA validated
3. Account created (inactive if email confirmation required)
4. Confirmation email sent
5. Show `RegisterResultViewModel` with next steps

### Password Reset Flow
1. User submits `SendPasswordResetLinkViewModel`
2. Reset token generated and emailed
3. User clicks link with token
4. Shows `ResetPasswordViewModel` with token
5. New password submitted and validated
6. Password updated, user notified

### Email Confirmation Flow
1. User receives email with confirmation link
2. Link contains `EmailConfirmationViewModel` parameters
3. Controller validates token
4. Activates account if valid
5. Shows success or error page

## Dependencies

### Internal
- `inzibackend.Authorization.Accounts`: Account service interfaces
- `inzibackend.Web.Session`: Session management
- `Microsoft.AspNetCore.Identity`: Authentication framework

### External
- reCAPTCHA for bot protection
- Email service for notifications
- SMS service for 2FA (optional)
- External authentication providers (optional)

## Related Documentation
- [Controllers/CLAUDE.md](../../Controllers/CLAUDE.md): Account controller implementation
- [Views/Account/CLAUDE.md](../../Views/Account/CLAUDE.md): Razor views
- [inzibackend.Web.Core/Authentication/CLAUDE.md](../../../../inzibackend.Web.Core/Authentication/CLAUDE.md): Core authentication infrastructure
- [Models/CLAUDE.md](../CLAUDE.md): Parent folder documentation