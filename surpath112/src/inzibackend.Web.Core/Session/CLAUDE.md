# Session Documentation

## Overview
This folder contains per-request session caching infrastructure to optimize performance by caching user session data for the duration of a single HTTP request.

## Contents

### Files

#### IPerRequestSessionCache.cs
- **Purpose**: Interface for per-request session caching
- **Key Method**: `GetCurrentLoginInformationsAsync()` - Returns cached session data
- **Usage**: Abstraction for session caching implementations

#### PerRequestSessionCache.cs
- **Purpose**: Implementation of per-request session caching using HttpContext.Items
- **Key Features**:
  - Caches session data in HttpContext.Items for request lifetime
  - Falls back to session service if no HttpContext available
  - Cache key: `__PerRequestSessionCache`
  - Avoids multiple database calls per request
- **Lifetime**: Transient dependency (new instance per injection)
- **Cache Scope**: Per HTTP request only

### Key Components
- **HttpContext.Items**: Request-scoped storage for cache
- **Session App Service**: Underlying service for session data retrieval
- **Login Information DTO**: Cached user session data

### Dependencies
- **Abp.Dependency**: Dependency injection infrastructure (ITransientDependency)
- **Microsoft.AspNetCore.Http**: HTTP context access (IHttpContextAccessor)
- **inzibackend.Sessions**: Session application service
- **inzibackend.Sessions.Dto**: GetCurrentLoginInformationsOutput DTO

## Architecture Notes

### Caching Strategy
- **Scope**: Single HTTP request
- **Storage**: HttpContext.Items dictionary
- **Invalidation**: Automatic at end of request
- **Thread-Safety**: HttpContext is request-specific

### Why Per-Request Caching?
Without caching:
- Multiple components call GetCurrentLoginInformations()
- Each call queries database for user/tenant info
- Significant overhead in complex pages/API calls

With per-request cache:
- First call loads from database
- Subsequent calls return cached data
- No stale data (cache cleared after request)
- Reduces database load significantly

### Cache vs. Session vs. Memory Cache
| Type | Scope | Duration | Use Case |
|------|-------|----------|----------|
| Per-Request Cache | Single request | Until response sent | Current user info |
| Session (Cookie/Redis) | User session | Minutes/hours | User authentication |
| Memory Cache (Redis/In-Memory) | Application-wide | Configurable TTL | Reference data |

## Business Logic

### Cache Retrieval Flow
```
Component calls GetCurrentLoginInformationsAsync()
  ↓
HttpContext exists?
  ├─ No → Call session service directly (background jobs, etc.)
  └─ Yes → Check HttpContext.Items["__PerRequestSessionCache"]
      ├─ Found → Return cached value
      └─ Not Found → Call session service, cache result, return
```

### Session Information Content
GetCurrentLoginInformationsOutput typically includes:
- **User**: Id, Username, Name, Surname, Email, Profile picture
- **Tenant**: Id, Name, Edition, Features
- **Application**: Version, Features enabled
- **Permissions**: User's granted permissions (for authorization)

### Cache Key Design
```csharp
httpContext.Items["__PerRequestSessionCache"]
```
- Prefixed with `__` (convention for internal/framework keys)
- Descriptive name indicates purpose
- Stored as object, cast to specific type

## Usage Across Codebase

### Consumed By
- **Base Controllers**: Access current user information
- **Authorization Handlers**: Check user permissions
- **Views/View Components**: Display user name, profile picture
- **Audit Logging**: Log user who performed action
- **Business Logic**: Tenant-specific behavior

### Typical Usage Pattern
```csharp
public class MyController : inzibackendControllerBase
{
    private readonly IPerRequestSessionCache _sessionCache;

    public MyController(IPerRequestSessionCache sessionCache)
    {
        _sessionCache = sessionCache;
    }

    public async Task<IActionResult> Index()
    {
        var session = await _sessionCache.GetCurrentLoginInformationsAsync();
        var userName = session.User.UserName;
        var tenantName = session.Tenant?.TenancyName;

        // Use session data...
    }
}
```

### Performance Impact
**Without Cache (10 components accessing session):**
- 10 database queries
- ~500ms total overhead

**With Cache (10 components accessing session):**
- 1 database query
- ~50ms total overhead
- 90% reduction in database load

## Implementation Details

### HttpContext.Items
```csharp
httpContext.Items["key"] = value;
```
- Dictionary<object, object> storage
- Request-scoped lifetime
- Thread-safe (HttpContext is per-request)
- Cleared automatically at end of request

### Transient Dependency
```csharp
public class PerRequestSessionCache : IPerRequestSessionCache, ITransientDependency
```
- New instance created per injection
- Not singleton (avoids stale HttpContext references)
- HttpContextAccessor provides correct HttpContext per request

### Fallback for Background Jobs
```csharp
if (httpContext == null)
{
    return await _sessionAppService.GetCurrentLoginInformations();
}
```
- Background jobs don't have HttpContext
- Hangfire/Quartz jobs execute outside HTTP request
- Falls back to direct service call (no caching)

## Performance Considerations

### When Cache Helps Most
- Complex pages with many components
- API calls with multiple authorization checks
- Navigation rendering with user info
- Audit logging on every action

### Cache Effectiveness
**High Traffic Page:**
- 100 requests/second
- 10 session checks per request
- Without cache: 1000 DB queries/second
- With cache: 100 DB queries/second

### Memory Impact
- Minimal (single DTO per request)
- Cache cleared automatically
- No memory leak risk
- No cache eviction needed

## Security Considerations

### Cache Isolation
- HttpContext is per-request (user-specific)
- No cross-user cache contamination
- Each user gets own cache instance
- Automatic cleanup prevents data leakage

### Stale Data Prevention
- Cache lifetime limited to request
- User permissions changes won't affect current request
- Next request gets fresh data
- Acceptable for security (changes effective immediately)

### Authorization Notes
- Cached permissions used for authorization
- Permission changes require new request to take effect
- Consider invalidating user sessions on critical permission changes
- Background jobs bypass cache (always fresh data)

## Testing Considerations

### Unit Testing
```csharp
[Fact]
public async Task Should_Cache_Session_Data()
{
    // Arrange
    var httpContext = new DefaultHttpContext();
    var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };
    var sessionService = Substitute.For<ISessionAppService>();
    sessionService.GetCurrentLoginInformations()
        .Returns(new GetCurrentLoginInformationsOutput { /* ... */ });

    var cache = new PerRequestSessionCache(httpContextAccessor, sessionService);

    // Act
    var result1 = await cache.GetCurrentLoginInformationsAsync();
    var result2 = await cache.GetCurrentLoginInformationsAsync();

    // Assert
    await sessionService.Received(1).GetCurrentLoginInformations(); // Only called once
    Assert.Equal(result1, result2); // Same instance returned
}
```

### Integration Testing
- Test cache behavior in realistic HTTP request
- Verify cache cleared between requests
- Test fallback when HttpContext unavailable

## Troubleshooting

### Cache Not Working
**Symptom**: Multiple database calls per request
- **Cause**: IPerRequestSessionCache not injected, using ISessionAppService directly
- **Solution**: Inject and use IPerRequestSessionCache

### Stale Session Data
**Symptom**: Permission changes not effective until next request
- **Expected Behavior**: Per-request cache by design
- **Solution**: If immediate effect needed, invalidate user session (force re-login)

### Background Job Errors
**Symptom**: NullReferenceException in background jobs
- **Cause**: Code assumes HttpContext exists
- **Solution**: Cache implementation already handles this (fallback to service)

## Best Practices

### Do Use Cache For
- ✅ User information display
- ✅ Authorization checks
- ✅ Audit logging
- ✅ Tenant-specific logic

### Don't Use Cache For
- ❌ Data that changes during request
- ❌ Cross-user data
- ❌ Long-running operations spanning multiple requests
- ❌ Background jobs (cache unavailable, use service directly)

### Dependency Injection
```csharp
// ✅ Correct: Inject interface
public MyClass(IPerRequestSessionCache cache) { }

// ❌ Wrong: Inject implementation directly
public MyClass(PerRequestSessionCache cache) { }

// ❌ Wrong: Use session service directly (bypasses cache)
public MyClass(ISessionAppService sessionService) { }
```

### Cache Key Convention
- Prefix with `__` for internal/framework keys
- Use descriptive names
- Avoid key collisions with application data
- Document custom cache keys