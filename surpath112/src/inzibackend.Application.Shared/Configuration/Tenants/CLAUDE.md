# Tenant Configuration Documentation

## Overview
This folder contains service interfaces and DTOs for tenant-specific configuration. Tenants can customize their instance including email, LDAP, billing, and user management policies.

## Contents

### Files
Service interface for tenant configuration operations

### Subfolders

#### Dto
Tenant configuration DTOs including email, LDAP, billing, and user management settings.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Tenant-isolated settings
- Override host defaults
- LDAP/AD integration
- Tenant-specific customization

## Business Logic
Tenant configuration for email, LDAP authentication, billing preferences, user policies.

## Usage Across Codebase
Tenant administration UI, LDAP authentication, email services, billing

## Cross-Reference Impact
Changes affect tenant-specific features and customization options