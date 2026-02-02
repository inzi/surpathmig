# Multi-Tenancy DTOs Documentation

## Overview
This folder contains core multi-tenancy DTOs for tenant management operations including tenant creation, editing, and tenant information retrieval. (Note: Payment and dashboard DTOs are in separate subfolders.)

## Contents

### Files
DTOs for tenant CRUD operations including:
- TenantDto - Tenant information
- CreateTenantInput - Tenant creation
- EditTenantInput - Tenant updates
- GetTenantsInput - Tenant queries
- TenantListDto - Tenant list items

## Key Components
- Tenant basic information
- Connection string management
- Edition assignment
- Tenant activation/deactivation
- Subscription tracking

## Architecture Notes
- Tenant isolation enforcement
- Per-tenant database support
- Edition-based feature access
- Subscription lifecycle

## Business Logic
Tenant registration, tenant management, edition assignment, subscription handling.

## Usage Across Codebase
Tenant registration UI, tenant management, subscription services

## Cross-Reference Impact
Changes affect tenant creation, management, and subscription flows