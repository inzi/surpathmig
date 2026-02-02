# Modified
## Filename
IRoleAppService.cs
## Relative Path
inzibackend.Application.Shared\Authorization\Roles\IRoleAppService.cs
## Language
C#
## Summary
The modified interface adds a return type to the DeleteRole method, enhancing its functionality by specifying the output type.
## Changes
The return type of the DeleteRole method was updated from Task to Task<GetRoleForEditOutput>.
## Purpose
The interface is used in role management within an ASP.NET Zero application, likely for dependency injection and service configuration.
