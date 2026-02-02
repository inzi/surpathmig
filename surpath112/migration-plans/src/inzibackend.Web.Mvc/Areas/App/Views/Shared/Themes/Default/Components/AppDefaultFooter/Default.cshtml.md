# Modified
## Filename
Default.cshtml
## Relative Path
inzibackend.Web.Mvc\Areas\App\Views\Shared\Themes\Default\Components\AppDefaultFooter\Default.cshtml
## Language
Unknown
## Summary
The modified FooterViewModel uses a class derived from GetContainerClass(), indicating container inversion of responsibilities. The unmodified version uses AppVersionHelper.Version directly.
## Changes
The class attribute was changed from using GetContainerClass() to AppVersionHelper.Version, removing the method call and replacing it with a direct value.
## Purpose
Part of the FooterViewModel model in an ASP.NET Zero application, defining footer layout structure with theme-based styling and version information.
