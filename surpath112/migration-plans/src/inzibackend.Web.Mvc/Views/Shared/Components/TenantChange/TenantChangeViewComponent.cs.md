# Modified
## Filename
TenantChangeViewComponent.cs
## Relative Path
inzibackend.Web.Mvc\Views\Shared\Components\TenantChange\TenantChangeViewComponent.cs
## Language
C#
## Summary
The modified file defines a TenantChangeViewComponent class that implements IViewComponent. It uses the Web.Session using statement to access session cache and maps login information from the session cache into a TenantChangeViewModel to render.
## Changes
The only change is in the using directive, replacing 'using inzibackend;' with 'using inzibackend.Web.Session;'.
## Purpose
The file provides functionality for handling tenant change views by accessing session data and converting it into a view model.
