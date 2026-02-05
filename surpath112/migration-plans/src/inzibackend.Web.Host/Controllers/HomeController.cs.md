# Modified
## Filename
HomeController.cs
## Relative Path
inzibackend.Web.Host\Controllers\HomeController.cs
## Language
C#
## Summary
The modified HomeController class includes a check using application configuration to determine the home page URL. If development mode is active or the URL is not null/empty, it redirects; otherwise, it links to 'Ui'.
## Changes
Added an additional condition in the Index method that checks the App:HomePageUrl configuration value before redirecting.
## Purpose
The file implements a controller action for handling home page routing based on environment and configuration settings.
