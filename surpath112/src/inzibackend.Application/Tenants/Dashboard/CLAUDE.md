# Tenant Dashboard Service Documentation

## Overview
Application service providing dashboard data and statistics for tenant administrators. Generates dashboard widgets, charts, and metrics specific to the tenant's compliance tracking operations. Includes a random data generator for development and testing purposes.

## Contents

### Files

#### TenantDashboardAppService.cs
- **Purpose**: Main tenant dashboard data provider
- **Key Features**:
  - Dashboard widget data retrieval
  - Compliance statistics and metrics
  - Recent activity summaries
  - User statistics
  - Department performance metrics
  - Trend analysis data
- **Typical Methods**:
  - `GetDashboardData()`: Comprehensive dashboard data
  - `GetComplianceStatistics()`: Compliance metrics over time
  - `GetRecentActivities()`: Latest system activities
  - `GetUserStatistics()`: User counts and status
  - `GetTopRecords()`: Top performing/problem areas

#### DashboardRandomDataGenerator.cs
- **Purpose**: Generates random/sample data for dashboard testing
- **Key Features**:
  - Populates dashboard with realistic test data
  - Useful for development and demo environments
  - Creates random statistics and trends
  - Prevents need for real production data during development
- **Usage**: Development and testing only, disabled in production

### Key Components

**Dashboard Widgets:**
- Compliance overview (compliant vs. non-compliant counts)
- Recent document uploads
- Pending approvals count
- Expiring documents alert
- Department compliance comparison
- User activity timeline

**Statistics Generation:**
- Time-series data for charts
- Aggregate counts and percentages
- Trend calculations
- Comparison metrics (period over period)

### Dependencies
- **External**:
  - ABP Framework (Application Services, Caching)
  - Entity Framework Core (data aggregation)
- **Internal**:
  - Surpath domain entities
  - Compliance evaluator services
  - User and cohort repositories
  - Record management repositories

## Architecture Notes
- **Pattern**: Application Service with DTO responses
- **Caching**: Dashboard data cached for performance
- **Tenant Scoped**: All data filtered to current tenant
- **Real-time**: Can use SignalR for live updates

## Business Logic

### Dashboard Data Compilation
1. Query compliance records for current period
2. Aggregate by status, category, department
3. Calculate compliance percentages
4. Identify trends (improving/declining)
5. Fetch recent activities
6. Compile into dashboard DTOs
7. Cache results for performance

### Key Metrics
- **Overall Compliance**: % of users fully compliant
- **Pending Items**: Count of items awaiting review
- **Expiring Soon**: Documents expiring within X days
- **Recent Activity**: Latest uploads, approvals, changes
- **Department Comparison**: Compliance rates by department
- **User Status**: Active, inactive, compliant, non-compliant counts

## Usage Across Codebase

### Primary Consumers
- Tenant admin dashboard page
- Compliance overview reports
- Manager performance dashboards
- Mobile app dashboard screens

### Typical Usage
```csharp
// Load dashboard on page load
var dashboardData = await _tenantDashboardAppService.GetDashboardData();

// Display compliance chart
var complianceStats = await _tenantDashboardAppService
    .GetComplianceStatistics(startDate, endDate);
```

## Performance Considerations
- **Caching**: Dashboard data cached with 5-15 minute expiration
- **Query Optimization**: Use indexed queries for aggregations
- **Async Loading**: Load widgets asynchronously
- **Pagination**: Limit recent activity to last N items
- **Background Refresh**: Update cache in background job

## Best Practices
- Cache dashboard data to reduce database load
- Refresh cache after major compliance updates
- Provide date range filters for historical views
- Use charts for visual representation of trends
- Include drill-down capability to detail views
- Show actionable items (what needs attention)

## Extension Points
- Add customizable dashboard widgets
- Allow users to arrange/hide widgets
- Add export to PDF/Excel functionality
- Implement real-time updates via SignalR
- Add predictive analytics (compliance forecasting)
- Support custom metrics and KPIs