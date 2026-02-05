# Modified
## Filename
_Layout.cshtml
## Relative Path
inzibackend.Web.Mvc\Views\Account\_Layout.cshtml
## Language
Unknown
## Summary
The modified file includes additional abp-src paths in its script tags compared to the unmodified version.
## Changes
Added two new script tags:
1. src="~/AbpServiceProxies/GetAll?v=@(AppTimes.StartupTime.Ticks)"
2. src="~@ScriptPaths.JQuery_Validation_Localization"
## Purpose
The modified file includes additional abp-proxies for dynamic scripts to enhance application functionality and performance.
