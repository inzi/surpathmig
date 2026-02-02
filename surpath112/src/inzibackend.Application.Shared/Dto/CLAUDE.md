# Base DTOs Documentation

## Overview
This folder contains fundamental base Data Transfer Objects used throughout the application. These DTOs provide common patterns for paging, sorting, filtering, and file operations that are inherited by more specific DTOs across the application.

## Contents

### Files

- **PagedInputDto.cs** - Basic paging support:
  - MaxResultCount - Number of items per page
  - SkipCount - Number of items to skip
  - Base for all paged queries

- **PagedAndSortedInputDto.cs** - Paging with sorting:
  - Extends PagedInputDto
  - Sorting - Sort expression (e.g., "Name ASC, CreatedDate DESC")
  - Most common base for list queries

- **PagedAndFilteredInputDto.cs** - Paging with text filter:
  - Extends PagedInputDto
  - Filter - Text search term
  - Used for searchable lists

- **PagedSortedAndFilteredInputDto.cs** - Complete list query:
  - Extends PagedAndSortedInputDto
  - Adds Filter property
  - All-in-one for complex list queries

- **FileDto.cs** - File upload/download:
  - FileName - Original file name
  - FileType - MIME type
  - FileToken - Temporary token for download
  - Used for file operations throughout application

### Key Components

#### Paging Pattern
Standard pattern for handling large datasets:
- Client specifies page size and skip count
- Server returns requested page
- Total count included for pagination UI
- Prevents performance issues with large tables

#### Sorting Pattern
- Flexible sort expressions
- Multi-column sorting support
- ASC/DESC direction
- Dynamic query building

#### File Handling Pattern
- Temporary tokens for secure file access
- No direct file paths exposed
- Token expiration for security

## Architecture Notes

### Base DTO Strategy
- **Inheritance**: Specific DTOs extend base DTOs
- **Composition**: Common patterns reused across features
- **Consistency**: Standard paging/sorting behavior everywhere
- **Performance**: Built-in limits prevent excessive queries

### Validation
- MaxResultCount capped (usually 1000)
- SkipCount validated (>= 0)
- Sorting expressions validated
- SQL injection prevention

## Business Logic

### Paging Use Cases
All list endpoints use paging:
- User lists
- Audit logs
- Entity change history
- Document lists
- Requirement lists
- Any large dataset

### File Operations
FileDto used for:
- Document uploads
- Profile picture uploads
- Excel imports
- Report exports
- Invoice downloads
- Any file transfer

## Usage Across Codebase
These base DTOs are:
- **Inherited** by hundreds of specific DTOs
- **Used** in all list queries
- **Referenced** in file upload/download operations
- **Extended** with feature-specific properties

## Cross-Reference Impact
Changes to these base DTOs have **massive impact**:
- All list queries inherit behavior
- Paging calculations throughout app
- File handling everywhere
- Sorting logic in all lists
- Breaking changes affect entire application