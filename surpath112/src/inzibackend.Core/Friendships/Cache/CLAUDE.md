# Friendships Cache Documentation

## Overview
High-performance caching layer for the friendship system, optimizing friend list queries and unread message counts for the real-time chat feature. Implements thread-safe cache operations with automatic synchronization.

## Contents

### Files

#### UserFriendsCache.cs
- **Purpose**: Main cache implementation for user friendships
- **Key Features**:
  - Singleton cache manager
  - Lazy loading of friend lists
  - Unread message tracking
  - Friend list modifications (add/remove/update)
  - Thread-safe operations with locking
  - Unit of work integration
  - Multi-tenancy support
- **Dependencies**: FriendshipRepository, ChatMessageRepository, UserStore, TenantCache

#### IUserFriendsCache.cs
- **Purpose**: Interface for friend cache operations
- **Methods**:
  - `GetCacheItem`: Get or load cache item
  - `GetCacheItemOrNull`: Get existing cache item without loading
  - `ResetUnreadMessageCount`: Clear unread count for a friend
  - `IncreaseUnreadMessageCount`: Increment unread count
  - `AddFriend`: Add friend to cache
  - `RemoveFriend`: Remove friend from cache
  - `UpdateFriend`: Update friend information

#### UserWithFriendsCacheItem.cs
- **Purpose**: Cache model representing a user with their friend list
- **Properties**: (inferred)
  - UserId: User identifier
  - TenantId: Tenant identifier
  - UserName: User's username
  - TenancyName: Tenant name
  - ProfilePictureId: User's profile picture
  - Friends: List of FriendCacheItem objects

#### FriendCacheItem.cs
- **Purpose**: Cache model representing a single friend
- **Properties**: (inferred)
  - FriendUserId: Friend's user ID
  - FriendTenantId: Friend's tenant ID (cross-tenant friends)
  - FriendUserName: Friend's username
  - FriendTenancyName: Friend's tenant name
  - FriendProfilePictureId: Friend's profile picture
  - State: Friendship state (enum)
  - UnreadMessageCount: Number of unread messages

#### FriendCacheItemExtensions.cs
- **Purpose**: Extension methods for friend cache items
- **Methods**: (inferred)
  - `ContainsFriend`: Check if friend list contains a specific friend
  - Other helper methods for cache item manipulation

#### UserFriendCacheSyncronizer.cs (Note: Typo in filename)
- **Purpose**: Event handler to synchronize cache with database changes
- **Function**: Listens to friendship/message events and updates cache accordingly
- **Pattern**: Cache-aside pattern with automatic invalidation

### Key Components

- **UserFriendsCache**: Core cache manager (singleton)
- **Cache Items**: Serializable DTOs for caching
- **Synchronizer**: Event-driven cache invalidation
- **Extensions**: Helper methods for cache operations

### Dependencies

- **External Libraries**:
  - ABP Framework (caching, UoW, multi-tenancy)

- **Internal Dependencies**:
  - `Friendship` entity and repository
  - `ChatMessage` entity and repository
  - `UserStore` (ASP.NET Identity)
  - `ITenantCache` (tenant caching)

## Architecture Notes

- **Pattern**: Cache-aside with lazy loading
- **Lifecycle**: Singleton (shared across application)
- **Thread Safety**: Lock-based synchronization
- **Invalidation**: Event-driven automatic updates
- **Performance**: In-memory caching with ABP cache manager

## Business Logic

### Cache Loading Flow
1. Request friend list for user
2. Check cache (`GetCacheItemOrNull`)
3. If not cached, query database
4. Build FriendCacheItem list with unread counts
5. Query user details
6. Store in cache
7. Return cache item

### Unread Message Tracking
- Count calculated from ChatMessage table
- Filtered by: ReadState=Unread, Side=Receiver
- Cached per friend per user
- Updated on message send/read events

### Thread Safety
- `_syncObj` lock for list modifications
- Prevents race conditions on friend list updates
- Lock scope minimized for performance

### Multi-Tenancy
- Friends can be cross-tenant
- Cache keyed by `UserIdentifier` (includes TenantId)
- Queries respect current tenant context

## Usage Across Codebase

Used by:
- Real-time chat system (SignalR hubs)
- Friend list UI rendering
- Unread message badge display
- Chat notification system
- Friendship management features

## Performance Considerations

### Cache Benefits
- Reduces database queries for friend lists
- Fast friend lookups (O(1) cache access)
- Efficient unread count tracking
- Reduces load on chat system

### Cache Strategy
- Lazy loading (only cache on first access)
- Keep alive until invalidated
- No expiration time (event-driven invalidation)
- Lock only during modifications

### Database Queries
- Single query to load all friendships
- Subquery for unread counts
- User details lookup
- All within unit of work

## Cache Invalidation Events

Triggered by:
- New friendship created → AddFriend
- Friendship deleted → RemoveFriend
- Friend status changed → UpdateFriend
- Message sent → IncreaseUnreadMessageCount
- Messages read → ResetUnreadMessageCount
- User profile updated → UpdateFriend

## Extension Points

- Add more cached user properties
- Implement cache expiration policies
- Add additional cache indexes
- Extend synchronization to more events