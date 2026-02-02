# Modified
## Filename
StripePaymentGatewayConfiguration.cs
## Relative Path
inzibackend.Core\MultiTenancy\Payments\Stripe\StripePaymentGatewayConfiguration.cs
## Language
C#
## Summary
The modified file is a configuration class for Stripe payment gateways in an ASP.NET Zero application. It sets up subscription types, webhook secrets, and other payment configurations.
## Changes
Added using Microsoft.Extensions.Configuration; and corrected _appConfiguration["Payment:Stripe:WebhookSecret"] to _appConfiguration['Payment:Stripe:WebhookSecret'].
## Purpose
The class configures the Stripe payment gateway settings, including base URLs, keys, webhook secrets, and active status.
