# Delegation Documentation

## Overview
User delegation system allowing users to temporarily grant their permissions to other users. Supports time-bound delegations with automatic expiration and validation.

## Contents

### Files

#### UserDelegation.cs
- **Purpose**: Entity representing a delegation relationship between users
- **Key Properties**: Source user, target user, start/end times

#### IUserDelegationManager.cs
- **Purpose**: Interface for delegation management operations
- **Key Methods**: Create, remove, and query delegations

#### UserDelegationManager.cs
- **Purpose**: Implementation of delegation business logic
- **Key Features**: Validation, expiration checking, delegation queries

#### IUserDelegationConfiguration.cs
- **Purpose**: Interface for delegation configuration settings
- **Key Settings**: Enable/disable delegation, time limits

#### UserDelegationConfiguration.cs
- **Purpose**: Implementation of delegation configuration
- **Key Features**: Settings management, default values

#### ActiveUserDelegationSpecification.cs
- **Purpose**: Specification pattern for active delegation queries
- **Key Logic**: Date range validation, status checking

### Key Components

- **UserDelegation**: Core delegation entity
- **UserDelegationManager**: Business logic implementation
- **ActiveUserDelegationSpecification**: Query filtering logic

### Dependencies

- **External Libraries**:
  - ABP Framework (Domain services, specifications)

- **Internal Dependencies**:
  - User entity
  - Authorization system

## Architecture Notes

- **Pattern**: Specification pattern for complex queries
- **Time-bound**: Automatic expiration of delegations
- **Validation**: Business rule enforcement

## Business Logic

- Time-limited permission sharing
- Automatic delegation expiration
- Circular delegation prevention
- Active delegation validation

## Usage Across Codebase

Used for:
- Temporary permission grants
- Vacation/absence coverage
- Administrative delegation
- Workflow approvals