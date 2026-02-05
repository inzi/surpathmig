# Modified
## Filename
ProfileController.cs
## Relative Path
inzibackend.Web.Host\Controllers\ProfileController.cs
## Language
C#
## Summary
The modified ProfileController removes the Microsoft.AspNetCore.Authorization.Graphics import and the ImageFormatValidator parameter from its constructor.
## Changes
Removed using Microsoft.AspNetCore.Authorization(Graphics); removed IImageFormatValidator imageFormatValidator from constructor parameters.
## Purpose
Simplified ProfileController by removing unused ImageFormatValidator dependency
