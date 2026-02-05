# Modified
## Filename
CustomizableDashboardControllerBase.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Controllers\CustomizableDashboardControllerBase.cs
## Language
C#
## Summary
The modified file introduces additional filtering logic in the GetView method to show only widgets defined in the view and present in the user's dashboard pages.
## Changes
Added Where clause in userDashboardPage.Widgets filtering based on DashboardViewConfiguration. Also added Where clause in dashboardDefinition.Widgets filtering based on userDashboard's pages.
## Purpose
To provide a more personalized and relevant view of widgets for each user.
