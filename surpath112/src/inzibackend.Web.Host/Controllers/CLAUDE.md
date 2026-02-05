# Controllers Documentation

## Overview
The Controllers folder contains MVC controllers that serve as endpoints for the Web Host API application. These controllers handle various aspects including authentication UI, payment processing, user profile management, and webhook integrations. Most controllers inherit from base classes in Web.Core for shared functionality.

## Contents

### Files

#### HomeController.cs
- **Purpose**: Default landing page controller for the API host
- **Inherits**: inzibackendControllerBase
- **Key Functionality**:
  - Redirects to UI controller in development environment
  - Redirects to configured home page URL in production
  - Auditing disabled for index action
  - Uses IWebHostEnvironment to determine environment

#### AntiForgeryController.cs
- **Purpose**: Provides anti-forgery token generation for CSRF protection
- **Inherits**: inzibackendControllerBase
- **Key Functionality**:
  - GetToken() method sets anti-forgery cookie and header
  - Used by client applications to obtain CSRF tokens
  - Essential for secure form submissions

#### UiController.cs
- **Purpose**: Handles basic authentication UI for IdentityServer scenarios
- **Inherits**: inzibackendControllerBase
- **Key Features**:
  - Login page rendering and processing
  - Logout functionality
  - Multi-tenancy support with tenant validation
  - Password change requirement checking
  - Two-factor authentication support
  - Return URL handling for OAuth flows
- **Views**:
  - Index: Home page for authenticated users
  - Login: Authentication form
- **Security**:
  - Validates tenant availability
  - Checks for inactive tenants
  - Handles various login failure scenarios

#### ProfileController.cs
- **Purpose**: Manages user profile operations including profile pictures
- **Inherits**: ProfileControllerBase (from Web.Core)
- **Authorization**: Requires authentication (AbpMvcAuthorize)
- **Dependencies**:
  - ITempFileCacheManager for temporary file handling
  - IProfileAppService for profile operations
- **Note**: Actual implementation in base class

#### ChatController.cs
- **Purpose**: Handles chat-related file operations and uploads
- **Inherits**: ChatControllerBase (from Web.Core)
- **Authorization**: Requires authentication
- **Functionality**: File upload/download for chat messages

#### StripeController.cs
- **Purpose**: Stripe payment webhook handler
- **Inherits**: StripeControllerBase (from Web.Core)
- **Key Components**:
  - StripeGatewayManager for payment processing
  - StripePaymentGatewayConfiguration for settings
  - IStripePaymentAppService for payment operations
- **Functionality**: Processes Stripe webhooks for payment events

#### AuthorizeNetController.cs
- **Purpose**: Authorize.Net payment integration
- **Inherits**: Likely from a base controller in Web.Core
- **Functionality**: Handles Authorize.Net payment callbacks and webhooks

#### TwitterController.cs
- **Purpose**: Twitter OAuth callback handler
- **Inherits**: Likely handles Twitter authentication callbacks
- **Functionality**: Processes Twitter OAuth responses

#### ConsentController.cs
- **Purpose**: Handles user consent for IdentityServer
- **Functionality**: Manages OAuth consent screens and user permissions

#### UsersController.cs
- **Purpose**: User management operations
- **Functionality**: User-related API endpoints

### Key Components

#### Base Class Pattern
All controllers inherit from either:
- `inzibackendControllerBase`: Direct inheritance for simple controllers
- Specific base classes in Web.Core: For complex functionality with shared logic

#### Authorization Attributes
- `[AbpMvcAuthorize]`: Requires authenticated user
- `[DisableAuditing]`: Prevents action from being audited

#### Dependency Injection
- Controllers use constructor injection
- Services injected include app services, managers, and configuration

### Dependencies

#### External Libraries
- Microsoft.AspNetCore.Mvc
- Microsoft.AspNetCore.Authorization
- Abp.AspNetCore.Mvc.Authorization

#### Internal Dependencies
- inzibackend.Web.Core (base controllers)
- inzibackend.Authorization
- inzibackend.MultiTenancy
- inzibackend.Storage
- inzibackend.Identity
- Various application services

## Architecture Notes

### Controller Responsibilities
1. **Thin Controllers**: Most logic delegated to base classes or services
2. **Inheritance Pattern**: Reusable logic in Web.Core base classes
3. **Separation of Concerns**: Controllers focus on HTTP handling

### Authentication Flow
1. UiController provides login UI for IdentityServer
2. AntiForgeryController provides CSRF tokens
3. Other controllers require authentication via attributes

### Payment Integration Pattern
- Webhook controllers (Stripe, AuthorizeNet) handle external callbacks
- Base classes in Web.Core contain actual processing logic
- Configuration injected via dependency injection

### Multi-Tenancy Handling
- UiController validates tenant during login
- Tenant context established for authenticated requests
- Per-tenant payment configurations supported

## Business Logic

### User Authentication
- Login supports username or email
- Multi-factor authentication supported
- Password change requirements enforced
- Tenant validation before authentication

### Profile Management
- Profile picture upload/download
- Temporary file caching for uploads
- Secured with authentication

### Payment Processing
- Stripe webhook processing for subscription events
- Authorize.Net integration for alternative payment method
- Webhook security validation in base classes

### Chat Functionality
- File attachments for chat messages
- Secure file upload/download
- Temporary file management

## Usage Across Codebase

### API Endpoints Exposed
- `/Home` - Default landing
- `/AntiForgery/GetToken` - CSRF token generation
- `/Ui/Login`, `/Ui/Logout` - Authentication UI
- `/Profile/*` - Profile management endpoints
- `/Chat/*` - Chat file operations
- `/Stripe/WebHook` - Stripe payment webhooks
- `/AuthorizeNet/*` - Authorize.Net callbacks

### Integration Points
- SignalR hubs use authentication from these controllers
- Angular/React apps obtain CSRF tokens via AntiForgeryController
- IdentityServer uses UiController for login UI
- External payment providers post to webhook controllers

### Security Impact
- All authenticated endpoints protected by JWT/Cookie authentication
- CSRF protection via anti-forgery tokens
- Webhook endpoints validate signatures
- Multi-tenancy enforced at controller level