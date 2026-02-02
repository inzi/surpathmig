# Friendships Documentation

## Overview
Social friendship system enabling user-to-user connections, friend requests, and relationship management. Foundation for chat and social features.

## Contents

### Files

#### Friendship.cs
- **Purpose**: Entity representing a friendship between two users
- **Key Properties**:
  - UserId / TenantId (user 1)
  - FriendUserId / FriendTenantId (user 2)
  - State (Accepted, Pending, Blocked)
  - Friend details (username, profile picture, tenancy name)
  - Creation time
- **Pattern**: Bidirectional relationships (two records per friendship)

#### FriendshipManager.cs
- **Purpose**: Business logic for friendship operations
- **Key Features**:
  - Send friend request
  - Accept/reject request
  - Block/unblock user
  - Query friendships
  - Validate cross-tenant friendships

#### IFriendshipManager.cs
- **Purpose**: Interface for friendship operations

#### FriendshipExtensions.cs
- **Purpose**: Extension methods for friendship queries
- **Methods**: Helper methods for common friendship operations

#### ChatUserStateWatcher.cs
- **Purpose**: Monitors online/offline state of friends
- **Features**: Real-time presence tracking for chat

### Subfolders

#### Cache/
- Friend list caching
- Unread message counts
- Performance optimization
- See [Cache/CLAUDE.md](Cache/CLAUDE.md)

### Key Components

- **Friendship**: Bidirectional relationship entity
- **FriendshipManager**: Relationship business logic
- **ChatUserStateWatcher**: Presence tracking
- **UserFriendsCache**: Performance layer

### Dependencies

- **External Libraries**:
  - ABP Framework (domain entities)

- **Internal Dependencies**:
  - User system
  - Chat system
  - Multi-tenancy
  - SignalR (for presence)

## Architecture Notes

- **Pattern**: Bidirectional friendship model
- **State Machine**: Pending → Accepted / Rejected / Blocked
- **Cross-Tenant**: Friends can be in different tenants
- **Caching**: Friend lists cached for performance

## Business Logic

### Friendship Lifecycle

#### Friend Request
1. User A sends request to User B
2. Two friendship records created (both Pending)
3. Notification sent to User B
4. User B sees pending request

#### Acceptance
1. User B accepts request
2. Both records updated to Accepted
3. Notification sent to User A
4. Chat enabled between users
5. Friend appears in both friend lists

#### Rejection
1. User B rejects request
2. Both records deleted
3. User A notified (optional)

#### Blocking
1. User A blocks User B
2. Friendship state updated to Blocked
3. Chat disabled
4. User B cannot send new requests

### Bidirectional Records
- Each friendship has two records
- One from each user's perspective
- Allows independent management
- Each user can delete their side
- Symmetric state updates

### Cross-Tenant Friendships
- Users from different tenants can be friends
- Enables inter-organizational communication
- Requires both tenants to allow it
- Useful for partnerships, collaborations

### Friend States
- **Pending**: Request sent, awaiting response
- **Accepted**: Active friendship
- **Blocked**: User blocked by other

## Usage Across Codebase

Used by:
- Chat system (friend validation)
- User search (exclude non-friends)
- Notifications
- Profile viewing permissions
- Social features
- Online presence

## Security Considerations

### Privacy
- Users control own friend list
- Block functionality for unwanted contact
- Privacy settings per user
- Friend-only features optional

### Cross-Tenant
- Validate tenant permissions
- Respect tenant isolation settings
- Audit cross-tenant friendships
- Admin oversight capability

## Performance

### Caching Strategy
- Friend lists cached per user
- Cache invalidation on changes
- Unread counts in cache
- Reduces database queries significantly

### Queries
- Indexed by UserId
- Filtered by State
- Sorted by creation time
- Pagination for large friend lists

## Extension Points

- Friend groups/lists
- Friend suggestions
- Mutual friends display
- Friend activity feed
- Friend limits per user
- Friendship analytics