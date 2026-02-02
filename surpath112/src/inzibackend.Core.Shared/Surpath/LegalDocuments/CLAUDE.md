# Surpath/LegalDocuments Documentation

## Overview
Data transfer objects and enums for managing legal documents (Privacy Policy and Terms of Service) within the application, supporting both file uploads and external URLs.

## Contents

### Files

#### DocumentType.cs
- **Purpose**: Enumeration of legal document types
- **Enum Values**:
  - PrivacyPolicy (1): Privacy policy documents
  - TermsOfService (2): Terms of service documents
- **Usage**: Categorizes legal documents for display and management

#### LegalDocumentDto.cs
- **Purpose**: Base DTO for legal document information
- **Key Properties**:
  - Id: Unique identifier (GUID)
  - Type: Document type (Privacy Policy or Terms)
  - ExternalUrl: Optional external hosting URL
  - FileName: Name of uploaded file
  - ViewUrl: Internal viewing URL
  - FileId: Binary storage reference
- **Inheritance**: Extends EntityDto<Guid>

#### CreateOrUpdateLegalDocumentDto.cs
- **Purpose**: DTO for create/update operations
- **Properties**:
  - LegalDocument: Document information
  - FileToken: Temporary file cache token
- **Usage**: Handles file upload during document creation/update

#### GetAllLegalDocumentsInput.cs
- **Purpose**: Input parameters for document listing
- **Usage**: Filtering and pagination for document queries

#### GetLegalDocumentForEditDto.cs
- **Purpose**: DTO for editing existing documents
- **Usage**: Returns document data for edit forms

#### GetLegalDocumentForEditInput.cs
- **Purpose**: Input for requesting document edit data
- **Usage**: Identifies document to edit

#### GetLegalDocumentForViewDto.cs
- **Purpose**: DTO for viewing document details
- **Usage**: Read-only document presentation

#### GetLegalDocumentForViewInput.cs
- **Purpose**: Input for requesting document view
- **Usage**: Identifies document to view

### Key Components
- Support for both uploaded files and external URLs
- File token system for temporary uploads
- Separate DTOs for different operations (CRUD)
- Binary storage integration via FileId

### Dependencies
- Abp.Application.Services.Dto: For base DTO classes
- System: For GUID identifiers
- Various namespaces for organization

## Architecture Notes
- Flexible document storage (local files or external URLs)
- GUID-based identification for documents
- Temporary file handling via tokens
- Clear separation of view/edit/create operations
- Multi-tenant ready (inherited from ABP framework)

## Business Logic
- **Document Types**: Limited to Privacy Policy and Terms of Service
- **Storage Options**: Internal file storage or external URL reference
- **File Handling**: Temporary cache with tokens before permanent storage
- **Access Control**: ViewUrl provides controlled document access

## Usage Across Codebase
These legal document DTOs are used in:
- Legal document management services
- Admin interfaces for document updates
- User agreement acceptance workflows
- Registration processes requiring legal acceptance
- Compliance and audit tracking
- Public document display endpoints
- Document versioning systems