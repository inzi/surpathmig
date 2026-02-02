# Modified
## Filename
_CreateOrEditModal.cshtml
## Relative Path
inzibackend.Web.Mvc\Areas\App\Views\WebhookSubscription\_CreateOrEditModal.cshtml
## Language
Unknown
## Summary
The modified file includes additional script tags containing JavaScript code that sets variables used in form rendering. These variables determine whether an action is an edit or create and handle headers for form submission.
## Changes
Added two lines of JavaScript to the beginning of the file: var createOrEditIsEdit = @Html.Raw(...); and var createOrEditHeaders = @Html.Raw(...); which are used in the form rendering logic.
## Purpose
The file is part of an ASP.NET application's infrastructure, likely handling dependency injection for form variables related to creating or editing webhooks.
