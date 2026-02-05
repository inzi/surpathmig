# Modified
## Filename
UserLoginInfoDto.cs
## Relative Path
inzibackend.Application.Shared\Sessions\Dto\UserLoginInfoDto.cs
## Language
C#
## Summary
The modified UserLoginInfoDto class extends EntityDto<long> and includes additional properties such as IsPaid, IsAlwaysDonor, IsCohortUser, CohortUserId (nullable Guid), Departments and DepartmentsAuthed (lists of Guid), CohortId, and Roles.
## Changes
Added properties: IsPaid (bool = false), IsAlwaysDonor (bool = false), IsCohortUser (bool = false), CohortUserId ( Guid? ), Departments (List<string>), DepartmentsAuthed (List<string>), CohortId (Guid?), Roles (List<string>).
## Purpose
The file defines an entity data type for user login information in the ASP.NET Zero solution.
