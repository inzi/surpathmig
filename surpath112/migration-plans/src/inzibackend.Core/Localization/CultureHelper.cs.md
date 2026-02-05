# Modified
## Filename
CultureHelper.cs
## Relative Path
inzibackend.Core\Localization\CultureHelper.cs
## Language
C#
## Summary
The modified file introduces additional static properties and methods related to culture information handling. Specifically, it adds an `IsRtl` property that checks if a culture uses right-to-left text direction.
## Changes
Added the following line in the modified content:
- public static bool IsRtl => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
## Purpose
The file likely provides utility methods for handling culture-specific information, such as determining text direction and calendar type, which is essential for internationalization within an ASP.NET Zero application.
