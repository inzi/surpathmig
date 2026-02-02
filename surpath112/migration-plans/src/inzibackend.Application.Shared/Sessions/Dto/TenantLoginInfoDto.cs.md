# Modified
## Filename
TenantLoginInfoDto.cs
## Relative Path
inzibackend.Application.Shared\Sessions\Dto\TenantLoginInfoDto.cs
## Language
C#
## Summary
The modified file extends the TenantLoginInfoDto class by adding a HasRecurringSubscription method that checks if the subscription has recurring payments.
## Changes
Added HasRecurringSubscription method which returns true if SubscriptionPaymentType is not Manual.
## Purpose
To provide functionality to determine if a subscription has recurring payment terms.
