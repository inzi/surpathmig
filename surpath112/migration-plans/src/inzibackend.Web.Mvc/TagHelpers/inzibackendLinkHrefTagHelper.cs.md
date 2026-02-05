# Modified
## Filename
inzibackendLinkHrefTagHelper.cs
## Relative Path
inzibackend.Web.Mvc\TagHelpers\inzibackendLinkHrefTagHelper.cs
## Language
C#
## Summary
The modified file is a helper class that processes link href attributes with specific modifications for CSS files. It handles .css and .min.css extensions by appending .min if necessary and skips processing in production mode.
## Changes
Added string.CompareInvariantIgnoreCase when checking for .css extension
## Purpose
To handle href modifications for CSS files, ensuring correct file paths are generated based on environment configuration.
