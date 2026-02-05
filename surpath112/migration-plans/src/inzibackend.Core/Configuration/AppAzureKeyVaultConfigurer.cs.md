# Modified
## Filename
AppAzureKeyVaultConfigurer.cs
## Relative Path
inzibackend.Core\Configuration\AppAzureKeyVaultConfigurer.cs
## Language
C#
## Summary
The modified file configures an Azure Key Vault with explicit certificate handling and dependency injection. It retrieves configuration settings, constructs URLs, and adds the key vault either using a pre-fetched X509 certificate or a managed identity credential.
## Changes
The modified file includes additional security measures by explicitly handling certificates (X509Store) and uses dependency injection with Microsoft.Extensions.Configuration.AzureKeyVault. The unmodified version lacks these features, directly creating a ClientSecretCredential from configuration properties without explicit certificate handling.
## Purpose
Both files configure an Azure Key Vault in the application context but differ in security practices: modified file ensures only authorized keys are used via certificates, while the unmodified does not.
