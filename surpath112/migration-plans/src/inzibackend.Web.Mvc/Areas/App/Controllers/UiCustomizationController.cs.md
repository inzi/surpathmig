# Modified
## Filename
UiCustomizationController.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Controllers\UiCustomizationController.cs
## Language
C#
## Summary
The modified UiCustomizationController includes an additional 'personal' parameter in its constructor when creating the model view. This allows passing a boolean value to GetUiManagementSettings(). The unmodified version does not include this parameter.
## Changes
Added 'personal = false' parameter to the constructor of UiCustomizationViewModel in the modified file.
## Purpose
The controller handles UI customization permissions and settings within an ASP.NET Zero application using Inzibackend services.
