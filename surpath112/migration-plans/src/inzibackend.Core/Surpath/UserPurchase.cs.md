# New
## Filename
UserPurchase.cs
## Relative Path
inzibackend.Core\Surpath\UserPurchase.cs
## Language
C#
## Summary
The provided C# file defines a UserPurchase entity within an ASP.NET Zero project. It includes various properties such as Name, Description, Status, OriginalPrice, AdjustedPrice, DiscountAmount, AmountPaid, PaymentPeriodType, PurchaseDate, ExpirationDate, IsRecurring, Notes, and MetaData. The BalanceDue is a calculated property derived from AdjustedPrice minus AmountPaid. The entity uses foreign key relationships to link with other entities like UserId, SurpathServiceId, CohortId, etc., and includes auditing features for tracking changes.
## Changes
New file
## Purpose
The file defines an Entity class in the domain model that represents user purchases, including their attributes and relationships.
