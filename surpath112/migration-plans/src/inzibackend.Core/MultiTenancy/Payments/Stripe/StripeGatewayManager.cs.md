# Modified
## Filename
StripeGatewayManager.cs
## Relative Path
inzibackend.Core\MultiTenancy\Payments\Stripe\StripeGatewayManager.cs
## Language
C#
## Summary
Bug fixed: When changing plans, creates a new payment record instead of reusing an old one.
## Changes
Updated `HandleEvent(TenantEditionChangedEventData)` method to create a new payment record with correct details.
## Purpose
Ensure accurate Stripe payment records when plan changes.
