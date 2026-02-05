# Dynamic Entity Properties Documentation

## Overview
Dynamic property system allowing runtime addition of custom properties to entities without schema changes. Enables tenant-specific or user-specific entity customization.

## Contents

### Files

#### AppDynamicEntityPropertyDefinitionProvider.cs
- **Purpose**: Defines which entities support dynamic properties
- **Features**:
  - Entity-level configuration
  - Input type associations
  - Property metadata
- **Pattern**: Provider pattern from ABP

### Key Components

- **AppDynamicEntityPropertyDefinitionProvider**: Configuration provider

### Dependencies

- **External Libraries**:
  - ABP Framework (dynamic properties module)

- **Internal Dependencies**:
  - Custom input types
  - Entity system

## Architecture Notes

- **Pattern**: Entity-Attribute-Value (EAV) pattern
- **Flexibility**: Runtime property addition
- **Storage**: Separate table for dynamic values
- **Type Safety**: Input type validation

## Business Logic

### Dynamic Property Lifecycle
1. Admin defines new property for entity type
2. Property configuration stored
3. Users can input values for property
4. Values validated against input type
5. Values stored in dynamic property table
6. Values loaded with entity

### Supported Entities
Configured in AppDynamicEntityPropertyDefinitionProvider:
- Users
- Tenants
- Other configurable entities

### Input Types
- Single line text
- Multi-line text
- Numeric
- Date
- Boolean
- Combobox (dropdown)
- Multi-select combobox (custom)

## Usage Across Codebase

Used by:
- Entity customization UI
- Admin property management
- Form generation
- Custom fields display

## Use Cases

- Tenant-specific custom fields
- User profile extensions
- Custom entity attributes
- Flexible data model
- No schema migrations needed

## Security Considerations

- Admin-only property definition
- Type validation on input
- Tenant isolation
- No SQL injection (EAV pattern)

## Extension Points

- Additional entity types
- Custom input types
- Property validation rules
- Property dependencies
- Conditional properties