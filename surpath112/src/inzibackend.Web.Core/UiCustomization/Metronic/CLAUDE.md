# UiCustomization/Metronic Documentation

## Overview
This folder contains Metronic theme-specific UI customization implementations. It provides 14 different theme variants (Default, Theme2-13) for the Metronic admin template, each with specific visual styling configurations. The system supports multi-level customization (application, tenant, and user levels) with settings for layout, menu, header, footer, and dark mode.

## Contents

### Files

#### UiThemeCustomizerBase.cs
- **Purpose**: Abstract base class for all theme customizers
- **Key Features**:
  - Settings management with theme-prefixed keys
  - Support for application, tenant, and user-level settings
  - Generic type-safe setting retrieval
  - Dark mode update functionality
  - Settings reset to parent defaults
- **Architecture**: Uses ISettingManager from ABP framework
- **Usage**: Inherited by all concrete theme customizers

#### ThemeDefaultUiCustomizer.cs
- **Purpose**: Default Metronic theme implementation with left sidebar navigation
- **Key Features**:
  - Implements IUiCustomizer interface
  - Left menu positioning (fluid layout)
  - Fixed subheader configuration
  - Configurable aside skin (light/dark)
  - Minimizable sidebar support
  - Dark mode with automatic aside skin switching
- **Settings Managed**:
  - Layout: DarkMode, LayoutType (fluid)
  - SubHeader: Fixed, Style, Size
  - Menu: AsideSkin, FixedAside, AllowMinimizing, DefaultMinimized, SubmenuToggle, Hoverable, SearchActive
  - Header: DesktopFixed, MobileFixed
  - Footer: FixedFooter
- **Business Logic**:
  - Dark mode automatically switches aside skin to "dark"
  - Light mode automatically switches aside skin to "light"
  - Settings cascade from application -> tenant -> user
  - Reset operations restore parent-level defaults

#### Theme2UiCustomizer.cs through Theme13UiCustomizer.cs
- **Purpose**: Alternative theme variants with different visual configurations
- **Theme Variations**:
  - **Theme2-3**: Alternative sidebar configurations
  - **Theme4-7**: Different header/footer combinations
  - **Theme8-13**: Various layout and color scheme options
- **Common Pattern**: Each extends UiThemeCustomizerBase and implements IUiCustomizer
- **Differences**: Vary in default colors, sidebar positions, header styles, and layout configurations

#### Theme0Customizer.cs (note the space in filename)
- **Purpose**: Appears to be a legacy or alternative default theme
- **Note**: Filename has unusual spacing ("Theme0Customizer .cs")
- **Status**: Likely legacy code or work-in-progress

### Key Components
- **Multi-level Settings**: Application-wide, per-tenant, and per-user customization
- **Theme Registry**: 14 theme variants selectable at runtime
- **Cascading Settings**: User settings override tenant, tenant overrides application
- **Dark Mode Support**: Automatic color scheme switching across all themes

### Dependencies
- **Abp.Configuration**: ABP settings infrastructure
- **inzibackend.Configuration**: Application-specific setting names
- **inzibackend.Configuration.Dto**: Theme settings DTOs
- **inzibackend.UiCustomization**: IUiCustomizer interface
- **inzibackend.UiCustomization.Dto**: UI customization DTOs

## Architecture Notes

### Theme Selection Pattern
1. User selects theme via UI
2. Settings stored at appropriate level (user/tenant/application)
3. Theme customizer loads settings and applies to UI
4. Frontend reads configuration and applies CSS/layout changes

### Settings Hierarchy
```
Application Settings (host-wide defaults)
  ├── Tenant Settings (override for specific tenant)
  │     └── User Settings (personal preferences)
```

### Dark Mode Implementation
- Dark mode is a boolean setting at all levels
- When enabled, automatically adjusts:
  - Aside/sidebar skin color
  - Overall color scheme
  - Component styling
- Reset operations restore parent-level dark mode preferences

### Configuration Storage
All settings stored in ABP's settings table with keys like:
- `{ThemeName}.DarkMode`
- `{ThemeName}.LeftAside.AsideSkin`
- `{ThemeName}.SubHeader.Style`

## Business Logic

### Theme Customization Flow
1. **Initial Load**: Read settings from database for current user/tenant
2. **User Modification**: Update user-level settings
3. **Tenant Admin**: Can set tenant-wide defaults
4. **Host Admin**: Can set application-wide defaults
5. **Reset**: Restore to parent level (user -> tenant, tenant -> application)

### Setting Update Operations
- **UpdateUserUiManagementSettingsAsync**: Save user preferences
- **UpdateTenantUiManagementSettingsAsync**: Save tenant defaults (resets affected users)
- **UpdateApplicationUiManagementSettingsAsync**: Save host defaults (resets affected tenants/users)

### Dark Mode Toggle
- Simple boolean toggle
- Automatically adjusts dependent settings (aside skin)
- Can be set at any level (application/tenant/user)
- Reset restores parent-level dark mode setting

## Usage Across Codebase

### Consumed By
- **UiCustomization Services**: Higher-level services select and use theme customizers
- **Configuration Controllers**: API endpoints for theme management
- **MVC Views**: Razor views read settings to render UI
- **JavaScript/TypeScript**: Frontend applies theme settings dynamically

### Integration Points
- **Startup Configuration**: Theme customizers registered in DI container
- **User Preferences**: Settings UI allows theme selection
- **Tenant Branding**: Tenants can customize their default theme
- **Multi-tenant Isolation**: Each tenant can have different default theme

## Performance Considerations
- Settings cached by ABP framework
- Theme selection happens once per page load
- Settings changes require page refresh or dynamic reload
- Minimal performance impact on rendering

## Security Considerations
- Settings scoped to current user/tenant automatically
- Only authorized users can modify tenant/application settings
- No sensitive data in theme settings
- Settings validated before storage

## Extensibility
To add a new theme:
1. Create new class inheriting from UiThemeCustomizerBase
2. Implement IUiCustomizer interface
3. Define theme-specific settings in GetUiSettings()
4. Register in DI container
5. Add theme name constant to AppConsts
6. Add frontend CSS/styling for new theme