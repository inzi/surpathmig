# Host Dashboard Service Documentation

## Overview
Application service providing dashboard data for the host/system administrator. Shows multi-tenant platform statistics, revenue metrics, tenant activity, and system health indicators. Distinct from tenant dashboards which show tenant-specific data.

## Contents

### Files

#### HostDashboardAppService.cs
- **Purpose**: Main host dashboard data provider for platform administrators
- **Key Features**:
  - Platform-wide statistics
  - All tenants overview
  - Revenue and subscription metrics
  - System health indicators
  - Tenant activity monitoring
  - Growth trends and analytics
- **Authorization**: Host admin only (not accessible to tenants)

#### IIncomeStatisticsReporter.cs
- **Purpose**: Interface for revenue reporting service
- **Key Methods**:
  - Income calculation per period
  - Revenue by edition/subscription type
  - Payment gateway statistics
  - Revenue trends over time

#### IncomeStatisticsReporter.cs
- **Purpose**: Implementation of income statistics reporting
- **Key Features**:
  - Aggregates subscription payments
  - Calculates MRR (Monthly Recurring Revenue)
  - ARR (Annual Recurring Revenue)
  - Revenue by tenant, edition, payment method
  - Churn analysis
  - Revenue forecasting

### Key Components

**Host Dashboard Widgets:**
- Total tenants (active, trial, expired)
- Total users across all tenants
- Revenue metrics (MRR, ARR, total income)
- New signups (daily, weekly, monthly)
- Active vs. inactive tenants
- Edition popularity distribution
- Payment gateway performance
- System resource usage

**Income Statistics:**
- Subscription revenue by period
- One-time payment vs. recurring
- Revenue by edition tier
- Payment success/failure rates
- Refund and churn rates
- Average revenue per tenant

**Tenant Analytics:**
- Tenant growth over time
- Tenant size distribution
- Feature usage across tenants
- Tenant health scores
- At-risk tenant identification

### Dependencies
- **External**:
  - ABP Framework (Multi-tenancy, Authorization)
  - Entity Framework Core
  - Payment gateway APIs
- **Internal**:
  - Tenant management repositories
  - Subscription payment repositories
  - Edition management
  - Feature usage tracking
  - System monitoring services

## Architecture Notes
- **Pattern**: Application Service with specialized reporters
- **Authorization**: Host (system admin) only
- **Caching**: Dashboard data cached with periodic refresh
- **Multi-Tenant Aware**: Aggregates data across all tenants
- **Performance**: Optimized queries with indexes on payment/tenant tables

## Business Logic

### Dashboard Data Compilation
1. Disable tenant filter to access all tenants
2. Query tenant statistics (count, statuses)
3. Calculate revenue metrics via IncomeStatisticsReporter
4. Aggregate user counts across tenants
5. Identify trends (growth, churn)
6. Compile system health metrics
7. Return comprehensive dashboard DTO

### Revenue Calculation
- **MRR**: Sum of all monthly subscription payments
- **ARR**: MRR × 12 or sum of annual subscriptions
- **ARPU**: Average Revenue Per User (total revenue / total users)
- **LTV**: Lifetime Value calculation
- **Churn Rate**: (Tenants lost / Total tenants) × 100

### Tenant Health Scoring
Factors include:
- Payment status (current vs. overdue)
- Feature usage (active vs. dormant)
- User activity levels
- Support ticket volume
- Login frequency
- Compliance with terms

## Usage Across Codebase

### Primary Consumers
- Host admin dashboard page
- SaaS metrics reporting
- Executive dashboards
- Investor reports
- System monitoring tools

### Typical Usage
```csharp
// Load host dashboard
var dashboardData = await _hostDashboardAppService.GetDashboardData();

// Get income statistics for last 12 months
var incomeStats = await _incomeStatisticsReporter
    .GetIncomeStatistics(startDate, endDate);

// Identify at-risk tenants
var atRiskTenants = await _hostDashboardAppService.GetAtRiskTenants();
```

## Key Metrics

### Growth Metrics
- New tenant signups per period
- Trial to paid conversion rate
- User growth rate
- Edition upgrade/downgrade rates

### Financial Metrics
- MRR/ARR
- ARPU (Average Revenue Per User)
- LTV (Lifetime Value)
- CAC (Customer Acquisition Cost) - if tracked
- Churn rate
- Revenue growth rate

### Operational Metrics
- System uptime
- Average response time
- Database size growth
- API call volume
- Background job performance
- Error rates

## Performance Considerations
- **Data Volume**: Aggregating across all tenants can be slow
- **Caching Strategy**: Cache dashboard for 15-30 minutes
- **Async Loading**: Load widgets asynchronously
- **Query Optimization**: Use materialized views or summary tables
- **Background Jobs**: Pre-calculate statistics in background
- **Pagination**: Limit tenant lists to prevent timeout

## Security Considerations
- **Host Only Access**: Strictly enforce host admin authorization
- **Tenant Privacy**: Don't expose tenant-specific sensitive data in aggregates
- **Data Isolation**: Ensure proper tenant filtering when drill-down
- **Audit Logging**: Log all host dashboard access
- **Rate Limiting**: Prevent abuse of expensive queries

## Reporting Capabilities

### Standard Reports
- Monthly revenue report
- Tenant growth report
- Edition popularity report
- Payment gateway performance
- Churn analysis report

### Export Options
- Excel export of all metrics
- PDF executive summary
- CSV for analysis in external tools
- API access for BI tools integration

## Monitoring and Alerts

### Alert Triggers
- High churn rate detected
- Revenue drop below threshold
- System performance degradation
- Failed payments exceed threshold
- Tenant inactivity patterns
- Security incidents

### Health Checks
- Database performance
- API response times
- Background job execution
- Payment gateway connectivity
- Email delivery rates
- File storage capacity

## Best Practices
- **Cache Aggressively**: Host dashboard queries are expensive
- **Index Properly**: Ensure indexes on tenant, subscription, payment tables
- **Monitor Performance**: Track dashboard load times
- **Trend Analysis**: Show trends, not just current numbers
- **Actionable Insights**: Highlight items requiring attention
- **Drill-Down Capability**: Link to detailed views of metrics

## Extension Points
- Add predictive analytics (churn prediction, revenue forecasting)
- Integrate with BI tools (Tableau, Power BI)
- Add custom dashboards per host admin role
- Implement real-time updates via SignalR
- Add cohort analysis (tenant performance by signup date)
- Support multi-currency revenue reporting
- Add tenant segmentation and filtering
- Implement A/B testing results dashboard