# Localization DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the internationalization (i18n) and localization (l10n) system. These DTOs enable multi-language support by managing language definitions, translatable resource strings, and language selection. The system allows administrators to add new languages and customize translations for the entire application.

## Contents

### Files

#### Language Management
- **ApplicationLanguageListDto.cs** - Language list item:
  - Name - Language code (en, es, fr, etc.)
  - DisplayName - Language name in native script
  - Icon - Flag or language icon
  - IsDefault - Default language flag
  - IsDisabled - Inactive languages

- **ApplicationLanguageEditDto.cs** - Language details for editing:
  - Language configuration
  - Enabled/disabled state
  - Default language flag

- **CreateOrUpdateLanguageInput.cs** - Language save operation:
  - Language details
  - Create new or update existing

- **SetDefaultLanguageInput.cs** - Change default language:
  - Language name to set as default
  - Applied to new users and unauthenticated sessions

#### Translation Management
- **GetLanguageTextsInput.cs** - Query translation strings:
  - SourceName - Resource source (e.g., "Surpath", "AbpWeb")
  - BaseLanguageName - Base language for comparison
  - TargetLanguageName - Language being translated
  - TargetValueFilter - Filter strings (All, EmptyOnes, NonEmptyOnes)
  - FilterText - Search term
  - Supports paging and sorting

- **LanguageTextListDto.cs** - Translatable string:
  - Key - Resource key (e.g., "Login", "Save", "Cancel")
  - BaseValue - Value in base language (usually English)
  - TargetValue - Translated value in target language
  - Used in translation grid for bulk editing

- **UpdateLanguageTextInput.cs** - Update single translation:
  - SourceName - Resource source
  - LanguageName - Target language
  - Key - Resource key
  - Value - Translated text

#### Output DTOs
- **GetLanguageForEditOutput.cs** - Complete language edit context:
  - Language details
  - Available languages for reference
  - Validation data

- **GetLanguagesOutput.cs** - All languages:
  - List of available languages
  - Default language information

### Key Components

#### Language Sources
Translation strings organized by source:
- **Surpath** - Application-specific strings
- **AbpWeb** - ABP framework web strings
- **AbpZero** - ABP Zero framework strings
- Each source independently translatable

#### Translation Workflow
- Base language (typically English) provides source text
- Translators see base value and enter target value
- Empty translations fall back to base language
- Can filter to show only untranslated strings

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **inzibackend.Dto** - Paging base DTOs
- **Abp.Localization** - Localization infrastructure

## Architecture Notes

### Resource Key Pattern
- Keys use dot notation (e.g., "Pages.Users.CreateNewUser")
- Hierarchical organization of translations
- Same key can have different values per language

### Translation Fallback
1. User's selected language
2. Tenant's default language
3. Host's default language
4. Base language (English)
5. Resource key itself (if all else fails)

### Culture Support
- Languages tied to .NET CultureInfo
- Right-to-left (RTL) language support
- Date, number, and currency formatting per culture

### Translation Storage
- Translations stored in database
- Can be edited at runtime
- No recompilation needed for translation changes
- Supports tenant-specific translation overrides

## Business Logic

### Language Management Workflow
1. **Add Language**: Admin creates ApplicationLanguageEditDto for new language (e.g., Spanish)
2. **Enable Language**: Language marked as enabled
3. **Translate Strings**: Translators use GetLanguageTextsInput to load strings
4. **Update Translations**: UpdateLanguageTextInput for each string
5. **Set Default**: SetDefaultLanguageInput if language should be default
6. **User Selection**: Users choose language in profile settings

### Translation Editing
1. Translator selects target language (e.g., Spanish)
2. Selects base language for reference (e.g., English)
3. System loads all resource strings via GetLanguageTextsInput
4. Translator sees English text and enters Spanish translation
5. Updates saved via UpdateLanguageTextInput
6. Changes immediately available to users

### Translation Filtering
- **All**: Show all translation strings
- **EmptyOnes**: Show only untranslated strings (for translation completion)
- **NonEmptyOnes**: Show only translated strings (for review)
- FilterText searches keys and values

## Usage Across Codebase
These DTOs are consumed by:
- **ILanguageAppService** - Language CRUD operations
- **Language Management UI** - Admin language configuration
- **Translation Editor** - Bulk translation interface
- **Localization System** - Runtime string lookup
- **User Profile** - Language selection
- **Tenant Settings** - Default language configuration

## Cross-Reference Impact
Changes to these DTOs affect:
- Language management interfaces
- Translation editing tools
- User language selection
- Multi-language support throughout application
- Default language configuration
- Translation workflow tools
- Localized text display everywhere