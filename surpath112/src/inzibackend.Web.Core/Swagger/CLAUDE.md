# Swagger Documentation

## Overview
This folder contains Swagger/OpenAPI customization and enhancement classes for improving API documentation quality and usability. It provides filters and extensions to enhance Swashbuckle's default behavior.

## Contents

### Files

#### SwaggerExtensions.cs
- **Purpose**: Extension methods for Swagger configuration
- **Key Methods**:
  - **InjectBaseUrl()**: Injects ABP app path into Swagger UI for correct API calls
  - **CustomDefaultSchemaIdSelector()**: Custom schema naming for generic types
- **Usage**: Called during Swagger setup in Startup.cs

#### SwaggerEnumParameterFilter.cs
- **Purpose**: Document filter for enum parameters in API operations
- **Functionality**: Enhances enum documentation with available values
- **Usage**: Automatically applied to all enum parameters

#### SwaggerEnumSchemaFilter.cs
- **Purpose**: Schema filter for enum type definitions
- **Functionality**: Improves enum documentation in schemas
- **Usage**: Applied to enum types in model definitions

#### SwaggerOperationFilter.cs
- **Purpose**: Operation filter for customizing operation documentation
- **Functionality**: Adds custom metadata to API operations
- **Usage**: Applied to all API operations

#### SwaggerOperationIdFilter.cs
- **Purpose**: Generates consistent operation IDs for API endpoints
- **Functionality**: Creates unique operation IDs based on controller and action
- **Usage**: Improves client SDK generation

### Key Components
- **Swagger UI Customization**: Inject JavaScript for ABP integration
- **Schema Generation**: Custom naming for complex types
- **Enum Documentation**: Better enum parameter documentation
- **Operation Metadata**: Enhanced operation information

### Dependencies
- **Swashbuckle.AspNetCore.SwaggerGen**: Swagger document generation
- **Swashbuckle.AspNetCore.SwaggerUI**: Swagger UI rendering
- **Abp.Extensions**: ABP extension methods (EnsureEndsWith)

## Architecture Notes

### Swagger/OpenAPI Integration
Swagger provides:
- Interactive API documentation
- Try-it-out functionality for testing endpoints
- Schema definitions for request/response models
- Client SDK generation (via tools like NSwag)

### Filter Pipeline
Swashbuckle applies filters in order:
1. **Document Filters**: Modify entire Swagger document
2. **Schema Filters**: Customize type schemas
3. **Operation Filters**: Modify individual operations
4. **Parameter Filters**: Customize parameters

## Business Logic

### InjectBaseUrl() - ABP Path Integration
```csharp
public static void InjectBaseUrl(this SwaggerUIOptions options, string pathBase)
{
    pathBase = pathBase.EnsureEndsWith('/');
    options.HeadContent = new StringBuilder(options.HeadContent)
        .AppendLine($"<script> var abp = abp || {{}}; abp.appPath = abp.appPath || '{pathBase}'; </script>")
        .ToString();
}
```

**Purpose**: Inject ABP application path into Swagger UI
- ABP JavaScript libraries need to know application base URL
- Multi-tenant scenarios may have different base paths
- Swagger UI needs correct URL for "Try it out" functionality

**Example**:
```javascript
// Application running at https://example.com/app/
// Injected script sets:
var abp = { appPath: '/app/' };

// ABP AJAX calls use correct base URL:
abp.ajax({ url: '/api/users' }) // Calls /app/api/users
```

### CustomDefaultSchemaIdSelector() - Generic Type Naming
```csharp
public static void CustomDefaultSchemaIdSelector(this SwaggerGenOptions options)
{
    string SchemaIdSelector(Type modelType)
    {
        if (!modelType.IsConstructedGenericType)
        {
            return modelType.Name;
        }

        var prefix = modelType.GetGenericArguments()
            .Select(SchemaIdSelector)
            .Aggregate<string>((previous, current) => previous + current);

        return modelType.Name.Split('`').First() + "Of" + prefix;
    }

    options.CustomSchemaIds(SchemaIdSelector);
}
```

**Problem**: Default Swagger naming for generics is unclear:
- `PagedResultDto`1` (confusing)
- `List`1` (not descriptive)

**Solution**: Custom naming convention:
- `PagedResultDtoOfUserDto` (clear)
- `ListOfString` (descriptive)
- `DictionaryOfStringAndInt32` (explicit)

**Examples**:
| Type | Default Name | Custom Name |
|------|--------------|-------------|
| `PagedResultDto<UserDto>` | PagedResultDto`1 | PagedResultDtoOfUserDto |
| `List<string>` | List`1 | ListOfString |
| `Dictionary<string, int>` | Dictionary`2 | DictionaryOfStringAndInt32 |

**Note**: Comment mentions Swashbuckle 5.0 may make this unnecessary (now at 6.x, could revisit)

## Usage Across Codebase

### Startup Configuration
```csharp
// ConfigureServices
services.AddSwaggerGen(options =>
{
    options.CustomDefaultSchemaIdSelector(); // Apply custom schema naming
    options.OperationFilter<SwaggerOperationIdFilter>();
    options.OperationFilter<SwaggerOperationFilter>();
    options.SchemaFilter<SwaggerEnumSchemaFilter>();
    options.ParameterFilter<SwaggerEnumParameterFilter>();
});

// Configure
app.UseSwaggerUI(options =>
{
    options.InjectBaseUrl("/"); // Inject ABP base URL
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});
```

### API Documentation Benefits
- **Developers**: Clear API documentation
- **Frontend Teams**: Interactive testing without Postman
- **Mobile Developers**: Generate client SDKs
- **QA Teams**: Manual API testing interface
- **Third-party Integrators**: Discover available endpoints

## Swagger Filters Deep Dive

### SwaggerEnumParameterFilter
**Purpose**: Document enum parameters with available values

**Example Enhancement**:
```csharp
// Enum definition
public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Suspended = 3
}

// Without filter: Parameter shows "integer"
// With filter: Parameter shows "integer (Active=1, Inactive=2, Suspended=3)"
```

### SwaggerEnumSchemaFilter
**Purpose**: Improve enum type documentation in schemas

**Benefits**:
- Shows available enum values in schema
- Displays enum names and numeric values
- Helps client SDK generators produce better code

### SwaggerOperationFilter
**Purpose**: Add custom metadata to operations

**Common Uses**:
- Add tags for grouping operations
- Add custom headers documentation
- Add example request/response bodies
- Document authentication requirements

### SwaggerOperationIdFilter
**Purpose**: Generate unique operation IDs

**Why Important**:
- Client SDK generators use operation IDs for method names
- Default operation IDs may conflict or be unclear
- Custom logic creates consistent, predictable names

**Example**:
```csharp
// Controller: UsersController
// Action: GetUsers
// Generated ID: Users_GetUsers
// SDK method: client.Users_GetUsers()
```

## Security Considerations

### Swagger UI in Production
- **Development**: Enable Swagger UI for testing
- **Production**: Consider disabling or securing with authentication
- **Configuration**:
```csharp
if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

### Information Disclosure
- Swagger exposes API structure
- May reveal internal implementation details
- Consider limited public API documentation
- Separate internal vs. external API docs

### Authentication in Swagger UI
Add JWT authentication to Swagger:
```csharp
services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
});
```

## Client SDK Generation

### NSwag Integration
Swagger documentation enables automatic SDK generation:

```bash
# Generate C# client
nswag openapi2csclient /input:swagger.json /output:ApiClient.cs

# Generate TypeScript client
nswag openapi2tsclient /input:swagger.json /output:api-client.ts
```

### Benefits of Custom Filters
- **CustomSchemaIdSelector**: Cleaner type names in generated code
- **SwaggerOperationIdFilter**: Better method names in SDKs
- **EnumFilters**: Proper enum generation (not just integers)

### Generated Code Example
```csharp
// With custom filters:
public class UsersClient
{
    public Task<PagedResultDtoOfUserDto> Users_GetUsers(int skipCount, int maxResultCount)
    {
        // Generated implementation
    }
}

// Enum properly typed:
public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Suspended = 3
}
```

## Performance Considerations

### Swagger Generation
- Document generated once at startup
- Cached for subsequent requests
- Minimal runtime overhead
- Filters run during generation only

### Swagger UI
- Static HTML/JavaScript files
- Loads API schema once
- Client-side rendering of documentation
- No server-side impact after initial load

## Best Practices

### Schema Naming
- Use descriptive names for generic types
- Avoid special characters in names
- Keep names consistent across API

### Operation IDs
- Use consistent naming convention
- Include controller and action name
- Avoid spaces and special characters
- Ensure uniqueness across API

### Enum Documentation
- Always use enum filters
- Provide XML comments on enum values
- Document enum purpose in summary

### Versioning
```csharp
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API V1", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "My API V2", Version = "v2" });
});
```

## Troubleshooting

### Swagger UI Not Loading
- Check Swagger middleware order in Configure()
- Verify SwaggerEndpoint path is correct
- Check WebConsts.SwaggerUiEnabled flag

### Schema Conflicts
- Use CustomSchemaIdSelector to resolve naming conflicts
- Check for types with same name in different namespaces
- Ensure custom schema IDs are unique

### Enum Not Showing Values
- Verify EnumParameterFilter registered
- Check XML documentation generation enabled
- Ensure enum has XML comments

### "Try it Out" Not Working
- Verify InjectBaseUrl() called with correct base path
- Check CORS configuration allows Swagger UI origin
- Ensure API endpoints have proper routing