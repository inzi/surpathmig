# Surpath DTO Bases Documentation

## Overview
This folder contains base DTOs and common patterns used across Surpath domain DTOs. These provide inheritance hierarchies and shared functionality for domain-specific data transfer objects.

## Contents

### Files
Base DTO classes providing common patterns:
- Audit fields (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
- Soft delete support (IsDeleted)
- Tenant scoping
- Common validation rules
- Standard DTO patterns

## Key Components
- EntityDtoBase - Base for domain entities
- AuditedDtoBase - With audit fields
- FullAuditedDtoBase - With soft delete
- Common validation attributes

## Architecture Notes
- DRY principle for DTOs
- Consistent audit fields
- Soft delete pattern
- Tenant isolation enforcement

## Business Logic
Provides common DTO inheritance structure for all Surpath domain entities with standard audit and soft delete patterns.

## Usage Across Codebase
Inherited by all Surpath entity DTOs in Dtos folder

## Cross-Reference Impact
Changes affect all Surpath DTOs that inherit from these bases