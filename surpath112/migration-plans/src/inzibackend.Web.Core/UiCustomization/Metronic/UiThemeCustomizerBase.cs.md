# Modified
## Filename
UiThemeCustomizerBase.cs
## Relative Path
inzibackend.Web.Core\UiCustomization\Metronic\UiThemeCustomizerBase.cs
## Language
C#
## Summary
The modified file introduces generic method overloads for retrieving and setting application-wide, tenant-specific, and user-specific settings. These methods are used to handle different data types seamlessly.
## Changes
Added generic method overloads for GetSettingValueAsync, GetSettingValueForApplicationAsync, and GetSettingValueForTenantAsync. Also added corresponding overload for ChangeSettingForUserAsync.
## Purpose
The class manages configuration settings related to user, application, and tenant-specific properties, including theme management and dark mode configurations.
