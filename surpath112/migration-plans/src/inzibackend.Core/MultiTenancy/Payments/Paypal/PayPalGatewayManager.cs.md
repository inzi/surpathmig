# Modified
## Filename
PayPalGatewayManager.cs
## Relative Path
inzibackend.Core\MultiTenancy\Payments\Paypal\PayPalGatewayManager.cs
## Language
C#
## Summary
The modified file implements a PayPal Gateway Manager class that handles payment processing across different environments (sandbox and live). The unmodified version has the same functionality but with minor syntax differences, specifically missing colons in switch-case statements.
## Changes
Removed colons after case keywords in switch statement for environment handling.
## Purpose
The file serves as a dependency injection point for PayPal payment processing, managing HTTP client based on configuration.
