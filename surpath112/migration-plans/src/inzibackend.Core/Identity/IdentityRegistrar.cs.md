# Modified
## Filename
IdentityRegistrar.cs
## Relative Path
inzibackend.Core\Identity\IdentityRegistrar.cs
## Language
C#
## Summary
The modified file introduces a new IdentityRegistrar class that adds logging functionality to the identity registration process. It configures several dependency injection bindings for authentication, authorization, editions, user stores, and security stamp validators.
## Changes
Added services.AddLogging() in the Register method.
## Purpose
This file is part of an ASP.NET Zero solution's IdentityRegistrar class, which sets up identity registration services with logging enabled.
