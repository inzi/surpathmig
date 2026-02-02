# Modified
## Filename
ModernHttpClientFactory.cs
## Relative Path
inzibackend.Application.Client\ApiClient\ModernHttpClientFactory.cs
## Language
C#
## Summary
The modified file adds a conditional compilation directive to include a trust method for local developer certificates only when the DEBUG flag is set.
## Changes
Added `#if DEBUG` directive wrapping the `TrustLocalDeveloperCert` call in the CreateMessageHandler method.
## Purpose
Configuration file for an HTTP client handler with conditional security settings.
