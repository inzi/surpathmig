# Legal Documents Application Service Documentation

## Overview
Application service managing legal documents such as Terms of Service, Privacy Policies, and HIPAA agreements. Provides versioning, type-based retrieval, and caching capabilities for displaying legal documents in the application UI and registration flows.

## Contents

### Files

#### ILegalDocumentsAppService.cs
- **Purpose**: Interface defining legal document management operations
- **Key Methods**:
  - `GetAll()`: Paginated list with filtering
  - `GetLegalDocumentForView()`: Retrieve document for display
  - `GetLegalDocumentForEdit()`: Retrieve document for editing
  - `CreateOrEdit()`: Create or update legal document with file upload
  - `Delete()`: Remove legal document
  - `GetLatestDocumentByType()`: Get most recent version of a document type
  - `GetBinaryObjectFromCache()`: Retrieve uploaded file from temporary cache
  - `IsLastDocumentOfType()`: Check if document is the only one of its type
  - `GetLegalDocumentUrls()`: Get cached URLs for all document types
- **Document Types Supported**:
  - Terms of Service
  - Privacy Policy
  - HIPAA Agreement
  - Other configurable types via `DocumentType` enum

#### LegalDocumentsAppService.cs (not shown, but implied)
- **Implementation**: Concrete implementation of interface
- **Key Features**:
  - Version management (track document changes over time)
  - File upload and binary storage integration
  - Type-based retrieval for current active documents
  - URL generation and caching
  - Multi-tenant support

### Key Components

**Legal Document Operations:**
- CRUD operations for legal document management
- Version tracking (multiple documents per type)
- Binary file storage for PDF/document files
- Type-based queries for latest versions
- URL caching for performance

**Integration Points:**
- Binary object storage system
- Temporary file cache for uploads
- Registration/onboarding flows
- User agreement acceptance tracking

### Dependencies
- **External**:
  - ABP Framework (Application Services, DTOs)
  - System.Collections.Generic
- **Internal**:
  - `inzibackend.Dto.FileDto`: File download representation
  - `inzibackend.Storage.BinaryObject`: Binary file storage
  - `inzibackend.Surpath.Dtos.LegalDocuments.*`: Legal document DTOs
  - Document storage infrastructure

## Architecture Notes
- **Pattern**: Application Service with interface abstraction
- **Versioning**: Multiple documents per type, retrieve latest by default
- **Caching**: URLs cached to avoid repeated database queries
- **File Management**: Uses binary object storage with temporary cache for uploads

## Business Logic

### Document Retrieval Flow
1. Client requests document by type (e.g., Terms of Service)
2. Service queries for latest document of that type
3. Returns document metadata and download URL
4. Client displays document to user

### Document Upload Flow
1. Client uploads new legal document with file
2. File stored in temporary cache with token
3. Service retrieves file from cache
4. Creates document record and saves binary object
5. Invalidates cached URLs
6. Returns new document ID

### Version Management
- Multiple versions of same document type can exist
- Query typically returns latest version by creation date
- Older versions retained for audit/history
- Users shown currently active version

### URL Caching Strategy
- `GetLegalDocumentUrls()` returns dictionary of type → URL
- Cached to reduce database queries during registration
- Cache invalidated when documents are created/updated/deleted
- Used in registration forms to display current legal documents

## Usage Across Codebase

### Primary Consumers
- **Registration Flow**: Display Terms of Service and Privacy Policy
- **Settings Pages**: Allow users to re-view legal documents
- **Admin Interface**: Manage and upload new document versions
- **Compliance Tracking**: Track user acceptance of legal agreements
- **Email Templates**: Include links to current legal documents

### Typical Usage Patterns
```csharp
// Get latest Terms of Service
var tos = await _legalDocumentsAppService
    .GetLatestDocumentByType(DocumentType.TermsOfService);

// Get all document URLs for registration form
var urls = await _legalDocumentsAppService.GetLegalDocumentUrls();
var tosUrl = urls[DocumentType.TermsOfService];
```

### Related Components
- User agreement acceptance tracking
- Registration workflow
- Email notification system
- Document version history

## Security Considerations
- **Public Access**: Some legal documents may be publicly accessible (pre-auth)
- **Admin Only Creation**: Only administrators can upload new legal documents
- **Version Control**: Audit trail for document changes
- **File Validation**: Uploaded files should be validated for type and size
- **Tenant Isolation**: Documents scoped per tenant if multi-tenant

## Compliance Features
- **HIPAA Support**: Special document type for healthcare compliance
- **Version History**: Retain all versions for legal audit trail
- **User Acceptance Tracking**: Track which version user agreed to
- **Timestamping**: Document creation and modification dates preserved

## Best Practices
- **Update Notifications**: Notify users when legal documents change
- **Version Numbers**: Include version numbers in document names
- **Effective Dates**: Track when new versions become effective
- **User Re-acceptance**: Require users to re-accept updated documents
- **Archive Old Versions**: Mark superseded versions as archived
- **PDF Format**: Use PDF for legal documents to prevent tampering

## Extension Points
- Add document approval workflow before publishing
- Implement document comparison/diff functionality
- Add digital signatures for document acceptance
- Track user acceptance history per document version
- Support multiple languages per document type
- Add document expiration/review reminders