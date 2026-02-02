# Modified
## Filename
DynamicEntityPropertyValuesController.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Controllers\DynamicEntityPropertyValuesController.cs
## Language
C#
## Summary
The modified file contains an updated DynamicEntityPropertyValuesController class that includes additional validation checks for dynamic entity existence using IDynamicEntityPropertyDefinitionManager.
## Changes
Added a check using _dynamicEntityPropertyDefinitionManager.ContainsEntity(entityFullName) in the ManageAll action method to ensure the entity exists.
## Purpose
The file serves as an API controller service for managing dynamic entity property values, utilizing dependency injection for flexible configuration.
