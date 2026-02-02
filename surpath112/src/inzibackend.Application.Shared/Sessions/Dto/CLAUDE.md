# Sessions DTOs Documentation

## Overview
This folder contains DTOs for user session management and current session information retrieval. Sessions track current user context including identity, tenant, and application metadata.

## Contents

### Files
DTOs including:
- GetCurrentLoginInformationsOutput - Current user session info
- UserLoginInfoDto - Logged-in user details
- TenantLoginInfoDto - Current tenant information
- ApplicationInfoDto - Application version and metadata

## Key Components
- Current user identity
- Tenant context
- Application version
- Security tokens
- Session metadata

## Architecture Notes
- Session state management
- Multi-tenant context
- Security token handling
- Application metadata

## Business Logic
Retrieve current session information, user context, tenant context, application info.

## Usage Across Codebase
Navigation bars, user menus, tenant switching, security context

## Cross-Reference Impact
Changes affect session display, user context retrieval, and tenant information