# Properties Documentation

## Overview
This folder contains project metadata and launch profiles for the ASP.NET Core MVC application, primarily used during development and deployment configuration.

## Files

### launchSettings.json
**Purpose**: Development launch configuration for Visual Studio, Rider, and dotnet CLI

**Profiles**:
- **IIS Express**: Launch via IIS Express (Visual Studio default)
- **inzibackend.Web.Mvc**: Launch via Kestrel (self-hosted)
- **Docker**: Launch in Docker container (if Docker support added)

**Configuration**:
```json
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "https://localhost:44302",
      "sslPort": 44302
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "inzibackend.Web.Mvc": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "https://localhost:44302",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

**Environment Variables**:
- `ASPNETCORE_ENVIRONMENT`: Development, Staging, or Production
- Custom variables for feature flags, database connections, etc.

## Usage

### Visual Studio
- F5 or Debug > Start Debugging uses selected profile
- Profile dropdown in toolbar
- Properties > Debug tab to edit

### Rider
- Run Configurations dropdown
- Edit Configurations to modify

### VS Code
- `.vscode/launch.json` references these profiles
- Debug panel (Ctrl+Shift+D) to launch

### Command Line
```bash
dotnet run --launch-profile "inzibackend.Web.Mvc"
```

## Architecture Notes

### Port Configuration
- Default HTTPS port: 44302
- HTTP redirects to HTTPS (enforced in Startup.cs)
- Port must not conflict with other services

### SSL Certificate
- Development: Uses `dotnet dev-certs https`
- Staging/Production: Requires proper SSL certificate
- IIS handles certificate in production

### Multiple Profiles
Useful for:
- Different authentication modes
- Different database connections
- Feature flag variations
- Performance profiling

## Related Documentation
- [Startup/CLAUDE.md](../Startup/CLAUDE.md): Application startup configuration