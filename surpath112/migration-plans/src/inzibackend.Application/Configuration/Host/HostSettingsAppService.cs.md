# Modified
## Filename
HostSettingsAppService.cs
## Relative Path
inzibackend.Application\Configuration\Host\HostSettingsAppService.cs
## Language
C#
## Summary
Updated password complexity requirements by converting enabled/disabled states from strings to boolean-like values.
## Changes
Added method `UpdatePasswordComplexitySettingsFromStringsAsync` and modified conditional update in `UpdateSecuritySettingsAsync`.
## Purpose
Ensure correct conversion of password complexity settings from string representations to actual boolean states.
