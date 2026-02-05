# Modified
## Filename
ProfitShare.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\view-resources\Areas\App\Views\CustomizableDashboard\Widgets\ProfitShare\ProfitShare.js
## Language
JavaScript
## Summary
The modified file adds a resize event handler that triggers additional data fetching and chart redraw. It maintains similar functionality but enhances responsiveness.
## Changes
Added `this.onResizeCompleted()` which calls `getProfitShare()`, and an extra `getProfitShare()` call during onresize in the modified version.
## Purpose
Dynamic profit share visualization widget with resize handling for improved application responsiveness.
