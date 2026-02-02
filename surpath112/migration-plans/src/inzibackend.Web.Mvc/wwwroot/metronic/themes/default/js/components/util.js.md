# Modified
## Filename
util.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\metronic\themes\default\js\components\util.js
## Language
JavaScript
## Summary
The getResponsiveValue function was not correctly handling cases where all breakpoints were larger than the viewport width.
## Changes
Modified the loop in getResponsiveValue to track and return only when a valid (<= viewport width) breakpoint is found.
## Purpose
Ensure that getResponsiveValue returns null if no breakpoints are suitable for the current viewport width.
