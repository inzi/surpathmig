# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

SurPath is a medical/healthcare management system for drug testing and screening management. It's built using .NET Framework 4.6 with an N-tier architecture pattern.

### Key Components
- **SurPath.Web** - ASP.NET MVC 5 web application
- **SurPath** - Windows desktop application (WinForms)
- **SurPath.Backend** - Windows service for background processing
- **HL7.Parser** - Service for parsing HL7 medical data format
- **SurPath.Business** - Business logic layer
- **SurPath.Data** - Data access layer using DAO pattern
- **SurPath.Entity** - Domain models and DTOs

### Technology Stack
- **Backend**: C# (.NET Framework 4.6), ASP.NET MVC 5
- **Database**: MySQL (MySql.Data)
- **Frontend**: Bootstrap, jQuery, jQuery UI, Knockout.js, GridMvc
- **PDF**: iTextSharp
- **Logging**: Serilog
- **SMS**: Twilio
- **Payments**: Authorize.NET
- **Medical Data**: HL7 format support

## Build Commands

```bash
# Restore NuGet packages
nuget restore Surpath-All.sln

# Build entire solution
msbuild Surpath-All.sln /p:Configuration=Debug
msbuild Surpath-All.sln /p:Configuration=Release

# Build specific components
msbuild SurPath-Win.sln          # Desktop application
msbuild SurScan-Web.sln          # Web application
msbuild HL7.sln                  # HL7 components
```

## Running Tests

All test projects use MSTest framework.

```bash
# Using dotnet CLI
dotnet test Surpath-All.sln

# Using MSTest directly (after building)
cd HL7UnitTests
msbuild HL7UnitTests.csproj
mstest /testcontainer:bin/Debug/HL7UnitTests.dll

# Using VSTest.Console
vstest.console.exe HL7UnitTests\bin\Debug\HL7UnitTests.dll
```

Test projects:
- HL7UnitTests
- SurPath.Backend.Tests
- Surpath.CSTestTests
- VBTests

## Code Analysis

Code analysis is configured for Debug builds only on key projects.

```bash
# Run code analysis during build
msbuild /p:Configuration=Debug /p:RunCodeAnalysis=true

# Run for specific project
msbuild SurPath/SurPath.csproj /p:Configuration=Debug /p:RunCodeAnalysis=true
```

Custom rulesets focus on detecting unused code (parameters, locals, private members).

## Architecture Patterns

### N-Tier Architecture
1. **Presentation**: ASP.NET MVC (SurPath.Web) and WinForms (SurPath)
2. **Business Logic**: SurPath.Business
3. **Data Access**: SurPath.Data with DAO pattern
4. **Entities**: SurPath.Entity for domain models
5. **Services**: Windows Service (SurPath.Backend) and HL7 Parser

### Key Integrations
- FormFox API for forms
- ClearStar for background checks
- HL7 for medical data exchange
- Twilio for SMS notifications
- Authorize.NET for payment processing

### Important Paths
- Connection strings: `app.config` / `web.config`
- Email templates: XSLT based in SurPath.Web
- Log files: `C:\Logs\`
- Database scripts: `/DB_Related_Files/`

### Development Notes
- Current branch: `release2024`
- Main branch for PRs: `master`
- Shared assembly version: 1.8.0.0
- Email configurations in app settings
- HL7 Parser has FTP capabilities and auto-matching features