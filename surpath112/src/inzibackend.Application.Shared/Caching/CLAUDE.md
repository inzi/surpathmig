# Caching Documentation

## Overview
This folder contains service interfaces and DTOs for application cache management. The caching system improves performance by storing frequently accessed data in memory, reducing database queries and improving response times.

## Contents

### Files
Service interface for cache operations (ICachingAppService.cs or similar)

### Subfolders

#### Dto
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Named caches for different data types
- Distributed caching support for multi-server
- Cache invalidation on data changes
- TTL (Time-To-Live) for automatic expiration

## Business Logic
Cache management operations including clearing caches, viewing cache statistics, and monitoring cache performance.

## Usage Across Codebase
Admin cache management interface and configuration services

## Cross-Reference Impact
Changes affect cache management UI and cache clearing workflows