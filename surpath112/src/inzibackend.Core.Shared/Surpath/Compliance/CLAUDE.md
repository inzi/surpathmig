# Surpath/Compliance Documentation

## Overview
Core compliance tracking model for monitoring student/user compliance with institutional requirements including drug testing, background checks, and immunization records.

## Contents

### Files

#### ComplianceValues.cs
- **Purpose**: Data model for tracking compliance status across multiple categories
- **Key Properties**:
  - Drug: Drug testing compliance status
  - Background: Background check compliance status
  - Immunization: Immunization record compliance status
  - InCompliance: Overall compliance status indicator
  - UserId: Associated user identifier (nullable)
  - TenantId: Tenant context for multi-tenant isolation
- **Default Values**: All compliance flags default to false

### Key Components
- Multi-category compliance tracking
- Overall compliance calculation
- User and tenant association
- Boolean flags for each compliance type

### Dependencies
- System namespaces for basic types
- Part of the inzibackend.Surpath.Compliance namespace

## Architecture Notes
- Simple POCO model for compliance state
- Nullable UserId allows for template or default compliance settings
- TenantId ensures data isolation in multi-tenant environment
- Extensible design for additional compliance categories

## Business Logic
- **Compliance Categories**: Tracks three main areas (drug, background, immunization)
- **Overall Status**: InCompliance indicates if all requirements are met
- **User Association**: Links compliance status to specific users
- **Multi-Tenant**: Each tenant has independent compliance tracking
- **Default State**: Non-compliant by default (all false)

## Usage Across Codebase
This compliance model is used in:
- Student compliance tracking services
- Compliance report generation
- Dashboard compliance widgets
- Notification services for non-compliance
- Administrative compliance overview
- API endpoints for compliance status
- Compliance verification workflows
- Audit and reporting systems