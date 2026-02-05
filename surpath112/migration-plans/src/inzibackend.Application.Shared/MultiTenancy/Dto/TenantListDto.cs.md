# Modified
## Filename
TenantListDto.cs
## Relative Path
inzibackend.Application.Shared\MultiTenancy\Dto\TenantListDto.cs
## Language
C#
## Summary
The modified TenantListDto adds two boolean properties: IsDonorPay and DeferDonorPerpetualPay, both initialized to false.
## Changes
Added 'IsDonorPay' bool { get; set; } = false;' and 'DeferDonorPerpetualPay' bool { get; set; } = false;'
## Purpose
To track donor payment information and deferred perpetual payment options for tenants.
