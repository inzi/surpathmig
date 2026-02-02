# New
## Filename
PropertyEntryExtensions.cs
## Relative Path
inzibackend.EntityFrameworkCore\EntityHistory\Extensions\PropertyEntryExtensions.cs
## Language
C#
## Summary
The provided C# code defines two static methods in a class that extends existing property entry functionality. The methods GetNewValue and GetOriginalValue check if an entity's state is Deleted or Added, respectively, to determine the appropriate return value (null for deleted/added entities). This likely supports tracking new values and original values of properties over time within an Entity Framework context.
## Changes
New file
## Purpose
Tracking changes in entity history by determining new/original values based on entity state.
