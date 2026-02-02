# Surpath/ParserClasses Documentation

## Overview
Document parsing utilities for extracting structured data from lab reports, particularly CRL (Clinical Reference Laboratory) drug test reports in PDF format.

## Contents

### Files

#### CRLLabReport.cs
- **Purpose**: Data model for parsed lab report information
- **Key Properties**:
  - Lab credentials (CLIA, SAMHSA, CAP numbers)
  - Patient information (name, DOB, ID, gender)
  - Sample details (ID, collection/received/reported dates)
  - Physician information (name, address, contact)
  - Site information (address, branch, contact)
  - Test results (urinalysis and initial tests)
  - Certification details (lab director, certifier)
- **Collections**: UrinalysisResults and InitialTestResults lists

#### CRLTestResult.cs
- **Purpose**: Individual test result model
- **Properties**:
  - TestName: Name of the test performed
  - Result: Test outcome value
  - Status: Pass/fail or similar status
  - CutoffOrExpectedValues: Reference ranges or cutoff values
- **Usage**: Represents individual drug test results

#### CRLLabReportParser.cs
- **Purpose**: Regex-based parser for extracting data from PDF text
- **Key Methods**:
  - ParseLabReport(): Main parsing method
  - ExtractValue(): Regex extraction helper
  - ParseDate(): Date string parsing
- **Features**:
  - Complex regex patterns for field extraction
  - Handles various date formats
  - Extracts nested test results
  - Parses lab certifications

#### PDFToTextConverter.cs
- **Purpose**: PDF to text conversion using PdfPig library
- **Key Method**:
  - ConvertPdfToText(): Converts PDF to structured text
- **Features**:
  - Word-level extraction with positioning
  - Smart line break detection based on positioning
  - Preserves document structure for parsing
  - Handles multi-page documents

### Key Components
- Complete lab report data model
- Sophisticated regex-based parsing
- PDF text extraction with layout preservation
- Support for multiple test result types
- Certification and compliance tracking

### Dependencies
- UglyToad.PdfPig: PDF processing library
- System.Text.RegularExpressions: Pattern matching
- System.Collections.Generic: Collections

## Architecture Notes
- Two-step process: PDF → Text → Structured Data
- Position-aware text extraction preserves layout
- Regex patterns tailored to CRL report format
- Extensible model for additional lab formats
- Clear separation of parsing and data models

## Business Logic
- **Lab Compliance**: Tracks CLIA, SAMHSA, CAP certifications
- **Chain of Custody**: Collection, receipt, and reporting dates
- **Test Results**: Separate urinalysis and initial test tracking
- **Reference Values**: Cutoff values for compliance determination
- **Multi-Site Support**: Handles different collection sites

## Usage Across Codebase
These parser classes are used in:
- Document upload and processing services
- Lab result import workflows
- Compliance tracking systems
- Drug test result management
- Report generation and display
- Data validation and verification
- Automated result extraction
- Historical record imports