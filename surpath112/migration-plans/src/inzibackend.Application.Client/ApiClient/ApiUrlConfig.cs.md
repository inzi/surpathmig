# Modified
## Filename
ApiUrlConfig.cs
## Relative Path
inzibackend.Application.Client\ApiClient\ApiUrlConfig.cs
## Language
C#
## Summary
The modified file changes the default host URL to use HTTPS with port 44301. The unmodified version uses HTTP with port 44302.
## Changes
The only change is in the `DefaultHostUrl` string, which was updated from using HTTP to HTTPS and a different port number.
## Purpose
The file configures the base URL for API requests, allowing changes based on environment (local vs production).
