# Modified
## Filename
ProfileControllerBase.cs
## Relative Path
inzibackend.Web.Core\Controllers\ProfileControllerBase.cs
## Language
C#
## Summary
The modified ProfileControllerBase class includes additional validation steps for profile pictures, such as checking file size against a maximum limit (5MB) and validating the image format using an IImageFormatValidator. The upload process now ensures that only valid image formats are processed.
## Changes
Added IImageFormatValidator validation step in UploadProfilePicture method, added MaxProfilePictureSize constant, and performed image format validation before loading the file.
## Purpose
To enforce security and consistency by validating profile picture files against size limits and acceptable formats.
