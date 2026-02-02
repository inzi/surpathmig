# Modified
## Filename
Theme2UiCustomizer.cs
## Relative Path
inzibackend.Web.Core\UiCustomization\Metronic\Theme2UiCustomizer.cs
## Language
C#
## Summary
The modified file introduces direct assignments to specific style strings in the GetUiSettings method. It also includes additional properties in the returned DTOs such as SubHeader.TitleStyle and SubHeader.ContainerStyle with fixed values.
## Changes
1. Added direct assignments for SubHeader.TitleStyle and SubHeader.ContainerStyle in the GetUiSettings method.
2. Removed these direct assignments from the unmodified version, instead using GetSettingValueAsync for all settings including the SubHeader styles which have fixed values.
## Purpose
The file is part of an UiCustomizer class managing UI configurations for a web application, specifically handling theme customizations across different user contexts.
