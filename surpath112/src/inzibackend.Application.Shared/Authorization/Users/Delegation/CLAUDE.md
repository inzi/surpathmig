# User Delegation Documentation

## Overview
This folder contains service interfaces and DTOs for the user delegation system. Delegation allows temporary transfer of access rights between users for coverage and collaboration.

## Contents

### Files
Service interface for delegation operations (IUserDelegationAppService.cs or similar)

### Subfolders

#### Dto
Delegation DTOs for creating and managing time-bounded delegations.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Time-bounded delegations
- Automatic expiration
- Audit trail of delegated actions
- Tenant-isolated

## Business Logic
Create delegation, accept delegation, time range validation, automatic expiration, delegation tracking.

## Usage Across Codebase
User profile, delegation management UI, authorization system, audit logs

## Cross-Reference Impact
Changes affect delegation management and delegated action tracking