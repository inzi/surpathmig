# Storage Documentation

## Overview
Configuration constants for binary object storage, defining size limits for stored binary data.

## Contents

### Files

#### BinaryObjectConsts.cs
- **Purpose**: Constants for binary storage constraints
- **Key Constants**:
  - BytesMaxSize: 10240 bytes (10 KB) maximum size for binary objects
- **Usage**: Validates binary data size before storage

### Key Components
- Size limit enforcement for binary storage
- 10 KB limit for small binary objects

### Dependencies
- No external dependencies
- Part of the inzibackend.Storage namespace

## Architecture Notes
- Small size limit suggests use for thumbnails or small files
- Likely used for database-stored binary objects
- Prevents excessive database growth from binary data

## Business Logic
- **Size Limit**: 10 KB maximum prevents storage abuse
- **Use Cases**: Profile pictures, small icons, thumbnails
- **Storage Strategy**: Small binaries in database, large files elsewhere

## Usage Across Codebase
This storage constant is used in:
- File upload validation
- Binary object entities
- Profile picture storage
- Document thumbnail generation
- Database binary column constraints
- API file upload endpoints
- Storage service implementations