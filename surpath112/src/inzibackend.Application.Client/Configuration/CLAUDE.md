# Configuration Documentation

## Overview
Manages user configuration retrieval and session management. This folder handles fetching user-specific configuration from the server and manages token refresh scenarios.

## Contents

### Files

#### UserConfigurationService.cs
- **Purpose**: Service for retrieving and managing user configuration with automatic token refresh
- **Key Features**:
  - Fetches user configuration for both authenticated and anonymous users
  - Automatic access token refresh when expired
  - Session timeout detection and handling
  - Callback support for token refresh and session timeout events
- **Methods**:
  - `GetAsync`: Main method to retrieve user configuration
  - `GetAuthenticatedUserConfig`: Handles authenticated user config with token management
  - `HandleSessionTimeOut`: Manages session expiration
  - `RefreshAccessTokenAndSendRequestAgain`: Refreshes token and retries request
- **Events**:
  - `OnSessionTimeOut`: Callback when session expires
  - `OnAccessTokenRefresh`: Callback when token is refreshed
- **Dependency**: ITransientDependency (new instance per request)

#### AbpUserConfigurationDtoExtensions.cs
- **Purpose**: Extension methods for AbpUserConfigurationDto
- **Features**: Helper methods for working with user configuration DTOs
- **Usage**: Provides convenient methods like `HasSessionUserId()`

### Key Components
- **Token Management**: Automatic token refresh without user intervention
- **Session Handling**: Graceful session timeout management
- **Configuration Cache**: User configuration retrieval and caching
- **Event System**: Callbacks for important lifecycle events

### Dependencies
- `Abp.Web.Models.AbpUserConfiguration`: ABP configuration models
- `inzibackend.ApiClient`: Core API client and token manager
- `Abp.Dependency`: Dependency injection interfaces

## Architecture Notes
- **Transient Service**: New instance created per dependency resolution
- **Token Lifecycle**: Manages complete token refresh workflow
- **Fallback Strategy**: Handles both authenticated and anonymous scenarios
- **Event-Driven**: Uses callbacks for decoupled event handling

## Business Logic
- **Configuration Flow**:
  1. Check if user is logged in
  2. Fetch appropriate configuration (authenticated/anonymous)
  3. Verify session validity
  4. Refresh token if needed
  5. Retry request with new token
- **Session Management**:
  - Detects expired sessions
  - Logs out user when refresh token expires
  - Notifies application of session changes

## Usage Across Codebase
- Used during application initialization
- Referenced for user-specific settings
- Consumed by UI for permission checks
- Part of the authentication infrastructure

## Error Handling
- Graceful degradation for expired sessions
- Automatic retry with refreshed tokens
- Callback notifications for error scenarios