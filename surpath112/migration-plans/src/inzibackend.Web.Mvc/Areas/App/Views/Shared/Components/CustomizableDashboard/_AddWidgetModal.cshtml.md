# Modified
## Filename
_AddWidgetModal.cshtml
## Relative Path
inzibackend.Web.Mvc\Areas\App\Views\Shared\Components\CustomizableDashboard\_AddWidgetModal.cshtml
## Language
Unknown
## Summary
The modified file includes an additional check using Any() to determine if Model.Widgets exists before rendering widget-related inputs. The unmodified version does not include this check.
## Changes
Added Any() method in the @if condition when checking for Model.Widgets.
## Purpose
Part of an MVC pattern implementation in ASP.NET Zero, defining a modal view for adding widgets to a dashboard.
