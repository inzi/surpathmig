# Recaptcha Documentation

## Overview
This folder contains the implementation for Google reCAPTCHA integration, providing bot protection and spam prevention for forms and API endpoints throughout the application.

## Contents

### Files

#### RecaptchaValidator.cs
- **Purpose**: Validates Google reCAPTCHA responses
- **Key Features**:
  - Validates reCAPTCHA tokens with Google's API
  - Configurable secret key
  - Score-based validation for reCAPTCHA v3
  - IP address verification support
- **Implementation**: IRecaptchaValidator interface

### Key Components
- **Recaptcha Validator**: Server-side token validation
- **Google API Integration**: HTTPS calls to Google
- **Score Threshold**: Configurable bot detection

### Dependencies
- HttpClient (API communication)
- Google reCAPTCHA API
- Configuration system (for secret keys)

## Architecture Notes
- Server-side validation only (secure)
- Async API calls for performance
- Configurable per environment
- Supports multiple reCAPTCHA versions

## Business Logic
- **Validation Flow**:
  1. Client includes reCAPTCHA token in request
  2. Server extracts token from request
  3. Validator calls Google API with token and secret
  4. Google returns success/score
  5. Application allows/denies based on result

## Security Considerations
- Secret key never exposed to client
- HTTPS only for API calls
- IP validation prevents token reuse
- Score threshold prevents sophisticated bots

## Usage Across Codebase
- Login forms (prevent brute force)
- Registration (prevent spam accounts)
- Contact forms (prevent spam)
- API endpoints (rate limiting)
- Password reset (security)