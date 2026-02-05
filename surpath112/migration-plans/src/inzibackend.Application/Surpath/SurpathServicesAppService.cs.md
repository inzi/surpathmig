# New
## Filename
SurpathServicesAppService.cs
## Relative Path
inzibackend.Application\Surpath\SurpathServicesAppService.cs
## Language
C#
## Summary
The class contains multiple query methods that could benefit from being refactored into a helper class due to repeated filter logic, as well as an Excel export method that should be moved into its own dedicated method.
## Changes
1. Extract common query patterns into a helper class.<br/>2. Refactor the complex Excel export query into its own method.<br/>3. Consider creating repository classes for each data source (tenants, users, etc.).<br/>4. Evaluate if any methods can be moved to a HostRepository.
## Purpose
Improve code maintainability and reduce duplication by extracting common patterns into helper classes.
