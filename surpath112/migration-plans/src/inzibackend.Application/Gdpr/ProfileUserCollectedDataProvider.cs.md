# Modified
## Filename
ProfileUserCollectedDataProvider.cs
## Relative Path
inzibackend.Application\Gdpr\ProfileUserCollectedDataProvider.cs
## Language
C#
## Summary
The modified file introduces an additional private readonly TenantManager instance variable and includes it in the constructor. The GetFiles method uses this new variable to retrieve tenant information.
## Changes
Added _tenantManager parameter to the constructor and assigned it to the instance variable. The GetFiles method now correctly utilizes _tenantManager to determine the tenancy name.
## Purpose
The class collects user profile data, including personal details and contact information, which is stored temporarily in a file for compliance purposes.
