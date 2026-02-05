# Modified
## Filename
TenantCustomizationController.cs
## Relative Path
inzibackend.Web.Core\Controllers\TenantCustomizationController.cs
## Language
C#
## Summary
The modified controller introduces an additional ImageFormatValidator for validating file formats when uploading logos and custom CSS. It includes this validator in the UploadLogo and UploadCustomCss methods.
## Changes
Added _imageFormatValidator property and updated UploadLogo and UploadCustomCss methods to include image format validation.
## Purpose
Ensures that logo and custom CSS files adhere to specified image formats, improving file integrity and consistency.
