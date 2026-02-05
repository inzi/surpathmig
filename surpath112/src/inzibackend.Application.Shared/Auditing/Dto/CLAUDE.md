# Auditing DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the audit logging system. These DTOs define the contract for querying and retrieving audit trails of user actions and entity changes within the application. The audit system tracks all significant operations including service calls, entity modifications, and system events.

## Contents

### Files

#### List DTOs
- **AuditLogListDto.cs** - Comprehensive audit log entry containing:
  - User identity (UserId, UserName)
  - Impersonation tracking (ImpersonatorTenantId, ImpersonatorUserId)
  - Service method execution details (ServiceName, MethodName, Parameters)
  - Execution metrics (ExecutionTime, ExecutionDuration)
  - Client information (ClientIpAddress, ClientName, BrowserInfo)
  - Exception tracking and custom data
  - Mapped in CustomDtoMapper for optimized queries

- **EntityChangeListDto.cs** - Simplified entity change list item for browsing entity modifications

- **EntityChangeDto.cs** - Detailed entity change record containing:
  - Change metadata (ChangeTime, ChangeType: Created/Updated/Deleted)
  - Entity identification (EntityId, EntityTypeFullName)
  - Tenant isolation (TenantId)
  - Change set tracking (EntityChangeSetId)
  - Entity entry snapshot (EntityEntry)

- **EntityPropertyChangeDto.cs** - Individual property change details within entity modifications

#### Input DTOs
- **GetAuditLogsInput.cs** - Comprehensive audit log query parameters:
  - Date range filtering (StartDate, EndDate)
  - User filtering (UserName)
  - Service filtering (ServiceName, MethodName, BrowserInfo)
  - Exception filtering (HasException)
  - Performance filtering (MinExecutionDuration, MaxExecutionDuration)
  - Implements IShouldNormalize for smart sorting defaults
  - Default sort: ExecutionTime DESC
  - Dynamic sorting normalization for User and AuditLog tables

- **GetEntityChangeInput.cs** - Entity change query with entity type and date filtering

### Key Components

#### Base Classes
- **EntityDto<long>** - Standard ABP DTO base for entities with long ID
- **PagedAndSortedInputDto** - Provides paging and sorting capabilities

#### Enumerations
- **EntityChangeType** (from ABP) - Created, Updated, Deleted

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **Abp.Events.Bus.Entities** - Entity change types
- **Abp.Runtime.Validation** - Validation infrastructure
- **inzibackend.Common** - Sorting helpers
- **inzibackend.Dto** - Base paging DTOs

## Architecture Notes

### Audit Trail Pattern
- **Two-Level Tracking**: Separates service-level auditing (AuditLog) from entity-level auditing (EntityChange)
- **Change Sets**: Groups related entity changes into logical transactions
- **Property-Level Granularity**: Tracks individual property modifications for detailed history

### Performance Optimization
- AuditLogListDto mapped in CustomDtoMapper for efficient SQL generation
- Paging support for large audit datasets
- Sorting normalization handles joined table references

### Security & Compliance
- **Impersonation Tracking**: Records who performed actions on behalf of others
- **Exception Logging**: Captures failures for security analysis
- **Client Identification**: IP address and browser tracking for security audits

### Query Flexibility
- Range-based querying (dates, durations)
- Multi-field filtering (user, service, method)
- Performance analysis (execution duration filtering)

## Business Logic

### Audit Use Cases
1. **Security Investigations**: Track user actions, login patterns, impersonation
2. **Compliance Reporting**: Generate audit reports for regulatory requirements
3. **Performance Analysis**: Identify slow-running operations
4. **Troubleshooting**: Review exception patterns and system behavior
5. **Entity History**: Track who changed what and when for any entity

### Data Retention
- Audit logs accumulate over time
- Expired log backup services manage retention
- Performance filtering helps with large datasets

## Usage Across Codebase
These DTOs are consumed by:
- **IAuditLogAppService** - Audit log viewing and querying
- **IEntityChangeAppService** - Entity change history services
- **Admin dashboards** - System monitoring and security
- **Compliance reports** - Audit trail documentation
- **Troubleshooting tools** - Error analysis and debugging

## Cross-Reference Impact
Changes to these DTOs affect:
- Audit log viewers in admin interfaces
- Compliance reporting systems
- Security monitoring dashboards
- Entity history features throughout the application
- Export/download functionality for audit data