# Host Dashboard DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the host (SaaS provider) dashboard. These DTOs provide analytics, statistics, and monitoring data for the multi-tenant platform including tenant growth, revenue tracking, edition statistics, and tenant health monitoring. This enables the SaaS operator to monitor business metrics and platform health.

## Contents

### Files

#### Dashboard Queries
- **GetDashboardDataInput.cs** - Dashboard data request:
  - Date range for analytics
  - Dashboard configuration options
  - Base input for dashboard queries

- **DashboardInputBase.cs** - Base class for dashboard queries:
  - Common filtering and date range parameters
  - Inherited by specific dashboard input DTOs

#### Top-Level Statistics
- **GetTopStatsInput.cs** - Request for top-level KPIs
- **TopStatsData.cs** - High-level platform statistics:
  - Total tenant count
  - Active subscriptions
  - Total revenue
  - Growth metrics

#### Income Statistics
- **GetIncomeStatisticsDataInput.cs** - Revenue analytics query:
  - Date range
  - Chart interval (daily, weekly, monthly)

- **GetIncomeStatisticsDataOutput.cs** - Revenue trend data:
  - Time series revenue data
  - Revenue by edition
  - Payment method breakdown

- **IncomeStastistic.cs** - Revenue data point:
  - Date/period
  - Revenue amount
  - Subscription count
  - Used in revenue charts

- **ChartDateInterval.cs** - Time interval enum (Daily, Weekly, Monthly, Annual)

#### Edition Statistics
- **GetEditionStatisticsInput.cs** - Edition usage query
- **GetEditionTenantStatisticsInput.cs** - Tenant count by edition query
- **GetEditionTenantStatisticsOutput.cs** - Tenant distribution by edition:
  - Edition name
  - Tenant count
  - Revenue per edition
  - Growth rate

- **TenantEdition.cs** - Edition assignment info:
  - Edition name
  - Tenant count
  - Used in pie charts

#### Tenant Tracking
- **GetRecentTenantsOutput.cs** - Recently created tenants:
  - New tenant signups
  - Tenant creation dates
  - Edition selections

- **RecentTenant.cs** - Recent tenant info:
  - Tenant name
  - Creation date
  - Edition
  - Subscription status

- **GetExpiringTenantsOutput.cs** - Expiring subscriptions:
  - Tenants approaching subscription end
  - Days until expiration
  - Renewal opportunities

- **ExpiringTenant.cs** - Expiring tenant details:
  - Tenant name
  - Expiration date
  - Days remaining
  - Edition and revenue

### Key Components

#### Dashboard Metrics
- **Revenue Tracking**: Income over time, by edition, by payment method
- **Tenant Growth**: New signups, churn rate, active tenants
- **Edition Distribution**: Tenant count per pricing tier
- **Subscription Health**: Expiring subscriptions, renewal rate
- **Top Stats**: KPIs for quick platform assessment

#### Time-Series Analytics
- Daily, weekly, monthly, annual intervals
- Trend analysis and forecasting
- Year-over-year comparisons

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **inzibackend.MultiTenancy.Payments.Dto** - Payment data

## Architecture Notes

### Analytics Architecture
- **Aggregate Queries**: Pre-calculated metrics for performance
- **Time Series**: Date-bucketed data for charts
- **Denormalization**: Duplicate data for faster dashboard loading
- **Caching**: Dashboard data cached with short TTL

### Performance Optimization
- Dashboard queries can be expensive
- Consider background jobs for metric calculation
- Cache results with 5-15 minute refresh
- Aggregate tables for historical data

### Multi-Currency Support
- Revenue tracked in USD (or configured base currency)
- Currency conversion for international tenants
- Revenue normalized for comparison

## Business Logic

### Host Dashboard Workflow
1. Host admin opens dashboard
2. System queries GetDashboardDataInput with date range
3. Returns TopStatsData for KPIs
4. Loads GetIncomeStatisticsDataOutput for revenue chart
5. Shows GetEditionTenantStatisticsOutput for edition breakdown
6. Displays GetRecentTenantsOutput for recent signups
7. Highlights GetExpiringTenantsOutput for renewal opportunities

### Revenue Analytics
- Track MRR (Monthly Recurring Revenue)
- Calculate churn rate
- Monitor ARPU (Average Revenue Per User)
- Identify growth trends

### Tenant Lifecycle Monitoring
- **Recent Tenants**: New signups indicate growth
- **Expiring Tenants**: Proactive renewal outreach
- **Churn Analysis**: Identify why tenants leave
- **Edition Upgrades**: Track tenant progression

### Business Metrics
- **CAC** (Customer Acquisition Cost): via marketing spend / new tenants
- **LTV** (Lifetime Value): average subscription duration × ARPU
- **Churn Rate**: expired subscriptions / total subscriptions
- **Growth Rate**: new tenants - churned tenants

## Usage Across Codebase
These DTOs are consumed by:
- **IHostDashboardAppService** - Dashboard data retrieval
- **Host Dashboard UI** - Charts, graphs, and KPI displays
- **Reporting Services** - Business intelligence reports
- **Alert Services** - Notifications for expiring subscriptions
- **Mobile Apps** - Dashboard views for host admins

## Cross-Reference Impact
Changes to these DTOs affect:
- Host dashboard interface
- Revenue charts and analytics
- Tenant health monitoring
- Subscription expiration alerts
- Business intelligence reports
- Edition statistics displays
- KPI calculation and display