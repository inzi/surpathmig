# Configuration Documentation

## Overview
This folder contains service interfaces and DTOs for application configuration management. The configuration system enables runtime customization of settings including email, themes, external authentication, and system-wide options without code changes.

## Contents

### Files
Service interface for configuration operations (IConfigurationAppService.cs or similar)

### Subfolders

#### Dto
Core configuration DTOs for email, theme, and external login settings.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

#### Host
Host-level (system-wide) configuration settings for the SaaS platform.
[Full details in Host/Dto/CLAUDE.md](Host/Dto/CLAUDE.md)

#### Tenants
Tenant-specific configuration allowing per-tenant customization.
[Full details in Tenants/Dto/CLAUDE.md](Tenants/Dto/CLAUDE.md)

## Architecture Notes
- Hierarchical settings (host → tenant → user)
- Runtime configuration changes
- Multi-level customization
- Settings inheritance and overrides

## Business Logic
Configuration management for host-level and tenant-level settings including security policies, billing, email, and UI themes.

## Usage Across Codebase
Configuration UI, email services, authentication providers, theme application

## Cross-Reference Impact
Changes affect configuration interfaces, settings storage, and all configurable features