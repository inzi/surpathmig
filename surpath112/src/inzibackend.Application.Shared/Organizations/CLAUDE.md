# Organizations Documentation

## Overview
This folder contains service interfaces and DTOs for the Organization Unit system. Organization Units provide hierarchical organizational structure for grouping users, roles, and departments with permission boundaries.

## Contents

### Files
Service interface for organization operations (IOrganizationUnitAppService.cs or similar)

### Subfolders

#### Dto
Complete organization unit DTOs including hierarchy management, user assignment, role assignment, and department linking.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Hierarchical tree structure
- Code-based positioning
- Multi-assignment (users, roles, departments)
- Permission scoping

## Business Logic
Create organizational hierarchy, assign users/roles to units, move units, link departments, organizational permission boundaries.

## Usage Across Codebase
Organization tree UI, user/role assignment, permission scoping, department management, data filtering

## Cross-Reference Impact
Changes affect organization management, user assignment, permission boundaries, and hierarchical data access