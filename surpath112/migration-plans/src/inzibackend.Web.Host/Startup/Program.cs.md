# Modified
## Filename
Program.cs
## Relative Path
inzibackend.Web.Host\Startup\Program.cs
## Language
C#
## Summary
The modified code includes an additional line that duplicates a configuration call (UseIIS()), which may be unintended.
## Changes
An extra line `UseIIS()` was added to the modified content after the logging configuration and before `UseIISIntegration()`, resulting in duplicate configuration calls.
## Purpose
The file configures the ASP.NET Zero web host setup, including setting up logging filters, IIS integration, and application startup.
