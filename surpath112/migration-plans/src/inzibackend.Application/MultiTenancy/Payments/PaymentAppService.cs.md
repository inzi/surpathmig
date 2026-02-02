# Modified
## Filename
PaymentAppService.cs
## Relative Path
inzibackend.Application\MultiTenancy\Payments\PaymentAppService.cs
## Language
C#
## Summary
The main change is in the `GetPaymentAsync` method where it now returns a mapped `SubscriptionPaymentDto` instead of just retrieving and returning the payment object.
## Changes
[object Object]
## Purpose
Improve type safety and readability by returning the actual payment data in a structured DTO format.
