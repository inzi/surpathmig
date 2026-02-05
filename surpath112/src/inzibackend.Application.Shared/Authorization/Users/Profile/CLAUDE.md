# User Profile Documentation

## Overview
This folder contains service interfaces and DTOs for user profile self-service management. Users manage their own profiles including personal information, security settings, and profile pictures.

## Contents

### Files
Service interface for profile operations (IProfileAppService.cs or similar)

### Subfolders

#### Dto
Profile DTOs for editing, password management, 2FA setup, and profile pictures.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Self-service profile editing
- Password change with current password verification
- Google Authenticator 2FA
- SMS verification
- Profile picture upload and cropping

## Business Logic
Profile editing, password changes, 2FA enrollment, SMS verification, profile picture management.

## Usage Across Codebase
User profile UI, security settings, authentication system

## Cross-Reference Impact
Changes affect profile editing interfaces and security features