# Chat Documentation

## Overview
This folder contains service interfaces and DTOs for the real-time chat messaging system. The chat system enables direct user-to-user communication with support for cross-tenant messaging, read receipts, and message history.

## Contents

### Files
Service interface for chat operations (IChatAppService.cs or similar)

### Subfolders

#### Dto
Complete chat messaging DTOs including messages, users, and read state management.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- SignalR for real-time message delivery
- Message duplication pattern (sender and receiver copies)
- Cross-tenant messaging support
- Read receipt tracking

## Business Logic
User-to-user messaging, friend list management, message history, and real-time notifications.

## Usage Across Codebase
Chat UI components, SignalR hubs, and notification system

## Cross-Reference Impact
Changes affect chat interfaces, real-time messaging, and notification badges