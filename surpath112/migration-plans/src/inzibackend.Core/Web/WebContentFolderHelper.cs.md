# Modified
## Filename
WebContentFolderHelper.cs
## Relative Path
inzibackend.Core\Web\WebContentFolderHelper.cs
## Language
C#
## Summary
The modified file contains a class that calculates the root path of a web project by searching through directories until it finds 'inzibackend.Web.sln'. It then checks for either the Web.Mvc or Web.Host folder and returns their paths if they exist.
## Changes
The only change is the order of the using directives: System.Linq was moved before Abp.Reflection.Extensions in the modified version.
## Purpose
To provide utility methods for determining project root paths, which are essential for locating views and connection strings used in Entity Framework Core applications within an ASP.NET Zero setup.
