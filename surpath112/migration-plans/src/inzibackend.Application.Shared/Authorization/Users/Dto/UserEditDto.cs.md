# Modified
## Filename
UserEditDto.cs
## Relative Path
inzibackend.Application.Shared\Authorization\Users\Dto\UserEditDto.cs
## Language
C#
## Summary
The modified UserEditDto class includes additional properties such as Address, SuiteApt, City, State, Zip, DateOfBirth, and removes IsAlwaysDonor compared to the unmodified version. It maintains similar functionality with public properties and required attributes.
## Changes
Added Address { get; set; }, SuitApt { get; set; }, City { get; set; }, State { get; set; }, Zip { get; set; }, DateOfBirth { get; set; }; removed IsAlwaysDonor { get; set; } with default value.
## Purpose
The file defines a data model for editing user entities in an ASP.NET Zero application, utilizing dependency injection and domain entity definitions.
