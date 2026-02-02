# Views/Shared/Components/AccountLanguages Documentation

## Overview
This View Component renders a language selection interface for the authentication pages, allowing users to switch between available application languages before or during login.

## Files

### AccountLanguagesViewComponent.cs
**Purpose**: View Component class for language selection

**Key Features**:
- Retrieves all active languages from ABP's language manager
- Identifies current language from session
- Returns view model with language list and current selection

**Dependencies**:
- `ILanguageManager`: ABP service for language management
- Base class: `inzibackendViewComponent`

**Method**:
- `InvokeAsync()`: Returns language selection view with model

### LanguageSelectionViewModel.cs
**Purpose**: View model for language selection

**Properties**:
- `CurrentLanguage`: Currently selected language info
- `Languages`: List of all active languages
- `CurrentUrl`: URL for return after language change

### Default.cshtml
**Purpose**: Razor view for language selection UI

**UI Elements**:
- Language switcher area with icons
- Links for each available language
- Visual indicator for current language
- CSS classes:
  - `language-switch-area`: Container
  - `language-icon`: Language icon
  - `language-icon-current`: Highlights current selection

**Behavior**:
- Only renders if multiple languages available
- Each language displayed with its icon
- Clicking language changes culture via `AbpLocalization` controller
- Returns to current page after culture change

## Usage

### Invocation
Called from authentication layout pages:
```razor
@await Component.InvokeAsync(typeof(AccountLanguagesViewComponent))
```

### User Experience
1. User sees available language icons
2. Current language is highlighted
3. Clicking another language:
   - Calls `/AbpLocalization/ChangeCulture`
   - Sets new culture cookie
   - Redirects back to current page
4. UI refreshes in new language

## Dependencies
- ABP Localization system
- `AbpLocalizationController`: Handles culture changes
- Language configuration from database/settings

## Related Documentation
- [Views/Shared/Components/CLAUDE.md](../CLAUDE.md): Parent folder documentation
- [AccountLogo/CLAUDE.md](../AccountLogo/CLAUDE.md): Logo component
- [TenantChange/CLAUDE.md](../TenantChange/CLAUDE.md): Tenant switcher component