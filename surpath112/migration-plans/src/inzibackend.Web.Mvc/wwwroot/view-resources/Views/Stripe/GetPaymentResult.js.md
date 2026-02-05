# Modified
## Filename
GetPaymentResult.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\view-resources\Views\Stripe\GetPaymentResult.js
## Language
JavaScript
## Summary
The modified file adds an additional line of code (abp.ui.setBusy('#loading');) after the promise chain in the catch block. This line is not present in the unmodified version.
## Changes
Added abp.ui.setBusy('#loading'); to handle UI loading state after payment processing.
## Purpose
Payment processing with retry logic and UI feedback.
