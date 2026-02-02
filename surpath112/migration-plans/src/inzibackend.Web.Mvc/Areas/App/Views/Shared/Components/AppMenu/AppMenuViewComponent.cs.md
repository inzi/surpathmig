# Modified
## Filename
AppMenuViewComponent.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Views\Shared\Components\AppMenu\AppMenuViewComponent.cs
## Language
C#
## Summary
The modified file introduces two new configuration properties (sideMenuClass and topMenuClass) with default values in the AppMenuViewComponent class. These properties enhance flexibility by allowing consistent menu styling across different environments.
## Changes
Added 'sideMenuClass' and 'topMenuClass' parameters to the constructor of AppMenuViewComponent, initializing them with default CSS classes. Updated the MenuViewModel instantiation in both the constructor and InvokeAsync method to include these new properties.
## Purpose
The file is part of an ASP.NET Zero solution, likely used for configuring navigation menu styling in web applications.
