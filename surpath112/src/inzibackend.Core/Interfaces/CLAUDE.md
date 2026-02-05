# Interfaces Documentation

## Overview
Common interfaces and contracts used across the domain layer.

## Contents

### Files

#### IHasArchiving.cs
- **Purpose**: Interface for entities that support archiving/soft delete
- **Properties**: `IsArchived` flag
- **Use Case**: Logical deletion without physical removal
- **Pattern**: Marker interface for entity filtering

### Key Components

- **IHasArchiving**: Archiving capability marker

### Dependencies

- **External Libraries**:
  - None (pure interface)

- **Internal Dependencies**:
  - Entity system

## Architecture Notes

- **Pattern**: Marker interface pattern
- **Usage**: Entity filtering and query interception
- **Soft Delete**: Alternative to hard deletion

## Business Logic

### Archiving vs Deletion
- Archived entities remain in database
- Excluded from normal queries by default
- Can be unarchived/restored
- Maintains referential integrity
- Preserves audit trail

### Use Cases
- User deactivation (vs deletion)
- Compliance record retention
- Historical data preservation
- Temporary removal from active use

## Usage Across Codebase

Used by:
- Entity queries (automatic filtering)
- Admin interfaces (show archived toggle)
- Restore functionality
- Data retention policies

## Extension Points

- Additional archiving metadata (archived date, archived by)
- Archiving reasons
- Auto-archiving rules
- Bulk archiving operations