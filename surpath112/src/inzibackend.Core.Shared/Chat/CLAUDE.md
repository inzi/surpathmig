# Chat Documentation

## Overview
Enumeration definitions for the chat messaging system, providing type-safe constants for message states and participant roles in conversations.

## Contents

### Files

#### ChatMessageReadState.cs
- **Purpose**: Defines the read status of chat messages
- **Enum Values**:
  - Unread (1): Message has not been read by recipient
  - Read (2): Message has been read by recipient
- **Usage**: Tracks message delivery and read receipts

#### ChatSide.cs
- **Purpose**: Identifies the role of a participant in a chat conversation
- **Enum Values**:
  - Sender (1): The message originator
  - Receiver (2): The message recipient
- **Usage**: Differentiates between conversation participants

### Key Components
- Explicit enum values for database storage consistency
- Clear binary states for message tracking
- Role-based participant identification

### Dependencies
- No external dependencies
- Part of the inzibackend.Chat namespace

## Architecture Notes
- Uses explicit integer values for database persistence
- Simple binary states for efficient querying
- Consistent enumeration pattern across the chat system

## Business Logic
- Messages start in Unread state when sent
- State transitions to Read when recipient views the message
- ChatSide distinguishes message direction in conversations
- Supports bidirectional communication tracking

## Usage Across Codebase
These enums are used in:
- Chat message entities and DTOs
- Real-time messaging services
- Message notification systems
- Chat history and conversation views
- Read receipt tracking
- Message filtering and queries