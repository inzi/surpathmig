# Modified
## Filename
SessionLockScreen.cshtml
## Relative Path
inzibackend.Web.Mvc\Views\Account\SessionLockScreen.cshtml
## Language
Unknown
## Summary
The modified file includes an additional hidden input field for 'ReturnUrl' which is used to pass the return URL from the view to the script.
## Changes
Added a new line in the form: <input type="hidden" name="ReturnUrl" value="@ViewBag.ReturnUrl"/>
## Purpose
To allow passing of the ReturnUrl parameter from the view to the script when handling form submissions.
