# Modified
## Filename
InvalidUserExporter.cs
## Relative Path
inzibackend.Application\Authorization\Users\Importing\InvalidUserExporter.cs
## Language
C#
## Summary
The modified file is an override of the InvalidUserExporter class within the ASP.NET Zero solution. It includes a change where roles are joined with a comma and exclamation mark when exporting to Excel.
## Changes
In the modified version, the role names in the exported Excel file are now joined using a comma followed by an exclamation mark (",!") instead of just a comma (",") as seen in the unmodified version.
## Purpose
The InvalidUserExporter class is used to export invalid user data from an application into an Excel file, aiding in debugging and analysis.
