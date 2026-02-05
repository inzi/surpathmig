# UI Customization Documentation

## Overview
This folder contains service interfaces and DTOs for UI theme and appearance customization. The UI customization system allows tenants and users to personalize the visual appearance of the application.

## Contents

### Files
Service interface for UI customization operations (IUiCustomizationSettingsAppService.cs or similar)

### Subfolders

#### Dto
UI customization settings DTOs for theme and layout preferences.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Hierarchical settings (user → tenant → system)
- Runtime theme switching
- CSS class generation
- No page refresh needed

## Business Logic
Theme selection, layout customization, color schemes, menu positioning, user/tenant theme preferences.

## Usage Across Codebase
UI settings page, theme application in views, CSS generation, layout rendering

## Cross-Reference Impact
Changes affect theme customization UI, CSS generation, and layout rendering