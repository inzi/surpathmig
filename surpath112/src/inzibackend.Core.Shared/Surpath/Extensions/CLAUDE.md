# Surpath/Extensions Documentation

## Overview
Utility extension methods and settings management for the Surpath domain, providing common operations for dates, enums, GUIDs, strings, and application configuration.

## Contents

### Files

#### DayOfWeekOrderExtensions.cs
- **Purpose**: Extensions for DayOfWeek ordering and formatting
- **Key Methods**:
  - GetDaysStartingWithMonday(): Returns days Monday-Sunday order
  - GetDaysStartingWith(): Flexible start day for week ordering
  - GetDayOfWeekAbbr(): Single-letter abbreviations (R for Thursday)
- **Note**: Located in inzibackend.Surpath.Statics namespace

#### enumExtensions.cs
- **Purpose**: Enum description extraction utilities
- **Key Methods**:
  - GetDescription(): Retrieves DescriptionAttribute value or enum name
- **Usage**: Display-friendly enum values for UI

#### GuidExtensions.cs
- **Purpose**: GUID validation and null-checking utilities
- **Key Methods**:
  - ActualGuid(): Checks if GUID is valid (not null/empty)
  - IsNullOrEmpty(): Combined null and empty check for nullable GUIDs
- **Note**: Global namespace (SurpathGuidExtensions)

#### StringExtensions.cs
- **Purpose**: HTML helper and JSON validation extensions
- **Key Methods**:
  - DropRootHtmlIdForClass(): Removes root from HTML ID attributes
  - DropRootHtmlNameForClass(): Removes root from HTML name attributes
  - IsJson(): Validates if string is valid JSON
- **Usage**: MVC form handling and JSON validation
- **Note**: Global namespace (SurpathStringExtensions)

#### SurpathSettings.cs
- **Purpose**: Application-wide configuration settings
- **Key Settings**:
  - File upload limits (5MB default)
  - Allowed file extensions (jpeg, jpg, png, pdf, txt, hl7)
  - Payment settings (EnableInSessionPayment)
  - Document handling (DummyDocuments mode)
  - Configuration key mappings
- **Methods**:
  - GetDto(): Converts settings to DTO
  - AllowedFileExtensionsArray: Parsed array of extensions

#### SurpathSettingsDto.cs
- **Purpose**: DTO for transferring settings to clients
- **Properties**: Mirrors SurpathSettings as transferable object
- **Default Values**: 5MB file limits, standard file extensions

### Key Components
- Week ordering utilities for scheduling
- Enum description support for UI display
- GUID validation helpers
- HTML form field manipulation
- JSON validation
- Centralized configuration management
- File upload constraints

### Dependencies
- System.ComponentModel: For DescriptionAttribute
- Newtonsoft.Json and System.Text.Json: For JSON operations
- System.Linq: For collection operations

## Architecture Notes
- Extension methods in global namespace for easy access
- Static settings class for application-wide configuration
- DTO pattern for settings transfer
- Configurable file upload limits and extensions
- Support for dummy document testing mode

## Business Logic
- **Week Display**: Customizable week start day for international support
- **File Uploads**: 5MB limit with specific allowed extensions
- **Payment Mode**: Optional in-session payment processing
- **Document Testing**: Dummy document mode for development/testing
- **HTML Helpers**: Support for nested model binding in MVC

## Usage Across Codebase
These extensions and settings are used throughout:
- Calendar and scheduling interfaces
- File upload validation
- Enum display in dropdowns and labels
- GUID validation in services
- MVC form generation
- JSON API responses
- Configuration-driven behavior
- Document upload services