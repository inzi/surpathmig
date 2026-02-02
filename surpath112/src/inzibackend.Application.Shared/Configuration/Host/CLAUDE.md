# Host Configuration Documentation

## Overview
This folder contains service interfaces and DTOs for host-level (system-wide) configuration. Host settings control global system behavior for the entire SaaS platform.

## Contents

### Files
Service interface for host configuration operations

### Subfolders

#### Dto
Complete host configuration DTOs including security, billing, user management, and tenant management settings.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- System-wide defaults
- Tenant setting inheritance
- Security policy enforcement
- SaaS platform management

## Business Logic
Host configuration for security policies, billing settings, tenant management, user policies, session timeouts.

## Usage Across Codebase
Host administration UI, security enforcement, billing services, tenant registration

## Cross-Reference Impact
Changes affect all tenants, security policies, and platform-wide settings