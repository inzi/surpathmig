# Modified
## Filename
WebUrlServiceBase.cs
## Relative Path
inzibackend.Web.Core\Url\WebUrlServiceBase.cs
## Language
C#
## Summary
The modified file contains minor formatting changes compared to the unmodified version, such as additional spaces around operators and return statements.
## Changes

- namespace declaration has an extra space after the opening curly brace,
+ namespace inzibackend.Web.Url{,
- method ReplaceTenancyNameInUrl has a space before the + operator,
+ private string ReplaceTenancyNameInUrl(string siteRootFormat, string tenancyName) {,
- return statement has an extra space after Replace,
+ return siteRootFormat.Replace(TenancyNamePlaceHolder + ".", TenancyNamePlaceHolder);,
- return statement has a space before the + operator in the last line,
+ return siteRootFormat.Replace(TenancyNamePlaceHolder, tenancyName + "");
## Purpose
The file is part of an ASP.NET Zero solution and handles URL configuration by replacing placeholders with actual tenancy names.
