# Modified
## Filename
Index.cshtml
## Relative Path
inzibackend.Web.Mvc\Areas\App\Views\Welcome\Index.cshtml
## Language
Unknown
## Summary
The modified file retrieves the CurrentWelcome property from ViewData and casts it to WelcomemessageDto, while the unmodified version uses static strings (L("WelcomePage_Title") and L("WelcomePage_Info")).
## Changes
Added line defining CurrentWelcome using ViewData["CurrentWelcome"] as WelcomemessageDto; removed lines with @L("WelcomePage_Title") and @L("WelcomePage_Info") in favor of dynamic property access.
## Purpose
Internationalization/Localization within an ASP.NET application, utilizing Inzibackend for data retrieval and display.
