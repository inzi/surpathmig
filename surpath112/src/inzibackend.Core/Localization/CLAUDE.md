# Localization Documentation

## Overview
Multi-language support system providing internationalization (i18n) for the application with culture-specific formatting and resource management.

## Contents

### Files

#### inzibackendLocalizationConfigurer.cs
- **Purpose**: Configures localization system
- **Key Features**:
  - Registers localization sources
  - Configures supported languages
  - Sets default language
  - Configures resource locations

#### ApplicationCulturesProvider.cs
- **Purpose**: Provides list of supported cultures/languages
- **Key Features**:
  - Available language list
  - Culture information
  - Flag icons
  - Display names

#### IApplicationCulturesProvider.cs
- **Purpose**: Interface for culture provider

#### CultureHelper.cs
- **Purpose**: Utility methods for culture operations
- **Methods**:
  - Culture validation
  - Culture switching
  - Default culture retrieval
  - Culture formatting helpers

#### LocaleMappingInfo.cs
- **Purpose**: Maps application locales to system cultures
- **Features**: Custom locale to culture code mapping

### Subfolders

#### inzibackend/
- Contains localization resource files (XML/JSON)
- Language-specific translations
- Organized by language code (en, es, fr, etc.)

### Key Components

- **inzibackendLocalizationConfigurer**: Configuration
- **ApplicationCulturesProvider**: Supported languages
- **CultureHelper**: Utility methods
- **Resource Files**: Translation strings

### Dependencies

- **External Libraries**:
  - ABP Framework (localization module)
  - System.Globalization

- **Internal Dependencies**:
  - Used throughout all layers

## Architecture Notes

- **Pattern**: Resource-based localization
- **Storage**: XML/JSON resource files
- **Fallback**: English as default
- **Runtime**: Dynamic language switching

## Business Logic

### Supported Languages

Typical supported cultures:
- English (en)
- Spanish (es)
- French (fr)
- German (de)
- Portuguese (pt-BR)
- Chinese (zh-Hans)
- And others...

### Localization Sources
- **inzibackend**: Application-specific strings
- **AbpWeb**: ABP framework strings
- **AbpZero**: ABP Zero module strings

### Resource Key Format
```
Pages_Administration_Users
Login_InvalidUserNameOrPassword
ValidationError_{PropertyName}
```

### Culture Selection Priority
1. User preference (saved in profile)
2. Browser language
3. Tenant default language
4. Application default (English)

## Usage Across Codebase

Localization used in:
- UI labels and buttons
- Validation messages
- Error messages
- Email templates
- Notifications
- Reports
- Navigation menus
- Help text

## Localization Methods

### Server-Side
```csharp
var message = L("WelcomeMessage");
var formatted = L("UserCount", userCount);
```

### Client-Side (JavaScript)
```javascript
var message = app.localize('WelcomeMessage');
```

## Adding New Languages

1. Create new resource file (e.g., inzibackend-it.xml)
2. Translate all keys
3. Add culture to ApplicationCulturesProvider
4. Add flag icon
5. Test formatting (dates, numbers, currency)

## Best Practices

- Use meaningful key names
- Group related keys (Pages_, Validation_, etc.)
- Provide context in comments
- Test with RTL languages (Arabic, Hebrew)
- Consider string length variations
- Use placeholders for dynamic values

## Extension Points

- Additional languages
- Custom localization sources
- Database-driven translations
- User-contributed translations
- Translation management UI
- Automatic translation integration