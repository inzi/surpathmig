# Views/Error Documentation

## Overview
This folder contains Razor views for displaying error pages in the Web Host application. These views provide user-friendly error messages for various HTTP status codes and application exceptions.

## Contents

### Files

#### Error.cshtml
- **Purpose**: Generic error page for unhandled exceptions and application errors
- **Model**: `Abp.Web.Mvc.Models.ErrorViewModel` from ABP Framework
- **Features**:
  - Displays error title and message
  - Shows detailed error information
  - Renders validation errors with member names
  - Handles empty error details gracefully
- **Structure**:
  - Error title (errorMessage or generic "Error")
  - Error subtitle (errorDetails from exception)
  - Error description (validation errors if present)
- **Styling**:
  - Loads Index.css for error page styles
  - Uses `.m-error_container`, `.m-error_title`, `.m-error_subtitle`, `.m-error_description` classes
- **Configuration**:
  - `ViewBag.DisableTenantChange = true`: Prevents tenant switching on error pages
  - `asp-append-version="true"`: Cache-busting for CSS

#### Error403.cshtml
- **Purpose**: Forbidden (403) error page for authorization failures
- **Scenario**: User authenticated but lacks permission for requested resource
- **Message**: Typically "Access Denied" or "Forbidden"
- **User Actions**: Redirect to home, contact admin, or login with different account

#### Error404.cshtml
- **Purpose**: Not Found (404) error page for invalid routes or missing resources
- **Scenario**: Requested URL doesn't match any route or resource not found
- **Message**: "Page Not Found" or "The resource you're looking for doesn't exist"
- **User Actions**: Navigate to home, check URL, or use search

### Key Components

#### Error Display Strategy
1. **Primary Message**: What went wrong (error type)
2. **Details**: Specific error information or exception message
3. **Validation Errors**: Field-specific validation failures
4. **User Guidance**: Navigation options (implied or explicit)

#### ABP Error Model Integration
- **ErrorInfo.Message**: Main error message
- **ErrorInfo.Details**: Detailed explanation
- **ErrorInfo.ValidationErrors**: Collection of field-level errors
  - `Message`: Validation failure description
  - `Members`: Affected fields

### Dependencies
- **Abp.Web.Mvc.Models**: ErrorViewModel
- **Abp.Collections.Extensions**: IsNullOrEmpty extension
- **Abp.Extensions**: String extensions
- **Index.css**: Error page styling from wwwroot/view-resources/Views/Error/

## Architecture Notes

### Error Handling Flow
1. Exception occurs or error status code returned
2. Error middleware intercepts
3. Routes to appropriate error view based on status code
4. Controller prepares ErrorViewModel
5. View renders error information
6. User sees formatted error page

### View Inheritance
- Inherits from `inzibackendRazorPage<ErrorViewModel>`
- Base page provides localization (`L()` method)
- Strongly-typed model binding for error information

### Status Code Handling
- 403 → Error403.cshtml (if exists, otherwise Error.cshtml)
- 404 → Error404.cshtml (if exists, otherwise Error.cshtml)
- 500 → Error.cshtml
- Others → Error.cshtml (generic fallback)

### Security Considerations
- Tenant switching disabled to prevent context manipulation
- Detailed errors only in development environment
- Production: Generic messages to avoid information leakage
- Validation errors sanitized before display

## Business Logic

### Error Message Hierarchy
1. **Specific Error**: Use detailed message if available
2. **Fallback**: Use generic "Error" if details empty
3. **Validation Details**: Show field-level errors when present

### Validation Error Display
- Iterates through `ValidationErrors` collection
- Shows error message with affected members
- Format: `* Message (Field1, Field2)`
- Line break between errors for readability

### User Experience Goals
- Clear communication of what happened
- Avoid technical jargon when possible
- Provide actionable guidance (implied links/buttons)
- Maintain professional appearance
- Don't overwhelm with details

### Error Types Handled
- **Application Errors**: Business logic exceptions
- **Validation Errors**: Input validation failures
- **Authorization Errors**: Permission denied (403)
- **Not Found Errors**: Missing resources (404)
- **Server Errors**: Unhandled exceptions (500)

## Usage Across Codebase

### Error Middleware Configuration

#### Startup.cs
```csharp
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/Error{0}");
}
```

### ErrorController
```csharp
public class ErrorController : inzibackendControllerBase
{
    [Route("Error")]
    public IActionResult Error()
    {
        var exceptionFeature = HttpContext.Features
            .Get<IExceptionHandlerPathFeature>();

        var model = new ErrorViewModel
        {
            ErrorInfo = new ErrorInfo
            {
                Message = exceptionFeature?.Error?.Message ?? "An error occurred",
                Details = exceptionFeature?.Error?.StackTrace
            }
        };

        return View(model);
    }

    [Route("Error/Error403")]
    public IActionResult Error403()
    {
        return View(new ErrorViewModel
        {
            ErrorInfo = new ErrorInfo
            {
                Message = "Forbidden",
                Details = "You don't have permission to access this resource"
            }
        });
    }

    [Route("Error/Error404")]
    public IActionResult Error404()
    {
        return View(new ErrorViewModel
        {
            ErrorInfo = new ErrorInfo
            {
                Message = "Not Found",
                Details = "The page you're looking for doesn't exist"
            }
        });
    }
}
```

### Application Error Handling
```csharp
try
{
    // Business logic
}
catch (UserFriendlyException ex)
{
    throw; // ABP handles and shows in Error view
}
catch (ValidationException ex)
{
    var errorInfo = new ErrorInfo
    {
        Message = "Validation failed",
        ValidationErrors = ex.Errors.Select(e => new ValidationErrorInfo
        {
            Message = e.ErrorMessage,
            Members = new[] { e.PropertyName }
        }).ToList()
    };
    throw new AbpException(errorInfo);
}
```

### View Rendering Flow
1. Error occurs in application code
2. Exception caught by middleware
3. Error information wrapped in ErrorViewModel
4. Appropriate error view selected (generic or status-specific)
5. View renders with error data
6. CSS styles applied for presentation
7. User sees formatted error page

## Development Notes

### Customizing Error Pages

#### Modifying Error View
```cshtml
@model Abp.Web.Mvc.Models.ErrorViewModel
<div class="error-page">
    <div class="error-icon">
        <i class="fa fa-exclamation-triangle"></i>
    </div>
    <h1>@(string.IsNullOrEmpty(Model.ErrorInfo.Message) ? L("Error") : Model.ErrorInfo.Message)</h1>
    <p>@Model.ErrorInfo.Details</p>

    @if (!Model.ErrorInfo.ValidationErrors.IsNullOrEmpty())
    {
        <ul class="validation-errors">
            @foreach (var error in Model.ErrorInfo.ValidationErrors)
            {
                <li>@error.Message</li>
            }
        </ul>
    }

    <a href="/" class="btn btn-primary">Go Home</a>
</div>
```

#### Adding Status Code Pages
1. Create Error{StatusCode}.cshtml (e.g., Error401.cshtml)
2. Add route in ErrorController
3. Configure status code middleware
4. Style consistently with other error pages

#### Environment-Specific Error Display
```csharp
if (env.IsDevelopment())
{
    model.ErrorInfo.Details = exception.StackTrace; // Show stack trace
}
else
{
    model.ErrorInfo.Details = "An error occurred"; // Generic message
}
```

### Testing Error Pages
1. **Development**: Enable detailed errors
2. **Production**: Test generic error messages
3. **403 Errors**: Try accessing forbidden resources
4. **404 Errors**: Navigate to invalid URLs
5. **Validation Errors**: Submit invalid forms
6. **Exception Handling**: Throw test exceptions

### Best Practices
- Keep error messages clear and concise
- Provide navigation back to working pages
- Log all errors for debugging (separate from display)
- Use localization for multi-language support
- Hide sensitive information in production
- Provide error codes for support reference
- Include timestamp for correlation with logs
- Offer contact information for critical errors

### Common Customizations
- Add error codes or reference IDs
- Include support contact information
- Add navigation buttons (Home, Back, Contact Support)
- Show different messages per environment
- Log errors with correlation IDs
- Integrate with error tracking services (Sentry, Application Insights)
- Custom styling per error type

### Accessibility Considerations
- Use semantic HTML (headings, paragraphs, lists)
- Ensure sufficient color contrast
- Provide clear focus indicators
- Support keyboard navigation
- Use ARIA labels where appropriate
- Make error messages screen-reader friendly

### Security Checklist
- [ ] No sensitive data in error messages (production)
- [ ] Stack traces hidden in production
- [ ] Validation errors don't leak system information
- [ ] Error logging separate from display
- [ ] Generic messages for authentication failures
- [ ] Rate limiting on error pages (prevent enumeration)
- [ ] CSRF protection on error pages if interactive