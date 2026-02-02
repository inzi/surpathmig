# Modified
## Filename
Upgrade.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\view-resources\Views\Payment\Upgrade.js
## Language
JavaScript
## Summary
Both files implement a click handler for a checkout button that uses AJAX to send payment data to an API endpoint. The modified version includes creating a FormData object before populating it with form values.
## Changes
The modified file adds the line 'var formData = new FormData();' before populating the form data, which is necessary for preparing the AJAX request.
## Purpose
Dependency injection and service setup within an ASP.NET Zero application
