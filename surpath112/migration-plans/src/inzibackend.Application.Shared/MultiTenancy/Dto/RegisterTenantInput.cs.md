# Modified
## Filename
RegisterTenantInput.cs
## Relative Path
inzibackend.Application.Shared\MultiTenancy\Dto\RegisterTenantInput.cs
## Language
C#
## Summary
The modified file introduces an additional 'AdminSurname' property to the 'RegisterTenantInput' class, enhancing its functionality for handling tenant registration details.
## Changes
Added 'AdminSurname { get; set; }' with [StringLength(AbpUserBase.MaxSurnameLength)] and [DisableAuditing] attributes. Also, corrected formatting by adding a closing bracket.
## Purpose
The file is part of an ASP.NET Zero solution for managing multi-tenant applications, providing structured data definitions for tenant registration inputs.
