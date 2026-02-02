# Tenant Dashboard DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for tenant-specific dashboard analytics. Unlike the host dashboard which monitors the entire SaaS platform, the tenant dashboard provides business metrics and KPIs for individual schools/organizations including student activity, compliance statistics, sales/revenue data, and operational metrics specific to the compliance tracking domain.

## Contents

### Files

#### Dashboard Data
- **GetDashboardDataInput.cs** - Dashboard query parameters:
  - Date range for analytics
  - Dashboard type (overview, detailed, specific metric)

- **GetDashboardDataOutput.cs** - Complete dashboard data:
  - Top stats, sales summary, member activity
  - Regional stats, profit share
  - Aggregated metrics for dashboard widgets

#### Top-Level Statistics
- **GetTopStatsOutput.cs** - Key performance indicators:
  - Total students/cohort users
  - Active requirements
  - Compliance percentage
  - Pending documents
  - Expired requirements
  - Financial summaries

#### Sales & Revenue
- **GetSalesSummaryInput.cs** - Sales query parameters:
  - Date period (daily, weekly, monthly)
  - Date range

- **GetSalesSummaryOutput.cs** - Sales trend data:
  - Revenue over time
  - Sales by category
  - Growth metrics

- **GetDailySalesOutput.cs** - Daily sales breakdown:
  - Sales per day
  - Used for trend charts

- **SalesSummaryData.cs** - Sales data point:
  - Date/period
  - Amount
  - Transaction count

- **SalesSummaryDatePeriod.cs** - Date period enum (Daily, Weekly, Monthly)

#### Member Activity
- **GetMemberActivityOutput.cs** - User activity metrics:
  - Active users
  - Login frequency
  - Feature usage
  - Engagement metrics

- **MemberActivity.cs** - Activity data point:
  - User or cohort activity levels
  - Timestamp
  - Activity type

#### Regional Statistics
- **GetRegionalStatsOutput.cs** - Geographic breakdown:
  - Students by region/state
  - Compliance by location
  - Regional performance

- **RegionalStatCountry.cs** - Regional data:
  - Country/state identifier
  - Count of students
  - Compliance percentage

#### Financial Metrics
- **GetProfitShareOutput.cs** - Profit/revenue breakdown:
  - Revenue by department
  - Revenue by cohort
  - Cost allocation

- **GetGeneralStatsOutput.cs** - General statistics:
  - Overall system health
  - Capacity utilization
  - Trending metrics

### Key Components

#### Dashboard Widgets
- Top Stats - High-level KPIs
- Sales Chart - Revenue trends
- Member Activity - User engagement
- Regional Map - Geographic distribution
- Profit Share - Financial breakdown
- General Stats - Operational metrics

#### Time-Based Analytics
- Real-time metrics
- Daily aggregations
- Weekly trends
- Monthly summaries
- Year-over-year comparisons

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **inzibackend.Dto** - Common DTOs

## Architecture Notes

### Analytics Architecture
- **Pre-Aggregation**: Metrics pre-calculated for performance
- **Caching**: Dashboard data cached with 5-15 minute TTL
- **Background Jobs**: Heavy calculations in background
- **Incremental Updates**: Update metrics on events rather than full recalc

### Performance Optimization
- Denormalized summary tables
- Indexed date columns for time-series queries
- Limited date ranges for dashboard queries
- Progressive loading for complex widgets

### Data Privacy
- Tenant-isolated dashboard data
- No cross-tenant data visibility
- PII removed from analytics
- Aggregated data only

## Business Logic

### Tenant Dashboard Workflow
1. Tenant admin opens dashboard
2. System loads GetDashboardDataInput with default date range
3. Returns GetDashboardDataOutput with all widgets
4. Top stats show current compliance state
5. Sales chart shows revenue trends
6. Member activity shows user engagement
7. Regional stats show geographic distribution

### Compliance Metrics
- **Total Students**: All active cohort users
- **Compliant Students**: Students meeting all requirements
- **Compliance Percentage**: Compliant / Total
- **Pending Documents**: Documents awaiting review
- **Expired Requirements**: Requirements past due date
- **Expiring Soon**: Requirements due in next 30 days

### Financial Tracking
- Revenue from fees
- Cost per student
- Profit by department/cohort
- Payment collection rate
- Outstanding balances

### Activity Monitoring
- Daily active users
- Feature usage (most used features)
- Document upload frequency
- Login patterns
- Engagement trends

### Regional Analysis
- Students by state
- Compliance by region
- Growth by geography
- Regional performance comparison

## Usage Across Codebase
These DTOs are consumed by:
- **ITenantDashboardAppService** - Dashboard data services
- **Tenant Dashboard UI** - Charts, graphs, and widgets
- **Reporting Services** - Compliance and financial reports
- **Mobile Apps** - Dashboard views for tenant admins
- **Email Digests** - Weekly/monthly dashboard summaries

## Cross-Reference Impact
Changes to these DTOs affect:
- Tenant dashboard interface
- KPI displays
- Sales and revenue charts
- Activity monitoring widgets
- Regional statistics displays
- Financial reports
- Compliance tracking dashboards
- Admin mobile apps