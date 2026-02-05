# New
## Filename
RecordCategoriesExcelExporter.cs
## Relative Path
inzibackend.Application\Surpath\Exporting\RecordCategoriesExcelExporter.cs
## Language
C#
## Summary
The RecordCategoriesExcelExporter class is responsible for exporting record categories into an Excel file named 'RecordCategories.xlsx'. It implements IRecordCategoriesExcelExporter and uses NpoiExcelExporterBase to handle Excel IO operations. The class takes a TimeZoneConverter, ABP session, and TempFileCacheManager in its constructor. The ExportToFile method creates a sheet with headers and populates it with data from record categories using AddHeader and AddObjects methods.
## Changes
New file
## Purpose
Part of the ASP.NET Zero solution, handles exporting record categories to Excel
