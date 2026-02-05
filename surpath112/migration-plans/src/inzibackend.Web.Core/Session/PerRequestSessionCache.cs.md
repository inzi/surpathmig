# Modified
## Filename
PerRequestSessionCache.cs
## Relative Path
inzibackend.Web.Core\Session\PerRequestSessionCache.cs
## Language
C#
## Summary
The PerRequestSessionCache class implements IPerRequestSessionCache and ITransientDependency. It caches login information across requests using dependency injection for HTTP context access and session service.
## Changes
The unmodified file had a typo in the interface name (ITransientDependency instead of ITransientDependency). The modified version corrects this typo.
## Purpose
To efficiently cache and retrieve user login information per request, reducing unnecessary re-renders.
