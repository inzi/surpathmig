# Email Templates Documentation

## Overview
HTML email template files used for transactional emails throughout the application. Provides consistent branding and formatting for all system-generated emails.

## Contents

### Files

#### default.html
- **Purpose**: Default email template with responsive layout
- **Template Variables**:
  - `{EMAIL_TITLE}`: Main email heading
  - `{EMAIL_SUB_TITLE}`: Secondary heading/subtitle
  - `{EMAIL_BODY}`: Main content body (HTML supported)
  - `{EMAIL_LOGO_URL}`: Path to company/institution logo
  - `{THIS_YEAR}`: Current year for copyright
- **Features**:
  - Responsive design (mobile-friendly)
  - Outlook conditional comments for compatibility
  - Professional branding layout
  - Social media integration (commented out Twitter link)
  - Surpath branding
- **Design**:
  - Header with logo
  - Centered content area (white background)
  - Footer with copyright
  - Color scheme: #4db3a4 (primary), #57697e (text), #eff3f8 (background)

## Architecture Notes

- **Pattern**: Template substitution with placeholder variables
- **Compatibility**: HTML 4.01 Transitional with Outlook support
- **Responsive**: Uses tables for maximum email client compatibility
- **Width**: 680px maximum width, 100% mobile adaptation

## Usage Across Codebase

Used by:
- Email sending service (`Net/Emailing`)
- Notification system
- Account management (password reset, account activation)
- Subscription emails (payment confirmations, expiration warnings)
- User invitations
- System notifications

## Template Integration

The template is loaded and variables are replaced at runtime by the email service:
- Template loaded from disk
- Variables replaced with actual content
- Rendered HTML sent via SMTP

## Customization

### Branding Elements
- Logo URL configurable per tenant
- Color scheme can be customized
- Social media links can be enabled/disabled

### Supported Content
- Plain text in EMAIL_BODY
- HTML markup in EMAIL_BODY (formatted content)
- Multi-line content with proper line-height

## Security Considerations

- Template does not include user-generated content directly
- All variables should be HTML-encoded before insertion
- No JavaScript or active content
- External images referenced (logo)