# Startup Documentation

## Overview
This folder contains application startup configuration including dependency injection setup, middleware pipeline configuration, authentication configuration, and module registration. It defines how the ASP.NET Core MVC application initializes and configures its services.

## Files

### Program.cs
**Purpose**: Application entry point

**Functionality**:
- Creates and configures `WebHostBuilder`
- Sets up Kestrel server
- Configures logging
- Loads configuration files
- Runs the application

**Structure**:
```csharp
public class Program {
    public static void Main(string[] args) {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseStartup<Startup>();
            });
}
```

### Startup.cs
**Purpose**: Application configuration and service registration

**Key Methods**:
- `ConfigureServices(IServiceCollection services)`: Dependency injection registration
- `Configure(IApplicationBuilder app)`: Middleware pipeline configuration

**Service Configuration**:
- ABP module system
- Entity Framework Core
- Authentication (cookies, JWT, external providers)
- Authorization policies
- MVC with Razor Pages
- SignalR for real-time features
- Hangfire for background jobs
- API versioning
- CORS policies
- Health checks

**Middleware Pipeline**:
```csharp
public void Configure(IApplicationBuilder app) {
    // Error handling
    app.UseExceptionHandler("/Error");

    // HTTPS redirection
    app.UseHttpsRedirection();

    // Static files
    app.UseStaticFiles();

    // Routing
    app.UseRouting();

    // CORS
    app.UseCors();

    // Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // ABP Framework
    app.InitializeAbp();

    // Endpoints
    app.UseEndpoints(endpoints => {
        endpoints.MapControllerRoute("default",
            "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapHub<ChatHub>("/signalr-chat");
    });
}
```

### inzibackendWebMvcModule.cs
**Purpose**: ABP module definition for MVC application

**Functionality**:
- Implements `AbpModule` lifecycle methods
- `PreInitialize()`: Early configuration
- `Initialize()`: Module initialization
- `PostInitialize()`: Final setup after all modules loaded

**Responsibilities**:
- Navigation provider registration
- View engine configuration
- Resource embedding
- Feature configuration
- Module dependencies declaration

**Example**:
```csharp
[DependsOn(
    typeof(inzibackendWebCoreModule),
    typeof(inzibackendApplicationModule),
    typeof(inzibackendEntityFrameworkModule))]
public class inzibackendWebMvcModule : AbpModule {
    public override void PreInitialize() {
        Configuration.Navigation.Providers.Add<AppNavigationProvider>();
        Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat =
            "{0}." + _appConfiguration["App:WebSiteRootAddress"];
    }

    public override void Initialize() {
        IocManager.RegisterAssemblyByConvention(typeof(inzibackendWebMvcModule).Assembly);
    }
}
```

### AuthConfigurer.cs
**Purpose**: Centralized authentication configuration

**Functionality**:
- Configures cookie authentication for MVC
- Configures JWT authentication for API
- Configures external authentication providers (OAuth)
- Sets authentication schemes and policies

**External Providers**:
- Google OAuth
- Facebook OAuth
- Microsoft Account
- Twitter OAuth
- OpenID Connect (generic)
- WS-Federation (ADFS)

**Example**:
```csharp
public static class AuthConfigurer {
    public static void Configure(IServiceCollection services, IConfiguration configuration) {
        // Cookie authentication
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
            });

        // External providers
        if (bool.Parse(configuration["Authentication:Google:IsEnabled"])) {
            services.AddGoogleAuthentication(configuration);
        }

        if (bool.Parse(configuration["Authentication:Facebook:IsEnabled"])) {
            services.AddFacebookAuthentication(configuration);
        }
    }
}
```

### RazorViewLocationExpander.cs
**Purpose**: Custom Razor view location strategy

**Functionality**:
- Extends default view search paths
- Supports theme-based view locations
- Enables view overrides per tenant
- Adds custom view directories

**View Search Order**:
1. Tenant-specific views (if multi-tenant)
2. Theme-specific views (if themed)
3. Area-specific views
4. Default MVC locations

## Architecture Notes

### Dependency Injection
Services registered in `ConfigureServices()`:
- **Transient**: Created each time requested
- **Scoped**: One instance per HTTP request
- **Singleton**: Single instance for application lifetime

### Middleware Order
Order is critical:
1. Exception handling (first to catch all errors)
2. HTTPS redirection
3. Static files
4. Routing
5. CORS
6. Authentication (before authorization)
7. Authorization (before endpoints)
8. Custom middleware
9. Endpoints (last)

### ABP Module System
- Modules declare dependencies via `[DependsOn]`
- ABP initializes modules in dependency order
- Lifecycle: PreInitialize → Initialize → PostInitialize
- Allows modular application architecture

### Configuration Sources
Configuration loaded from (in order):
1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. Environment variables
4. User secrets (development only)
5. Command-line arguments

## Usage Across Codebase

### Adding New Service
```csharp
// In ConfigureServices
services.AddTransient<IMyService, MyService>();
```

### Adding New Middleware
```csharp
// In Configure
app.UseMiddleware<MyCustomMiddleware>();
```

### Adding New External Provider
```csharp
// In AuthConfigurer
services.AddAuthentication()
    .AddOAuth("NewProvider", options => {
        options.ClientId = configuration["Auth:NewProvider:ClientId"];
        options.ClientSecret = configuration["Auth:NewProvider:ClientSecret"];
    });
```

## Dependencies

### Internal
- `inzibackend.Web.Core`: Shared web components
- `inzibackend.Application`: Application services
- `inzibackend.EntityFrameworkCore`: Data access
- `inzibackend.Core`: Domain layer

### External
- ASP.NET Core framework
- ABP Framework
- Entity Framework Core
- SignalR
- Hangfire
- Various authentication providers

## Related Documentation
- [AuthConfigurers/CLAUDE.md](AuthConfigurers/CLAUDE.md): Tenant-aware external authentication
- [inzibackend.Web.Core/CLAUDE.md](../../../inzibackend.Web.Core/CLAUDE.md): Shared web infrastructure
- [Program.cs and Startup.cs]: Core application files (in root directory)