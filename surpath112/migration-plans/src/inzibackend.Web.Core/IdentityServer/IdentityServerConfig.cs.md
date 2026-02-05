# Modified
## Filename
IdentityServerConfig.cs
## Relative Path
inzibackend.Web.Core\IdentityServer\IdentityServerConfig.cs
## Language
C#
## Summary
The modified code hashes each client's secret value using SHA256 before adding it to the ClientSecrets array.
## Changes
In the GetClients method, each client's Value property is hashed using SHA256 when creating a new Secret object.
## Purpose
To securely store client secrets by hashing them, enhancing security against potential breaches.
