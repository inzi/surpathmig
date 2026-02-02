# Custom Input Types Documentation

## Overview
Custom UI input type definitions for dynamic forms and property management. Extends ABP's dynamic entity property system with custom input controls.

## Contents

### Files

#### MultiSelectCombobox.cs
- **Purpose**: Custom input type definition for multi-select dropdown
- **Use Case**: Allow users to select multiple values from a list
- **Integration**: Used with dynamic entity properties system
- **Pattern**: Custom input type implementation

### Key Components

- **MultiSelectCombobox**: Multi-select dropdown input type

### Dependencies

- **External Libraries**:
  - ABP Framework (dynamic properties module)

- **Internal Dependencies**:
  - Dynamic entity properties system
  - UI rendering system

## Architecture Notes

- **Pattern**: Type-safe input type definitions
- **Extensibility**: Easy to add new input types
- **Integration**: ABP dynamic properties framework

## Usage Across Codebase

Used by:
- Dynamic property management
- Custom forms
- Entity customization UI
- Admin configuration screens

## Extension Points

- Additional custom input types (date range, file upload, rich text, etc.)
- Custom validation rules
- Custom rendering logic
- Input type metadata