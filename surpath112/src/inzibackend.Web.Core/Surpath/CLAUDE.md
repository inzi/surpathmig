# Surpath Documentation

## Overview
This folder contains Surpath-specific web infrastructure, particularly focused on enhanced logging and monitoring capabilities. It provides custom middleware and event handlers for comprehensive exception tracking and diagnostics.

## Contents

### Files

#### WatchDogLogger.cs
- **Purpose**: Custom exception logging middleware and event handlers
- **Key Components**:
  - `SurpathWatchdogExceptionMiddleware`: HTTP middleware for exception logging
  - `SurpathWatchdogEventBus`: Event handler for ABP exceptions
  - `SurpathWatchdogValidation`: Exception filter for validation errors
- **Key Features**:
  - Captures client IP addresses for all exceptions
  - Logs HTTP status codes with exceptions
  - Integrates with ABP's event bus
  - Provides detailed exception context
- **Extension Method**: `UseSurpathExceptionLogger` for easy middleware registration

### Key Components
- **Exception Middleware**: HTTP pipeline exception interceptor
- **Event Bus Handler**: ABP exception event subscriber
- **Validation Filter**: MVC action filter for validation exceptions
- **Structured Logging**: Consistent exception format

### Dependencies
- Microsoft.AspNetCore.Http (HTTP context access)
- Abp.Events.Bus (event bus integration)
- Castle.Core.Logging (logging framework)
- Microsoft.AspNetCore.Mvc.Filters (action filters)

## Architecture Notes
- **Middleware Pipeline**: Integrates early in request pipeline
- **Event-Driven**: Subscribes to ABP exception events
- **Multi-Layer**: Catches exceptions at different levels
- **Structured Logging**: Consistent log format for analysis

## Business Logic
- **Exception Tracking**:
  1. Captures all unhandled exceptions
  2. Enriches with client IP address
  3. Includes HTTP status code
  4. Logs exception type and message
  5. Maintains exception stack trace

- **Log Format**:
  `Watchdog Exception: statuscode={code} clientip={ip} type={type} message={message}`

## Security Considerations
- IP addresses logged for security auditing
- No sensitive data exposed in logs
- Exception details sanitized
- Rate limiting considerations

## Usage Across Codebase
- Startup.cs middleware configuration
- Global exception handling
- Security incident tracking
- Performance monitoring
- Debugging production issues

## Configuration
Middleware registration in Startup.cs:
```csharp
app.UseSurpathExceptionLogger();
```

## Monitoring Benefits
- Track error patterns by IP
- Identify malicious activity
- Monitor application health
- Debug client-specific issues
- Compliance audit trail