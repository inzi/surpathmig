# Modified
## Filename
GetUsersInput.cs
## Relative Path
inzibackend.Application.Shared\Authorization\Users\Dto\GetUsersInput.cs
## Language
C#
## Summary
The modified file defines a class GetUsersInput which implements several interfaces including PagedAndSortedInputDto, IShouldNormalize and IGetUsersInput. It contains properties such as Filter, Permissions, Role, OnlyLockedUsers and a Normalize method that sets the default sorting to 'Name,Surname' if Sorting is null.
## Changes
The only change in the modified file is an extra space after the comma in the string value "Name,Surname" within the Normalize method.
## Purpose
This class serves as a data input provider for user authorization, handling normalization and filtering of user data.
