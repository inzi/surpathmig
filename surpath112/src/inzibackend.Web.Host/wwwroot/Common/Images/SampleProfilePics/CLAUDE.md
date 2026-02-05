# SampleProfilePics Documentation

## Overview
This folder contains sample profile pictures used for demonstration, testing, and placeholder purposes in the inzibackend application. These images provide realistic profile pictures for seed data, demo accounts, and testing scenarios.

## Contents

### Files
- **sample-profile-01.jpg through sample-profile-10.jpg**: 10 sample profile pictures in JPEG format
  - Generic, professional-looking profile images
  - Suitable for demo accounts and testing
  - Used when seeding database with test users
  - Placeholder images before users upload their own

### Key Components

#### Image Specifications
- **Format**: JPEG (.jpg)
- **Purpose**: Sample/placeholder profile pictures
- **Count**: 10 different images
- **Usage**: Testing, demos, seed data

### Dependencies
- None - static image files
- Referenced by seeding code
- Fallback images for missing profiles

## Architecture Notes

### Naming Convention
- Sequential numbering: 01 through 10
- Consistent prefix: "sample-profile-"
- Zero-padded numbers for proper sorting

### Storage Strategy
- Stored in wwwroot for direct web access
- Publicly accessible via URL
- No authentication required
- Separate from actual user uploads

### File Organization
- Grouped in dedicated subfolder
- Easy to maintain and update
- Clear separation from production uploads

## Business Logic

### Use Cases
1. **Database Seeding**: Initial test data includes sample profiles
2. **Demo Accounts**: New demo tenants get varied profile pictures
3. **Testing**: UI testing with realistic profile images
4. **Placeholders**: Default images before user uploads

### Image Selection
- Random selection from 10 available images
- Provides variety in test data
- Avoids monotonous placeholder appearance

## Usage Across Codebase

### Referenced By

#### Seeding Code
- `Migrations/Seed/Tenants/DefaultTenantBuilder.cs` or similar
- Randomly assigns sample pictures to test users
- Copies to user profile locations

#### Profile Services
- May reference as fallback images
- Used in user creation utilities
- Testing helper methods

### URL Patterns
- Direct access: `/Common/Images/SampleProfilePics/sample-profile-01.jpg`
- Used in HTML: `<img src="~/Common/Images/SampleProfilePics/sample-profile-01.jpg">`
- API references for profile picture URLs

### Not Used For
- Production user profiles (stored elsewhere)
- Sensitive data
- Dynamic content

## Development Notes

### Adding New Samples
1. Add JPEG files with consistent naming
2. Follow existing pattern: sample-profile-{number}.jpg
3. Ensure appropriate image quality
4. Use professional, neutral images
5. Update seeding logic if needed

### Image Guidelines
- Professional appearance
- Neutral backgrounds
- Various demographics for diversity
- Appropriate resolution for web display
- Optimized file size

### Maintenance
- Periodically review for appropriateness
- Replace if design standards change
- Ensure all 10 images are present
- Check file sizes for optimization