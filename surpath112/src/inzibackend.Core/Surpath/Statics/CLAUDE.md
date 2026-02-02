# Statics Documentation

## Overview
Static utility classes and configuration settings for the Surpath module. Contains ID generation utilities and configuration constants (currently commented out).

## Contents

### Files

#### SlotIdGenerator.cs
- **Purpose**: Cryptographically secure random ID generator for slot identification
- **Key Functionality**:
  - Generates random alphanumeric IDs of specified length
  - Supports optional prefix (up to 2 characters)
  - Uses cryptographically secure random number generation
  - Excludes ambiguous characters (I, L, 0) for better readability
- **Technical Details**:
  - Uses RandomNumberGenerator for cryptographic security
  - Character set: "ABCDEFGHJKMNOPQRSTUVWXYZ123456789"
  - Prefix automatically converted to uppercase
  - Thread-safe static implementation

#### SurpathSettings.cs
- **Purpose**: Configuration settings container (currently commented out)
- **Intended Functionality**:
  - File upload settings (max sizes, allowed extensions)
  - Root folder paths for record storage
  - User-friendly size display values
- **Configuration Items (when active)**:
  - MaxfiledataLength: Maximum file size (default 5MB)
  - MaxProfilefiledataLength: Maximum profile file size
  - AllowedFileExtensions: Permitted file types (jpeg, jpg, png, pdf, txt, hl7)
  - SurpathRecordsRootFolder: Root directory for record storage

### Key Components

- **SlotIdGenerator**: Static class for generating unique slot identifiers
- **SurpathSettings**: Configuration constants (currently disabled)

### Dependencies

- **External Libraries**:
  - System.Security.Cryptography (for secure random generation)

- **Internal Dependencies**: None

## Architecture Notes

- **Security**: Uses cryptographically secure random number generation
- **Readability**: Excludes ambiguous characters from generated IDs
- **Flexibility**: Supports prefixes for categorization
- **Configuration**: Settings class prepared for future activation

## Business Logic

### ID Generation
- Generates human-readable unique identifiers
- Suitable for slot booking, rotation assignments, or other unique references
- Prefix support allows categorization (e.g., "DS" for drug screen, "BC" for background check)
- Maximum prefix length of 2 characters ensures consistent ID format

### Character Selection
- Excludes I, L, 0 to avoid confusion with 1 and O
- Uses uppercase only for consistency
- Mix of letters and numbers for entropy

## Usage Across Codebase

The SlotIdGenerator is likely used for:
- Generating unique rotation slot identifiers
- Creating confirmation codes
- Generating temporary access tokens
- Any scenario requiring unique, human-readable identifiers

The SurpathSettings (when enabled) would control:
- File upload validation
- Storage path configuration
- Size limit enforcement