# Extensions Documentation

## Overview
This folder contains extension methods for ASP.NET Core application configuration, specifically for handling forwarded headers in proxy scenarios.

## Contents

### Files

#### ApplicationBuilderExtensions.cs
- **Purpose**: Extension methods for IApplicationBuilder configuration
- **Key Method**: `UseinzibackendForwardedHeaders()`
- **Functionality**:
  - Configures forwarded headers middleware
  - Enables handling of X-Forwarded-For and X-Forwarded-Proto headers
  - Clears known networks and proxies (trusts all proxies)
- **Usage**: Called in Startup.cs during middleware pipeline configuration

### Key Components
- **Forwarded Headers Middleware**: Reads proxy headers and updates HttpContext
- **X-Forwarded-For**: Original client IP address
- **X-Forwarded-Proto**: Original protocol (HTTP/HTTPS)

### Dependencies
- **Microsoft.AspNetCore.Builder**: ASP.NET Core application builder
- **Microsoft.AspNetCore.HttpOverrides**: Forwarded headers support

## Architecture Notes

### Middleware Purpose
When an application runs behind a reverse proxy (NGINX, IIS, load balancer), the original client information is forwarded via HTTP headers. This middleware reads those headers and updates HttpContext so the application sees the correct:
- Client IP address
- Original protocol (HTTP vs HTTPS)

### Configuration Details
```csharp
ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
```
- Enables processing of both forwarded IP and protocol
- Essential for correct redirect URLs and logging

```csharp
options.KnownNetworks.Clear();
options.KnownProxies.Clear();
```
- Removes default restrictions on which proxies to trust
- Trusts all proxies (appropriate for controlled proxy environments)
- **Security Note**: Only safe when application is not directly internet-facing

## Business Logic

### Proxy Scenarios
- **Load Balancer**: Distribute traffic across multiple app servers
- **NGINX Reverse Proxy**: SSL termination and static file serving
- **IIS ARR**: Application Request Routing
- **Cloud Providers**: Azure App Service, AWS ELB

### Client IP Logging
Without this middleware:
- Logs show proxy IP instead of client IP
- Security logging less effective
- Rate limiting affects proxy instead of client

With this middleware:
- Correct client IP available in HttpContext.Connection.RemoteIpAddress
- Security logs show real client addresses
- Rate limiting works per-client

### HTTPS Detection
Without this middleware:
- App thinks all requests are HTTP (proxy terminated SSL)
- Redirect URLs use http:// instead of https://
- Cookies may not set Secure flag

With this middleware:
- App correctly detects HTTPS from X-Forwarded-Proto
- Redirect URLs use correct protocol
- Security features work correctly

## Usage Across Codebase

### Consumed By
- **Startup.cs**: Called early in middleware pipeline configuration
- **Logging Infrastructure**: Uses correct IP addresses
- **Security Features**: Rate limiting, IP blocking
- **URL Generation**: Correct protocol in generated URLs

### Middleware Pipeline Position
Should be called early in pipeline, typically:
```csharp
app.UseinzibackendForwardedHeaders(); // First
app.UseAuthentication();
app.UseAuthorization();
// ... other middleware
```

## Security Considerations

### Trust All Proxies
```csharp
options.KnownNetworks.Clear();
options.KnownProxies.Clear();
```
- **Risk**: Application trusts any proxy's forwarded headers
- **Mitigation**: Only safe when application is behind controlled proxy
- **Best Practice**: If internet-facing, specify known proxy IPs

### Header Spoofing
- If application is directly internet-facing, attackers can forge headers
- Always deploy behind trusted proxy in production
- Consider restricting known proxies in high-security scenarios

### Recommended Deployment
✅ Internet → Trusted Proxy → Application (safe)
❌ Internet → Application (vulnerable to header spoofing)

## Performance Considerations
- Minimal overhead (header parsing only)
- No database or external service calls
- Runs early in pipeline before heavy processing

## Deployment Notes

### Production Checklist
1. Ensure application is behind reverse proxy/load balancer
2. Verify proxy forwards X-Forwarded-For and X-Forwarded-Proto headers
3. Test client IP logging shows correct addresses
4. Verify HTTPS redirects use correct protocol
5. Consider restricting known proxies for added security

### Proxy Configuration Examples

**NGINX:**
```nginx
proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
proxy_set_header X-Forwarded-Proto $scheme;
```

**IIS ARR:**
- Enable "Preserve client IP in X-Forwarded-For header"
- Enable "Preserve original Host header"

**Azure App Service:**
- Automatically sets forwarded headers
- No additional configuration required