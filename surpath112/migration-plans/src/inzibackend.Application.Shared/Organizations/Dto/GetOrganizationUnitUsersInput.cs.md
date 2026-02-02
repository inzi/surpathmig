# Modified
## Filename
GetOrganizationUnitUsersInput.cs
## Relative Path
inzibackend.Application.Shared\Organizations\Dto\GetOrganizationUnitUsersInput.cs
## Language
C#
## Summary
The modified file is a data-to-model (DTM) class that extends PagedAndSortedInputDto. It includes an Id property with range validation and a Normalize method that configures sorting based on user information, converting userName to user.userName and addedTime to ouUser.creationTime.
## Changes
Added a comment line after the using statements in the modified file.
## Purpose
The class is used for normalizing input data related to organization units' users within an ASP.NET Zero application.
