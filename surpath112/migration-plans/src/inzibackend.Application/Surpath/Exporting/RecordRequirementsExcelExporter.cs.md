# New
## Filename
RecordRequirementsExcelExporter.cs
## Relative Path
inzibackend.Application\Surpath\Exporting\RecordRequirementsExcelExporter.cs
## Language
C#
## Summary
The provided C# code defines a class RecordRequirementsExcelExporter which extends NpoiExcelExporterBase and implements IRecordRequirementsExcelExporter. This class is responsible for exporting record requirements into an Excel file using the NPOI library. The class constructor initializes instance variables with timeZoneConverter, abpSession, and tempFileCacheManager. The ExportToFile method creates a new Excel package, adds headers and data to a sheet named 'RecordRequirements', populating it with fields such as Name, Description, Metadata, IsSurpathOnly, TenantDepartmentName, CohortName, SurpathServiceName, and TenantSurpathServiceName from the provided DTOs.
## Changes
New file
## Purpose
The file is used to export record requirements into an Excel file with a predefined structure using NPOI library.
