# DashboardCustomization Documentation

## Overview
This folder contains infrastructure for customizable dashboards with dynamic views and widgets. It provides base classes for defining dashboard components that can be loaded dynamically with their associated view files, JavaScript, and CSS.

## Contents

### Files

#### ViewDefinition.cs
- **Purpose**: Base class for defining dashboard views with their associated files
- **Key Properties**:
  - **Id**: Unique identifier for the view
  - **ViewFile**: Path to the Razor view file (.cshtml)
  - **JavascriptFile**: Optional path to JavaScript file
  - **CssFile**: Optional path to CSS file
  - **JavascriptClassName**: JavaScript class name (defaults to Id)
- **Usage**: Extended by widget and filter view definitions

#### WidgetViewDefinition.cs
- **Purpose**: Defines a dashboard widget component
- **Inherits**: ViewDefinition
- **Key Features**:
  - Associates view with widget configuration
  - Supports dynamic widget loading
  - Links JavaScript class for widget behavior
- **Usage**: Dashboard widgets like charts, stats, recent activity

#### WidgetFilterViewDefinition.cs
- **Purpose**: Defines filter controls for dashboard widgets
- **Inherits**: ViewDefinition
- **Key Features**:
  - Filter UI components for data filtering
  - Associated with parent widget
  - Configurable date ranges, categories, etc.
- **Usage**: Date pickers, dropdowns, search filters for widgets

### Key Components
- **View Definitions**: Metadata for dashboard components
- **Dynamic Loading**: Runtime loading of views and assets
- **Widget System**: Pluggable dashboard widgets
- **Filter System**: Reusable filter controls

### Dependencies
None (self-contained infrastructure classes)

## Architecture Notes

### Design Pattern
- **Definition Pattern**: Separate definition from implementation
- **Composite Pattern**: Views, JavaScript, and CSS bundled together
- **Convention Over Configuration**: JavascriptClassName defaults to Id

### Dashboard Composition
```
Dashboard
  ├── Widget 1
  │   ├── View (Razor)
  │   ├── JavaScript (behavior)
  │   ├── CSS (styling)
  │   └── Filters
  │       └── Filter View Definitions
  ├── Widget 2
  └── Widget N
```

### File Organization
- View files: Typically in ~/Areas/App/Views/Dashboard/
- JavaScript: ~/wwwroot/view-resources/Areas/App/Views/Dashboard/
- CSS: ~/wwwroot/view-resources/Areas/App/Views/Dashboard/

## Business Logic

### View Registration
1. Create ViewDefinition with file paths
2. Register with dashboard system
3. System loads view dynamically based on user configuration
4. Associated JavaScript/CSS loaded automatically

### Widget Lifecycle
1. Widget definition registered at startup
2. User adds widget to their dashboard
3. System loads widget view file
4. JavaScript class instantiated
5. CSS applied to widget container
6. Widget renders with data

### Filter Integration
1. Filter view definition associated with widget
2. Filter renders above or beside widget
3. User interacts with filter controls
4. JavaScript triggers widget data refresh
5. Widget re-renders with filtered data

## Usage Across Codebase

### Consumed By
- **DashboardViewConfiguration**: Registry of available dashboard views
- **DashboardAppService**: Backend service for dashboard management
- **Dashboard Controllers**: MVC controllers rendering dashboards
- **JavaScript Dashboard Library**: Frontend widget management

### Widget Examples
Typical widgets might include:
- **Sales Statistics**: Chart showing revenue trends
- **Recent Orders**: Table of latest transactions
- **User Activity**: Timeline of recent actions
- **Compliance Summary**: Overview of requirement status
- **Payment Status**: Financial dashboard widget

### Filter Examples
Common filters include:
- **Date Range Picker**: Filter by date period
- **Organization Unit Filter**: Filter by OU
- **Status Filter**: Filter by status (active/inactive)
- **Category Filter**: Filter by category/type

## Extensibility

### Adding a New Widget
1. Create widget view definition:
```csharp
var myWidget = new WidgetViewDefinition(
    id: "MyWidget",
    viewFile: "~/Areas/App/Views/Dashboard/_MyWidget.cshtml",
    javascriptFile: "~/view-resources/Areas/App/Views/Dashboard/_MyWidget.js",
    cssFile: "~/view-resources/Areas/App/Views/Dashboard/_MyWidget.css"
);
```

2. Create view file with widget markup
3. Create JavaScript class matching Id
4. Create CSS for widget styling
5. Register widget in dashboard configuration

### Adding a Filter to Widget
1. Create filter view definition:
```csharp
var myFilter = new WidgetFilterViewDefinition(
    id: "MyWidgetFilter",
    viewFile: "~/Areas/App/Views/Dashboard/_MyWidgetFilter.cshtml",
    javascriptFile: "~/view-resources/Areas/App/Views/Dashboard/_MyWidgetFilter.js"
);
```

2. Associate filter with widget
3. Implement filter UI in view
4. Implement filter logic in JavaScript
5. Wire up filter change events to widget refresh

## Performance Considerations
- Views loaded on-demand (not all widgets loaded initially)
- JavaScript/CSS bundled for production
- Widget data loaded asynchronously via AJAX
- Filters trigger efficient backend queries

## Security Considerations
- Widget access controlled by permissions
- Data filtered by tenant and user context
- No sensitive data in view definitions
- Widget data secured at API level