# Common Documentation

## Overview
The Common folder contains shared static assets that are used across multiple areas of the Web Host application. This includes images, logos, and potentially other reusable resources that maintain consistency throughout the application.

## Contents

### Files
No files exist directly in the Common folder - all assets are organized in subfolders.

### Key Components

#### Shared Assets Strategy
- Centralized location for reusable resources
- Consistent branding across application
- Easy maintenance and updates
- Clear organization by asset type

### Dependencies
- Served via ASP.NET Core Static Files middleware
- No processing or transformation required
- Direct browser access via URL paths

## Subfolders

### Images
[See Images/CLAUDE.md]
- Application logos in multiple variants (SVG format)
- Default profile picture (PNG format)
- Theme-specific logo versions (light/dark backgrounds)
- Size-specific logo versions (full/small)
- **Subfolder**: SampleProfilePics with 10 sample profile images

## Architecture Notes

### File Organization
```
Common/
├── Images/
│   ├── logo.svg (primary logo)
│   ├── app-logo-on-light.svg (light background variant)
│   ├── app-logo-on-dark.svg (dark background variant)
│   ├── app-logo-small.svg (compact version)
│   ├── default-profile-picture.png (user profile fallback)
│   └── SampleProfilePics/ (10 sample images)
```

### Naming Conventions
- Lowercase with hyphens (kebab-case)
- Descriptive names indicating purpose
- Consistent prefixes (app-logo-*, sample-profile-*)
- File extensions indicate format

### Access Pattern
- Static files served from `/Common/*` URL path
- No authentication required for public assets
- Browser caching enabled
- CDN-friendly structure

## Business Logic

### Asset Management
1. **Logos**: Multiple variants for different contexts
2. **Profile Pictures**: Default fallback for users
3. **Samples**: Test data and demos
4. **Branding**: Consistent visual identity

### Common Usage Patterns
- **Login Pages**: Display branded logo
- **Navigation**: Show compact logo in header
- **Profile Display**: Fallback to default picture
- **Email Templates**: Include logo in communications
- **Error Pages**: Maintain branding consistency

## Usage Across Codebase

### Static File Configuration

#### Startup.cs
```csharp
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 1 year
        ctx.Context.Response.Headers.Append(
            "Cache-Control", "public,max-age=31536000");
    }
});
```

### View References
```cshtml
<!-- Logo in layout -->
<img src="~/Common/Images/logo.svg" alt="@L("AppName")" />

<!-- Profile picture with fallback -->
<img src="@(user.ProfilePictureUrl ?? "~/Common/Images/default-profile-picture.png")"
     alt="@user.Name"
     onerror="this.src='/Common/Images/default-profile-picture.png'" />
```

### CSS References
```css
.login-page {
    background-image: url('/Common/Images/app-logo-on-dark.svg');
}

.navbar-brand {
    content: url('/Common/Images/app-logo-small.svg');
}
```

### API Responses
```csharp
public class TenantDto
{
    public string LogoUrl { get; set; } // May reference /Common/Images/logo.svg
}
```

### Referenced By
- **Razor Views**: All pages needing logos or default pictures
- **CSS Stylesheets**: Background images and branding
- **Email Templates**: Logo in email headers
- **API Responses**: URLs to public assets
- **Documentation**: Screenshots and branding elements

## Development Notes

### Adding New Assets

#### To Add Common Images
1. Determine appropriate subfolder (Images, Fonts, Icons, etc.)
2. Follow existing naming conventions
3. Optimize file size before adding
4. Update documentation
5. Clear browser cache for testing

#### Creating New Subfolders
```
Common/
├── Images/ (existing)
├── Fonts/ (if needed)
├── Icons/ (if needed)
├── Documents/ (if needed)
└── Styles/ (if needed for shared CSS)
```

### Asset Optimization

#### Image Optimization
- **SVG**: Use SVGO to remove unnecessary data
- **PNG**: Use TinyPNG or ImageOptim for compression
- **JPEG**: Quality 80-85 usually sufficient
- Target: < 100KB per image for fast loading

#### Performance Considerations
- Enable browser caching (long duration)
- Consider CDN for high-traffic sites
- Use appropriate image formats
- Lazy load images where appropriate
- Provide appropriate dimensions in HTML

### Best Practices
- Keep common assets truly common (not feature-specific)
- Use appropriate formats (SVG for logos, PNG for transparency)
- Maintain consistent naming conventions
- Document purpose of each asset
- Version control all assets
- Optimize for web before committing
- Provide fallbacks for missing assets
- Use relative paths in code

### Security Considerations
- Common folder is publicly accessible
- Never store sensitive information
- Don't include internal documentation
- Sanitize uploaded files before placing here
- Consider rate limiting for larger files
- Monitor for unauthorized file additions

### Accessibility Guidelines
- Always provide alt text for images
- Ensure logos have sufficient contrast
- Don't convey information solely through images
- Provide text alternatives
- Test with screen readers
- Ensure keyboard navigation works with image-based elements

### Common Customizations
- Add custom fonts subfolder
- Include favicon variants
- Add social media preview images
- Include PWA manifest icons
- Add print-specific assets
- Include themed variants for seasonal branding

### Maintenance Tasks
- [ ] Review and remove unused assets quarterly
- [ ] Optimize file sizes periodically
- [ ] Update branding assets when changed
- [ ] Verify all references to removed assets
- [ ] Document new asset additions
- [ ] Test cross-browser compatibility
- [ ] Verify mobile display