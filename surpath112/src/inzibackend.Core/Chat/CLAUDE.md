# Chat Documentation

## Overview
Real-time one-on-one chat messaging system with message persistence, read state tracking, and friendship integration. Supports cross-tenant messaging for multi-tenant chat capabilities.

## Contents

### Files

#### ChatMessage.cs
- **Purpose**: Entity representing a chat message
- **Table**: AppChatMessages
- **Key Properties**:
  - `UserId` / `TenantId`: Message sender
  - `TargetUserId` / `TargetTenantId`: Message recipient
  - `Message`: Message content (max 4KB)
  - `Side`: Sender or Receiver perspective
  - `ReadState`: Read status for sender side
  - `ReceiverReadState`: Read status for receiver side
  - `SharedMessageId`: Links sender/receiver copies
  - `CreationTime`: Message timestamp
- **Pattern**: Dual-record storage (one for sender, one for receiver)

#### ChatMessageManager.cs
- **Purpose**: Business logic for chat operations
- **Key Features**:
  - Send message (creates two records)
  - Mark messages as read
  - Query message history
  - Unread count calculation
  - Message validation

#### IChatMessageManager.cs
- **Purpose**: Interface for chat operations

#### ChatFeatureChecker.cs
- **Purpose**: Checks if chat feature is enabled
- **Use**: Feature flag checking for tenant

#### IChatFeatureChecker.cs
- **Purpose**: Interface for feature checking

#### ChatCommunicator.cs / IChatCommunicator.cs
- **Purpose**: Real-time communication abstraction
- **Implementation**: SignalR hub communication

#### NullChatCommunicator.cs
- **Purpose**: Null object pattern implementation for non-real-time scenarios

#### ChatChannel.cs
- **Purpose**: Enum or model for chat channels

### Key Components

- **ChatMessage**: Message entity with dual-record pattern
- **ChatMessageManager**: Business logic layer
- **ChatCommunicator**: Real-time delivery abstraction
- **ChatFeatureChecker**: Feature flag validation

### Dependencies

- **External Libraries**:
  - ABP Framework (domain entities)
  - SignalR (real-time communication)

- **Internal Dependencies**:
  - Friendship system
  - User management
  - Friend cache
  - Feature management

## Architecture Notes

- **Pattern**: Dual-record messaging (sender and receiver copies)
- **Real-Time**: SignalR integration via IChatCommunicator
- **Multi-Tenancy**: Cross-tenant chat support
- **Persistence**: All messages stored in database

## Business Logic

### Message Sending Flow
1. Validate sender and receiver are friends
2. Create sender-side message record (Side=Sender, ReadState=Read)
3. Create receiver-side message record (Side=Receiver, ReadState=Unread)
4. Link records with SharedMessageId
5. Send real-time notification via SignalR
6. Update friend cache unread count

### Message Reading Flow
1. User opens chat with friend
2. Query messages for conversation
3. Mark receiver-side messages as read
4. Update friend cache (reset unread count)
5. Notify sender via SignalR (optional)

### Dual-Record Pattern
- Each message stored twice
- Allows independent deletion per user
- Separate read states
- Enables per-user message history
- Privacy: each user controls their copy

### Cross-Tenant Messaging
- TenantId can differ between users
- Friendships can span tenants
- Enables inter-organizational communication
- Requires friendship establishment first

## Usage Across Codebase

Used by:
- Chat UI (real-time messaging)
- SignalR hubs (Web.Core/Chat)
- Friendship system
- Notification system
- Unread badge display
- Message history views

## Security Considerations

### Access Control
- Only friends can message each other
- Friendship validation before sending
- Tenant isolation when required
- User authentication required

### Message Content
- Maximum length: 4KB
- HTML encoding required
- No file attachments (text only)
- XSS protection in UI

### Privacy
- Users control their message copies
- Message deletion doesn't affect other user
- Read receipts via ReadState
- No message editing (by design)

## Database Structure

### Message Records
- Two records per message
- Indexed by UserId and TargetUserId
- Filtered by tenancy
- Ordered by CreationTime

### Queries
- Conversation history: Filter by user pair
- Unread count: Count where ReadState=Unread
- Recent messages: Order by CreationTime DESC

## Real-Time Features

### SignalR Integration
- New message notifications
- Typing indicators (optional)
- Online/offline status
- Read receipts

### Event Flow
1. User sends message
2. Message saved to database
3. SignalR hub notified
4. Hub pushes to receiver's connection
5. Receiver UI updates instantly

## Extension Points

- File attachments
- Message reactions (emoji)
- Message editing (with history)
- Message search
- Group chat support
- Message encryption
- Voice/video integration
- Message threading