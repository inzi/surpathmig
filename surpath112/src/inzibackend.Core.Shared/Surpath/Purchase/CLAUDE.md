# Surpath/Purchase Documentation

## Overview
Purchase and payment processing interfaces and models for Authorize.Net integration, supporting user purchases of Surpath services with pre-authorization workflows.

## Contents

### Files

#### AuthorizeNetSettings.cs
- **Purpose**: Configuration settings for Authorize.Net API
- **Key Properties**:
  - ApiLoginID: Merchant API login identifier
  - PublicClientKey: Public key for client-side tokenization
  - ApiTransactionKey: Secret key for server-side API calls
  - TransactionId: Transaction identifier for tracking
- **Usage**: Secure storage of payment gateway credentials

#### IPurchaseAppService.cs
- **Purpose**: Service interface for purchase operations
- **Key Methods**:
  - DonorCurrent(): Check if donor/user is current
  - GetChortUserId(): Get cohort user ID from user ID
  - SandboxPayment(): Test payment in sandbox mode
  - DoPurchaseFromHelper(): Process purchase transaction
  - AuthorizeNetSettings(): Retrieve gateway settings
  - PreAuth(): Pre-authorize payment transaction
  - GetUserRolesAsStringList(): Get user's roles
- **Dependencies**: AuthorizeNet.Api.Contracts.V1

#### PreAuthDto.cs
- **Purpose**: DTO for pre-authorization with user registration
- **User Properties**:
  - Name, Surname, MiddleName: User identification
  - UserName, EmailAddress, Password: Account credentials
  - Address details (Address, SuiteApt, City, State, Zip)
- **Authentication Properties**:
  - IsExternalLogin: External auth indicator
  - ExternalLoginAuthSchema: Auth provider type
  - ReturnUrl: Post-auth redirect URL
  - SingleSignIn: SSO indicator
- **Payment Properties**:
  - AuthNetSubmit: Payment submission data
  - AuthNetCaptureResultDto: Capture results
  - TransactionId: Transaction tracking

### Key Components
- Complete purchase workflow interface
- User registration combined with payment
- Pre-authorization support
- Sandbox testing capability
- Role-based access verification
- Cohort user association

### Dependencies
- Abp.Application.Services: For service interfaces
- AuthorizeNet.Api.Contracts.V1: Gateway integration
- Abp.Auditing: For audit logging
- inzibackend.Surpath: Domain models

## Architecture Notes
- Service interface pattern for testability
- Combined registration and payment flow
- Support for external authentication
- Sandbox mode for development/testing
- Settings abstraction for security
- Pre-auth/capture payment model

## Business Logic
- **User Verification**: Check donor/user currency before purchase
- **Cohort Association**: Link purchases to cohort users
- **Pre-Authorization**: Validate payment before service activation
- **Role Management**: Verify user permissions for purchases
- **Registration Flow**: Optional user registration during purchase
- **External Auth**: Support for SSO during purchase

## Usage Across Codebase
These purchase components are used in:
- Service purchase workflows
- User registration with payment
- Shopping cart checkout
- Payment gateway integration
- Transaction history tracking
- Financial reporting
- Subscription management
- Role-based service access