# Common/Images Documentation

## Overview
This folder contains shared image assets used throughout the Web Host application. These images include logos, branding elements, and default user profile pictures that are referenced across multiple views and components.

## Contents

### Files

#### default-profile-picture.png
- **Purpose**: Default placeholder image for user profiles without custom pictures
- **Format**: PNG with transparency support
- **Usage**:
  - Shown when user hasn't uploaded a profile picture
  - Fallback for broken or missing profile images
  - Used in user display components
- **Typical Display**: Small thumbnail size (32x32, 50x50, or similar)

#### logo.svg
- **Purpose**: Primary application logo in SVG format
- **Format**: SVG (Scalable Vector Graphics)
- **Usage**:
  - Main branding in UI header
  - Login page logo
  - Email templates
  - Documentation
- **Advantages**: Scalable without quality loss, small file size

#### app-logo-on-light.svg
- **Purpose**: Logo variant optimized for light backgrounds
- **Format**: SVG
- **Usage**:
  - White or light-colored UI themes
  - Light mode displays
  - Documents with light backgrounds
- **Design**: Darker colors for contrast on light backgrounds

#### app-logo-on-dark.svg
- **Purpose**: Logo variant optimized for dark backgrounds
- **Format**: SVG
- **Usage**:
  - Dark UI themes
  - Dark mode displays
  - Dark-colored headers or footers
- **Design**: Lighter colors for contrast on dark backgrounds

#### app-logo-small.svg
- **Purpose**: Compact version of the logo for space-constrained displays
- **Format**: SVG
- **Usage**:
  - Navigation bars
  - Mobile displays
  - Favicons (if converted)
  - Compact layouts
- **Design**: Simplified or icon-only version of full logo

### Key Components

#### Logo Variants Strategy
- Multiple versions for different contexts
- SVG format ensures quality at any size
- Theme-appropriate variants (light/dark)
- Size-appropriate variants (full/small)

#### Profile Picture Handling
- PNG format for photo-realistic default
- Transparency support for flexible backgrounds
- Generic design suitable for all users

### Dependencies
- No external dependencies
- Static files served directly
- Referenced by HTML `<img>` tags and CSS `background-image`

## Subfolders

### SampleProfilePics
[See SampleProfilePics/CLAUDE.md]
- Contains 10 sample profile pictures for testing and demos
- JPEG format photo-realistic samples
- Used in database seeding and test scenarios

## Architecture Notes

### File Organization
- Common assets centralized for easy maintenance
- Descriptive file names indicate usage
- Consistent naming convention
- Grouped by asset type

### Format Selection
- **SVG**: Logos and vector graphics (scalable, small size)
- **PNG**: Photos and complex images (transparency support)
- **JPEG**: Sample photos (SampleProfilePics subfolder)

### Responsive Design
- SVG logos scale to any size
- Multiple variants for different contexts
- No need for multiple resolution versions

## Business Logic

### Logo Display Logic
1. **Theme Detection**: Check if dark or light theme active
2. **Logo Selection**: Choose appropriate variant
3. **Size Context**: Use full or small version based on available space
4. **Fallback**: Use primary logo.svg if variants not available

### Profile Picture Logic
1. **Check User Picture**: Look for custom uploaded picture
2. **Fallback**: Use default-profile-picture.png if none exists
3. **Error Handling**: Show default if custom picture fails to load
4. **Caching**: Browser caches default picture for performance

### Branding Consistency
- Same logo across all application areas
- Consistent with marketing materials
- Professional appearance maintained
- Easy to update globally

## Usage Across Codebase

### HTML References

#### Logo in Views
```html
<!-- Full logo on light background -->
<img src="~/Common/Images/app-logo-on-light.svg" alt="Application Logo" />

<!-- Small logo in navbar -->
<img src="~/Common/Images/app-logo-small.svg" alt="Logo" class="navbar-logo" />

<!-- Theme-aware logo -->
<img src="~/Common/Images/@(isDarkTheme ? "app-logo-on-dark.svg" : "app-logo-on-light.svg")" alt="Logo" />
```

#### Profile Pictures
```html
<!-- User profile picture with fallback -->
<img src="@(user.ProfilePictureUrl ?? "~/Common/Images/default-profile-picture.png")"
     alt="@user.Name"
     onerror="this.src='/Common/Images/default-profile-picture.png'" />
```

### CSS References
```css
.login-header {
    background-image: url('/Common/Images/app-logo-on-dark.svg');
    background-size: contain;
    background-repeat: no-repeat;
}

.user-avatar {
    background-image: url('/Common/Images/default-profile-picture.png');
}
```

### Email Templates
```html
<img src="https://yourdomain.com/Common/Images/logo.svg"
     alt="Company Logo"
     style="width: 150px;" />
```

### Referenced By
- **Login.cshtml**: Logo display
- **Index.cshtml**: Branding elements
- **Email templates**: Header logos
- **Profile components**: Default profile pictures
- **Navigation bars**: Small logo variants
- **Error pages**: Branding consistency

## Development Notes

### Updating Logo

#### Replacing Logo Files
1. Create new SVG files with same dimensions
2. Test on light and dark backgrounds
3. Ensure scalability at various sizes
4. Replace files with same names (no code changes needed)
5. Clear browser cache for testing

#### Creating Logo Variants
```svg
<!-- app-logo-on-light.svg - darker colors -->
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 200 50">
    <path fill="#333333" d="..." />
</svg>

<!-- app-logo-on-dark.svg - lighter colors -->
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 200 50">
    <path fill="#FFFFFF" d="..." />
</svg>
```

### Profile Picture Guidelines
- **Default Picture**:
  - Neutral, professional appearance
  - Appropriate for all users
  - Clear at small sizes
  - Works on any background color

- **File Specifications**:
  - Format: PNG with transparency
  - Size: 200x200 pixels (or similar square)
  - Color depth: 24-bit with alpha channel
  - File size: < 50KB optimized

### Image Optimization

#### SVG Optimization
- Remove unnecessary metadata
- Simplify paths where possible
- Use SVGO tool for optimization
- Test rendering in browsers

#### PNG Optimization
- Use tools like TinyPNG or ImageOptim
- Balance quality vs file size
- Maintain transparency
- Test appearance at target sizes

### Testing Images
1. **Logo Display**:
   - Test on light backgrounds
   - Test on dark backgrounds
   - Verify at various sizes
   - Check in different browsers

2. **Profile Pictures**:
   - Test default picture display
   - Verify fallback behavior
   - Check in circular crops
   - Test at thumbnail sizes

3. **Performance**:
   - Verify file sizes acceptable
   - Check browser caching
   - Test load times
   - Optimize if needed

### Best Practices
- Use SVG for logos and vector graphics
- Provide theme-appropriate variants
- Maintain consistent branding
- Optimize file sizes
- Use descriptive file names
- Document color values for brand consistency
- Keep backup of original design files
- Version control logo changes

### Accessibility Considerations
- Always provide meaningful alt text
- Ensure logos have sufficient contrast
- Don't rely solely on logos to convey information
- Provide text alternatives where appropriate
- Test with screen readers
- Ensure logos don't interfere with text readability

### Common Customizations
- Update logo with new branding
- Add additional size variants
- Create seasonal or promotional logos
- Add favicon versions
- Generate PWA icons from logo
- Create social media preview images