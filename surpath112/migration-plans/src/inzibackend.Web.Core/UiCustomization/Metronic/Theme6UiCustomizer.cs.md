# Modified
## Filename
Theme6UiCustomizer.cs
## Relative Path
inzibackend.Web.Core\UiCustomization\Metronic\Theme6UiCustomizer.cs
## Language
C#
## Summary
Both files contain an implementation of Theme6UiCustomizer class that handles UI theme customizations. The modified version includes additional code in GetHostUiManagementSettings method.
## Changes
Added call to ResetDarkModeSettingsAsync(changerUser) in the GetHostUiManagementSettings method which was not present in the unmodified version.
## Purpose
The class provides methods to get and update UI theme settings for different users and contexts, including dark mode configuration.
