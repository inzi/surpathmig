# Authorization Documentation

## Overview
Authorization-related data access implementations, specifically focused on user management and security operations. Contains custom repositories that extend the application's authorization capabilities beyond the standard ABP framework offerings.

## Contents

### Subfolders

### Users
Custom user repository with password expiration management and bulk update capabilities. [See Users documentation](Users/CLAUDE.md)

## Architecture Notes

**Repository Organization**
- Follows domain-driven design with repositories organized by aggregate root
- Extends base repository functionality for specific authorization needs
- Maintains separation between data access and business logic

**Security Focus**
- Password lifecycle management
- User account security enforcement
- Compliance with security policies

## Business Logic

**Authorization Data Access**
- Custom queries for security-related operations
- Efficient bulk operations for user management
- Integration with password policy enforcement

## Usage Across Codebase

**Integration with Authorization System**
- Provides data access for authorization services
- Supports security policy implementation
- Enables password rotation and expiration features