# Properties Documentation

## Overview
Assembly metadata and version information for the Core project.

## Contents

### Files

#### AssemblyInfo.cs
- **Purpose**: Defines assembly-level attributes
- **Key Attributes**:
  - AssemblyTitle
  - AssemblyDescription
  - AssemblyVersion
  - AssemblyCulture
  - ComVisible settings
  - InternalsVisibleTo (for testing)

### Key Components

- **Assembly Metadata**: Version, title, description
- **Internal Visibility**: Test project access

### Usage

- Build system versioning
- Assembly reflection
- NuGet package metadata
- Internal member access for testing

## Notes

Modern .NET projects often move these attributes to .csproj file, but AssemblyInfo.cs is still valid and commonly used for legacy compatibility.