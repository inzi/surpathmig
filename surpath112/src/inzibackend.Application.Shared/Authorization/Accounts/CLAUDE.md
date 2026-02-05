# Authorization/Accounts Documentation

## Overview
Service interface definitions for account management operations including user registration, authentication, password reset, email activation, tenant management, and user impersonation capabilities in a multi-tenant SaaS application.

## Contents

### Files

#### Service Interfaces
- **IAccountAppService.cs** - Primary service interface defining all account-related operations:
  - Tenant availability checking and resolution
  - User registration and email activation
  - Password reset functionality
  - User and tenant impersonation
  - Account switching between linked accounts

### Key Components
- **IAccountAppService** - Main application service interface extending IApplicationService from ABP framework
  - Async methods for all account operations
  - Clear separation of concerns with dedicated DTOs for each operation
  - Support for multi-tenant scenarios

### Dependencies
- Abp.Application.Services - ABP framework application service base
- Authorization.Accounts.Dto - All DTOs for account operations
- System.Threading.Tasks - Async operation support

## Subfolders

### Dto
Contains all Data Transfer Objects for account operations. Key categories include:
- Registration and account creation DTOs
- Authentication and token management
- Password reset and recovery
- Email activation workflows
- Tenant resolution and availability checking
- User and tenant impersonation
- Account switching functionality

[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Service interface follows ABP application service patterns
- All operations are asynchronous for better scalability
- Clear Input/Output DTO pattern for all operations
- Designed for multi-tenant SaaS architecture
- Supports complex authentication scenarios including impersonation

## Business Logic
- **Tenant Resolution**: Supports checking tenant availability and resolving tenant IDs from various inputs
- **Registration Flow**: Comprehensive user registration with profile information
- **Password Management**: Secure password reset with code-based verification
- **Email Activation**: Two-step email verification process
- **Impersonation**: Multiple impersonation modes:
  - User impersonation within a tenant
  - Tenant administrator impersonation
  - Delegated impersonation for support scenarios
  - Back to impersonator functionality
- **Account Linking**: Support for switching between linked accounts

## Usage Across Codebase
This service interface is implemented by:
- AccountAppService in the Application layer
- Consumed by AccountController in Web.Mvc layer
- Used by authentication middleware and filters
- Referenced by tenant management components
- Integrated with identity and authorization systems