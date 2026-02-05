# Properties Documentation

## Overview
This folder contains assembly-level metadata and development environment configuration files for the Web.Core project.

## Contents

### Files

#### AssemblyInfo.cs
- **Purpose**: Assembly-level attributes and metadata
- **Key Attributes**:
  - **AssemblyConfiguration**: Empty (set by build process)
  - **AssemblyCompany**: Empty (customizable)
  - **AssemblyProduct**: "inzibackend.Web.Core"
  - **AssemblyTrademark**: Empty
  - **ComVisible**: false (not visible to COM components)
  - **Guid**: "0f0e141d-2626-4114-9805-3afd8275a6b0" (COM type library ID)
- **Usage**: Legacy metadata for .NET assembly information

#### launchSettings.json
- **Purpose**: Development environment launch profiles for debugging
- **Contains**: Configuration for running application during development
- **Usage**: Visual Studio/Rider use these profiles for F5 debugging

### Key Components
- **Assembly Metadata**: Project identification and COM interop settings
- **Launch Profiles**: Development server configurations

### Dependencies
- **System.Reflection**: Assembly attribute support
- **System.Runtime.InteropServices**: COM interop attributes

## Architecture Notes

### AssemblyInfo.cs - Legacy Pattern
In .NET Core/.NET 6+, most assembly attributes moved to project file:
```xml
<PropertyGroup>
  <Product>inzibackend.Web.Core</Product>
  <Company>Your Company</Company>
  <Version>1.0.0</Version>
</PropertyGroup>
```

AssemblyInfo.cs still useful for:
- COM interop settings
- InternalsVisibleTo attributes
- Custom assembly-level attributes

### COM Visibility
```csharp
[assembly: ComVisible(false)]
```
- Types in this assembly not accessible via COM
- Standard for pure .NET assemblies
- Prevents COM registration
- GUID still assigned for compatibility

### Type Library GUID
```csharp
[assembly: Guid("0f0e141d-2626-4114-9805-3afd8275a6b0")]
```
- Unique identifier for assembly if exposed to COM
- Not used if ComVisible(false)
- Generated when project created
- Should not be changed after release

## Launch Settings (launchSettings.json)

### Purpose
Development-time configuration for:
- Environment variables
- Launch URLs
- SSL settings
- IIS Express vs Kestrel
- Command line arguments

### Typical Structure
```json
{
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "inzibackend.Web.Core": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### Common Settings
- **commandName**: "IISExpress" or "Project" (Kestrel)
- **launchBrowser**: Auto-open browser on start
- **applicationUrl**: URLs to listen on
- **environmentVariables**: Development environment config
- **workingDirectory**: Set working directory (optional)

## Usage Across Codebase

### AssemblyInfo.cs
- **Reflection**: Code can read assembly attributes at runtime
- **COM Interop**: COM clients cannot see types (ComVisible=false)
- **Metadata**: Visual Studio displays product name, etc.

### launchSettings.json
- **Visual Studio**: F5 debugging uses these profiles
- **JetBrains Rider**: Similar profile selection
- **dotnet run**: Command line tool uses these settings
- **Not Deployed**: Development-only file (not published)

## Configuration Best Practices

### AssemblyInfo.cs
- Keep minimal (most attributes in .csproj now)
- Only include attributes not supported in project file
- Don't change GUID after initial release
- Keep ComVisible(false) unless COM interop required

### launchSettings.json
- Multiple profiles for different scenarios
- Use ASPNETCORE_ENVIRONMENT for environment switching
- Set appropriate applicationUrl for local testing
- Don't commit secrets (use user secrets instead)

## Environment Variables

### Common Development Variables
```json
"environmentVariables": {
  "ASPNETCORE_ENVIRONMENT": "Development",
  "ASPNETCORE_URLS": "https://localhost:5001",
  "ConnectionStrings__Default": "Server=localhost;...",
  "Authentication__JwtBearer__SecurityKey": "dev-key-here"
}
```

### Best Practices
- Use "Development" environment in launch settings
- Override with appsettings.Development.json
- Never commit production connection strings
- Use user secrets for sensitive data

## Deployment Notes

### AssemblyInfo.cs
- Compiled into assembly
- Included in published output
- Metadata accessible via reflection
- COM settings affect registration

### launchSettings.json
- **Not Deployed**: Development-only file
- **Not in Published Output**: Excluded by default
- **Not in Production**: Server configuration managed separately
- **Version Control**: Safe to commit (no secrets)

### Production Configuration
Instead of launch settings, production uses:
- **Environment Variables**: Set on host/container
- **appsettings.Production.json**: Environment-specific config
- **Azure App Settings**: Cloud provider configuration
- **Kubernetes ConfigMaps**: Container orchestration config

## Security Considerations

### Launch Settings
- Contains development-only configuration
- May contain localhost URLs with test data
- Should not contain production secrets
- Use user secrets for sensitive dev data

### User Secrets Alternative
For sensitive development data:
```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Server=localhost;..."
```

Benefits:
- Not in source control
- User-specific configuration
- Overrides launch settings
- More secure for team development

## Visual Studio Integration

### Profile Selection
- Debug dropdown shows profiles from launchSettings.json
- "IIS Express" profile uses IIS Express server
- Project name profile uses Kestrel
- Can add custom profiles for different scenarios

### Environment Switching
Create multiple profiles for different environments:
```json
{
  "profiles": {
    "Development": {
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "Staging": {
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Staging"
      }
    }
  }
}
```

## Troubleshooting

### Port Already in Use
If applicationUrl ports conflict:
1. Change ports in launchSettings.json
2. Or stop other application using the port
3. Or use dynamic port (remove applicationUrl)

### Environment Not Applied
If environment variables not working:
1. Check profile is selected in Visual Studio
2. Verify environmentVariables syntax
3. Restart Visual Studio/IDE
4. Check launchSettings.json is in Properties folder

### Assembly Attributes Not Recognized
If assembly attributes aren't working:
1. Ensure using System.Reflection namespace
2. Check for duplicate attributes in .csproj
3. Rebuild project (clean + build)
4. Verify attributes are assembly-level [assembly: ...]