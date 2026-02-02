# Modified
## Filename
EditionAppService.cs
## Relative Path
inzibackend.Application\Editions\EditionAppService.cs
## Language
C#
## Summary
This file is a service class that manages editions in an ASP.NET Zero application. It provides methods for getting editions, creating and updating editions, moving tenants between editions, and managing tenant counts.
## Changes
The modified version includes additional validation checks when deleting an edition (checking if the edition has expiring users) and when creating an edition (ensuring that any referenced expiring edition is free).
## Purpose
This file serves as a service provider for edition management, enabling CRUD operations on editions while adhering to business rules.
