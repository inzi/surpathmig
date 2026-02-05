# Editions Documentation

## Overview
This folder contains service interfaces and DTOs for SaaS edition (pricing tier) management. Editions define feature sets and pricing plans that control tenant capabilities and subscription billing.

## Contents

### Files
Service interface for edition operations (IEditionAppService.cs or similar)

### Subfolders

#### Dto
Complete edition DTOs including edition CRUD, feature management, and tenant migration.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Feature-based access control
- Multi-tier pricing (Free, Basic, Premium, Enterprise)
- Runtime feature toggling
- Edition migration support

## Business Logic
Edition creation with feature definitions, tenant subscription to editions, edition upgrades/downgrades, feature availability checking.

## Usage Across Codebase
Tenant registration, subscription management, feature checking, billing services

## Cross-Reference Impact
Changes affect subscription flows, feature restrictions, billing, and tenant registration