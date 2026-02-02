# Dashboard Customization Documentation

## Overview
User-customizable dashboard system allowing drag-and-drop widget management, personalized layouts, and saved dashboard configurations per user.

## Contents

### Files

#### Dashboard.cs
- **Purpose**: Entity representing a user's saved dashboard
- **Key Properties**:
  - DashboardName: Reference to dashboard definition
  - User assignment
  - Tenant scope
- **Pattern**: User-specific dashboard instances

#### Page.cs
- **Purpose**: Entity representing a dashboard page/tab
- **Features**: Multi-page dashboard support
- **Relationships**: One-to-many with widgets

#### Widget.cs
- **Purpose**: Entity representing a widget instance on a dashboard
- **Key Properties**:
  - WidgetId: Reference to widget definition
  - Position coordinates
  - Size dimensions
  - Filter values
  - Page assignment
- **Pattern**: Widget placement and configuration

#### WidgetFilter.cs
- **Purpose**: Entity storing user's filter selections for widgets
- **Use**: Persist filter values (date ranges, department filters, etc.)

### Subfolders

#### Definitions/
- Widget definitions
- Dashboard definitions
- Filter definitions
- See [Definitions/CLAUDE.md](Definitions/CLAUDE.md)

### Key Components

- **Dashboard**: User's dashboard configuration
- **Page**: Dashboard page/tab
- **Widget**: Widget instance with position and settings
- **WidgetFilter**: User's filter preferences

### Dependencies

- **External Libraries**:
  - ABP Framework (domain entities)

- **Internal Dependencies**:
  - User system
  - Permission system
  - Widget definition system

## Architecture Notes

- **Pattern**: Entity-Component system for dashboards
- **Persistence**: User configurations saved to database
- **Multi-Tenancy**: Tenant-scoped dashboards
- **Flexibility**: Full drag-and-drop customization

## Business Logic

### Dashboard Lifecycle
1. User selects base dashboard (tenant or host)
2. System creates user-specific dashboard instance
3. User customizes widget layout
4. Configuration persisted to database
5. Loaded on subsequent visits

### Widget Management
- Add widgets from available list
- Drag to reposition
- Resize widgets
- Remove widgets
- Configure widget filters
- Reset to default layout

### Pages
- Multiple pages per dashboard
- Tab-based navigation
- Page-specific widgets
- Organize related widgets

### Filters
- Global filters (date range)
- Widget-specific filters
- Saved filter values
- Applied on widget load

## Usage Across Codebase

Used by:
- Dashboard UI
- Widget rendering service
- Dashboard management API
- User preferences

## Security Considerations

- Users can only modify own dashboards
- Permission-based widget visibility
- Tenant isolation
- No cross-user dashboard access

## Extension Points

- Custom dashboard layouts
- Additional widget types
- Dashboard templates
- Dashboard sharing
- Dashboard export/import
- Widget marketplace