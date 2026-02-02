# Modified
## Filename
ActiveUserDelegationSpecification.cs
## Relative Path
inzibackend.Core\Authorization\Delegation\ActiveUserDelegationSpecification.cs
## Language
C#
## Summary
The modified file defines an ActiveUserDelegationSpecification class that implements Specification<UserDelegation>. It includes properties for source and target user IDs, a constructor assigning these values, and an override method ToExpression() which returns a LINQ query expression checking if the current time falls within specified start and end times.
## Changes
The only change is in the namespace declaration: 'inzibackend' was changed to 'inzb-backend'.
## Purpose
This file defines a specification for delegating user authorization based on active user IDs and their availability during specific time periods.
