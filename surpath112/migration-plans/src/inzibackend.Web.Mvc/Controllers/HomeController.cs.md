# Modified
## Filename
HomeController.cs
## Relative Path
inzibackend.Web.Mvc\Controllers\HomeController.cs
## Language
C#
## Summary
The modified controller includes a check for forceNewRegistration which calls sign out before redirecting.
## Changes
Added a call to _signInManager.SignOutAsync() when forceNewRegistration is true.
## Purpose
Handles user authentication and redirects, ensuring proper user management during redirections.
