# Modified
## Filename
_SelectAnEntityModal.cshtml
## Relative Path
inzibackend.Web.Mvc\Areas\App\Views\DynamicProperty\_SelectAnEntityModal.cshtml
## Language
Unknown
## Summary
Both files contain using statements and use Html.PartialAsync for two partials. The key difference is the method used to retrieve the list type: modified uses new SelectList(Model) while unmodified uses Model.List.
## Changes
The modified file replaces `new<SelectList>(Model)` with `Model.List` in the @Html.DropDownList call.
## Purpose
These files are part of a modal view in an ASP.NET Zero application, handling entity selection.
