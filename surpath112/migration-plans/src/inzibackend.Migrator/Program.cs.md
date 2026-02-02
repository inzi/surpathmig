# Modified
## Filename
Program.cs
## Relative Path
inzibackend.Migrator\Program.cs
## Language
C#
## Summary
The modified file introduces an additional using statement for resolving IocResolver, properly closes it after execution, and adjusts the control flow to return earlier if certain conditions are met.
## Changes
Added a new using (var resolver = ...) block for IocResolver, closed it after migration execution, and adjusted the control flow logic in the Main method.
## Purpose
The file appears to be part of an ASP.NET Core dependency injection configuration, handling logging setup and conditional execution based on environment variables.
