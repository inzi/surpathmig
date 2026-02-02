# Helpers Documentation

## Overview
Security-focused helper utilities for the Surpath system, primarily containing HTML sanitization capabilities to prevent XSS attacks and validate file types for secure document handling.

## Contents

### Files

#### HtmlSanitizer.cs
- **Purpose**: Comprehensive HTML sanitization utility to prevent XSS attacks
- **Key Functionality**:
  - Removes dangerous HTML elements (scripts, iframes, forms, etc.)
  - Strips malicious attributes (event handlers, data attributes)
  - Whitelist-based tag and attribute filtering
  - URL validation for href and src attributes
  - File type validation for legal documents
- **Security Features**:
  - Regex-based detection of dangerous patterns
  - Whitelist approach for allowed tags and attributes
  - Special handling for URL attributes to prevent javascript: attacks
  - HTML encoding of attribute values

### Key Components

- **SanitizeHtml()**: Main sanitization method that removes all dangerous content
- **IsHtmlSafe()**: Validation method to check if HTML contains dangerous elements
- **IsAllowedFileType()**: File type validator for legal documents

### Allowed Elements

**Tags**: h1-h6, p, div, span, br, hr, ul, ol, li, dl, dt, dd, table, thead, tbody, tr, th, td, a, strong, em, b, i, u, s, small, blockquote, pre, code, img

**Attributes**: href, src, alt, title, class, id, name, width, height, target, rel, type, colspan, rowspan

### Dependencies

- **External Libraries**:
  - System.Text.RegularExpressions
  - System.Net.WebUtility (HTML encoding)

## Architecture Notes

- **Pattern**: Static utility class with regex-based parsing
- **Security**: Defense-in-depth with multiple layers of sanitization
- **Performance**: Pre-compiled regex patterns for efficiency
- **Extensibility**: Easy to add/remove allowed tags and attributes

## Business Logic

### Sanitization Process
1. Remove all script tags and content
2. Strip event attributes (onclick, onload, etc.)
3. Remove javascript: protocols from URLs
4. Remove potentially dangerous tags (iframe, object, embed, form, meta, base, svg)
5. Strip data and style attributes
6. Apply whitelist filtering for tags and attributes
7. HTML-encode attribute values

### URL Validation
- Only allows http://, https://, mailto:, relative paths (/), and anchors (#)
- Blocks javascript: and other potentially dangerous protocols

### File Type Validation
- Allows only HTML, HTM, CSS, and PDF files for legal documents
- Case-insensitive extension checking

## Usage Across Codebase

This sanitizer is likely used for:
- Processing user-generated content
- Sanitizing legal document HTML before display
- Validating file uploads for legal documents
- Cleaning HTML content from external sources
- Ensuring safe display of welcome messages or notifications