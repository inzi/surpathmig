# Modified
## Filename
EmailSettingsChecker.cs
## Relative Path
inzibackend.Core\Net\Emailing\EmailSettingsChecker.cs
## Language
C#
## Summary
The modified file implements an EmailSettingsChecker class with two methods (EmailSettingsValid and EmailSettingsValidAsync) that use asynchronous methods to retrieve setting values. The unmodified version uses synchronous methods.
## Changes
1. Replaced GetSettingValue with GetSettingValueAsync in both EmailSettingsValid and EmailSettingsValidAsync methods.
2. Added [TransientDependency] attribute if present (not explicitly shown but inferred from purpose).
3. Kept all other code unchanged.
## Purpose
To validate email settings as a transient dependency for an ASP.NET Zero application.
