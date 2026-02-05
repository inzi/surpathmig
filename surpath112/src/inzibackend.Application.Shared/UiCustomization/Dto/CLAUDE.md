# UI Customization DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for UI theme and appearance customization. These DTOs allow tenants and users to personalize the visual appearance of the application including themes, colors, and layout preferences.

## Contents

### Files

- **UiCustomizationSettingsDto.cs** - Complete UI customization:
  - BaseSettings - Theme settings from Configuration.Dto
  - IsLeftMenuUsed - Left vs top navigation
  - IsTopMenuUsed - Top menu bar visibility
  - IsTabMenuUsed - Tab-based navigation
  - MenuPosition - Menu placement
  - SubHeaderFixed - Sticky subheader
  - HeaderFixed - Sticky header
  - FooterFixed - Sticky footer
  - Theme colors and layout preferences

### Key Components

#### Customizable Elements
- Overall theme (14 available themes from AppConsts)
- Header appearance and behavior
- Menu position and style
- Sub-header (breadcrumb area)
- Footer visibility and position
- Layout width (fixed/fluid)

#### Customization Scope
- User-level customizations
- Tenant default customizations
- System-wide defaults

### Dependencies
- **inzibackend.Configuration.Dto** - ThemeSettingsDto
- **inzibackend.Core.Shared** - Theme constants

## Architecture Notes

### Customization Hierarchy
1. User personal customizations (highest priority)
2. Tenant theme settings
3. System default theme
4. Hardcoded defaults (fallback)

### CSS Class Generation
- Settings generate CSS classes dynamically
- Classes applied to body/html elements
- No page refresh needed for theme changes
- JavaScript handles theme switching

### Storage
- User preferences in UserSettings table
- Tenant defaults in TenantSettings
- Cached for performance
- Loaded on login/session start

## Business Logic

### UI Customization Workflow
1. User opens UI customization settings
2. System loads UiCustomizationSettingsDto with current values
3. User modifies theme, colors, layout
4. Saves changes
5. UI immediately reflects new theme
6. Preferences saved for future sessions

### Theme Options
- Material Design themes
- Light/Dark modes
- High contrast themes
- Custom color schemes
- Branded tenant themes

### Layout Variations
- **Left Menu**: Traditional sidebar navigation
- **Top Menu**: Horizontal menu bar
- **Tab Menu**: Tab-based interface
- **Mega Menu**: Expanded menu with submenus

### Responsive Behavior
- Mobile devices auto-adjust layout
- Fixed headers improve usability
- Collapsible menus for small screens
- Touch-friendly on mobile

## Usage Across Codebase
These DTOs are consumed by:
- **IUiCustomizationSettingsAppService** - Customization operations
- **UI Settings Page** - Theme customization interface
- **Layout Views** - Apply theme classes
- **CSS Generation** - Dynamic stylesheet creation
- **Mobile Apps** - Theme preferences

## Cross-Reference Impact
Changes affect:
- UI customization interface
- Theme application logic
- CSS class generation
- Layout rendering
- User preference storage