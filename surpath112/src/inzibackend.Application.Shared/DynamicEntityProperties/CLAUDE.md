# Dynamic Entity Properties Documentation

## Overview
This folder contains service interfaces and DTOs for the dynamic property system. This system enables runtime addition of custom fields to entities without code changes or database migrations, providing flexible data models and tenant-specific customizations.

## Contents

### Files
Service interface for dynamic property operations (IDynamicEntityPropertyAppService.cs or similar)

### Subfolders

#### Dto
Complete dynamic property DTOs including property definitions, entity associations, and value management.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- EAV (Entity-Attribute-Value) pattern
- Runtime schema extension
- No code changes or migrations needed
- Tenant-specific custom fields

## Business Logic
Define custom properties, associate with entities, set values for entity instances. Enables per-tenant data model customization.

## Usage Across Codebase
Dynamic field configuration, entity forms with custom fields, reporting with custom properties

## Cross-Reference Impact
Changes affect custom field management, entity forms, and data model extension features