# New
## Filename
MedicalUnit.cs
## Relative Path
inzibackend.Core\Surpath\MedicalUnit.cs
## Language
C#
## Summary
The file defines a MedicalUnit class within the inzibackend namespace. It includes required fields such as Name, PrimaryContact, Address1, City, State, ZipCode, and HospitalId, along with optional TenantId. The class uses auditing features (FullAuditedEntity) and implements IMayHaveTenant, indicating it can have a tenant relationship through the HospitalId foreign key.
## Changes
New file
## Purpose
 Defines the data model for medical units in an ASP.NET Zero application, including auditing capabilities.
