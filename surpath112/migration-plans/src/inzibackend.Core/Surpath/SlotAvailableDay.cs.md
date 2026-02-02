# New
## Filename
SlotAvailableDay.cs
## Relative Path
inzibackend.Core\Surpath\SlotAvailableDay.cs
## Language
C#
## Summary
New class SlotAvailableDay implemented in namespace inzibackend.Surpath. The class implements FullAuditedEntity and IMayHaveTenant interfaces. It has properties: TenantId (nullable int), DayOfWeek (DayOfWeek type), RotationSlotRotationSlotFK (ForeignKey to RotationSlot table), and IsSelected (boolean defaulting to false). The class is annotated with [Table("SlotAvailableDays")] indicating it's part of a database entity framework.
## Changes
New file
## Purpose
Entity class for mapping to a database table in an ASP.NET Zero application
