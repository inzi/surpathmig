# Branch: task/invoicegen

## Purpose

The purpose of this branch is to create a Hangfire job that updates invoicing for cohort users.

## Planned Changes

- **1: Create UserInvoice Entity** (pending)
  - 1.1: Review ASP.NET Zero v11 entity patterns (pending)
  - 1.2: Implement UserInvoice entity (pending)
  - 1.3: Create UserInvoiceStatus enum (pending)
  - 1.4: Write unit tests (pending)
- **2: Create UserInvoiceStatus Enum** (pending)
  - 2.1: Review ASP.NET Zero v11 documentation for enum implementation patterns (pending)
  - 2.2: Implement UserInvoiceStatus enum following ASP.NET Zero v11 conventions (pending)
- **3: Create PriceConfiguration Entity** (pending)
  - 3.1: Review ASP.NET Zero v11 documentation for entity creation patterns and best practices (pending)
  - 3.2: Implement PriceConfiguration entity following ASP.NET Zero v11 conventions (pending)
  - 3.3: Add proper XML documentation to the entity class and properties (pending)
  - 3.4: Implement validation logic according to ASP.NET Zero v11 patterns (pending)
- **4: Create InvoiceFrequency Enum** (pending)
- **5: Update DbContext with New Entities** (pending, depends on: 1, 2, 3, 4)
  - 5.1: Review ASP.NET Zero v11 documentation in ragdocs MCP server for DbContext configuration patterns (pending)
  - 5.2: Implement DbSet properties for new entities (pending)
  - 5.3: Configure entity relationships and constraints in OnModelCreating (pending)
  - 5.4: Apply appropriate multi-tenancy configurations based on ASP.NET Zero v11 patterns (pending)
  - 5.5: Write integration tests for the updated DbContext (pending)
- **6: Create Database Migration** (pending, depends on: 5)
  - 6.1: Review ASP.NET Zero v11 documentation on the ragdocs mcp server for migration examples before implementation (pending)
  - 6.2: Check for any version-specific considerations for EF Core migrations in ASP.NET Zero v11 (pending)
- **7: Create IPriceManager Interface** (pending, depends on: 3, 4)
- **8: Implement PriceManager** (pending, depends on: 7)
  - 8.1: Review ASP.NET Zero v11 documentation for domain services (pending)
  - 8.2: Verify repository usage patterns against ASP.NET Zero v11 examples (pending)
  - 8.3: Check ASP.NET Zero v11 testing utilities for domain services (pending)
  - 4: Set up PriceManager class and dependencies (pending)
  - 5: Implement GetApplicablePriceConfigurationAsync method (pending, depends on: 4)
  - 6: Implement ShouldGenerateInvoiceForUserAsync method (pending, depends on: 4, 5)
  - 7: Write comprehensive unit tests (pending, depends on: 4, 5, 6)
- **9: Create IUserInvoiceManager Interface** (pending, depends on: 1, 2)
- **10: Implement UserInvoiceManager** (pending, depends on: 9)
  - 1: Set up UserInvoiceManager class and dependencies (pending)
  - 2: Implement GenerateInvoiceNumberAsync method (pending, depends on: 1)
  - 3: Implement HasExistingInvoiceAsync method (pending, depends on: 1)
  - 4: Implement CalculateBillingPeriod method (pending, depends on: 1)
  - 5: Implement CreateInvoiceAsync method (pending, depends on: 1, 2, 3, 4)
- **11: Create DTOs for User Invoices** (pending, depends on: 1, 2, 3, 4)
- **12: Create IUserInvoiceAppService Interface** (pending, depends on: 11)
- **13: Implement UserInvoiceAppService** (pending, depends on: 8, 10, 12)
  - 1: Set up UserInvoiceAppService class and dependencies (pending)
  - 2: Implement Get methods with authorization (pending, depends on: 1)
  - 3: Implement GenerateInvoiceForUserAsync method (pending, depends on: 1)
  - 4: Implement ProcessInvoicesForEligibleUsersAsync with batch processing (pending, depends on: 3)
  - 5: Implement CreateInvoiceAsync with validation (pending, depends on: 1)
  - 6: Implement invoice status update methods (pending, depends on: 1, 2)
- **14: Configure AutoMapper for DTOs** (pending, depends on: 11, 13)
- **15: Create UserInvoiceGenerationJob for Hangfire** (pending, depends on: 13)
- **16: Write Integration Tests for Invoice Generation** (pending, depends on: 15)
  - 1: Set up test data and fixtures (pending)
  - 2: Test individual invoice generation (pending, depends on: 1)
  - 3: Test batch invoice processing (pending, depends on: 1, 2)
  - 4: Test edge cases and error handling (pending, depends on: 1, 2, 3)

## Modified Files

- (empty)

## Summary of Changes

- (empty) 