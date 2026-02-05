# Modified
## Filename
_Theme12Settings.cshtml
## Relative Path
inzibackend.Web.Mvc\Areas\App\Views\UiCustomization\_Theme12Settings.cshtml
## Language
Unknown
## Summary
The modified file introduces two new checkboxes in the theme settings: DarkMode and SearchActive. The DarkMode checkbox uses Model/Layout_darkMode instead of Model.Layout.DarkMode and has a different label. The SearchActive checkbox now uses Model.Menu_searchActive instead of Model.Menu.SearchActive and its label is updated to match.
## Changes
Added two new checkboxes for theme customization:
1. DarkMode: Uses Model_LAYOUT_DARKMODE, label 'UiCustomization_DarkMode'
2. SearchActive: Uses Model_MENU_SEARCHACTIVE, label 'UiCustomization_Menu_searchActive'
## Purpose
Configuration file for customizing the layout and menu settings in an ASP.NET application.
