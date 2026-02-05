# Models Documentation

## Overview
This folder contains the data transfer objects (DTOs), view models, and input/output models used across the web layer. These models define the contracts for API endpoints, authentication flows, and user consent management.

## Contents

### Files
*No files directly in this folder - models organized in subfolders*

### Key Components
- **Token Authentication Models**: JWT authentication contracts
- **Consent Models**: OAuth consent flow models
- **View Models**: UI presentation models

### Dependencies
- System.ComponentModel.DataAnnotations (model validation)
- Newtonsoft.Json (serialization)
- IdentityServer4 (OAuth models)

## Subfolders

### TokenAuth
Models for token-based authentication system
- **Authentication DTOs**: Login request/response models
- **External Auth Models**: OAuth provider integration
- **Impersonation Models**: User switching for admins
- **2FA Models**: Two-factor authentication flow
- **Business Value**: Secure, flexible authentication

### Consent
OAuth consent and scope management models
- **Consent UI Models**: User consent interface
- **Scope Models**: Permission scope definitions
- **Process Results**: Consent decision outcomes
- **Configuration**: Consent behavior settings
- **Business Value**: GDPR compliance and user control

## Architecture Notes
- **DTO Pattern**: Clean separation from domain entities
- **Validation**: Data annotations for input validation
- **Immutability**: Many models use readonly properties
- **Single Responsibility**: Each model has one purpose

## Business Logic
- Models enforce business rules through validation
- Required fields ensure data completeness
- Format validation (email, phone, etc.)
- Range validation for numeric values
- Custom validation for complex rules

## Security Considerations
- No sensitive data in response models
- Input validation prevents injection attacks
- Tokens have limited lifetime
- Consent models ensure user control

## Usage Across Codebase
- **Controllers**: Use models for action parameters
- **Services**: Return models from methods
- **Views**: Bind to view models
- **APIs**: Define endpoint contracts
- **Tests**: Validate model behavior