# Surpath DTOs Documentation

## Overview
This folder is currently empty. DTOs (Data Transfer Objects) for the Surpath domain are defined in the `inzibackend.Application.Shared` project instead, following ABP best practices for application service contracts.

## Architecture Note

The Surpath DTOs can be found in:
- **Location**: `src/inzibackend.Application.Shared/Surpath/Dtos/`
- **Purpose**: Separation of concerns - Core layer contains domain entities, Application.Shared contains service contracts and DTOs
- **Pattern**: Clean Architecture / DDD principles

## Related Documentation

For Surpath DTOs, see:
- [inzibackend.Application.Shared/Surpath/Dtos/CLAUDE.md](../../../../inzibackend.Application.Shared/Surpath/Dtos/CLAUDE.md)

This folder structure follows ABP Framework conventions where:
- **Core**: Domain entities, domain services, business rules
- **Application.Shared**: Service interfaces, DTOs, enums
- **Application**: Service implementations

## Why Empty?

Having an empty Dtos folder in Core may indicate:
1. Historical artifact from refactoring
2. Placeholder for future Core-specific DTOs
3. Organizational structure maintained for consistency

Best practice is to keep DTOs in Application.Shared layer to maintain proper layering.