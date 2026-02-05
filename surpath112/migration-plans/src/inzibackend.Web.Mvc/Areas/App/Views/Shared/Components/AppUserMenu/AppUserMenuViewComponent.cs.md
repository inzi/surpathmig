# Modified
## Filename
AppUserMenuViewComponent.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Views\Shared\Components\AppUserMenu\AppUserMenuViewComponent.cs
## Language
C#
## Summary
The modified file introduces a new method signature that allows passing a profile image CSS class. This enables custom profile image styling in the UI.
## Changes
Added two new parameters: 'profileImageCssClass' with default value of empty string, and 'renderOnlyIcon' was already present but now there's an additional 'ProfileImageCssClass' property in the UserMenuViewModel to accept the new parameter.
## Purpose
The changes enhance UI customization by allowing profile image CSS class configuration.
