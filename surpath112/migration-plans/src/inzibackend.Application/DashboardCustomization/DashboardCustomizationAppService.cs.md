# Modified
## Filename
DashboardCustomizationAppService.cs
## Relative Path
inzibackend.Application\DashboardCustomization\DashboardCustomizationAppService.cs
## Language
C#
## Summary
Adjusted the condition to only prevent adding a widget multiple times when it's not allowed.
## Changes
Modified the if-statement in AddWidget method to check if the widget allows multiple instances before preventing duplicate additions.
## Purpose
Prevent unnecessary warning when adding an allowed multiple instance widget.
