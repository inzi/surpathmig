# Chat DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the real-time chat messaging system. These DTOs support user-to-user instant messaging including message sending, retrieval, read state tracking, message history, and chat user listings. The chat system enables direct communication between users within and across tenants.

## Contents

### Files

#### Message DTOs
- **ChatMessageDto.cs** - Individual chat message:
  - UserId, TenantId - Sender identification
  - TargetUserId, TargetTenantId - Recipient identification
  - Side - Whether message is sent or received (ChatSide enum)
  - ReadState - Sender's read state
  - ReceiverReadState - Recipient's read state
  - Message - Message content (text)
  - CreationTime - Timestamp
  - SharedMessageId - Unique ID shared between sender/receiver copies
  - Supports cross-tenant messaging

- **ChatMessageExportDto.cs** - Message data for export/archival purposes

- **ChatUserDto.cs** - Chat participant information:
  - User profile data
  - Online status
  - Unread message count
  - Last message info

- **ChatUserWithMessagesDto.cs** - Chat participant with message history:
  - Combines user info with recent messages
  - Used for chat window initialization

#### Input DTOs
- **GetUserChatMessagesInput.cs** - Message history query:
  - TenantId and UserId of chat partner
  - Paging parameters for message history
  - MinMessageId for incremental loading

- **MarkMessagesAsReadInput.cs** - Bulk read status update:
  - TenantId and UserId of chat partner
  - Marks all messages as read from specified user

#### Output DTOs
- **GetUserChatFriendsWithSettingsOutput.cs** - Complete chat initialization:
  - List of chat contacts
  - Chat settings and preferences
  - Unread counts per contact

### Key Components

#### Enumerations (from inzibackend.Core.Shared)
- **ChatSide** - Sender or Receiver perspective
- **ChatMessageReadState** - Unread or Read status

#### Multi-Tenant Chat
- Messages can cross tenant boundaries
- Each message stores both sender and receiver tenant IDs
- Enables communication between users in different organizations

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **inzibackend.Dto** - Paging and filtering base DTOs
- **inzibackend.Core.Shared.Chat** - Chat enumerations

## Architecture Notes

### Message Duplication Pattern
- Each message stored twice: once for sender, once for receiver
- SharedMessageId links the two copies
- Allows independent read states and deletion
- Enables per-user message retention policies

### Read State Tracking
- **ReadState**: Sender's own read tracking (for sent messages)
- **ReceiverReadState**: Recipient's read status
- Enables "delivered" and "read" indicators
- Supports typing indicators and presence

### Real-Time Updates
- DTOs designed for SignalR real-time push
- ChatMessageDto sent immediately on send
- Read receipts pushed via MarkMessagesAsReadInput
- Online status updates via ChatUserDto

### Performance Optimization
- Paging support for message history
- MinMessageId for incremental loading
- Unread counts cached in ChatUserDto
- Recent messages included in ChatUserWithMessagesDto to minimize queries

## Business Logic

### Chat Workflow
1. **User Opens Chat**: GetUserChatFriendsWithSettingsOutput loads contacts
2. **Select Contact**: GetUserChatMessagesInput loads message history
3. **Send Message**: ChatMessageDto created and pushed via SignalR
4. **Receive Message**: Real-time ChatMessageDto delivery to recipient
5. **Mark Read**: MarkMessagesAsReadInput clears unread badges
6. **Export History**: ChatMessageExportDto for archival/compliance

### Cross-Tenant Communication
- Users in Tenant A can chat with users in Tenant B
- Useful for multi-school district scenarios
- Supports vendor-customer communication
- Enables host-tenant support chat

### Read Receipts
- Messages initially created as Unread
- Recipient marks messages as Read when viewed
- Sender sees ReceiverReadState change
- Provides delivery and read confirmation

### Chat Friends List
- Shows users with recent conversations
- Displays unread message counts
- Shows online/offline status
- Sorted by most recent activity

## Usage Across Codebase
These DTOs are consumed by:
- **IChatAppService** - Chat message services
- **SignalR ChatHub** - Real-time message delivery
- **Chat UI Components** - Message display and input
- **Notification System** - Unread message badges
- **Mobile Apps** - Push notifications for messages
- **Export Services** - Message history archival

## Cross-Reference Impact
Changes to these DTOs affect:
- Chat interface and message display
- Real-time SignalR message contracts
- Mobile app chat features
- Notification badges and counts
- Message export functionality
- Cross-tenant communication features
- Read receipt indicators