# Tenants Documentation

## Overview
Database seeding components for tenant-specific data, responsible for creating default tenants and their associated roles, users, and initial configuration during multi-tenant setup.

## Contents

### Files

#### DefaultTenantBuilder.cs
- **Purpose**: Creates the default tenant in the system
- **Key Functionality**:
  - Creates "Default" tenant if not exists
  - Associates with default edition
  - Sets up tenant-specific database connection
  - Initializes tenant configuration

#### TenantRoleAndUserBuilder.cs
- **Purpose**: Sets up roles and users for each tenant
- **Key Functionality**:
  - Creates standard tenant roles (Admin, User)
  - Sets up tenant admin user
  - Assigns permissions to roles
  - Creates demo users if configured
  - Configures tenant-specific security settings

### Key Components

**Tenant Initialization**
- Default tenant name: "Default"
- Tenant-specific admin account
- Role-based access control setup
- Permission assignment

**Role Hierarchy**
- Admin: Full tenant permissions
- User: Standard user permissions
- Custom roles support

### Dependencies
- Role management system
- User management system
- Permission framework
- Tenant entities

## Architecture Notes

**Multi-Tenancy Support**
- Isolated tenant data
- Tenant-specific roles
- Separate admin per tenant

**Seeding Strategy**
- Creates if not exists
- Updates existing data
- Maintains data integrity

## Business Logic

**Default Tenant Setup**
- Name: "Default"
- Admin user per tenant
- Standard role structure
- Isolated permissions

**Security Configuration**
- Tenant-specific admin
- Role-based permissions
- User access control
- Password policies

## Usage Across Codebase

**Tenant Provisioning**
- New tenant creation
- Demo data setup
- Testing environments
- Development initialization

**Integration Points**
- Called after host setup
- Part of tenant onboarding
- Used in testing scenarios
- Development environment setup