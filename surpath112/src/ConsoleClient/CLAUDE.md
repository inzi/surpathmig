# ConsoleClient Documentation

## Overview
The ConsoleClient is a standalone .NET 6 console application designed for batch processing of lab reports. It primarily focuses on parsing PDF lab reports from CRL (Clinical Reference Laboratory), extracting patient information, and matching it with existing cohort users in the Surpath system database. This application operates independently of the main web application while leveraging the same domain services and data access layers.

## Contents

### Files

#### Program.cs
- **Purpose**: Main entry point for the console application that processes lab report PDFs
- **Key Functionality**:
  - Authenticates a user session using ABP's `LogInManager`
  - Scans a specified directory for PDF files
  - Parses CRL lab reports using `CRLLabReportParser`
  - Extracts patient SSN and formats it (XXX-XX-XXXX format)
  - Looks up users by SSN in the `UserPid` repository
  - Matches found users with cohort users
  - Outputs Chain of Custody (COC) information with patient details
- **Configuration**:
  - Hard-coded import path: `D:\Surpath\Import\August DS Uploads`
  - Login credentials: `chris@inzi.com` / `123qwe`
- **Key Methods**:
  - `Main()`: Orchestrates the entire processing workflow
  - `FormatSsn()`: Formats 9-digit SSN to standard format
  - `parseLabResults()`: Bulk parses lab report PDFs (currently unused)
  - `ListPdfFiles()`: Recursively finds all PDF files in a directory

#### ConsoleAppModule.cs
- **Purpose**: ABP module configuration for the console application
- **Key Functionality**:
  - Configures module dependencies (Application, EntityFrameworkCore, Redis, Hangfire)
  - Sets up database connection strings from configuration
  - Registers the ABP Zero license code
  - Enables database localization for language management
  - Registers custom services including the dummy `IWebUrlService`
- **Module Dependencies**:
  - `inzibackendApplicationModule`: Core application services
  - `inzibackendEntityFrameworkCoreModule`: Data access layer
  - `AbpEntityFrameworkCoreModule`: ABP EF Core support
  - `AbpRedisCacheModule`: Redis caching (optional)
  - `AbpHangfireAspNetCoreModule`: Background job processing (optional)

#### ConsoleClient.csproj
- **Purpose**: Project configuration and dependencies
- **Framework**: .NET 6.0
- **Key Features**:
  - Console application output type
  - Implicit usings enabled
  - Nullable reference types enabled
- **Package Dependencies**:
  - `Abp.AspNetZeroCore.Web` v4.1.0
  - `Abp.Castle.Log4Net` v7.3.0
  - `Abp.HangFire.AspNetCore` v7.3.0
  - `Abp.RedisCache` v7.3.0
  - `Newtonsoft.Json` v13.0.1
- **Project Reference**:
  - `inzibackend.Application`: Main application layer

#### log4net.config
- **Purpose**: Logging configuration for the console application
- **Configuration**:
  - Rolling file appender targeting `../../../App_Data/Logs/Logs.txt`
  - Maximum file size: 10MB
  - Keeps up to 10 backup files
  - Log level: DEBUG
  - Pattern includes level, date, thread, logger name, and message

#### appsettings.json
- **Purpose**: Main application configuration
- **Key Settings**:
  - Database connection strings (Default, Hangfire, surpathlive)
  - Redis cache configuration
  - Authentication settings (JWT enabled, social logins disabled)
  - Identity Server configuration
  - Payment gateway settings (PayPal, Stripe, AuthorizeNet)
  - Health check configurations
- **Environment-specific files**:
  - `appsettings.chrisdev.json`: Development environment settings
  - `appsettings.Production.json`: Production environment settings

### Key Components

#### Lab Report Processing Pipeline
1. **PDF Discovery**: Scans directories for PDF files
2. **PDF Parsing**: Converts PDF to text and extracts structured data
3. **Patient Identification**: Extracts and formats SSN
4. **Database Lookup**: Queries UserPid repository for matching SSN
5. **Cohort Matching**: Links user to cohort user records
6. **Result Output**: Displays COC and patient information

#### Authentication Flow
- Uses ABP's `LogInManager` for authentication
- Creates an authenticated session context
- All database operations occur within this session

#### Unit of Work Pattern
- Each PDF processing iteration uses a separate unit of work
- Disables multi-tenancy filters for cross-tenant data access
- Ensures transactional consistency for database operations

### Dependencies

#### External Libraries
- **ABP Framework**: Application framework and infrastructure
- **Entity Framework Core**: ORM for database access
- **Castle Windsor**: IoC container
- **Log4Net**: Logging framework
- **Redis**: Distributed caching (optional)
- **Hangfire**: Background job processing (optional)
- **Newtonsoft.Json**: JSON serialization

#### Internal Dependencies
- **inzibackend.Application**: Application services and DTOs
- **inzibackend.EntityFrameworkCore**: Database context and repositories
- **inzibackend.Core**: Domain entities and business logic
- **inzibackend.Surpath**: Surpath-specific domain services
- **inzibackend.MultiTenancy**: Multi-tenant support
- **inzibackend.Authorization**: Authorization and authentication services

## Subfolders

### DependencyInjection
This subfolder contains dependency injection configuration and dummy service implementations for the console application. It provides service registrations and mock implementations required for the console to operate independently of the web infrastructure.

Key components:
- **ServiceCollectionRegistrar**: Handles service registration for identity, tenancy, and other core services
- **DummyWebUrlService**: Mock implementation of IWebUrlService that returns localhost URLs for web-specific operations

The folder ensures all required dependencies are available while providing safe no-op implementations for web-specific features that aren't needed in the console context.

## Architecture Notes

### Design Patterns
- **Dependency Injection**: Extensive use of constructor injection via ABP/Castle Windsor
- **Unit of Work**: Transactional boundaries for database operations
- **Repository Pattern**: Data access through ABP repositories
- **Module Pattern**: ABP module system for component organization

### Conventions
- Follows ABP framework conventions for module initialization
- Uses async/await patterns for asynchronous operations
- Entity Framework includes for eager loading relationships
- Hard-coded configuration values (should be moved to settings)

### Security Considerations
- Credentials are hard-coded in source (security risk)
- No input validation on PDF parsing results
- Direct database access without additional security layers

### Performance Considerations
- Processes PDFs sequentially (could be parallelized)
- Each PDF creates a new unit of work (database connection)
- No caching of user lookups between iterations

## Business Logic

### Patient Matching Algorithm
1. Extract SSN from lab report PDF
2. Format SSN to standard format (XXX-XX-XXXX)
3. Query UserPid table for matching SSN with PidType = "SSN"
4. Filter for non-deleted users only
5. Match found user with CohortUser records
6. Output patient details with Chain of Custody information

### Domain Concepts
- **CohortUser**: Represents a user enrolled in a specific cohort/program
- **UserPid**: Stores various patient identifiers (SSN, MRN, etc.)
- **Chain of Custody (COC)**: Tracking identifier for lab specimens
- **Slip ID**: Alternative identifier extracted from lab reports

## Usage Across Codebase

### Integration Points
- Shares the same database with the main web application
- Uses common domain entities and repositories
- Leverages application services for business logic
- Follows the same multi-tenancy architecture

### Deployment Scenarios
- Scheduled batch processing of lab reports
- Manual execution for specific import folders
- Development/testing of lab report parsing logic

### Impact Radius
Changes to this console application would affect:
- Lab report processing workflows
- Patient matching accuracy
- Data integrity in UserPid and CohortUser tables
- Audit logging and compliance tracking

### Cross-References
The console client interacts with several key components:
- **inzibackend.Surpath.ParserClasses**: Lab report parsing logic
- **inzibackend.MultiTenancy.TenantManager**: Multi-tenant data access
- **inzibackend.Authorization.LogInManager**: Authentication services
- **Entity repositories**: UserPid, CohortUser data access

## Known Issues and Technical Debt

1. **Hard-coded Credentials**: Login credentials should be in secure configuration
2. **Hard-coded Paths**: Import directory path should be configurable
3. **No Error Recovery**: Limited error handling for PDF parsing failures
4. **Sequential Processing**: Could benefit from parallel processing
5. **Missing Validation**: No validation of parsed data before database operations
6. **Unused Code**: `parseLabResults` method is defined but not used
7. **Console Output Only**: No persistent logging of processing results