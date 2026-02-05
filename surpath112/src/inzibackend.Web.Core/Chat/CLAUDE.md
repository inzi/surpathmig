# Chat Documentation

## Overview
This folder contains the real-time chat infrastructure for the application, built on SignalR for WebSocket-based communication. It enables instant messaging, presence tracking, and social features across web and mobile platforms.

## Contents

### Files
*No files directly in this folder - functionality organized in subfolder*

### Key Components
- **SignalR Integration**: Real-time WebSocket communication
- **Chat Hub**: Central connection point for chat clients
- **Message Routing**: Tenant-aware message delivery

### Dependencies
- Microsoft.AspNetCore.SignalR (WebSocket framework)
- Abp.RealTime (presence tracking)
- inzibackend.Chat (business logic layer)

## Subfolders

### SignalR
Real-time chat implementation using SignalR
- **ChatHub**: WebSocket connection endpoint
- **SignalRChatCommunicator**: Server-to-client messaging
- **Message DTOs**: Type-safe communication contracts
- **Presence Tracking**: Online/offline status management
- **Business Value**: Enables instant communication and collaboration

## Architecture Notes
- **Real-time Communication**: WebSocket with fallback to long-polling
- **Scalable Design**: Support for Redis backplane for multi-server
- **Multi-tenant**: Complete tenant isolation
- **Event-driven**: Async message processing

## Business Logic
- **Features Supported**:
  - One-to-one messaging
  - Online presence indicators
  - Message delivery status
  - Friendship management
  - Typing indicators
  - Read receipts

## Usage Across Codebase
- Web application chat interface
- Mobile app messaging
- Admin communication tools
- System-wide notifications
- Customer support chat