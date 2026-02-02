# LegalDocuments Documentation

## Overview
Entity model for managing legal documents such as Privacy Policies and Terms of Service within the multi-tenant system. Supports both file-based and externally hosted documents.

## Contents

### Files

#### LegalDocument.cs
- **Purpose**: Entity representing legal documents for tenants
- **Key Properties**:
  - FileId: Reference to binary storage for uploaded documents
  - Type: Document classification (Privacy Policy or Terms of Service)
  - ExternalUrl: Support for externally hosted documents
  - FileName: Original uploaded file name
  - ViewUrl: Controller-generated URL for viewing
- **Features**:
  - Multi-tenant support (IMayHaveTenant)
  - Full auditing (creation, modification, deletion tracking)
  - Flexible storage (binary or external URL)
  - Auto-generated GUID on creation

### Key Components

- **LegalDocument**: Core entity for legal document management

### Dependencies

- **External Libraries**:
  - ABP Framework (Entities, Auditing)

- **Internal Dependencies**:
  - DocumentType enum (from inzibackend.Core.Shared)

## Architecture Notes

- **Pattern**: Domain entity with full auditing
- **Storage**: Supports dual storage strategy (binary or external)
- **Multi-tenancy**: Tenant-isolated documents
- **Auditing**: Tracks all changes with user and timestamp

## Business Logic

### Document Storage Options
1. **Binary Storage**: FileId references uploaded document in binary storage
2. **External Hosting**: ExternalUrl points to externally hosted document
3. **Hybrid**: Can support both simultaneously

### Document Types
- Privacy Policy
- Terms of Service
- Extensible through DocumentType enum

### URL Management
- ViewUrl provides consistent access point through application controller
- Abstracts storage location from consumers

## Usage Across Codebase

This entity is used for:
- Displaying legal documents to users during registration
- Compliance tracking for user acceptance
- Administrative management of legal content
- Version history through auditing features
- Multi-tenant legal document customization

## Security Considerations

- Documents are tenant-isolated
- Access controlled through ViewUrl controller
- File uploads should be validated (see HtmlSanitizer)
- External URLs should be validated for security