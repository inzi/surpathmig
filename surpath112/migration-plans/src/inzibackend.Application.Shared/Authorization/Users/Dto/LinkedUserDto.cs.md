# Modified
## Filename
LinkedUserDto.cs
## Relative Path
inzibackend.Application.Shared\Authorization\Users\Dto\LinkedUserDto.cs
## Language
C#
## Summary
The LinkedUserDto class defines a user entity with properties for tenant ID, tenancy name, username and a method to format the login name. The GetShownLoginName method returns either the username or a formatted string combining tenancy name and username based on multi-tenancy configuration.
## Changes
The modified file includes an escape character backslash before quotes in the GetShownLoginName method for better code formatting, while the unmodified version uses regular quotes. This change does not affect functionality but improves readability.
## Purpose
This class is part of the ASP.NET Zero solution's backend service configuration, providing data binding and model-to-scheme mapping.
