# Consent Models Documentation

## Overview
This folder contains models related to OAuth consent flow and scope management, primarily used for IdentityServer integration. These models handle user consent for third-party applications accessing protected resources.

## Contents

### Files

#### ConsentInputModel.cs
- **Purpose**: Captures user's consent decisions
- **Key Properties**:
  - `Button`: Action taken (consent/deny)
  - `ScopesConsented`: List of approved scopes
  - `RememberConsent`: Whether to remember decision
  - `ReturnUrl`: Post-consent redirect URL

#### ConsentViewModel.cs
- **Purpose**: View model for consent UI
- **Properties**:
  - `ClientName`: Application requesting access
  - `ClientUrl`: Application website
  - `ClientLogoUrl`: Application logo
  - `AllowRememberConsent`: Whether remembering is allowed
  - `IdentityScopes`: Identity-related permissions
  - `ResourceScopes`: API resource permissions

#### ScopeViewModel.cs
- **Purpose**: Represents individual permission scope
- **Properties**:
  - `Name`: Scope identifier
  - `DisplayName`: Human-readable name
  - `Description`: Detailed description
  - `Required`: Whether scope is mandatory
  - `Checked`: Whether user selected it
  - `Emphasize`: Whether to highlight scope

#### ProcessConsentResult.cs
- **Purpose**: Result of processing consent
- **Properties**:
  - `IsRedirect`: Whether to redirect user
  - `RedirectUri`: Where to redirect
  - `ValidationError`: Any validation errors
  - `ConsentResponse`: IdentityServer response

#### ConsentOptions.cs
- **Purpose**: Configuration for consent behavior
- **Static Properties**:
  - `EnableOfflineAccess`: Allow offline access scope
  - `OfflineAccessDisplayName`: Display text
  - `MustChooseOneErrorMessage`: Validation message

### Key Components
- **Input Models**: Capture user decisions
- **View Models**: Display consent UI
- **Result Models**: Process consent outcomes
- **Configuration**: Consent behavior options

### Dependencies
- IdentityServer4 (OAuth/OpenID provider)
- System.ComponentModel.DataAnnotations (validation)

## Architecture Notes
- MVVM pattern for consent UI
- Separation of input/output models
- Configuration through static options
- Validation at model level

## Business Logic
- **Consent Flow**:
  1. User redirected to consent page
  2. ConsentViewModel displays requested scopes
  3. User approves/denies via ConsentInputModel
  4. ProcessConsentResult determines next action
  5. User redirected back to client application

## Security Considerations
- Explicit user consent required
- Scope minimization principle
- Consent can be remembered or per-request
- Clear scope descriptions for informed consent

## Usage Across Codebase
- IdentityServer consent endpoint
- OAuth authorization flow
- Third-party app integrations
- API access control