# Modified
## Filename
SubscribableEdition.cs
## Relative Path
inzibackend.Core\Editions\SubscribableEdition.cs
## Language
C#
## Summary
The modified file extends the Edition class with subscription features. It adds properties for pricing periods (DailyPrice, WeeklyPrice, MonthlyPrice, AnnualPrice) and a method GetPaymentAmount which retrieves payment amounts based on the specified period.
## Changes
Added [NotMapped] public bool IsFree; line before the property declaration. Changed the GetPaymentAmount method to use GetPaymentAmountOrNull instead of directly referencing it.
## Purpose
The file is part of an ASP.NET Zero solution, providing subscription management functionality including payment calculation and multi-tenant configuration.
