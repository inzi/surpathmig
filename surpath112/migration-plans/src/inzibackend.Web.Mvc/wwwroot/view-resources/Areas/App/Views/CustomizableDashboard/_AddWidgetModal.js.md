# Modified
## Filename
_AddWidgetModal.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\view-resources\Areas\App\Views\CustomizableDashboard\_AddWidgetModal.js
## Language
JavaScript
## Summary
The modified file implements a modal widget addition functionality with configuration options (widgetId, dashboardName, pageId) and triggers notifications upon saving. It uses dependency injection through abp.services.app.dashboardCustomization.
## Changes
Added .always() to set busy state to false after saving; added .done() handler for post-save actions including notification and event triggering.
## Purpose
Configuration of modal widget addition functionality with post-saving triggers.
