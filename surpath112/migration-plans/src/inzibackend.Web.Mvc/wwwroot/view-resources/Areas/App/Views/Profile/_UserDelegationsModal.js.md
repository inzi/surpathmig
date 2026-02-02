# Modified
## Filename
_UserDelegationsModal.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\view-resources\Areas\App\Views\Profile\_UserDelegationsModal.js
## Language
JavaScript
## Summary
Both files define a modal interface for managing user delegations with a data table displaying user information. The modified version includes an additional function `getDelegatedUsers()` that triggers an AJAX reload after deletion to ensure the data is refreshed, while the unmodified version does not have this extra step.
## Changes
The modified file adds an `getDelegatedUsers()` function that calls `DataTable.ajax.reload()` after deleting a user delegation. The unmodified file lacks this additional function.
## Purpose
Both files are part of an ASP.NET Zero application's dependency injection configuration, setting up the modal interface and data table functionality for managing user delegations.
