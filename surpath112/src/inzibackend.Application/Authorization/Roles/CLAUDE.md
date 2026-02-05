# Role Management Services

## Overview
Application services for role management including role CRUD, permission assignment, and role-based authorization configuration.

## Key Features
- Role creation and editing
- Permission assignment to roles
- Static vs. dynamic roles
- Role deletion (with dependency checks)
- Role listing with filtering

## Business Logic
Manages roles and their permissions, enforces permission hierarchies, prevents deletion of roles with assigned users, and provides role-based authorization data for the security system.

## Usage
Consumed by role management UI and authorization system for permission checking.