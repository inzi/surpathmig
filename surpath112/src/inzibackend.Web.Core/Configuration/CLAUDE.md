# Configuration Documentation

## Overview
This folder contains services for accessing and modifying application configuration at runtime. It provides abstractions over the .NET configuration system to enable dynamic configuration updates and centralized configuration access.

## Contents

### Files

#### AppConfigurationAccessor.cs
- **Purpose**: Service for reading application configuration
- **Key Features**:
  - Implements IAppConfigurationAccessor interface
  - Provides typed access to configuration values
  - Supports configuration sections and nested values
  - Thread-safe configuration reading
- **Usage**: Injected into services needing configuration access

#### AppConfigurationWriter.cs
- **Purpose**: Service for writing/updating configuration at runtime
- **Key Features**:
  - Implements IAppConfigurationWriter interface
  - Updates appsettings.json dynamically
  - Maintains configuration file formatting
  - Triggers configuration reload after updates
- **Security**: Requires admin permissions for updates

### Key Components
- **Configuration Accessor**: Read-only configuration access
- **Configuration Writer**: Runtime configuration updates
- **Configuration Abstraction**: Decouples from IConfiguration

### Dependencies
- Microsoft.Extensions.Configuration (configuration framework)
- Microsoft.Extensions.Options (options pattern)
- Newtonsoft.Json (JSON manipulation for writer)

## Architecture Notes
- Follows Interface Segregation Principle (separate read/write)
- Uses dependency injection for configuration access
- Supports hot-reload of configuration changes
- Thread-safe operations for concurrent access

## Business Logic
- **Configuration Access**:
  - Centralized configuration reading
  - Type conversion and validation
  - Default value support
  - Section-based organization

- **Configuration Updates**:
  - Runtime configuration changes
  - Persistence to appsettings.json
  - Automatic application reload
  - Audit trail of changes

## Security Considerations
- Write operations restricted to administrators
- Sensitive values encrypted in configuration
- Connection strings protected
- API keys stored securely

## Usage Across Codebase
- Database connection string access
- JWT authentication settings
- Email/SMS service configuration
- Storage provider settings
- Feature flags and toggles