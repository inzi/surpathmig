# Storage Documentation

## Overview
Binary file storage system for managing uploaded files, profile pictures, documents, and temporary files. Provides abstraction over storage backends (database, file system, cloud storage).

## Contents

### Files

#### BinaryObject.cs
- **Purpose**: Entity representing a stored binary file
- **Key Properties**:
  - TenantId: Multi-tenant isolation
  - Bytes: File content (byte array)
  - Description: File metadata
- **Storage**: Database by default

#### IBinaryObjectManager.cs / BinaryObjectManager.cs
- **Purpose**: Service for managing binary objects
- **Key Features**:
  - Save file
  - Get file
  - Delete file
  - Tenant isolation
- **Pattern**: Manager pattern

#### DbBinaryObjectManager.cs
- **Purpose**: Database-backed binary storage implementation
- **Features**: Stores files in database BLOB fields

#### TempFileCacheManager.cs
- **Purpose**: Temporary file caching for upload workflows
- **Key Features**:
  - Cache uploaded files during multi-step processes
  - Automatic cleanup
  - Token-based file retrieval
- **Use Case**: File upload before form submission

#### ITempFileCacheManager.cs
- **Purpose**: Interface for temporary file management

#### TempFileInfo.cs
- **Purpose**: Metadata for temporary files
- **Properties**: File name, content type, length, token

#### FileUploadCacheOutput.cs
- **Purpose**: Output model for file upload operations
- **Properties**: File token, metadata

### Key Components

- **BinaryObject**: File storage entity
- **BinaryObjectManager**: File management service
- **TempFileCacheManager**: Temporary file handling
- **Storage Abstractions**: Pluggable storage backends

### Dependencies

- **External Libraries**:
  - ABP Framework (binary objects module)
  - Entity Framework Core

- **Internal Dependencies**:
  - Configuration system
  - Multi-tenancy
  - Caching system

## Architecture Notes

- **Pattern**: Repository pattern with abstraction layer
- **Storage Options**: Database (default), file system, Azure Blob, AWS S3
- **Caching**: Temporary files cached in memory/distributed cache
- **Multi-Tenancy**: Complete tenant isolation

## Business Logic

### File Upload Flow
1. User selects file in UI
2. File uploaded to temporary cache
3. Server returns file token
4. User completes form
5. Form submitted with file token
6. Service retrieves from temp cache
7. File saved to permanent storage
8. Temp cache entry removed

### Binary Storage
- Small files (<1MB): Database storage
- Large files: File system or cloud storage
- Very large files: Stream processing
- Metadata always in database

### Temporary Cache
- Time-limited (e.g., 1 hour)
- Automatic cleanup
- Prevents abandoned files
- Token-based security

### File Types Stored
- Profile pictures
- Document uploads
- Compliance documents
- Drug test results
- Background check reports
- CSV imports
- Export files

## Usage Across Codebase

Used by:
- User profile pictures
- Document management
- File upload components
- Report generation
- Import/export operations
- Temporary file handling
- Image processing

## Storage Backends

### Database Storage (Default)
- **Pros**: Simple, no external dependencies, transactional
- **Cons**: Database size grows, slower for large files
- **Use For**: Small files, profile pictures, metadata

### File System Storage
- **Pros**: Fast, unlimited size, external backups easy
- **Cons**: Backup complexity, server-specific
- **Use For**: Medium files, documents

### Cloud Storage (Azure Blob, AWS S3)
- **Pros**: Scalable, distributed, CDN support
- **Cons**: Network dependency, cost
- **Use For**: Large files, high traffic, global distribution

## Security Considerations

### Access Control
- Tenant isolation enforced
- User permissions checked before retrieval
- No direct file access (URLs)
- Token-based temporary access

### File Validation
- File type validation
- Size limits enforced
- Virus scanning (recommended)
- Content inspection for malicious code

### Storage Security
- Encrypted at rest (database/cloud)
- Secure transmission (HTTPS)
- No predictable file names
- Access logging

## Performance Considerations

### Caching
- Binary objects can be cached
- Reduce database load
- CDN for public files
- Browser caching headers

### Large Files
- Stream processing (don't load entire file in memory)
- Chunked uploads for >10MB
- Resume capability
- Progress indicators

## Extension Points

- Additional storage providers
- Image resizing/optimization
- Thumbnail generation
- File conversion
- Virus scanning integration
- CDN integration
- File versioning
- Retention policies