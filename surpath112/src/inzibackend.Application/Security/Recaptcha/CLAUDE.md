# reCAPTCHA Validation Documentation

## Overview
Service abstraction for Google reCAPTCHA validation to prevent automated bot attacks on forms. Provides interface-based abstraction allowing for production validation or null/disabled validation for development/testing.

## Contents

### Files

#### IRecaptchaValidator.cs
- **Purpose**: Interface defining reCAPTCHA validation contract
- **Key Method**:
  - `ValidateAsync(string captchaResponse)`: Validates reCAPTCHA token from client

#### NullRecaptchaValidator.cs
- **Purpose**: No-op implementation that always returns valid
- **Use Case**: Development, testing, or when reCAPTCHA is disabled
- **Key Feature**: Always returns success without calling Google API
- **When Used**:
  - Local development environments
  - Automated testing
  - reCAPTCHA disabled in configuration
  - Integration/staging environments without reCAPTCHA keys

### Key Components

**Validation Flow:**
1. Client submits form with reCAPTCHA response token
2. Server receives token
3. Validator sends token to Google reCAPTCHA API
4. Google validates and returns score/success
5. Server accepts or rejects based on validation result

**Implementation Strategy:**
- Interface allows switching between implementations
- Production: Real validator calling Google API
- Development: Null validator (always succeeds)
- Configuration-driven selection

### Dependencies
- **External**:
  - Google reCAPTCHA API (production implementation)
  - HTTP client for API calls
- **Internal**:
  - Configuration settings for API keys
  - Logging for validation failures

## Architecture Notes
- **Pattern**: Strategy pattern with interface abstraction
- **Configuration**: API keys stored in configuration
- **Environment-Aware**: Different implementations for dev/prod
- **Dependency Injection**: Implementation selected at startup

## Business Logic

### Production Validation (Real Implementation)
1. Receive reCAPTCHA response token from client
2. Send POST request to Google reCAPTCHA API with:
   - Secret key (server-side)
   - Response token (from client)
   - User's IP address (optional)
3. Parse API response
4. Check success flag and score (v3) or success flag (v2)
5. Return validation result

### Null Validation (Development)
- Immediately returns success
- No API calls made
- Logs that null validator is being used
- Allows testing without reCAPTCHA configuration

### Score Thresholds (v3)
- reCAPTCHA v3 returns score from 0.0 to 1.0
- Threshold typically 0.5 (configurable)
- Scores above threshold = human
- Scores below threshold = likely bot

## Usage Across Codebase

### Protected Forms
- User registration
- Login page
- Password reset requests
- Contact forms
- Comment submissions
- Any public-facing form susceptible to bots

### Typical Usage
```csharp
[HttpPost]
public async Task<IActionResult> Register(RegisterInput input)
{
    var isValid = await _recaptchaValidator
        .ValidateAsync(input.CaptchaResponse);

    if (!isValid)
    {
        throw new UserFriendlyException("reCAPTCHA validation failed");
    }

    // Continue with registration...
}
```

### Configuration Selection
```csharp
// In Startup.cs or Module
if (configuration.GetValue<bool>("Recaptcha:IsEnabled"))
{
    services.AddTransient<IRecaptchaValidator, RecaptchaValidator>();
}
else
{
    services.AddTransient<IRecaptchaValidator, NullRecaptchaValidator>();
}
```

## Security Considerations
- **Secret Key Protection**: Never expose secret key to client
- **HTTPS Only**: Always use HTTPS for reCAPTCHA
- **Token Single-Use**: Validate tokens only once
- **Timeout**: reCAPTCHA tokens expire after a few minutes
- **Backup Strategy**: Have fallback if Google API is down

## Configuration
### Required Settings
```json
{
  "Recaptcha": {
    "IsEnabled": true,
    "SiteKey": "your-site-key",
    "SecretKey": "your-secret-key",
    "Version": "v3",
    "ScoreThreshold": 0.5
  }
}
```

## Testing Considerations
- Use null validator for automated tests
- Use test keys provided by Google for manual testing
- Mock the validator interface for unit tests
- Test both success and failure scenarios
- Verify UI shows reCAPTCHA widget correctly

## Common Issues
- **API Key Mismatch**: Ensure site key matches secret key
- **Domain Whitelist**: Configure allowed domains in Google console
- **CORS Issues**: Ensure proper CORS configuration
- **Token Expiration**: Implement token refresh for long forms
- **Rate Limiting**: Handle Google API rate limits

## Best Practices
- **Enable in Production**: Always use real validation in production
- **Log Failures**: Log reCAPTCHA validation failures for monitoring
- **User Experience**: Provide clear error messages
- **Fallback**: Have manual review process if reCAPTCHA fails
- **Monitor Success Rate**: Track validation success/failure rates
- **Update Regularly**: Keep reCAPTCHA library/approach current

## Extension Points
- Add multiple reCAPTCHA providers (hCaptcha, etc.)
- Implement custom scoring logic
- Add honeypot fields as additional protection
- Rate limit form submissions per IP
- Implement progressive challenge (harder CAPTCHA for suspicious activity)
- Add analytics on bot detection rates