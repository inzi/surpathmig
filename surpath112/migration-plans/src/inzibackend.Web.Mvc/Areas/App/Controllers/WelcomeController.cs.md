# Modified
## Filename
WelcomeController.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Controllers\WelcomeController.cs
## Language
C#
## Summary
The modified WelcomeController implements dependency injection for an IWelcomemessagesAppService. It initializes this service in the constructor and uses it to retrieve data which is then displayed in the view.
## Changes
Added a constructor that accepts and assigns IWelcomemessagesAppService, added code to get and display data from the service.
## Purpose
To implement dependency injection for message service and bind its output to the view.
