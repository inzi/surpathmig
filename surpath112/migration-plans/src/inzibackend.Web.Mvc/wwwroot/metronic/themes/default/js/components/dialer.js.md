# Modified
## Filename
dialer.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\metronic\themes\default\js\components\dialer.js
## Language
JavaScript
## Summary
The modified file introduces additional validation for the 'dialer' attribute in the element constructor, adds new methods like _format and _parse for value handling, modifies event handlers to use 'input' instead of 'change', and changes initialization logic based on the presence of 'dialer' data.
## Changes
1. Modified code checks for 'dialer' attribute using has() method in constructor, while unmodified uses if(!element) check.
2. Added _format and _parse methods for value formatting and parsing.
3. Changed event handler from 'change' to 'input'.
4. Initialization logic differs based on presence of 'dialer' data.
## Purpose
Provides dialer functionality with enhanced input handling, state management, and conditional initialization.
