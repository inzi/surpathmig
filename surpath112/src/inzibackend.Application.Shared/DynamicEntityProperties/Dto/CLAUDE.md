# Dynamic Entity Properties DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the dynamic property system. This powerful feature allows runtime addition of custom fields to entities without code changes or database migrations. Administrators can define new properties and associate them with entities, enabling tenant-specific customizations and flexible data models.

## Contents

### Files

#### Core Property DTOs
- **DynamicPropertyDto.cs** - Property definition:
  - PropertyName - Unique identifier
  - DisplayName - User-friendly label
  - InputType - Control type (textbox, dropdown, checkbox, etc.)
  - Permission - Access control for property
  - TenantId - Tenant-specific or shared property

- **DynamicPropertyValueDto.cs** - Allowed values for dropdown/select properties:
  - Value - Internal value
  - DisplayValue - User-facing label
  - Used for dropdowns, radio buttons, checkboxes

#### Entity Association
- **DynamicEntityPropertyDto.cs** - Links properties to entities:
  - EntityFullName - Target entity type (e.g., "inzibackend.Authorization.Users.User")
  - DynamicPropertyId - Property to add to entity
  - TenantId - Tenant scope
  - Enables runtime schema extension

- **DynamicEntityPropertyValueDto.cs** - Actual property values for entity instances:
  - EntityId - Specific entity instance
  - DynamicEntityPropertyId - Which property
  - Value - Actual value stored
  - TenantId - Tenant isolation

#### Query DTOs
- **DynamicEntityPropertyGetAllInput.cs** - Query parameters:
  - EntityFullName - Filter by entity type
  - Paging and sorting support

- **GetAllEntitiesHasDynamicPropertyOutput.cs** - List entities with dynamic properties:
  - Returns entity types that have custom fields
  - Used for admin UI to show extensible entities

### Key Components

#### Input Types
Supported control types:
- SingleLineString - Text input
- MultiLineString - Textarea
- Number - Numeric input
- Dropdown - Select from predefined values
- Checkbox - Boolean flag
- Date - Date picker
- ComboBox - Autocomplete select

#### Multi-Tenancy
- Host can define shared properties
- Tenants can define tenant-specific properties
- Property values tenant-isolated
- Enables per-tenant customization

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **inzibackend.Dto** - Paging base DTOs

## Architecture Notes

### Dynamic Schema Pattern
- **No Code Changes**: Add fields without modifying entity classes
- **No Migrations**: Properties stored in separate tables
- **Runtime Discovery**: UI dynamically renders custom fields
- **Tenant-Safe**: Complete isolation of property values

### EAV Pattern
Implements Entity-Attribute-Value pattern:
- DynamicProperty = Attribute definition
- DynamicEntityProperty = Attribute-Entity association
- DynamicEntityPropertyValue = Actual values (EAV)

### Performance Considerations
- Additional queries for dynamic properties
- Consider caching property definitions
- Index entity + property lookups
- Minimize dynamic properties for frequently queried entities

### Security
- Permission-based property access
- Tenant isolation enforced
- Property values audited
- Admin-only property creation

## Business Logic

### Dynamic Property Lifecycle
1. **Create Property**: Admin defines DynamicPropertyDto (e.g., "Employee ID")
2. **Define Values**: Add DynamicPropertyValueDto options (for dropdowns)
3. **Associate with Entity**: Create DynamicEntityPropertyDto (add "Employee ID" to User entity)
4. **Set Values**: Users/admins set DynamicEntityPropertyValueDto for specific users
5. **Query**: Retrieve and display custom properties

### Use Cases
1. **Custom Fields**: Add tenant-specific fields (Employee ID, Badge Number)
2. **Industry Compliance**: Custom data for regulations (License Number, Certification Date)
3. **Integration Data**: Store third-party system IDs
4. **Temporary Fields**: Add fields for pilots without permanent schema changes
5. **Per-Tenant Schema**: Different tenants need different data fields

### Property Value Management
- Values typed as strings
- Conversion handled by input type
- Validation per input type (number range, date format)
- Required/optional flagging

### Entity Extension Points
Common entities extended:
- User (custom user attributes)
- OrganizationUnit (department-specific data)
- Tenant (custom tenant metadata)
- Domain entities (CohortUser, Requirement, etc.)

## Usage Across Codebase
These DTOs are consumed by:
- **IDynamicEntityPropertyAppService** - Property CRUD operations
- **IDynamicEntityPropertyValueAppService** - Value management
- **Admin UI** - Dynamic property configuration
- **Entity Forms** - Runtime field rendering
- **Reporting** - Include custom fields in reports
- **Import/Export** - Handle dynamic properties in bulk operations

## Cross-Reference Impact
Changes to these DTOs affect:
- Dynamic property management interfaces
- Entity form renderers (must handle dynamic fields)
- Reporting systems (include custom fields)
- Data import/export tools
- Tenant customization capabilities
- Schema extension features
- Custom field displays throughout UI