# Modified
## Filename
inzibackendScriptSrcTagHelper.cs
## Relative Path
inzibackend.Web.Mvc\TagHelpers\inzibackendScriptSrcTagHelper.cs
## Language
C#
## Summary
The modified file implements a script source tag helper that processes attributes to handle src references. It includes checks for abp-ignore-src-modification and modifies hrefs by adding .min.js where applicable, while also conditionally removing the attribute based on production environment.
## Changes
Added conditional removal of abp-src attribute in production mode; modified href handling logic with explicit checks for .js and .min.js extensions.
## Purpose
Configuration class for processing script source tags to optimize minification and build processes.
