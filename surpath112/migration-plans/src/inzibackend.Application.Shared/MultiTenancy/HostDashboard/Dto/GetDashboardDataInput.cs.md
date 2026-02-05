# Modified
## Filename
GetDashboardDataInput.cs
## Relative Path
inzibackend.Application.Shared\MultiTenancy\HostDashboard\Dto\GetDashboardDataInput.cs
## Language
C#
## Summary
The modified file is a C# class that extends DashboardInputBase and implements IShouldNormalize. It includes a property IncomeStatisticsDateInterval and a Normalize method TrimTime(), which currently contains redundant assignments to StartDate.
## Changes
The modified version of the file introduces an unnecessary duplicate line in the TrimTime() method, where StartDate is assigned twice without any meaningful change.
## Purpose
This class likely serves as input validation or data normalization for a dashboard within an ASP.NET Zero application.
