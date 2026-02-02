# Installation Services

## Overview
Application services for initial application setup wizard, guiding administrators through database setup, admin account creation, and configuration.

## Contents
- Database connection testing and setup
- Admin user creation
- Default tenant creation
- Initial configuration
- License key validation (if applicable)
- Sample data seeding (optional)

## Subfolders
- **Dto**: DTOs for installation wizard steps

## Business Logic
Executes first-time setup workflow including database migration, seed data creation, admin account establishment, and initial configuration persistence.

## Usage
Consumed by installation wizard UI during first application deployment.