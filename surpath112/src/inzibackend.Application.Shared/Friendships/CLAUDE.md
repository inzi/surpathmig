# Friendships Documentation

## Overview
This folder contains service interfaces and DTOs for the user friendship and social connection system. Friendships enable user-to-user connections, friend requests, contact management, and user blocking for social features.

## Contents

### Files
Service interface for friendship operations (IFriendshipAppService.cs or similar)

### Subfolders

#### Dto
Friendship DTOs including requests, friend management, and blocking.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Bi-directional friendship storage
- Cross-tenant friendships
- Request/accept workflow
- One-sided blocking

## Business Logic
Send friend requests, accept/reject requests, manage friend lists, block/unblock users, cross-tenant communication.

## Usage Across Codebase
Chat contact lists, user profile, social features, mobile apps

## Cross-Reference Impact
Changes affect friend management UI, chat contacts, and social features