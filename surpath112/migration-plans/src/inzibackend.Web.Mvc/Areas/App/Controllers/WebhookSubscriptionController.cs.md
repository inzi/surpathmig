# Modified
## Filename
WebhookSubscriptionController.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Controllers\WebhookSubscriptionController.cs
## Language
C#
## Summary
The modified file contains additional methods such as WebhookSubscriptionController and implements several actions including Index, EditModal, CreateModal, Detail, and WebHookEventDetail. The controller uses dependency injection for services and follows an MVVM architecture.
## Changes
The parameter names in the EditModal action changed from _id to subscriptionId. In the CreateModal action, the model initialization was modified to use GetSubscription(subscriptionId) instead of GetSubscription(_id). Additionally, the WebHookEventDetail action uses id instead of _id as a parameter.
## Purpose
The file serves as an ASP.NET Zero controller for managing webhook subscriptions, including CRUD operations and fetching events.
