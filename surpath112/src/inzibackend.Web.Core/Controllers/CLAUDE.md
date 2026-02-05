# Controllers Documentation

## Overview
This folder contains base controllers and specialized controllers that provide core web functionality across both MVC and API projects. These controllers handle authentication, file management, user profiles, payment processing, and other cross-cutting concerns.

## Contents

### Files

#### inzibackendControllerBase.cs
- **Purpose**: Base controller class for all application controllers
- **Key Features**:
  - Inherits from AbpController for ABP framework integration
  - Localization support with inzibackendConsts.LocalizationSourceName
  - Identity error handling with CheckErrors method
  - Multi-tenancy cookie management
- **Used By**: All controllers in the application

#### TokenAuthController.cs
- **Purpose**: Handles JWT token-based authentication
- **Key Features**:
  - User login/logout functionality
  - External authentication provider support
  - Two-factor authentication flow
  - Token refresh mechanism
  - Impersonation support
  - User delegation
- **Endpoints**: `/api/TokenAuth/Authenticate`, `/api/TokenAuth/RefreshToken`
- **Business Logic**: Complete authentication workflow including 2FA

#### FileController.cs
- **Purpose**: Manages file uploads, downloads, and document serving
- **Key Features**:
  - Binary object storage management
  - Legal document viewing
  - Temporary file caching
  - MIME type handling
  - Surpath-specific document management
- **Security**: Auditing can be disabled for specific actions

#### ProfileControllerBase.cs
- **Purpose**: Base controller for user profile management
- **Key Features**:
  - Profile picture upload/download
  - User settings management
  - Profile information updates
- **Inheritance**: Extended by MVC and API profile controllers

#### ChatControllerBase.cs
- **Purpose**: Base controller for chat functionality
- **Key Features**:
  - Chat message retrieval
  - Conversation management
  - Friend list operations
- **Integration**: Works with SignalR for real-time updates

#### UsersControllerBase.cs
- **Purpose**: Base controller for user management
- **Key Features**:
  - User CRUD operations
  - Role assignment
  - Permission management
  - User unlock functionality

#### TenantCustomizationController.cs
- **Purpose**: Handles tenant-specific customization
- **Key Features**:
  - Logo upload/management
  - CSS customization
  - Tenant branding settings
- **Multi-tenancy**: Tenant-isolated customization

#### StripeControllerBase.cs
- **Purpose**: Base controller for Stripe payment integration
- **Key Features**:
  - Payment processing
  - Subscription management
  - Webhook handling
  - Payment method management

#### AuthorizeNetControllerBase.cs
- **Purpose**: Base controller for Authorize.Net payment integration
- **Key Features**:
  - Alternative payment processor
  - Transaction management
  - Recurring billing support

#### DemoUiComponentsController.cs
- **Purpose**: Demo UI components for development/testing
- **Features**:
  - Sample UI elements
  - Component showcase
  - Development tools

#### ErrorController.cs
- **Purpose**: Centralized error handling
- **Features**:
  - Error page rendering
  - Exception logging
  - User-friendly error messages

### Key Components
- **Base Controllers**: Reusable functionality for derived controllers
- **Authentication**: Complete token-based auth system
- **File Management**: Binary storage and retrieval
- **Payment Processing**: Multiple payment provider support
- **Profile Management**: User profile and settings

### Dependencies
- Microsoft.AspNetCore.Mvc (MVC framework)
- Abp.AspNetCore.Mvc (ABP MVC integration)
- inzibackend.Application (business logic)
- Payment provider SDKs (Stripe, Authorize.Net)
- Storage providers (Azure, AWS, local)

## Architecture Notes
- **Inheritance Hierarchy**: Base controllers provide common functionality
- **Separation of Concerns**: Each controller has specific responsibility
- **Multi-tenant Aware**: All controllers respect tenant boundaries
- **Security First**: Authentication/authorization built-in

## Business Logic
- **Authentication Flow**: JWT tokens with refresh capability
- **File Management**: Abstracted storage with multiple providers
- **Payment Processing**: Provider-agnostic payment handling
- **Profile Customization**: User and tenant-level customization

## Security Considerations
- JWT token validation on all protected endpoints
- File upload validation and virus scanning
- Payment data never stored locally (PCI compliance)
- Tenant isolation enforced at controller level

## Usage Across Codebase
- **MVC Project**: Controllers inherited for web views
- **API Project**: Controllers exposed as REST endpoints
- **Mobile Apps**: Consume API endpoints
- **Background Jobs**: Use controller logic via services
- **Integration Tests**: Test controller endpoints

## Configuration
Key settings affecting controllers:
- `Authentication:JwtBearer`: Token configuration
- `Storage:Provider`: File storage provider
- `Payment:Stripe`: Stripe API keys
- `Payment:AuthorizeNet`: Authorize.Net credentials