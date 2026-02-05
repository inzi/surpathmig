# Modified
## Filename
_EditModal.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\view-resources\Areas\App\Views\Editions\_EditModal.js
## Language
JavaScript
## Summary
The modified file implements a modals.EditEditionModal function that includes additional error handling by setting the modal to busy and then back to not busy after saving.
## Changes
Added .always(setBusy(false)) in the save method's promise chain.
## Purpose
To manage the modal's state correctly after saving an edition.
