# Modified
## Filename
AzureKeyVaultConfiguration.cs
## Relative Path
inzibackend.Core\Configuration\AzureKeyVaultConfiguration.cs
## Language
C#
## Summary
The modified file introduces additional configuration properties for Azure Key Vault such as AzureADApplicationId, AzureADCertThumbprint, and methods UsesCertificate() and UsesManagedIdentity(). These additions enhance the configuration options for managing Azure AD applications.
## Changes
Added properties: AzureADApplicationId (string), AzureADCertThumbprint (string). Added method: UsesCertificate(). The unmodified version lacks these new properties and method. Also, the unmodified version includes a TenantId property which is absent in the modified one.
## Purpose
The file configures an Azure Key Vault implementation within an ASP.NET Zero application, enabling configuration of Azure AD authentication settings.
