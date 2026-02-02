# Modified
## Filename
event-handler.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\metronic\themes\default\js\components\event-handler.js
## Language
JavaScript
## Summary
The modified file introduces a new configuration option 'one' in the _triggerEvent method, which allows triggering events with one-time execution. It also adds an additional parameter to the on and one methods for adding event handlers. The public trigger method now returns null if the event is not fired.
## Changes
Added 'one' property to _triggerEvent method; updated _addEvent and _removeEvent methods to accept this new configuration; modified trigger, on, one, and off methods in the public interface to include the 'one' parameter when adding event handlers. The debug method was also updated to remove the handlerId argument.
## Purpose
This class provides event handling functionality for an ASP.NET Zero application, allowing for dependency injection of event handlers with optional one-time execution.
