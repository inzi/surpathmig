# Modified
## Filename
ScriptPaths.cs
## Relative Path
inzibackend.Web.Mvc\Resources\ScriptPaths.cs
## Language
C#
## Summary
The modified file contains a typo where a string with an embedded quote is incorrectly enclosed in template literals (backticks) instead of double quotes. This could cause compilation errors.
## Changes
In the jQuery Timeago region, the string was incorrectly formatted using backticks instead of double quotes, which can lead to syntax issues if the string contains quotes.
## Purpose
The class manages script paths for different localizations used in JavaScript files (e.g., jQuery validation, time ago, select2) ensuring they are correctly referenced relative to the web root path.
