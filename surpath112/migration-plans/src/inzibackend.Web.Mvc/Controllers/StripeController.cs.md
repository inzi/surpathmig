# Modified
## Filename
StripeController.cs
## Relative Path
inzibackend.Web.Mvc\Controllers\StripeController.cs
## Language
C#
## Summary
The modified file introduces a new parameter (TenantId) in the Purchase method of the StripeController class. It also includes an additional property (TenantId) in the CreatePaymentSession input when processing payments.
## Changes
Added 'int? tenantId' as a parameter to the 'Purchase' method and included it in the 'StrippeCreatePaymentSessionInput'.
## Purpose
The file is part of an ASP.NET Zero application, handling Stripe payment processing with dependency injection for services and managing different tenancy contexts.
