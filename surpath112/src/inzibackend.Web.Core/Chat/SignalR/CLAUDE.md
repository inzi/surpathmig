# SignalR Chat Documentation

## Overview
This folder contains the real-time chat implementation using SignalR for WebSocket-based communication. It provides instant messaging capabilities, online presence tracking, and friendship management in real-time across the application.

## Contents

### Files

#### ChatHub.cs
- **Purpose**: SignalR hub for real-time chat communication
- **Key Features**:
  - Extends OnlineClientHubBase for presence tracking
  - Handles message sending between users
  - Manages online/offline status
  - Multi-tenant message isolation
- **Key Methods**:
  - `SendMessage`: Sends chat messages between users
  - Connection lifecycle management (OnConnected, OnDisconnected)

#### SignalRChatCommunicator.cs
- **Purpose**: Server-side communicator for pushing chat events to clients
- **Key Features**:
  - Implements IChatCommunicator interface
  - Sends messages to specific users or groups
  - Handles friendship notifications
  - Online status broadcasting
- **Business Logic**: Routes messages to correct tenant/user connections

#### SendChatMessageInput.cs
- **Purpose**: DTO for chat message input
- **Properties**:
  - `TenantId`: Target tenant identifier
  - `UserId`: Target user identifier
  - `Message`: Message content
  - `TenancyName`: Tenant name for display
  - `UserName`: Sender's display name
  - `ProfilePictureId`: Sender's avatar

#### SendFriendshipRequestInput.cs
- **Purpose**: DTO for friendship request notifications
- **Properties**:
  - `UserId`: Target user for friendship
  - `TenantId`: Target tenant
  - Additional friendship metadata

### Key Components
- **ChatHub**: Central SignalR hub for connections
- **SignalRChatCommunicator**: Server-to-client push messaging
- **Input DTOs**: Type-safe message contracts

### Dependencies
- Microsoft.AspNetCore.SignalR (real-time communication)
- Abp.AspNetCore.SignalR (ABP SignalR integration)
- Abp.RealTime (online client management)
- inzibackend.Chat (chat business logic)

## Architecture Notes
- Hub-based architecture for WebSocket management
- Automatic reconnection handling
- Group-based message routing for scalability
- Connection state tracking per user

## Business Logic
- **Message Flow**:
  1. Client connects to ChatHub via SignalR
  2. User sends message through hub method
  3. Hub validates and processes message
  4. Message saved to database
  5. SignalRChatCommunicator pushes to recipient
  6. Real-time delivery if online, queued if offline

- **Presence Management**:
  - Tracks online/offline status
  - Notifies friends of status changes
  - Handles multiple connections per user

## Security Considerations
- Authentication required for hub connections
- Tenant isolation for all messages
- Message content validation
- Connection authorization per user

## Usage Across Codebase
- Web chat interface in MVC views
- Mobile app real-time messaging
- Admin broadcast messages
- System notifications
- Friend request notifications