# Permissions Documentation

## Overview
This folder contains service interfaces and DTOs for the permission system. Permissions define fine-grained access control for features and operations throughout the application.

## Contents

### Files
Service interface for permission operations (IPermissionAppService.cs or similar)

### Subfolders

#### Dto
Permission DTOs for hierarchical permission trees and permission management.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Hierarchical permission structure
- Dot-notation naming (e.g., "Pages.Users.Create")
- Runtime permission checking
- Role and user-level permissions

## Business Logic
Permission tree retrieval, permission assignment to roles/users, runtime authorization checks.

## Usage Across Codebase
Role management, user management, authorization attributes, permission checking

## Cross-Reference Impact
Changes affect authorization throughout the application and permission configuration interfaces