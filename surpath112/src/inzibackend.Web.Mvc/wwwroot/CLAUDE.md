# wwwroot Documentation

## Overview
The wwwroot folder contains all static files and client-side resources for the MVC web application. This includes JavaScript/TypeScript files, CSS stylesheets, images, fonts, and third-party libraries. The folder follows ASP.NET Core conventions where static files are served directly to clients.

## Contents

### Core Folders

#### view-resources/
- **Purpose**: JavaScript/TypeScript files corresponding to Razor views
- **Structure**: Mirrors the Views folder structure
- **Subfolders**:
  - `Areas/App/Views/`: JavaScript for App area views
  - `Views/`: JavaScript for root-level views
- **Key Features**:
  - Modal management scripts
  - DataTable configurations
  - AJAX operations
  - Form validations
  - Event handlers

#### Common/
- **Purpose**: Shared JavaScript utilities and helpers
- **Key Components**:
  - `Scripts/`: Common JavaScript utilities
    - `appUserNotificationHelper.js`: Notification handling
    - `datatables/`: DataTable extensions
    - `helpers/`: Utility functions
    - `ModalManager.js`: Modal dialog management
  - `Images/`: Shared images and icons
  - `Styles/`: Common CSS files
- **Features**: Reusable client-side components

#### metronic/
- **Purpose**: Metronic theme assets (commercial UI theme)
- **Contents**:
  - Theme CSS files
  - JavaScript components
  - Font icons
  - Theme plugins
- **Version**: Integrated with ASP.NET Zero

#### lib/
- **Purpose**: Third-party JavaScript libraries (managed by npm/yarn)
- **Key Libraries**:
  - jQuery and plugins
  - Bootstrap
  - Select2
  - SweetAlert2
  - Moment.js
  - SignalR client
  - DataTables
- **Management**: Dependencies defined in package.json

#### dist/
- **Purpose**: Compiled and bundled assets
- **Contents**:
  - Minified JavaScript bundles
  - Compiled CSS files
  - Source maps
- **Generation**: Created by gulp build process

#### assets/
- **Purpose**: Application-specific static assets
- **Contents**:
  - Custom images
  - Application icons
  - Media files
- **Organization**: Structured by feature/component

#### fonts/
- **Purpose**: Web fonts and icon fonts
- **Contents**:
  - Custom fonts
  - Icon font files
  - Font CSS definitions
- **Types**: WOFF, WOFF2, TTF, EOT formats

#### Plugins/
- **Purpose**: Additional jQuery plugins and components
- **Contents**:
  - Custom plugins
  - Extended functionality
  - Widget components
- **Integration**: Used by specific features

#### swagger/
- **Purpose**: Swagger UI for API documentation
- **Contents**:
  - Swagger UI assets
  - API documentation interface
- **Usage**: API testing and documentation

### Configuration Files

#### favicon.ico
- Application favicon
- Displayed in browser tabs

## Architecture Notes

### File Organization

#### View-Specific JavaScript
Each view can have corresponding JavaScript files:
- Pattern: `/view-resources/{Area}/Views/{Controller}/{Action}.js`
- Example: `/view-resources/Areas/App/Views/Compliance/_CreateNewRecordModal.js`
- Minified versions: `.min.js` files for production

#### Bundling Strategy
- Configured in `bundles.json`
- Processed by `gulpfile.js`
- Separate bundles for:
  - Layout/common scripts
  - View-specific scripts
  - Vendor libraries

#### Naming Conventions
- Modal scripts prefixed with underscore: `_ModalName.js`
- Minified files: `.min.js`
- Bundle outputs in `_Bundles` folders

## JavaScript Patterns

### Module Pattern
```javascript
var app = app || {};
(function ($) {
    app.ModuleName = function () {
        // Module implementation
    };
})(jQuery);
```

### Modal Management
- ModalManager for dialog handling
- Consistent open/close patterns
- AJAX loading of modal content
- Form submission handling

### DataTable Configuration
- Server-side processing
- Custom column renderers
- Action buttons
- Filtering and sorting

### AJAX Patterns
- ABP service proxies
- Consistent error handling
- Loading indicators
- Success notifications

## Key Components

### Notification System
- Real-time notifications via SignalR
- Toast notifications
- Custom notification formatters
- Navigation from notifications

### Form Handling
- jQuery validation
- Unobtrusive validation
- AJAX form submission
- File upload support

### UI Enhancements
- Select2 for dropdowns
- Date/time pickers
- File upload widgets
- Rich text editors

## Gulp Build Process

### Tasks
- **JavaScript**: Minification, bundling
- **CSS**: Compilation, minification
- **Fonts**: Copying to dist
- **Images**: Optimization

### Bundle Configuration
Defined in `bundles.json`:
- Input files
- Output locations
- Minification settings
- Source map generation

## Dependencies

### Package Management
- npm/yarn for package management
- package.json defines dependencies
- node_modules contains packages

### Key Libraries
- **jQuery**: DOM manipulation
- **Bootstrap**: UI framework
- **DataTables**: Grid component
- **Select2**: Enhanced dropdowns
- **SignalR**: Real-time communication
- **Moment.js**: Date/time handling

## Usage Patterns

### View Integration
Views reference scripts via:
```html
@section Scripts {
    <script src="~/view-resources/Areas/App/Views/Controller/Action.js"></script>
}
```

### Bundle References
Layout files include bundles:
```html
<script src="~/view-resources/Areas/App/Views/_Bundles/app-layout-libs.js"></script>
```

### Dynamic Loading
Modals and components loaded via:
```javascript
abp.ajax({
    url: '/App/Controller/ModalAction',
    type: 'POST',
    data: JSON.stringify(data)
});
```

## Performance Optimization

### Minification
- All JavaScript minified for production
- CSS minified and combined
- Source maps for debugging

### Caching
- Static files cached by browser
- Version query strings for cache busting
- CDN support for common libraries

### Lazy Loading
- Modals loaded on demand
- View-specific scripts loaded per page
- Deferred loading for non-critical resources

## Security Considerations

### CSRF Protection
- Anti-forgery tokens in AJAX requests
- Validation on server-side

### XSS Prevention
- Input sanitization
- Output encoding
- Content Security Policy headers

### File Upload Security
- File type validation
- Size limits
- Virus scanning integration

## Related Components
- `/Areas/App/Views/`: Corresponding Razor views
- `/Controllers/`: Server-side controllers
- `bundles.json`: Bundle configuration
- `gulpfile.js`: Build automation
- `package.json`: npm dependencies