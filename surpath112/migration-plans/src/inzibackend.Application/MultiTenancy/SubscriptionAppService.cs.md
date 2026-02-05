# Modified
## Filename
SubscriptionAppService.cs
## Relative Path
inzibackend.Application\MultiTenancy\SubscriptionAppService.cs
## Language
C#
## Summary
The modified file adds functionality to reset the subscription end date when enabling recurring payments.
## Changes
Added line: tenant.SubscriptionEndDateUtc = null; in the EnableRecurringPayments method.
## Purpose
The class implements methods for managing recurring payment subscriptions through an event bus, resetting the end date upon enabling.
