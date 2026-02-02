# Profile Documentation

## Overview
This folder contains the user profile management application service, handling all aspects of user profile operations including profile editing, password management, two-factor authentication, profile pictures, and SMS verification.

## Contents

### Files

#### ProfileAppService.cs
- **Purpose**: Core application service for user profile management operations
- **Key Features**:
  - Profile viewing and editing for current user
  - Password change functionality
  - Two-factor authentication (Google Authenticator and SMS)
  - Profile picture management with cropping support
  - SMS phone number verification
  - Language preference management
  - Timezone settings management
  - GDPR data collection preparation
- **Key Methods**:
  - `GetCurrentUserProfileForEdit()`: Retrieves current user profile for editing
  - `UpdateCurrentUserProfile()`: Updates user profile information
  - `ChangePassword()`: Handles password changes with validation
  - `UpdateProfilePicture()`: Manages profile picture upload and cropping
  - `SendVerificationSms()`: Initiates SMS verification process
  - `VerifySmsCode()`: Validates SMS verification codes
  - `UpdateGoogleAuthenticatorKey()`: Generates new 2FA keys
  - `GetPasswordComplexitySetting()`: Returns password requirements
  - `PrepareCollectedData()`: Initiates GDPR data collection job

### Key Components

**Services:**
- `ProfileAppService`: Main service implementing `IProfileAppService`

**Dependencies Injected:**
- `IBinaryObjectManager`: Manages binary data storage for profile pictures
- `ITimeZoneService`: Handles timezone operations
- `IFriendshipManager`: Manages friend relationships
- `GoogleTwoFactorAuthenticateService`: Google 2FA implementation
- `ISmsSender`: SMS sending service
- `ICacheManager`: Cache operations for SMS codes
- `ITempFileCacheManager`: Temporary file storage
- `IBackgroundJobManager`: Background job execution
- `ProfileImageServiceFactory`: Profile image service creation

### Dependencies
- **External:**
  - `Abp.*`: ABP framework core libraries
  - `Microsoft.AspNetCore.Identity`: ASP.NET Core Identity
  - `SixLabors.ImageSharp`: Image processing library

- **Internal:**
  - `inzibackend.Authorization.Users.Dto`: User DTOs
  - `inzibackend.Authorization.Users.Profile.Dto`: Profile-specific DTOs
  - `inzibackend.Storage`: Binary object storage
  - `inzibackend.Surpath`: Business-specific settings
  - Profile Cache subfolder components

## Subfolders

### Cache
SMS verification code caching infrastructure for temporary storage of verification codes during the SMS verification process. Contains `SmsVerificationCodeCacheItem` and cache extension methods.

## Architecture Notes
- **Pattern**: Application Service pattern with ABP framework
- **Authorization**: Uses `[AbpAuthorize]` attribute for secured endpoints
- **Auditing**: Some methods use `[DisableAuditing]` for sensitive operations
- **Image Processing**: Supports image cropping and resizing with format preservation
- **Caching Strategy**: Uses typed caching for SMS verification codes
- **Background Jobs**: Leverages background jobs for GDPR data preparation
- **Multi-tenancy**: Fully supports multi-tenant operations with `AbpSession`

## Business Logic

### Profile Management
- Users can edit their basic profile information (name, email, phone)
- Phone number changes require re-verification via SMS
- Timezone preferences are stored per user
- Language preferences affect the entire user interface

### Two-Factor Authentication
- **Google Authenticator**:
  - Generates QR codes for mobile app setup
  - Creates unique keys per user
  - Can be enabled/disabled by user
- **SMS Verification**:
  - Generates 6-digit codes (100000-999999)
  - Stores codes in cache with user identifier as key
  - Validates codes and marks phone numbers as confirmed

### Profile Pictures
- Supports both uploaded images and Gravatar
- Image cropping with X, Y, Width, Height parameters
- Maximum size limit defined by `SurpathSettings.MaxProfilefiledataLength`
- Stores images as binary objects with cleanup of old images
- Supports friend profile picture retrieval
- Can update other users' pictures with proper permissions

### Password Management
- Validates current password before allowing changes
- Respects password complexity settings from configuration
- Returns detailed password requirements to clients

### Security Features
- Permission-based access control for updating other users' pictures
- SMS code validation prevents brute force with cache-based storage
- Anonymous access allowed only for specific operations (password settings, public profile pictures)

## Usage Across Codebase

### Cross-Reference Analysis
The ProfileAppService is likely used by:
- MVC controllers for profile pages
- API endpoints for mobile applications
- Admin interfaces for user management
- Authentication workflows for 2FA setup
- GDPR compliance modules

### Impact Areas
Changes to this service affect:
- User authentication flows
- Profile management UI
- Mobile app profile features
- Admin user management tools
- Two-factor authentication systems
- GDPR data export functionality

## Security Considerations
- SMS codes are temporarily cached, not stored permanently
- Profile picture permissions checked before allowing updates
- Password changes require current password verification
- Google Authenticator keys are stored encrypted
- Anonymous access restricted to specific read-only operations
- Image uploads validated for size constraints
- Phone verification required for phone number changes