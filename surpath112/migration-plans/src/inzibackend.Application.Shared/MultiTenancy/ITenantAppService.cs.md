# Modified
## Filename
ITenantAppService.cs
## Relative Path
inzibackend.Application.Shared\MultiTenancy\ITenantAppService.cs
## Language
C#
## Summary
The modified file adds 'using System.Collections.Generic;' and includes a new method 'GetTenantsList()' in the ITenantAppService interface.
## Changes
1. Added 'using System.Collections.Generic;'
2. Added 'Task<List<TenantListDto>> GetTenantsList();' to ITenantAppService interface
## Purpose
The interface defines methods for tenant operations including creating, retrieving, updating, and deleting tenants, serving as a service contract in an ASP.NET Zero application.
