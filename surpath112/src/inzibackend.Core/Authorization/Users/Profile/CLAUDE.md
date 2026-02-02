# Profile Documentation

## Overview
Profile image management system supporting multiple image sources including local storage and Gravatar integration. Implements a factory pattern for flexible profile image service selection.

## Contents

### Files

#### IProfileImageService.cs
- **Purpose**: Interface defining profile image service operations
- **Key Methods**: GetProfilePicture, GetProfilePictureUrl

#### ProfileImageServiceFactory.cs
- **Purpose**: Factory for creating appropriate profile image service instances
- **Key Functionality**: Service selection based on configuration or user preference

#### LocalProfileImageService.cs
- **Purpose**: Implementation for locally stored profile images
- **Key Features**: File-based storage, binary object management

#### GravatarProfileImageService.cs
- **Purpose**: Integration with Gravatar service for profile images
- **Key Features**: Email-based avatar retrieval, fallback support

### Key Components

- **IProfileImageService**: Common interface for all image services
- **ProfileImageServiceFactory**: Service instantiation logic
- **LocalProfileImageService**: Local storage implementation
- **GravatarProfileImageService**: Gravatar API integration

### Dependencies

- **External Libraries**:
  - Gravatar API (for GravatarProfileImageService)
  - ABP Framework

- **Internal Dependencies**:
  - User entity
  - Binary object storage

## Architecture Notes

- **Pattern**: Factory pattern with strategy selection
- **Extensibility**: Easy to add new image service providers
- **Flexibility**: Runtime service selection

## Business Logic

- Supports multiple image sources
- Automatic fallback mechanisms
- User preference respect
- Centralized image management

## Usage Across Codebase

Used for:
- User profile displays
- Avatar management in UI
- User settings interfaces
- Comment/activity displays