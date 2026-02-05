# Modified
## Filename
IAccountAppService.cs
## Relative Path
inzibackend.Application.Shared\Authorization\Accounts\IAccountAppService.cs
## Language
C#
## Summary
The modified file contains an interface with several service tasks related to user impersonation and account management. The key difference from the unmodified version is that 'BackToImpersonator()' was moved before 'SwitchToLinkedAccountOutput'.
## Changes
The order of two methods in the interface has been changed: 'BackToImpersonator()' was moved before 'SwitchToLinkedAccountOutput'.
## Purpose
This interface likely contains service tasks used for user impersonation and account activation within an ASP.NET Zero application, possibly part of a dependency injection configuration.
