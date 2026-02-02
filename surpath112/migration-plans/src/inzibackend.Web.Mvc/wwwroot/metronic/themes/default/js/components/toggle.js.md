# Modified
## Filename
toggle.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\metronic\themes\default\js\components\toggle.js
## Language
JavaScript
## Summary
The modified file introduces additional functionality for handling toggle states with different modes. It includes an option to specify the mode (on/off) which affects how the toggle is triggered.
## Changes
1. Added 'mode' attribute handling in KTToggle.createInstances() - now using document.body instead of BODY\[0].
2. Updated _construct method to include 'mode' attribute handling.
3. Modified handlers to check for mode state before triggering _toggle().
## Purpose
Enhanced toggle functionality with support for different modes (on/off) which determine how the toggle action is triggered.
