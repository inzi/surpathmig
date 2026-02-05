# Modified
## Filename
EntityHistoryHelper.cs
## Relative Path
inzibackend.Core\EntityHistory\EntityHistoryHelper.cs
## Language
C#
## Summary
The modified file introduces additional using statements and expanded arrays for tracking types. It also includes a new TrackedTypes property that combines HostSideTrackedTypes and TenantSideTrackedTypes.
## Changes
Added 'using inzibackend.Surpath;' statement, expanded HostSideTrackedTypes and TenantSideTrackedTypes with additional type entries, and added the TrackedTypes property which combines these arrays after grouping by type name.
## Purpose
The file provides utility classes for tracking entity history across different types in an ASP.NET Zero application.
