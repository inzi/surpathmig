# Properties Documentation

## Overview
The Properties folder contains Visual Studio and development configuration files that define how the Web Host application is launched and debugged during development.

## Contents

### Files

#### launchSettings.json
- **Purpose**: Defines launch profiles for different development scenarios
- **IIS Settings**:
  - Windows Authentication: Disabled
  - Anonymous Authentication: Enabled
  - IIS Express URL: https://localhost:44301/
  - SSL Port: 44301

- **Launch Profiles**:

  1. **IIS Express**:
     - Command: IISExpress
     - Launches browser automatically
     - Uses IIS Express for hosting

  2. **Mobile**:
     - Command: Project (Kestrel)
     - Environment: Development
     - URL: https://0.0.0.0:44301 (all network interfaces)
     - Purpose: Allows mobile devices on network to access API

  3. **inzibackend.Web.Host** (Default):
     - Command: Project (Kestrel)
     - Environment: Development
     - URL: https://localhost:44301
     - Standard development profile

  4. **Docker**:
     - Command: Docker
     - Dynamic service URL mapping
     - Publishes all ports
     - Container-based development

### Key Components

#### Development URLs
- Primary: https://localhost:44301
- Mobile: https://0.0.0.0:44301
- Docker: Dynamic based on container

#### Environment Configuration
- ASPNETCORE_ENVIRONMENT set to "Development"
- Enables development-specific features
- Detailed error pages and logging

### Dependencies
- Visual Studio or VS Code for profile selection
- IIS Express for Windows development
- Docker Desktop for container profile
- SSL certificate for HTTPS

## Architecture Notes

### Port Configuration
- Consistent port 44301 across all profiles
- HTTPS enforced in all scenarios
- SSL certificate required for development

### Mobile Development Support
- 0.0.0.0 binding allows external access
- Useful for testing mobile apps
- Same port as standard development

### Container Support
- Docker profile for containerized development
- Dynamic port mapping
- Simplifies container-based testing

## Business Logic

### Profile Selection Logic
1. **IIS Express**: Windows developers with IIS
2. **Mobile**: Mobile app developers needing network access
3. **Project**: Standard .NET Core development
4. **Docker**: Container-based development/deployment

### Security Considerations
- HTTPS enforced even in development
- Mobile profile exposes to network (security risk)
- Authentication still required for API access

## Usage Across Codebase

### Development Workflow
1. Developer selects profile in IDE
2. Application starts with profile settings
3. Browser launches (if configured)
4. API available at configured URL

### Configuration Impact
- Affects Startup.cs environment detection
- Influences logging configuration
- Determines available development features

### Related Configuration
- appsettings.Development.json for Development environment
- Startup.cs reads environment for configuration
- CORS settings must allow development URLs