# Sessions Documentation

## Overview
Contains proxy services for session management operations. Provides client-side access to user session information and sign-in token management.

## Contents

### Files

#### ProxySessionAppService.cs
- **Purpose**: Client proxy for ISessionAppService, handling session-related operations
- **Key Methods**:
  - `GetCurrentLoginInformations`: Retrieves current user's login information including user details, tenant, and application settings
  - `UpdateUserSignInToken`: Updates the user's sign-in token for security purposes
- **Base Class**: Inherits from ProxyAppServiceBase
- **Interface**: Implements ISessionAppService
- **Authentication**: Requires authenticated access for all operations

### Key Components
- **Session Information**: Current user and tenant context
- **Token Management**: Sign-in token updates
- **Login Context**: Complete login state information

### Dependencies
- `inzibackend.Sessions.Dto`: Session-specific DTOs
- `ProxyAppServiceBase`: Base proxy service class

## Architecture Notes
- **Stateless Sessions**: Session info retrieved from server on demand
- **Token Security**: Regular token updates for enhanced security
- **Context Management**: Provides application-wide user context

## Business Logic
- **Session Flow**:
  1. User logs in via authentication service
  2. Session service provides current context
  3. Application uses session info for authorization
  4. Token updated periodically for security
- **Information Provided**:
  - Current user details
  - Tenant information
  - Application configuration
  - Permission grants

## Usage Across Codebase
- Used during application initialization
- Referenced for user context throughout app
- Consumed by UI for displaying user info
- Part of the authentication/authorization infrastructure
- Integrated with ApplicationContext for state management

## Security Considerations
- Authenticated endpoints only
- Sign-in token rotation for security
- Session validation on each request
- Multi-tenant context enforcement

## Cross-References
- **ApplicationContext**: Stores session information
- **Authentication Services**: Works with login/logout flows
- **User Services**: Related to user profile information
- **Configuration Services**: Provides user-specific configuration