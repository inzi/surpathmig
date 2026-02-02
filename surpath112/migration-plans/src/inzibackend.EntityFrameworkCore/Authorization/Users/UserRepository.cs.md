# Modified
## Filename
UserRepository.cs
## Relative Path
inzibackend.EntityFrameworkCore\Authorization\Users\UserRepository.cs
## Language
C#
## Summary
The modified code adds a check for context.IsModified() when retrieving expired password users, enhancing consistency across different database contexts.
## Changes
Added `context.IsModified()` to the query condition in `GetPasswordExpiredUserIds` method.
## Purpose
To ensure consistent handling of password expiration checks across different database contexts.
