# Dynamic Entity Property Values Documentation

## Overview
This folder contains service interfaces and DTOs for managing actual values of dynamic properties on entity instances. While Dynamic Entity Properties defines which custom fields exist, this folder handles the data values themselves.

## Contents

### Files
Service interface for property value operations (IDynamicEntityPropertyValueAppService.cs or similar)

### Subfolders

#### Dto
DTOs for managing dynamic property values on specific entity records.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- EAV value storage
- Tenant-isolated values
- Type-safe value handling
- Query performance considerations

## Business Logic
CRUD operations for custom field values on entity instances. Used whenever custom fields are displayed or edited.

## Usage Across Codebase
Entity edit forms, value display, import/export with custom fields, reporting

## Cross-Reference Impact
Changes affect custom field value management and entity data displays