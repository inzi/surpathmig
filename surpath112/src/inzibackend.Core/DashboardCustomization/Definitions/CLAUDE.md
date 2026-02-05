# Dashboard Definitions Documentation

## Overview
Configuration classes defining the dashboard customization system. Specifies available widgets, filters, and dashboard layouts for both host and tenant users with permission-based access control.

## Contents

### Files

#### DashboardConfiguration.cs
- **Purpose**: Central configuration for all dashboards, widgets, and filters
- **Key Features**:
  - Defines all widget definitions (tenant and host)
  - Defines filter definitions for widgets
  - Defines dashboard layouts
  - Permission-based widget access
  - Multi-tenancy support (Host vs Tenant widgets)
- **Tenant Widgets**:
  - DailySales (with date range filter)
  - GeneralStats
  - ProfitShare
  - MemberActivity
  - RegionalStats
  - SalesSummary (with date range filter)
  - TopStats
  - **SurpathDept** (custom - with department filter)
  - **SurpathCohortCompliance** (custom compliance tracking)
- **Host Widgets**:
  - IncomeStatistics
  - TopStats
  - EditionStatistics
  - SubscriptionExpiringTenants
  - RecentTenants (with date range filter)
- **Filters**:
  - DateRangePicker (global filter)
  - SurpathDeptFilter (custom department filter)

#### DashboardDefinition.cs
- **Purpose**: Represents a single dashboard configuration
- **Properties**:
  - `Name`: Dashboard identifier
  - `AvailableWidgets`: List of widget IDs available on this dashboard
- **Pattern**: Simple DTO for dashboard structure

#### WidgetDefinition.cs
- **Purpose**: Defines a widget's metadata and configuration
- **Properties**: (inferred from usage)
  - Id: Widget identifier
  - Name: Localization key
  - Side: Multi-tenancy side (Host/Tenant)
  - UsedWidgetFilters: List of compatible filters
  - PermissionDependency: Required permissions to view widget

#### WidgetFilterDefinition.cs
- **Purpose**: Defines a filter that can be applied to widgets
- **Properties**: (inferred from usage)
  - Id: Filter identifier
  - Name: Localization key

### Key Components

- **DashboardConfiguration**: Central configuration registry
- **DashboardDefinition**: Individual dashboard layout
- **WidgetDefinition**: Widget metadata and permissions
- **WidgetFilterDefinition**: Filter metadata

### Dependencies

- **External Libraries**:
  - ABP Framework (authorization, multi-tenancy)

- **Internal Dependencies**:
  - `inzibackendDashboardCustomizationConsts` (constants)
  - `AppPermissions` (permission definitions)

## Architecture Notes

- **Pattern**: Configuration-driven UI generation
- **Multi-Tenancy**: Separate widget sets for Host and Tenant
- **Permissions**: Widget visibility controlled by permissions
- **Extensibility**: New widgets added by extending configuration
- **Filters**: Reusable filters across multiple widgets

## Business Logic

### Dashboard System
1. Configuration loaded at startup
2. User permissions evaluated
3. Available widgets filtered
4. Dashboard rendered with permitted widgets

### Permission Dependencies
- **Simple**: Single permission required
- **Combined**: Multiple permissions with `requiresAll` flag
- Example: GeneralStats requires both Dashboard and AuditLogs permissions

### Filter System
- Filters defined globally
- Widgets declare which filters they support
- Filters applied to widget data queries
- Date range filter most commonly used

### Custom Surpath Widgets
- **SurpathDept**: Department-level statistics with custom filter
- **SurpathCohortCompliance**: Cohort compliance tracking dashboard
- Integrated into default tenant dashboard

## Usage Across Codebase

Used by:
- Dashboard rendering system
- Widget service (data loading)
- UI framework (widget placement)
- Permission system (access control)
- Dashboard customization UI (drag-and-drop)

## Default Dashboards

### Tenant Dashboard
Includes: GeneralStats, DailySales, ProfitShare, MemberActivity, RegionalStats, TopStats, SalesSummary, SurpathDept, SurpathCohortCompliance

### Host Dashboard
Includes: IncomeStatistics, TopStats, EditionStatistics, SubscriptionExpiringTenants, RecentTenants

## Extension Points

- Add new widget definitions to `WidgetDefinitions` list
- Add new filter definitions to `WidgetFilterDefinitions` list
- Create new dashboard layouts in `DashboardDefinitions` list
- Assign permissions to control widget visibility
- Associate filters with widgets

## Localization

All widget and filter names are localization keys:
- `WidgetDailySales` → localized text
- `FilterDateRangePicker` → localized text
- `SurpathDeptWidget` → localized text