# Modified
## Filename
AccessTokenManager.cs
## Relative Path
inzibackend.Application.Client\ApiClient\AccessTokenManager.cs
## Language
C#
## Summary
The modified code adds a check to ensure that the refresh token has not expired before setting its expiration date. This provides an additional layer of security by preventing access to an expired token.
## Changes
Added a DateTime check in GetAccessToken() method to throw exception if current time exceeds refresh token's expiration date.
## Purpose
To enhance security by validating the refresh token's expiration status before use.
