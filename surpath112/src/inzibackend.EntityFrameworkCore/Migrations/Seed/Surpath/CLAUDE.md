# Surpath Documentation

## Overview
Surpath-specific database seeding components responsible for importing and initializing medical compliance data including drugs, test categories, panels, code types, and other domain-specific reference data from the legacy Surpath system.

## Contents

### Files

#### DrugSeeder.cs
- **Purpose**: Imports drug testing data from legacy database
- **Key Functionality**:
  - Seeds test categories (drug testing types)
  - Imports drug panels configurations
  - Creates drug entities with testing parameters
  - Establishes drug-panel relationships
  - Uses Unit of Work pattern for transaction management

#### CodeTypeSeeder.cs
- **Purpose**: Imports code type reference data
- **Key Functionality**:
  - Seeds department codes
  - Creates confirmation value types
  - Imports code classifications
  - Establishes lookup tables for the system

#### DrugSeedDataClass.cs
- **Purpose**: Data transfer object for drug import
- **Key Functionality**:
  - Maps legacy drug data structure
  - Provides property mapping
  - Handles data transformation

#### SafeGetString.cs
- **Purpose**: Utility for safe data extraction
- **Key Functionality**:
  - Null-safe string extraction from data readers
  - Handles DBNull values
  - Prevents import failures from null data

### Key Components

**Import Process**
1. Test Categories (drug test types)
2. Panels (groupings of tests)
3. Drugs (individual substances)
4. Drug-Panel relationships
5. Code types and references

**Data Mapping**
- Legacy ID tracking (IdMatch)
- Cross-reference maintenance
- Data transformation logic

**Transaction Management**
- Separate transactions per entity type
- Rollback capability
- Error isolation

### Dependencies
- SurpathliveSeedHelper (legacy DB connection)
- MySql.Data.MySqlClient
- Entity Framework Core
- Unit of Work pattern

## Architecture Notes

**Legacy Integration**
- Direct SQL queries to legacy database
- ETL-style data transformation
- ID mapping for relationships

**Performance Considerations**
- Batch processing
- Transaction scoping
- Suppressed transactions for bulk inserts

**Error Handling**
- Logging of import progress
- Safe null handling
- Transaction rollback on failure

## Business Logic

**Drug Testing Domain**
- Test categories define testing types
- Panels group related drug tests
- Drugs linked to multiple panels
- Measurement units and thresholds

**Reference Data**
- Code types for categorization
- Department codes for organization
- Confirmation values for validation

**Data Integrity**
- Maintains referential integrity
- Preserves legacy relationships
- Creates new ID mappings

## Usage Across Codebase

**Initial Setup**
- Run during first deployment
- Migration from legacy system
- Development environment setup

**Configuration**
- Controlled by appsettings.json
- DoImport flag enables/disables
- Connection string to legacy DB

**Related Systems**
- Drug testing workflows
- Compliance checking
- Record categorization
- Department management