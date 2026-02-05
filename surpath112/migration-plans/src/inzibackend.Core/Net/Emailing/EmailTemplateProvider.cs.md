# Modified
## Filename
EmailTemplateProvider.cs
## Relative Path
inzibackend.Core\Net\Emailing\EmailTemplateProvider.cs
## Language
C#
## Summary
The modified code adds additional using statements and includes inzibackend.Tenancy in the namespace, while also including inzibackend.MultiTenancy twice. The GetDefaultTemplate method now uses tenantId to determine the tenancy key.
## Changes
Added two using statements for inzibackend.MultiTenancy and inzibackend.Tenancy, and included inzibackend.Tenancy in the namespace. The GetDefaultTemplate method was modified to use tenantId instead of hardcoding 'host'.
## Purpose
The EmailTemplateProvider class provides dependency injection for email template management, including support for different tenancies.
