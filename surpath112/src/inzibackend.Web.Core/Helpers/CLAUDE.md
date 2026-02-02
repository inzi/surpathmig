# Helpers Documentation

## Overview
This folder contains utility classes for resolving hosting environment issues, specifically for IIS in-process hosting scenarios where the current directory may not be set correctly.

## Contents

### Files

#### CurrentDirectoryHelpers.cs
- **Purpose**: Fix current directory issues when running under IIS in-process hosting (ANCM)
- **Key Method**: `SetCurrentDirectory()`
- **Functionality**:
  - Detects if running under IIS ANCM (ASP.NET Core Module) in-process
  - Retrieves physical application path from IIS configuration
  - Sets Environment.CurrentDirectory to application root
- **Platform**: Windows-only (uses kernel32.dll and ANCM interop)
- **Usage**: Called early in application startup (Program.cs or Startup.cs)

### Key Components
- **P/Invoke Interop**: Windows kernel32.dll and aspnetcorev2_inprocess.dll
- **ANCM Integration**: Retrieves IIS configuration data
- **Environment Variables**: Checks ASPNETCORE_IIS_PHYSICAL_PATH

### Dependencies
- **System.Runtime.InteropServices**: P/Invoke support
- **Windows Kernel32.dll**: GetModuleHandle
- **ANCM Module**: http_get_application_properties

## Architecture Notes

### Problem Being Solved
When ASP.NET Core apps run under IIS with in-process hosting:
- Current directory may not be set to application root
- Relative file paths fail (configuration files, logs, content)
- Application may look in wrong directory for resources

### Solution Approach
1. Check for ASPNETCORE_IIS_PHYSICAL_PATH environment variable
2. If not found, check if running under ANCM in-process
3. Use P/Invoke to call ANCM function to get application path
4. Set Environment.CurrentDirectory to application root

### IIS Configuration Data Structure
```csharp
struct IISConfigurationData {
    IntPtr pNativeApplication;           // Native IIS application pointer
    string pwzFullApplicationPath;       // Physical path (e.g., C:\inetpub\wwwroot\app)
    string pwzVirtualApplicationPath;    // Virtual path (e.g., /app)
    bool fWindowsAuthEnabled;            // Windows authentication enabled
    bool fBasicAuthEnabled;              // Basic authentication enabled
    bool fAnonymousAuthEnable;           // Anonymous authentication enabled
}
```

### Platform Detection
```csharp
if (GetModuleHandle(AspNetCoreModuleDll) == IntPtr.Zero)
{
    return; // Not running under ANCM in-process, skip
}
```

## Business Logic

### Execution Flow
1. **Check Environment Variable**: Look for ASPNETCORE_IIS_PHYSICAL_PATH
2. **Detect ANCM**: Check if aspnetcorev2_inprocess.dll is loaded
3. **Get IIS Config**: Call http_get_application_properties via P/Invoke
4. **Extract Path**: Read pwzFullApplicationPath from struct
5. **Set Directory**: Update Environment.CurrentDirectory

### Error Handling
- Try-catch wrapper catches all exceptions and ignores
- Failures are silent (graceful degradation)
- Application continues even if directory cannot be set

### Why This Matters
Without correct current directory:
- ❌ Configuration files not found (appsettings.json)
- ❌ Log files written to wrong location
- ❌ Relative paths in code fail
- ❌ Application may fail to start

With correct current directory:
- ✅ Configuration loads properly
- ✅ Logs written to application directory
- ✅ Relative paths work as expected
- ✅ Application runs correctly

## Usage Across Codebase

### Consumed By
- **Program.cs**: Called before CreateHostBuilder
- **Startup**: Ensures correct directory during initialization
- **Configuration**: Allows relative paths to appsettings.json
- **Logging**: Enables relative log file paths

### Typical Usage Pattern
```csharp
// Program.cs
public static void Main(string[] args)
{
    CurrentDirectoryHelpers.SetCurrentDirectory(); // Fix IIS path issue
    CreateHostBuilder(args).Build().Run();
}
```

## Platform Considerations

### Windows IIS Only
- Uses Windows-specific APIs (kernel32.dll)
- ANCM is Windows/IIS specific
- Harmlessly no-ops on other platforms (module not found)

### IIS Hosting Modes
- **In-Process**: ANCM loads app in IIS worker process (w3wp.exe)
  - This helper is required for in-process mode
- **Out-of-Process**: App runs as separate process
  - This helper typically not needed

### Non-IIS Scenarios
- **Kestrel**: Not needed (current directory already correct)
- **Linux/Docker**: No-ops gracefully (module not found)
- **Azure App Service**: May be needed depending on configuration

## Performance Considerations
- P/Invoke has minimal overhead
- Only runs once at application startup
- No ongoing performance impact
- Fails fast if not applicable

## Security Considerations
- Uses safe P/Invoke patterns
- No security-sensitive operations
- Read-only access to IIS configuration
- No user input or external data

## Deployment Notes

### IIS Deployment
1. Ensure application published correctly
2. CurrentDirectoryHelpers called early in startup
3. Test file access (configuration, logs) works correctly
4. Verify logs written to application directory

### Troubleshooting
**Symptom**: Configuration files not found
- **Cause**: Current directory not set correctly
- **Solution**: Ensure SetCurrentDirectory() called in Program.Main()

**Symptom**: Logs written to Windows\System32
- **Cause**: Current directory defaulting to system directory
- **Solution**: Call SetCurrentDirectory() before logging initialization

### Configuration Files
With this helper:
```csharp
// Can use relative paths
.AddJsonFile("appsettings.json")
.AddJsonFile($"appsettings.{env}.json")
```

Without this helper (IIS in-process):
```csharp
// May need absolute paths
.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"))
```

## Reference
Based on official Microsoft documentation:
https://github.com/aspnet/AspNetCore.Docs/blob/master/aspnetcore/host-and-deploy/aspnet-core-module/samples_snapshot/2.x/CurrentDirectoryHelpers.cs