# Views Documentation

## Overview
The Views folder contains Razor views for the minimal UI provided by the Web Host application. These views primarily support IdentityServer authentication flows, error pages, and OAuth consent screens. The Web Host is mainly an API server but provides these essential UI pages for authentication scenarios.

## Contents

### Files

#### inzibackendRazorPage.cs
- **Purpose**: Base class for all Razor pages in the application
- **Inherits**: AbpRazorPage<TModel> from ABP framework
- **Key Features**:
  - Sets default localization source to inzibackendConsts.LocalizationSourceName
  - Provides localization support for all views
  - Generic type parameter for strongly-typed models

#### _ViewImports.cshtml
- **Purpose**: Global using statements and tag helpers for all Razor views
- **Typical Content**:
  - Common namespace imports
  - Tag helper registrations
  - Shared directives

### Key Components

#### Base Razor Page
- All views inherit from inzibackendRazorPage
- Automatic localization support
- ABP framework integration

### Dependencies
- Abp.AspNetCore.Mvc.Views
- ASP.NET Core Razor runtime

## Subfolders

### Consent
**Purpose**: OAuth/IdentityServer consent screen views

#### Index.cshtml
- Displays OAuth consent form
- Shows requested permissions/scopes
- Allow/Deny buttons for user consent

#### _ScopeListItem.cshtml
- Partial view for individual scope display
- Renders permission details
- Reusable component for scope list

### Error
**Purpose**: Error page templates for various HTTP status codes

#### Error.cshtml
- Generic error page template
- Displays error messages and details
- Fallback for unhandled exceptions

#### Error403.cshtml
- Forbidden (403) error page
- Unauthorized access message
- Links to login or home page

#### Error404.cshtml
- Not Found (404) error page
- Resource not found message
- Navigation options for users

### Ui
**Purpose**: Basic authentication UI views

#### Index.cshtml
- Home page for authenticated users
- Displays user information
- Shows tenant context in multi-tenant mode
- Minimal dashboard or redirect logic

#### Login.cshtml
- Login form view
- Username/email and password fields
- Tenant name field for multi-tenant scenarios
- Remember me checkbox
- Form validation messages

## Architecture Notes

### View Rendering Context
1. Views only rendered for specific scenarios:
   - IdentityServer authentication flows
   - OAuth consent screens
   - Error handling
   - Direct browser access to API host

2. Main application UI is separate:
   - Angular/React app for full UI
   - These views for authentication only

### Localization Pattern
- All views use localized strings
- Localization source from inzibackendConsts
- Multi-language support built-in

### Minimal UI Design
- Basic Bootstrap styling
- Focus on functionality over aesthetics
- Consistent with IdentityServer templates

## Business Logic

### Authentication Flow
1. User accesses protected resource
2. Redirected to Login.cshtml
3. Credentials validated
4. Success → Index.cshtml or return URL
5. Failure → Login with error message

### Consent Flow
1. OAuth client requests permissions
2. User shown Consent/Index.cshtml
3. Scopes displayed via _ScopeListItem
4. User approves/denies
5. Redirect back to client application

### Error Handling
- 403: Authorization failures
- 404: Invalid routes/resources
- Generic: Unexpected errors
- User-friendly messages with navigation options

## Usage Across Codebase

### Controllers Using Views

#### UiController
- Returns Login view for authentication
- Returns Index view for authenticated home
- Model binding with view models

#### ConsentController
- Returns Consent views for OAuth flows
- Processes consent decisions

#### Error Handling
- Global exception handler returns Error views
- Status code pages return specific error views

### View Locations
- Standard MVC conventions followed
- Views/{Controller}/{Action}.cshtml
- Shared views in root Views folder

### Static Resources
Referenced from wwwroot:
- CSS files for styling
- JavaScript for form validation
- Images and icons

### Integration Points
- IdentityServer requires these views
- External OAuth providers redirect here
- API authentication fallback UI
- Error page handling for browser requests