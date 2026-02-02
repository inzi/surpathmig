# Roles Documentation

## Overview
This folder contains service interfaces and DTOs for role-based access control. Roles aggregate permissions and are assigned to users for simplified access management.

## Contents

### Files
Service interface for role operations (IRoleAppService.cs or similar)

### Subfolders

#### Dto
Role DTOs for CRUD operations and permission assignment.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Static roles (system-defined, cannot be deleted)
- Dynamic roles (tenant-created)
- Default role assignment for new users
- Multi-role user membership

## Business Logic
Role creation, permission assignment to roles, user-role assignment, static role protection.

## Usage Across Codebase
Role management UI, user management, permission checking, authorization

## Cross-Reference Impact
Changes affect role management interfaces and user authorization throughout the application