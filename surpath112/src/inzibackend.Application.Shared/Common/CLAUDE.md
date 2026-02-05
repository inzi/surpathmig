# Common Documentation

## Overview
This folder contains shared service interfaces and DTOs used across multiple features. These common components provide standardized functionality like user lookup, edition retrieval, and common lookup operations.

## Contents

### Files
Service interface for common operations (ICommonLookupAppService.cs or similar)

### Subfolders

#### Dto
Shared DTOs for user search, edition defaults, and common lookup operations.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Reusable across multiple features
- Standard patterns for lookups and searches
- Multi-tenant aware

## Business Logic
Common lookup operations used throughout the application including user search, edition selection, and shared utilities.

## Usage Across Codebase
Dropdown components, autocomplete fields, user selection widgets throughout the application

## Cross-Reference Impact
Changes affect all user lookup interfaces and selection components