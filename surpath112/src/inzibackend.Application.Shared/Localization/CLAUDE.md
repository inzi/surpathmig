# Localization Documentation

## Overview
This folder contains service interfaces and DTOs for internationalization (i18n) and localization (l10n). The localization system enables multi-language support through language management and translation of all application text.

## Contents

### Files
Service interface for localization operations (ILanguageAppService.cs or similar)

### Subfolders

#### Dto
Complete localization DTOs including language management and translation editing.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Database-stored translations
- Runtime translation updates
- Translation fallback chain
- Culture-specific formatting

## Business Logic
Add languages, translate resource strings, set default language, user language selection, RTL language support.

## Usage Across Codebase
All UI text, user profile language settings, localized date/number formatting, translation management UI

## Cross-Reference Impact
Changes affect language management, translation editing, and all localized text display