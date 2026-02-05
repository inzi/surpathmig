# Localization Resources Documentation

## Overview
This folder contains language-specific resource files for the inzibackend application. Each file contains translated strings for a specific language/culture.

## Contents

### Resource Files

Typically contains XML or JSON files named by culture code:
- `inzibackend.xml` or `inzibackend-en.xml` - English (default)
- `inzibackend-es.xml` - Spanish
- `inzibackend-fr.xml` - French
- `inzibackend-de.xml` - German
- `inzibackend-pt-BR.xml` - Portuguese (Brazil)
- Additional languages as supported

### File Structure

XML format example:
```xml
<localizationDictionary culture="en">
  <texts>
    <text name="WelcomeMessage">Welcome to inzibackend!</text>
    <text name="Pages_Users">Users</text>
    <text name="ValidationError_Required">{0} is required</text>
  </texts>
</localizationDictionary>
```

## Key Categories

### Navigation and Pages
- `Pages_*`: Page titles and navigation items
- `Menu_*`: Menu item labels

### Validation Messages
- `ValidationError_*`: Field validation errors
- `Error_*`: General error messages

### Entities and Fields
- Entity names (User, Tenant, Role, etc.)
- Field labels (UserName, Email, PhoneNumber, etc.)

### Actions
- `Create`, `Edit`, `Delete`, `Save`, `Cancel`
- Domain-specific actions

### Notifications
- `Notification_*`: System notifications
- `Email_*`: Email template strings

### Surpath Domain
- Compliance-related strings
- Drug testing terminology
- Background check messages
- Document types
- Service names

## Best Practices

- Keep keys consistent across languages
- Use placeholders {0}, {1} for dynamic values
- Maintain alphabetical or logical grouping
- Comment complex or context-specific strings
- Test formatting with actual data
- Consider cultural differences (not just language)

## Translation Process

1. English (source) defined first
2. Professional translation to target languages
3. Native speaker review
4. Context validation
5. UI testing with translations
6. Iterative refinement

## Usage

Strings accessed via:
- `L("KeyName")` in C# code
- `@L("KeyName")` in Razor views
- `app.localize('KeyName')` in JavaScript

## Maintenance

- Update all language files when adding new features
- Mark untranslated strings clearly
- Version control tracks translation changes
- Regular audits for missing translations
- Community contributions welcome