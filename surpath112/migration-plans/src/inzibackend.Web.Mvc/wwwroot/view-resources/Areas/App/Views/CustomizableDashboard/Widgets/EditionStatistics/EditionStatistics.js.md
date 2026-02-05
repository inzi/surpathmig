# Modified
## Filename
EditionStatistics.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\view-resources\Areas\App\Views\CustomizableDashboard\Widgets\EditionStatistics\EditionStatistics.js
## Language
JavaScript
## Summary
The modified code implements a widget that fetches edition statistics and displays them with a pie chart. It initializes using _widgetBase.runDelayed, checks for data before rendering, and updates charts on date changes.
## Changes
Replaced _widgetManager with _widgetBase in delayed method calls; added check for result.data; modified event handler to use _widgetBase.runDelayed.
## Purpose
Manages widget lifecycle and data fetching efficiently in the host dashboard.
