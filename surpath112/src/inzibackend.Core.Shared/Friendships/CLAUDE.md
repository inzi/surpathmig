# Friendships Documentation

## Overview
Defines the state of user relationships within the application's social features, enabling connection management between users.

## Contents

### Files

#### FriendshipState.cs
- **Purpose**: Enumeration for friendship/connection states between users
- **Enum Values**:
  - Accepted (1): Active friendship/connection between users
  - Blocked (2): One user has blocked the other
- **Usage**: Manages user relationship states in social features

### Key Components
- Binary state management for user relationships
- Explicit values for database consistency
- Simple relationship model

### Dependencies
- No external dependencies
- Part of the inzibackend.Friendships namespace

## Architecture Notes
- Simplified friendship model (no pending state visible here)
- Explicit integer values ensure database consistency
- Supports blocking functionality for user safety

## Business Logic
- **Accepted**: Users can communicate and see each other's activities
- **Blocked**: Prevents interaction between users
- State changes affect messaging, visibility, and notifications
- Likely additional states handled elsewhere (e.g., pending requests)

## Usage Across Codebase
This enum is used in:
- Friendship/connection entities
- User relationship queries
- Chat/messaging access control
- Social feature visibility rules
- Notification filtering
- User privacy settings
- Block list management