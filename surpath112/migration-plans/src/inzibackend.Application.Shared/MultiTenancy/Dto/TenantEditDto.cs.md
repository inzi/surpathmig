# Modified
## Filename
TenantEditDto.cs
## Relative Path
inzibackend.Application.Shared\MultiTenancy\Dto\TenantEditDto.cs
## Language
C#
## Summary
The modified TenantEditDto class includes additional properties related to payment configuration and flags, enhancing its functionality beyond the unmodified version.
## Changes
Added three new properties: DeferDonorPay (bool = false), DeferDonorPerpetualPay (bool = false), and ClientPaymentType (EnumClientPaymentType = InvoiceClient). Also added [DisableAuditing] attribute to ConnectionString property.
## Purpose
The file serves as a data transfer object (DTO) for editing tenant details in an ASP.NET Zero application, providing enhanced configuration options for multi-tenant environments.
