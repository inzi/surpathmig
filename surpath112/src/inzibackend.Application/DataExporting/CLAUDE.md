# Data Exporting Services

## Overview
Centralized data export infrastructure supporting Excel export across the application with consistent formatting and functionality.

## Subfolders
- **Excel**: Excel export base classes and implementations
  - **NPOI**: Active NPOI-based infrastructure (open-source)
  - **EpPlus**: Disabled EPPlus-based infrastructure (licensing issues)

## Architecture
All entity Excel exporters inherit from base classes in the Excel subfolder to ensure consistent file format, localization, and styling.

## Usage
Provides base functionality for 30+ entity-specific exporters throughout the application.