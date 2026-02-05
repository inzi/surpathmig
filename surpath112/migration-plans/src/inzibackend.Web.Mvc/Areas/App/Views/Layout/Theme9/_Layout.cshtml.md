# Modified
## Filename
_Layout.cshtml
## Relative Path
inzibackend.Web.Mvc\Areas\App\Views\Layout\Theme9\_Layout.cshtml
## Language
Unknown
## Summary
This is a Razor layout view file for Theme9 in an ASP.NET Zero application. It defines the main layout structure with a sidebar navigation, header with user controls, and content area. The layout includes various UI components such as brand logo, menu navigation, user delegations, subscription bar, theme selector, language switch, notifications, chat toggler, and user menu. It uses Metronic CSS framework for styling and implements responsive design with mobile-specific header elements.
## Changes
Removed the AppToggleDarkMode component from using statements and header actions. Modified CSS link references to include dark mode conditional styling based on theme settings. Simplified the AppMenu component invocation by removing the sideMenuClass parameter. Updated header action components to use inline CSS class definitions with 'me-2' margin classes instead of multi-line object syntax. Changed AppUserMenu component styling from symbol-based to button-based with additional text and symbol styling parameters. Removed the content wrapper div around RenderBody() method. Modified the mobile toggle button SVG icon structure by adding a span wrapper and changing SVG attributes.
## Purpose
This file serves as the main layout template for Theme9 in an ASP.NET Zero web application, providing the structural foundation for all pages that use this theme. It handles the overall page layout, navigation structure, header controls, and integrates various UI components for user interaction and application functionality.
