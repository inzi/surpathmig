# Modified
## Filename
GetUserDelegationsInput.cs
## Relative Path
inzibackend.Application.Shared\Authorization\Users\Delegation\Dto\GetUserDelegationsInput.cs
## Language
C#
## Summary
The modified file implements a class that handles paginated and sorted requests for user delegation data. It includes a Normalize method that adjusts sorting strings to ensure consistency.
## Changes
The modified code adds handling of 'userName ASC' in the Normalize method, converting it to 'Username', while the unmodified version only checks for null without this explicit conversion.
## Purpose
This class is part of an ASP.NET Zero solution, serving as a service that manages paginated user data requests with proper sorting normalization.
