# Users Documentation

## Overview
This folder contains service interfaces and DTOs for comprehensive user management including CRUD operations, profile management, and user delegation.

## Contents

### Files
Service interface for user operations (IUserAppService.cs or similar)

### Subfolders

#### Dto
Complete user management DTOs with 20+ files for all user operations.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

#### Profile
User profile self-service management including profile editing, password changes, and 2FA.
[Full details in Profile/Dto/CLAUDE.md](Profile/Dto/CLAUDE.md)

#### Delegation
User delegation system for temporary access rights transfer.
[Full details in Delegation/Dto/CLAUDE.md](Delegation/Dto/CLAUDE.md)

## Architecture Notes
- Multi-tenant user isolation
- Role-based permissions
- Account linking support
- Import/export capabilities

## Business Logic
User CRUD, role assignment, profile management, account linking, bulk import, login tracking.

## Usage Across Codebase
User management UI, authentication, profile pages, delegation features

## Cross-Reference Impact
Changes affect user management, authentication, and profile features throughout application