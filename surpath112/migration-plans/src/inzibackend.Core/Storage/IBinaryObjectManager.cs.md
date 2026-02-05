# Modified
## Filename
IBinaryObjectManager.cs
## Relative Path
inzibackend.Core\Storage\IBinaryObjectManager.cs
## Language
C#
## Summary
The modified IBinaryObjectManager interface introduces additional optional parameters to its methods. The GetOrNullAsync method now accepts a TenantId and tracking boolean, while all methods have been updated to include these optional parameters.
## Changes
Added optional 'TenantId' parameter (int?) and 'tracking' boolean parameter to the GetOrNullAsync method. All methods now accept these optional parameters. The interface remains an abstract base class for binary object management operations.
## Purpose
The interface serves as a contract for classes managing binary objects, enabling operations like retrieval, saving, and deletion with optional configuration parameters.
