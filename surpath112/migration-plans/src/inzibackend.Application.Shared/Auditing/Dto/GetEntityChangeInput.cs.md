# Modified
## Filename
GetEntityChangeInput.cs
## Relative Path
inzibackend.Application.Shared\Auditing\Dto\GetEntityChangeInput.cs
## Language
C#
## Summary
The modified file introduces a custom sorting mechanism in both GetEntityChangeInput and GetEntityTypeChangeInput classes. The Normalize method adds logic to prefix 'User.' before 'UserName' entries and 'EntityChange.' before others.
## Changes
Added sorting transformation logic in the Normalize method of GetEntityChangeInput, which prefixes 'User.' for 'UserName' and 'EntityChange.' for other strings.
## Purpose
Customize sorting order based on user roles or entity types to enhance data organization.
