# Models/Install Documentation

## Overview
This folder contains view models for the application installation wizard, which guides users through initial setup including database configuration, default admin account creation, and system settings.

## Files

### InstallViewModel.cs
**Purpose**: View model for installation wizard

**Properties**:
- `ConnectionString`: Database connection string
- `DatabaseProvider`: MySQL, SQL Server, PostgreSQL, etc.
- `AdminEmailAddress`: Default admin email
- `AdminPassword`: Default admin password
- `DefaultLanguage`: System default language
- `WebSiteUrl`: Application base URL
- `ServerUrl`: API server URL (if separate)
- `SeedSampleData`: Whether to populate sample data

**Validation**:
- Connection string must be valid and accessible
- Database provider must be supported
- Admin credentials must meet requirements
- URLs must be valid and accessible

## Installation Flow

### Step 1: Welcome
- Shows installation wizard welcome page
- Displays system requirements
- Checks prerequisites (dependencies, permissions)

### Step 2: Database Configuration
- User selects database provider
- Enters connection string
- Tests database connection
- Creates database if it doesn't exist
- Applies initial schema migrations

### Step 3: Admin Account
- User creates default administrator account
- Email address
- Password (must meet strength requirements)
- Optional: Phone number for 2FA

### Step 4: System Settings
- Default language selection
- Time zone configuration
- Application URLs
- Email settings (optional)
- Optional: Seed sample data for demonstration

### Step 5: Completion
- Finalizes installation
- Creates configuration files
- Sets installation flag (prevents re-running wizard)
- Redirects to login page

## Architecture Notes

### Installation Detection
Application checks if installed on startup:
```csharp
if (!AppInstallationHelper.IsInstalled())
{
    // Redirect to installation wizard
    return RedirectToAction("Index", "Install");
}
```

Installation flag stored in:
- Configuration file: `appsettings.json` (`IsInstalled: true`)
- Or database: `AbpSettings` table
- Or file system: `.installed` file

### Database Migration
During installation:
1. Creates database (if doesn't exist)
2. Applies EF Core migrations
3. Seeds initial data:
   - Default host tenant
   - Admin user
   - Default roles and permissions
   - System settings
4. Optional: Seeds sample data

### Security Considerations
- Installation wizard only accessible if not installed
- One-time use (cannot re-run without manual intervention)
- Strong password requirements for admin
- Database credentials validated before storing
- Configuration encrypted after installation

## Usage

### First-Time Setup
1. Deploy application to server
2. Navigate to application URL
3. Automatically redirected to `/Install`
4. Follow wizard steps
5. Installation completes
6. Redirected to login page

### Re-installation
To re-run installation (caution: destroys data):
1. Stop application
2. Delete database
3. Remove `.installed` flag from configuration
4. Start application
5. Installation wizard appears again

## Dependencies

### Internal
- `inzibackend.EntityFrameworkCore`: Database migrations
- `inzibackend.Core`: Domain entities
- Configuration management
- Seed data classes

### External
- Entity Framework Core: Database operations
- Database provider packages (MySQL.Data, Npgsql, etc.)
- Configuration providers (JSON, environment variables)

## Business Logic

### Database Provider Selection
Supported providers:
- **MySQL/MariaDB**: Default for this application
- **SQL Server**: Microsoft SQL Server
- **PostgreSQL**: Open-source alternative
- **SQLite**: For development/testing only

Each provider requires:
- Appropriate NuGet package
- Connection string format
- DbContext configuration

### Sample Data Seeding
If enabled, creates:
- Sample tenants
- Sample users (various roles)
- Sample compliance records
- Sample documents
- Demonstration data for testing features

Useful for:
- Demonstrations
- Testing
- Training environments

Not recommended for production installations.

## Related Documentation
- [Controllers/CLAUDE.md](../../Controllers/CLAUDE.md): Install controller
- [Views/Install/CLAUDE.md](../../Views/Install/CLAUDE.md): Installation wizard views
- [inzibackend.EntityFrameworkCore/Migrations/CLAUDE.md](../../../../inzibackend.EntityFrameworkCore/Migrations/CLAUDE.md): Database migrations
- [inzibackend.EntityFrameworkCore/Migrations/Seed/CLAUDE.md](../../../../inzibackend.EntityFrameworkCore/Migrations/Seed/CLAUDE.md): Seed data
- [Models/CLAUDE.md](../CLAUDE.md): Parent folder documentation