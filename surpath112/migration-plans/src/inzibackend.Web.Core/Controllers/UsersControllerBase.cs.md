# Modified
## Filename
UsersControllerBase.cs
## Relative Path
inzibackend.Web.Core\Controllers\UsersControllerBase.cs
## Language
C#
## Summary
Both files define an abstract controller class with an ImportFromExcel action method that handles file uploads and enqueues a background job to process the file.
## Changes
The modified file adds a using directive for Abp.BackgroundJobs, which was missing in the unmodified version. This indicates that the BackgroundJobManager is now properly referenced in the namespace.
## Purpose
Both files provide the setup for handling Excel imports by saving uploaded files and enqueuing background processing tasks.
