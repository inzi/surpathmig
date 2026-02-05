# HealthCheck Documentation

## Overview
This folder contains health check registration infrastructure for monitoring application health. It configures built-in health checks for database connectivity, user data validation, and cache availability.

## Contents

### Files

#### AbpZeroHealthCheck.cs
- **Purpose**: Extension method to register all application health checks
- **Key Method**: `AddAbpZeroHealthCheck()`
- **Registered Health Checks**:
  1. **Database Connection**: Validates database connectivity
  2. **Database Connection with user check**: Validates database and user data integrity
  3. **Cache**: Validates cache provider availability (Redis/In-Memory)
- **Extensibility**: Includes comment placeholder for custom health checks
- **Usage**: Called in Startup.ConfigureServices() during application initialization

### Key Components
- **Health Check Builder**: ASP.NET Core health check infrastructure
- **Database Check**: Validates EF Core DbContext can connect
- **User Check**: Ensures critical user data is accessible
- **Cache Check**: Confirms cache provider is operational

### Dependencies
- **Microsoft.Extensions.DependencyInjection**: DI container
- **inzibackend.HealthChecks**: Health check implementations
  - `inzibackendDbContextHealthCheck`: Database connectivity test
  - `inzibackendDbContextUsersHealthCheck`: Database with user validation
  - `CacheHealthCheck`: Cache provider validation

## Architecture Notes

### Health Check Pattern
Health checks follow the ASP.NET Core health check API:
1. Register checks during startup
2. Expose via endpoint (/health)
3. Each check returns Healthy, Degraded, or Unhealthy
4. Monitoring systems poll endpoint for status

### Health Check Endpoint
Typically exposed at:
- `/health` - All checks summary
- `/health/ready` - Readiness check (K8s readiness probe)
- `/health/live` - Liveness check (K8s liveness probe)

### Check Execution
- Health checks run on-demand when endpoint is called
- Can be configured to run periodically
- Results cached for performance
- Failures trigger alerts in monitoring systems

## Business Logic

### Database Connection Check
- **Purpose**: Verify database is reachable
- **Implementation**: Executes simple database query
- **Failure Scenarios**:
  - Database server down
  - Network connectivity issues
  - Invalid connection string
  - Database credentials invalid
- **Impact**: Application cannot function without database

### Database with User Check
- **Purpose**: Verify database and critical user data
- **Implementation**: Queries user table for expected data
- **Failure Scenarios**:
  - Database accessible but corrupted
  - User table missing or empty
  - Data integrity issues
- **Impact**: Authentication and authorization will fail

### Cache Check
- **Purpose**: Verify cache provider is operational
- **Implementation**: Attempts cache read/write operation
- **Failure Scenarios**:
  - Redis server down (if using Redis)
  - Network issues to cache server
  - Cache configuration invalid
- **Impact**:
  - Degraded performance (cache misses)
  - Increased database load
  - May cause application slowdown

## Usage Across Codebase

### Consumed By
- **Startup.cs**: Registers health checks during ConfigureServices
- **Health Check Middleware**: Exposes /health endpoint
- **Monitoring Systems**:
  - Azure Application Insights
  - AWS CloudWatch
  - Prometheus
  - Kubernetes probes
- **Load Balancers**: Remove unhealthy instances from pool

### Integration Points
```csharp
// Startup.cs - ConfigureServices
services.AddAbpZeroHealthCheck();

// Startup.cs - Configure
app.UseHealthChecks("/health");
```

## Health Check Results

### Response Format
```json
{
  "status": "Healthy",
  "results": {
    "Database Connection": {
      "status": "Healthy",
      "description": "Database connection successful"
    },
    "Database Connection with user check": {
      "status": "Healthy",
      "description": "Database and user data valid"
    },
    "Cache": {
      "status": "Healthy",
      "description": "Cache provider operational"
    }
  }
}
```

### Status Values
- **Healthy**: All checks passed, application fully operational
- **Degraded**: Some checks failed but app partially functional (e.g., cache down)
- **Unhealthy**: Critical checks failed, application should not receive traffic

## Extensibility

### Adding Custom Health Checks
```csharp
// 1. Create custom health check class
public class MyCustomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        // Check something critical
        bool isHealthy = CheckMyService();

        if (isHealthy)
            return Task.FromResult(HealthCheckResult.Healthy("Service OK"));
        else
            return Task.FromResult(HealthCheckResult.Unhealthy("Service down"));
    }
}

// 2. Register in AbpZeroHealthCheck.cs
builder.AddCheck<MyCustomHealthCheck>("My Custom Check");
```

### Example Custom Checks
- **External API**: Verify critical API is reachable
- **File System**: Check disk space available
- **Message Queue**: Verify RabbitMQ/Azure Service Bus connection
- **Email Service**: Test SMTP connectivity
- **Storage**: Verify blob storage accessible

## Performance Considerations
- Health checks should be fast (<1 second)
- Avoid expensive operations
- Use timeouts to prevent hanging
- Consider caching results for frequently polled endpoints
- Database checks should use simple queries

## Security Considerations
- Health endpoint may leak infrastructure details
- Consider authentication for detailed health info
- Separate public health endpoint from detailed diagnostics
- Don't expose sensitive data in health check responses

## Deployment Notes

### Kubernetes Integration
```yaml
livenessProbe:
  httpGet:
    path: /health/live
    port: 80
  initialDelaySeconds: 30
  periodSeconds: 10

readinessProbe:
  httpGet:
    path: /health/ready
    port: 80
  initialDelaySeconds: 10
  periodSeconds: 5
```

### Load Balancer Integration
- Configure LB to poll /health endpoint
- Remove instances that return Unhealthy
- Health check interval: 10-30 seconds
- Failure threshold: 2-3 consecutive failures

### Monitoring Integration
- Configure alerts on Unhealthy status
- Track health check trends over time
- Set up notifications for degraded state
- Monitor health check response times