# Modified
## Filename
Startup.cs
## Relative Path
inzibackend.Web.Mvc\Startup\Startup.cs
## Language
C#
## Summary
The Swagger UI configuration was not properly set up after enabling the Swagger middleware.
## Changes
Added app.UseSwaggerUI() call to configure the Swagger UI endpoint after enabling Swagger.
## Purpose
Ensure Swagger UI is correctly configured and accessible on the /swagger route.
