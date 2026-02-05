# Modified
## Filename
PaypalController.cs
## Relative Path
inzibackend.Web.Mvc\Controllers\PaypalController.cs
## Language
C#
## Summary
The modified file introduces a redundant assignment in the constructor, adds a tenant ID check, and includes a method to set a tenant cookie. These changes are aimed at enhancing payment processing functionality by allowing per-tenant operations.
## Changes
1. Removed redundant _payPalConfiguration assignment in the constructor.
2. Added check for tenantId parameter and SetTenantIdCookie() call before proceeding with purchase logic.
3. Modified the Purchase method to accept only paymentId instead of (paymentId, int? tenantId).
4. The GetSuccessUrlAsync and GetErrorUrlAsync methods remain unchanged.
## Purpose
The file is part of an ASP.NET Zero solution handling payment processing operations such as initiating purchases, confirming payments, and generating success/error URLs with optional parameters for tenant-specific routing.
