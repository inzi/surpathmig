# Tenants Documentation

## Overview
This folder contains service interfaces and DTOs for tenant-specific functionality including the tenant dashboard. Tenants are individual schools/organizations in the multi-tenant SaaS platform.

## Contents

### Files
Service interface for tenant operations (ITenantAppService.cs or similar)

### Subfolders

#### Dashboard
Tenant-specific dashboard analytics and KPIs for business metrics.
[Full details in Dashboard/Dto/CLAUDE.md](Dashboard/Dto/CLAUDE.md)

## Architecture Notes
- Tenant isolation
- Per-tenant analytics
- Edition-based features
- Tenant-specific settings

## Business Logic
Tenant creation, dashboard analytics, tenant settings, usage tracking, compliance metrics, financial tracking.

## Usage Across Codebase
Tenant dashboard UI, tenant administration, analytics, reporting

## Cross-Reference Impact
Changes affect tenant dashboard, tenant management, and tenant-specific analytics