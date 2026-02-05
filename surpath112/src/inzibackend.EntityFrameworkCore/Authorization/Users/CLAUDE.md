# Users Documentation

## Overview
Custom user repository implementation providing specialized user data access operations, particularly focused on password expiration management and bulk user updates.

## Contents

### Files

#### UserRepository.cs
- **Purpose**: Custom repository for User entity with password management features
- **Key Functionality**:
  - Inherits from `inzibackendRepositoryBase<User, long>`
  - Implements `IUserRepository` interface
  - Password expiration detection and management
  - Bulk update operations for password change requirements

### Key Components

**UserRepository Class**
- Custom repository extending base repository for User entities
- Primary key type: long (for user IDs)

**Key Methods**

`GetPasswordExpiredUserIds(DateTime passwordExpireDate)`
- Returns list of user IDs with expired passwords
- Complex LINQ query checking:
  - Most recent password change from RecentPasswords table
  - Falls back to user creation date if no password history
  - Only includes active users not already flagged for password change
- Uses distinct to avoid duplicates

`UpdateUsersToChangePasswordOnNextLogin(List<long> userIdsToUpdate)`
- Bulk update operation using Z.EntityFramework.Plus
- Sets ShouldChangePasswordOnNextLogin flag to true
- Filters to only active users not already flagged
- Efficient bulk update without loading entities

### Dependencies
- Z.EntityFramework.Plus (for bulk update operations)
- Abp.EntityFrameworkCore
- Base repository infrastructure
- User and RecentPasswords entities

## Architecture Notes

**Performance Optimizations**
- Bulk update using Entity Framework Plus
- Efficient queries avoiding N+1 problems
- Direct database updates without entity loading

**Query Complexity**
- Joins between Users and RecentPasswords tables
- Conditional logic for users with/without password history
- Multi-tenant aware through base repository

**Design Patterns**
- Repository pattern for data access abstraction
- Dependency injection through constructor
- Interface-based programming (IUserRepository)

## Business Logic

**Password Expiration Logic**
1. Checks most recent password change date
2. If no password history exists, uses account creation date
3. Compares against provided expiration date
4. Only affects active users
5. Skips users already flagged for password change

**Bulk Update Strategy**
- Identifies affected users via ID list
- Updates database directly for performance
- Maintains data consistency with filters

## Usage Across Codebase

**Primary Consumers**
- User management services
- Password policy enforcement
- Security compliance features
- Authentication/authorization systems

**Integration Points**
- Called by background jobs for password expiration
- Used in user administration interfaces
- Part of security policy enforcement

**Related Entities**
- User: Core user entity
- RecentPasswords: Password history tracking
- Tenant: Multi-tenancy support

**Security Implications**
- Enforces password rotation policies
- Supports compliance requirements
- Maintains password change history