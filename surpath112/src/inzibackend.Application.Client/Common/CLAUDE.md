# Common Documentation

## Overview
Contains proxy services for common lookup operations used across the application. Provides client-side access to shared functionality like user search and edition information.

## Contents

### Files

#### ProxyCommonLookupAppService.cs
- **Purpose**: Client proxy for ICommonLookupAppService, providing common lookup operations
- **Key Methods**:
  - `GetEditionsForCombobox`: Retrieves available editions for dropdown selection
  - `FindUsers`: Searches for users with pagination support
  - `GetDefaultEditionName`: Gets the default edition name (synchronous wrapper)
- **Base Class**: Inherits from ProxyAppServiceBase
- **Interface**: Implements ICommonLookupAppService
- **Special Note**: Uses AsyncHelper.RunSync for synchronous execution of GetDefaultEditionName

### Key Components
- **User Search**: Paginated user lookup functionality
- **Edition Management**: Edition information retrieval
- **Lookup Services**: Common dropdown/combobox data providers

### Dependencies
- `Abp.Application.Services.Dto`: ABP framework DTOs
- `Abp.Threading`: AsyncHelper for sync/async conversion
- `inzibackend.Common.Dto`: Common DTOs
- `inzibackend.Editions.Dto`: Edition-related DTOs

## Architecture Notes
- **Lookup Pattern**: Provides reusable lookup services
- **Synchronous Support**: Includes sync wrapper for legacy code compatibility
- **DTO-based**: Returns standardized DTO formats for UI consumption

## Business Logic
- **User Search**: Enables finding users across the system
- **Edition Selection**: Supports multi-edition SaaS model
- **Default Values**: Provides system defaults for initialization

## Usage Across Codebase
- Used by UI dropdowns and comboboxes
- Referenced in user selection dialogs
- Consumed by edition selection interfaces
- Part of the common UI infrastructure