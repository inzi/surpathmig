# Modified
## Filename
ChatHub.cs
## Relative Path
inzibackend.Web.Core\Chat\SignalR\ChatHub.cs
## Language
C#
## Summary
The modified ChatHub class introduces a simplified constructor signature compared to the unmodified version. It also lacks HTML sanitization of messages before sending.
## Changes
The modified file removes the IHtmlSanitizer parameter from the constructor, adds a using statement for System.Threading.Tasks, and omits message sanitization within the SendMessage method.
## Purpose
To reduce dependencies by simplifying the constructor signature and remove message sanitization, potentially increasing vulnerabilities. The unmodified version includes these features to enhance security.
