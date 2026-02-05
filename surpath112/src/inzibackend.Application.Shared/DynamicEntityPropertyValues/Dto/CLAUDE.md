# Dynamic Entity Property Values DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for managing the actual values of dynamic properties on entity instances. While DynamicEntityProperties defines which custom fields exist on which entities, this folder handles the CRUD operations for the actual data values stored in those custom fields for specific entity records.

## Contents

### Files
The folder contains DTOs for managing dynamic property values including:
- Value creation and update inputs
- Value query parameters
- Value list outputs
- Bulk value operations

### Key Components

#### Value Management
- Create values for dynamic properties on entities
- Update existing dynamic property values
- Query values by entity and property
- Delete dynamic property values
- Supports all input types defined in DynamicProperty system

#### Multi-Tenant Value Isolation
- Property values scoped to tenants
- Tenant cannot access other tenant's values
- Host can manage shared property values

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **inzibackend.DynamicEntityProperties.Dto** - Property definitions

## Architecture Notes

### EAV Storage Pattern
- Values stored separately from main entity tables
- Join queries required to retrieve custom field data
- Flexible but impacts query performance
- Consider caching for frequently accessed properties

### Type Safety
- Values stored as strings
- Type conversion based on input type
- Validation enforced per input type (number, date, etc.)

## Business Logic

### Value Lifecycle
1. Admin creates DynamicEntityProperty for entity
2. User/admin sets value via this DTO layer
3. Value stored in tenant-isolated storage
4. Value retrieved and displayed with entity
5. Value updated/deleted as needed

### Use Cases
- Store custom user attributes (employee ID, badge number)
- Track tenant-specific entity data
- Extend entities without schema changes
- Support regulatory compliance fields

## Usage Across Codebase
These DTOs are consumed by:
- **IDynamicEntityPropertyValueAppService** - Value CRUD operations
- **Entity Edit Forms** - Custom field inputs
- **Entity Display Views** - Show custom field values
- **Import/Export** - Bulk value management
- **Reporting** - Include custom fields in reports

## Cross-Reference Impact
Changes affect:
- Custom field value management
- Entity forms with dynamic properties
- Data import/export with custom fields
- Reporting systems