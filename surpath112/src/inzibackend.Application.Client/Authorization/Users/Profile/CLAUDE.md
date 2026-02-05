# Authorization/Users/Profile Documentation

## Overview
Contains proxy service implementations for user profile management operations. These services provide client-side access to profile-related functionality including profile editing, password management, profile pictures, and authentication settings.

## Contents

### Files

#### ProxyProfileAppService.cs
- **Purpose**: Client proxy for IProfileAppService, handling user profile operations
- **Key Methods**:
  - `GetCurrentUserProfileForEdit`: Retrieves current user's profile for editing
  - `UpdateCurrentUserProfile`: Updates user profile information
  - `ChangePassword`: Handles password change requests
  - `UpdateProfilePicture`: Updates user's profile picture
  - `GetPasswordComplexitySetting`: Retrieves password complexity requirements
  - `GetProfilePicture`: Various overloads for retrieving profile pictures (by ID, username, user ID)
  - `ChangeLanguage`: Updates user's language preference
  - `UpdateGoogleAuthenticatorKey`: Manages 2FA Google Authenticator setup
  - `SendVerificationSms/VerifySmsCode`: SMS-based verification workflow
  - `PrepareCollectedData`: GDPR-related data collection preparation
- **Base Class**: Inherits from ProxyAppServiceBase
- **Interface**: Implements IProfileAppService

#### ProxyProfileControllerService.cs
- **Purpose**: Client proxy for profile-related controller endpoints
- **Features**: Alternative API access path for profile operations
- **Usage**: Used when direct controller access is preferred over application services

### Key Components
- **Profile Management**: Complete user profile CRUD operations
- **Security Features**: Password management and 2FA configuration
- **Media Handling**: Profile picture upload and retrieval
- **Localization**: Language preference management
- **Privacy Compliance**: GDPR data collection support

### Dependencies
- `inzibackend.Authorization.Users.Dto`: User-related DTOs
- `inzibackend.Authorization.Users.Profile.Dto`: Profile-specific DTOs
- `ProxyAppServiceBase`: Base class for proxy services

## Architecture Notes
- **Proxy Pattern**: Implements remote proxy pattern for server-side services
- **Async/Await**: All operations are asynchronous for better performance
- **RESTful Design**: Maps to RESTful API endpoints on the server
- **Type Safety**: Strongly typed DTOs for all operations

## Business Logic
- **Profile Pictures**: Supports multiple retrieval methods (by user ID, username, profile picture ID)
- **Password Security**: Enforces complexity settings retrieved from server
- **Two-Factor Authentication**: Supports Google Authenticator and SMS verification
- **Multi-tenancy**: Profile operations respect tenant context
- **Friend System**: Special endpoint for friend profile pictures

## Usage Across Codebase
- Used by UI components for profile management screens
- Referenced in authentication workflows for password changes
- Consumed by settings pages for 2FA configuration
- Integrated with file upload components for profile pictures