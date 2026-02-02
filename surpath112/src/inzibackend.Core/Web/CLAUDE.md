# Web Utilities Documentation

## Overview
Web-related utility functions and helpers for folder management and web content organization.

## Contents

### Files

#### WebContentFolderHelper.cs
- **Purpose**: Helper for locating web content folders
- **Key Features**:
  - Resolve wwwroot path
  - Locate static files
  - Content directory management
  - Environment-specific paths

### Key Components

- **WebContentFolderHelper**: Web content path resolution

### Dependencies

- **External Libraries**:
  - ASP.NET Core
  - System.IO

- **Internal Dependencies**:
  - Hosting environment
  - Configuration system

## Architecture Notes

- **Pattern**: Helper/utility pattern
- **Purpose**: Centralize web content path logic
- **Usage**: Simplify file access in web projects

## Usage Across Codebase

Used by:
- Static file serving
- Image uploads
- Template file access
- Resource file loading
- Profile picture storage
- Sample data loading

## Common Operations

- Get wwwroot path
- Locate specific content folders
- Resolve relative to absolute paths
- Environment-specific content paths