# Caching DTOs Documentation

## Overview
This folder contains a single Data Transfer Object (DTO) for application cache management. The cache system provides performance optimization by storing frequently accessed data in memory, reducing database queries and improving response times across the application.

## Contents

### Files

- **CacheDto.cs** - Simple cache identifier:
  - Name - Cache name/key for identification
  - Used to target specific caches for operations (clear, get stats)

### Key Components

The cache system manages multiple named caches such as:
- User permissions cache
- Tenant configuration cache
- Localization resource cache
- Application settings cache
- Entity caches for frequently accessed data

### Dependencies
- None - Pure DTO with no external dependencies

## Architecture Notes

### Cache Management Pattern
- **Named Caches**: Each cache has unique identifier
- **Administrative Control**: Allows selective cache clearing
- **Multi-Tenant**: Cache names may include tenant context

### Cache Operations
Typical operations using CacheDto:
- **Clear Cache**: Remove specific cache by name
- **Get Cache Stats**: Retrieve metrics for named cache
- **List Caches**: Enumerate all available caches

## Business Logic

### Cache Clearing Use Cases
1. **After Configuration Changes**: Clear config cache when settings updated
2. **Permission Changes**: Clear permission cache when roles modified
3. **Tenant Changes**: Clear tenant-specific caches
4. **Troubleshooting**: Clear caches to resolve stale data issues
5. **Deployment**: Clear all caches after application updates

### Performance Considerations
- Clearing caches temporarily degrades performance (cache miss)
- System automatically repopulates caches on demand
- Selective clearing minimizes performance impact

## Usage Across Codebase
CacheDto is consumed by:
- **ICachingAppService** - Cache management operations
- **Admin Dashboard** - Cache monitoring and management UI
- **Configuration Services** - Auto-clear related caches on config changes
- **Deployment Scripts** - Clear caches during deployments
- **Troubleshooting Tools** - Manual cache management

## Cross-Reference Impact
Changes to CacheDto affect:
- Admin cache management interface
- Cache clearing workflows
- Performance monitoring dashboards
- System health checks