# Controllers Documentation

## Overview
This folder contains root-level MVC controllers that handle public-facing and authentication-related functionality for the web application. These controllers manage user authentication, registration, payments, legal documents, and the initial user experience before entering the authenticated App area.

## Contents

### Core Controllers

#### HomeController.cs
- **Purpose**: Application entry point and routing controller
- **Key Actions**:
  - `Index`: Routes users to login or App area based on authentication status
- **Features**:
  - Handles forced new registration
  - Tenant registration redirection
  - Smart routing based on session
- **Dependencies**: SignInManager for authentication

#### AccountController.cs
- **Purpose**: User authentication and account management
- **Key Features**:
  - Login/Logout functionality
  - Password reset and recovery
  - Two-factor authentication
  - External login providers (Google, Facebook, etc.)
  - Email confirmation
  - User impersonation
  - Language switching
  - Tenant switching
- **Security**: Anti-forgery tokens, captcha support
- **Size**: ~55KB - largest controller handling comprehensive auth flows

#### TenantRegistrationController.cs
- **Purpose**: New tenant registration and onboarding
- **Key Actions**:
  - `SelectEdition`: Edition/plan selection
  - `Register`: Tenant registration form
  - `RegisterTenant`: Process registration
- **Features**:
  - Multi-step registration
  - Edition feature comparison
  - Payment integration
  - Trial period handling
- **Dependencies**: TenantRegistrationAppService

### Payment Controllers

#### PaymentController.cs
- **Purpose**: Payment processing and subscription management
- **Key Features**:
  - Payment gateway integration
  - Subscription purchase
  - Payment history
  - Invoice generation
  - Payment method management
  - Recurring billing
- **Integrations**: Stripe, PayPal
- **Size**: ~20KB - comprehensive payment handling

#### StripeController.cs
- **Purpose**: Stripe payment gateway integration
- **Key Actions**:
  - `Purchase`: Process Stripe payments
  - `ConfirmPayment`: Handle payment confirmation
  - `CreatePaymentSession`: Setup payment sessions
- **Features**:
  - Webhook handling
  - Payment intent management
  - Subscription handling
- **Security**: Stripe webhook signature validation

#### PaypalController.cs
- **Purpose**: PayPal payment gateway integration
- **Key Actions**:
  - `Purchase`: Process PayPal payments
  - `ConfirmPayment`: Handle payment confirmation
  - `CancelPayment`: Handle cancellations
- **Features**:
  - PayPal checkout flow
  - IPN handling
  - Subscription management

### Legal & Compliance Controllers

#### LegalDocumentsPublicController.cs
- **Purpose**: Public access to legal documents
- **Key Features**:
  - Terms of Service display
  - Privacy Policy access
  - Cookie Policy viewing
  - Document versioning
  - Acceptance tracking
  - Multi-language support
- **Public Access**: No authentication required

#### ConsentController.cs
- **Purpose**: User consent management (GDPR compliance)
- **Key Features**:
  - Cookie consent
  - Data processing consent
  - Consent withdrawal
  - Consent history
- **Compliance**: GDPR requirements

### Utility Controllers

#### InstallController.cs
- **Purpose**: Initial application setup and installation
- **Key Actions**:
  - `Index`: Installation wizard
  - `CheckDatabase`: Database connectivity
  - `ConfigureDatabase`: Database setup
- **Features**:
  - First-time setup
  - Database initialization
  - Admin user creation
- **Usage**: Only available during initial setup

#### ProfileController.cs
- **Purpose**: Public user profile viewing
- **Key Features**:
  - Public profile display
  - User information viewing
  - Avatar display
- **Access**: Limited public access to user profiles

#### UsersController.cs
- **Purpose**: Basic user-related public endpoints
- **Features**:
  - User lookup
  - Public user information
- **Minimal**: Very limited functionality

## Architecture Notes

### Controller Hierarchy
```
inzibackendControllerBase
├── HomeController
├── AccountController
├── TenantRegistrationController
├── PaymentController
├── StripeController
├── PaypalController
├── LegalDocumentsPublicController
├── ConsentController
├── InstallController
├── ProfileController
└── UsersController
```

### Design Patterns
- **Base Controller**: All inherit from inzibackendControllerBase
- **Dependency Injection**: Constructor injection for services
- **Async/Await**: Asynchronous action methods
- **Action Filters**: Authorization, validation, anti-forgery

### Security Model
- **Public Controllers**: No authentication required
- **Mixed Access**: Some actions public, some authenticated
- **Anti-Forgery**: CSRF protection on forms
- **SSL Required**: HTTPS enforcement in production

## Business Logic

### Authentication Flow
1. User arrives at HomeController
2. Redirected to AccountController/Login
3. Authentication via local or external provider
4. Session establishment
5. Redirect to App area

### Registration Flow
1. TenantRegistrationController/SelectEdition
2. Choose subscription plan
3. Fill registration form
4. Payment processing (if required)
5. Tenant creation
6. Email confirmation
7. Auto-login and redirect

### Payment Flow
1. Edition selection
2. PaymentController initiates
3. Gateway-specific controller (Stripe/PayPal)
4. Payment processing
5. Webhook confirmation
6. Subscription activation

## Dependencies

### Application Services
- `IAccountAppService`: Account operations
- `ITenantRegistrationAppService`: Tenant registration
- `IPaymentAppService`: Payment processing
- `ILegalDocumentAppService`: Legal document management

### Framework Services
- `SignInManager`: Authentication management
- `UserManager`: User operations
- `TenantManager`: Tenant operations
- `IConfiguration`: Application configuration

### External Services
- Stripe API
- PayPal API
- Email services
- SMS services (for 2FA)

## Usage Across Codebase

### Entry Points
- `/`: HomeController/Index
- `/Account/Login`: Login page
- `/Account/Register`: User registration
- `/TenantRegistration`: Tenant registration
- `/Payment`: Payment processing

### Related Components
- Views: `/Views/Account/`, `/Views/TenantRegistration/`, etc.
- Models: `/Models/Account/`, `/Models/TenantRegistration/`, etc.
- JavaScript: `/wwwroot/view-resources/Views/`
- Application Services: `inzibackend.Application`

## Configuration

### Routes
Default routing pattern:
```
{controller=Home}/{action=Index}/{id?}
```

### Authentication
- Cookie authentication
- JWT bearer tokens (for API)
- External providers (OAuth)

### Authorization
- Most actions public or allow anonymous
- Specific actions require authentication
- Admin actions restricted

## Error Handling
- Global exception handling
- User-friendly error pages
- Logging via ILogger
- Audit trail for sensitive operations

## Best Practices
- Input validation on all forms
- Anti-forgery tokens
- Secure password policies
- Rate limiting on authentication
- Audit logging for security events