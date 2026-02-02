# Modified
## Filename
_CreateOrEditModal.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\view-resources\Areas\App\Views\WebhookSubscriptions\_CreateOrEditModal.js
## Language
JavaScript
## Summary
The modified file adds a regex validation rule for the 'webhookUri' field in the createOrEditWebhookSubscriptionModal form, ensuring it matches the specified pattern.
## Changes
Added $.validator.addMethod('regex', function (value, element, regexp) { return this.optional(element) || regexp.test(value); }, abp.localization.localize('InvalidPattern'));
## Purpose
The file is part of an ASP.NET Zero application and defines validation rules for a modal form used to create or edit webhook subscription configurations.
