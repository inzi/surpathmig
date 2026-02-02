# Modified
## Filename
LoginAttempts.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\view-resources\Areas\App\Views\Users\LoginAttempts.js
## Language
JavaScript
## Summary
The modified file adds functionality for creating a DataTable instance directly on the 'LoginAttemptsTable', includes event handling with e parameter in click handlers, and adjusts date formatting for range selection.
## Changes
$dataTable = $('#LoginAttemptsTable').DataTable({ ... }),function click(e) { ... },start.format('YYYY-MM-DDT00:00:00Z'),end.format('YYYY-MM-DDT23:59:59.999Z')
## Purpose
Enhancing the DataTable initialization and event handling for better user interaction
