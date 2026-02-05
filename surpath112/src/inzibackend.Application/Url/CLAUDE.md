# URL Services

## Overview
Application services for URL generation and management including application URLs, email links, and deep links.

## Key Features
- Generate application URLs
- Create email verification links
- Generate password reset URLs
- Create deep links for mobile
- URL shortening (if applicable)

## Business Logic
Generates tenant-aware URLs for emails and links, handles subdomain or path-based multi-tenancy URLs, and creates secure token-based URLs for sensitive operations.

## Usage
Consumed by email services, notification services, and any component generating URLs for external use.