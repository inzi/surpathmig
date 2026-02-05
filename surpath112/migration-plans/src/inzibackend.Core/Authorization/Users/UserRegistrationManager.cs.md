# Modified
## Filename
UserRegistrationManager.cs
## Relative Path
inzibackend.Core\Authorization\Users\UserRegistrationManager.cs
## Language
C#
## Summary
The modified file adds additional parameters to the RegisterAsync method, including middle name, address details, city, state, zip code, date of birth, and phone number. It also adjusts how default roles are assigned by ensuring they belong to the current tenant.
## Changes
Added fields: MiddleName, Address, SuiteApt, City, State, Zip, DateOfBirth, Phonenumber. Modified defaultRoles loop to check if roles belong to the current tenant.
## Purpose
The UserRegistrationManager class is used in an ASP.NET Zero application for managing user registration processes, including creating new users and handling related notifications.
