# Modified
## Filename
UserLinkManager.cs
## Relative Path
inzibackend.Core\Authorization\Users\UserLinkManager.cs
## Language
C#
## Summary
The modified code introduces an additional property _abpSession and initializes it in the constructor. This allows for session management functionality that wasn't present before.
## Changes
Added _abpSession property initialized to AbpSession.Instance in the constructor. The Link method now uses this property which was not present in the unmodified version.
## Purpose
The file implements an ABP session-based user linking mechanism, enabling users to be linked across different accounts within the same application.
