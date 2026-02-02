# User Profile DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for user profile self-service management. These DTOs enable users to manage their own profiles including personal information, security settings, profile pictures, password changes, two-factor authentication (Google Authenticator), and SMS verification. This is distinct from admin user management - these are user-facing profile operations.

## Contents

### Files

#### Profile Edit DTOs
- **CurrentUserProfileEditDto.cs** - Self-service profile editing:
  - Full name (Name, Middlename, Surname)
  - Account basics (UserName, EmailAddress, PhoneNumber)
  - Phone confirmation (IsPhoneNumberConfirmed)
  - Timezone preference
  - Two-factor authentication (QrCodeSetupImageUrl, IsGoogleAuthenticatorEnabled)
  - Physical address (Address, SuiteApt, City, State, Zip)
  - Personal data (DateOfBirth)
  - All fields validated with string length and required constraints

#### Password Management
- **ChangePasswordInput.cs** - Password change request:
  - CurrentPassword for verification
  - NewPassword with confirmation
  - Validation for password complexity

- **GetPasswordComplexitySettingOutput.cs** - Returns password requirements:
  - Minimum length
  - Required character types (uppercase, lowercase, numbers, symbols)
  - Used to display requirements to users before password change

#### Profile Pictures
- **UpdateProfilePictureInput.cs** - Profile picture upload:
  - Base64 encoded image data
  - Image type and size information
  - Cropping coordinates (X, Y, Width, Height)

- **UploadProfilePictureOutput.cs** - Upload result:
  - Temporary file token
  - Used for image preview before confirmation

- **GetProfilePictureOutput.cs** - Current profile picture:
  - ProfilePicture as base64 string
  - Used for display in UI

- **GetFriendProfilePictureInput.cs** - Retrieve another user's profile picture for chat/social features

#### Two-Factor Authentication (Google Authenticator)
- **UpdateGoogleAuthenticatorKeyOutput.cs** - 2FA setup response:
  - QR code image for Google Authenticator app
  - Secret key for manual entry
  - Used during 2FA enrollment process

#### SMS Verification
- **SendVerificationSmsInputDto.cs** - Request SMS verification code:
  - Phone number to verify
  - Used for phone number confirmation workflow

- **VerifySmsCodeInputDto.cs** - Verify SMS code:
  - Code received via SMS
  - Completes phone verification process

### Key Components

#### Self-Service Philosophy
All operations designed for current user acting on their own profile:
- Cannot edit other users (admin operations in different DTOs)
- Security-conscious (password confirmation required)
- User-friendly (QR codes, password complexity display)

#### Security Features
- Password change requires current password
- Two-factor authentication with Google Authenticator
- SMS verification for phone numbers
- Profile picture with size/type validation

### Dependencies
- **Abp.Authorization.Users** - User base classes and constants (AbpUserBase)
- **System.ComponentModel.DataAnnotations** - Validation attributes
- **inzibackend.Authorization.Users** - UserConsts for limits

## Architecture Notes

### Security-First Design
- **Current Password Verification**: ChangePasswordInput requires current password
- **2FA Enrollment**: QR code generation for secure setup
- **SMS Verification**: Phone number confirmation with code
- **Image Validation**: Profile pictures validated for type and size

### User Experience
- **Password Complexity Display**: Show requirements before user attempts change
- **QR Code Setup**: Visual 2FA enrollment with Google Authenticator
- **Image Cropping**: Client-side cropping coordinates for profile pictures
- **Preview Before Save**: Temporary tokens for image preview

### Validation Strategy
- String length validation using ABP constants
- Required fields for essential information
- Email format validation
- Phone number format validation

## Business Logic

### Profile Management Workflow
1. User navigates to profile settings
2. Retrieves CurrentUserProfileEditDto with current values
3. Updates desired fields
4. Submits for validation and save

### Password Change Workflow
1. User requests password change
2. System returns GetPasswordComplexitySettingOutput with requirements
3. User enters current password and new password
4. System validates current password and complexity requirements
5. Password updated and user notified

### Two-Factor Authentication Setup
1. User enables 2FA in profile
2. System generates secret key and QR code
3. UpdateGoogleAuthenticatorKeyOutput returns QR code image
4. User scans QR code with Google Authenticator app
5. User verifies setup with first code
6. 2FA enabled for account

### SMS Verification Workflow
1. User adds/changes phone number
2. Clicks "Send Verification Code"
3. SendVerificationSmsInputDto triggers SMS
4. User receives code via SMS
5. User enters code in VerifySmsCodeInputDto
6. System validates code
7. IsPhoneNumberConfirmed set to true

### Profile Picture Update
1. User selects image file
2. Client crops image and converts to base64
3. UpdateProfilePictureInput sent with cropping coordinates
4. System processes and stores image
5. Returns temporary token in UploadProfilePictureOutput
6. User confirms or re-crops
7. Picture saved permanently

## Usage Across Codebase
These DTOs are consumed by:
- **IProfileAppService** - Profile management service
- **User Profile UI** - Self-service profile editing interface
- **Security Settings** - Password and 2FA configuration
- **SMS Services** - Phone verification workflow
- **Image Processing Services** - Profile picture handling
- **Authentication System** - 2FA validation during login

## Cross-Reference Impact
Changes to these DTOs affect:
- User profile editing interfaces
- Password change workflows
- Two-factor authentication setup screens
- SMS verification process
- Profile picture upload/cropping features
- Security settings configuration
- Mobile app profile management