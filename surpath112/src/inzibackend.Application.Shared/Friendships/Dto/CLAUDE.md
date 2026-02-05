# Friendships DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the user friendship and social connection system. Friendships enable users to connect with each other, manage contacts, send friend requests, and control blocked users. This social layer supports features like chat, activity feeds, and user discovery.

## Contents

### Files

#### Core DTOs
- **FriendshipDto.cs** - Friendship relationship:
  - FriendUserId - The friend's user ID
  - FriendTenantId - Friend's tenant (for cross-tenant friendships)
  - FriendUserName - Friend's username
  - FriendProfilePictureId - Friend's avatar
  - State - Friendship state (Accepted, Blocked, etc.)
  - CreationTime - When friendship created

#### Request Management
- **CreateFriendshipRequestInput.cs** - Send friend request:
  - UserId and TenantId of target user
  - Creates pending friendship request

- **CreateFriendshipRequestByUserNameInput.cs** - Send friend request by username:
  - TenancyName and UserName of target
  - Allows friendly request without knowing user ID

- **AcceptFriendshipRequestInput.cs** - Accept pending friend request:
  - UserId and TenantId of requester
  - Converts pending request to active friendship

#### Friend Management
- **RemoveFriendInput.cs** - Unfriend a user:
  - UserId and TenantId of friend to remove
  - Deletes friendship relationship

#### Blocking
- **BlockUserInput.cs** - Block a user:
  - UserId and TenantId to block
  - Prevents messages and interaction

- **UnblockUserInput.cs** - Unblock a previously blocked user:
  - UserId and TenantId to unblock
  - Restores ability to interact

### Key Components

#### Friendship States (from Core.Shared)
- **Accepted** - Active friendship
- **Blocked** - User blocked by current user
- Pending states handled by request system

#### Cross-Tenant Friendships
- Users in different tenants can be friends
- Enables communication across organizational boundaries
- Useful for multi-school districts, vendor relationships

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **inzibackend.Core.Shared.Friendships** - FriendshipState enum

## Architecture Notes

### Bi-Directional Relationships
- Friendship stored twice (once per user)
- Each user has their own copy with their perspective
- Allows independent blocking and removal
- Enables per-user friendship settings

### Request/Accept Flow
- User A sends request to User B
- Request pending until User B accepts
- On accept, both users get friendship records
- On reject, request deleted

### Blocking Behavior
- Blocked user cannot send messages
- Blocked user's messages hidden
- Blocking is one-sided (User A blocks B, but B doesn't know)
- Can unblock to restore communication

## Business Logic

### Friendship Workflow
1. **Send Request**: User A uses CreateFriendshipRequestInput for User B
2. **Notification**: User B notified of friend request
3. **Accept**: User B uses AcceptFriendshipRequestInput
4. **Active Friendship**: Both users now have FriendshipDto
5. **Chat Enabled**: Users can now message each other
6. **Remove**: Either user can use RemoveFriendInput to unfriend

### Blocking Workflow
1. **Block User**: User A uses BlockUserInput for User B
2. **Communication Blocked**: B cannot message A
3. **Hidden Content**: A doesn't see B's messages/activity
4. **Unblock**: A uses UnblockUserInput to restore
5. **Optional Re-Friend**: Users can become friends again after unblock

### Friend Discovery
- Find users via search (FindUsersInput in Common)
- Send friend request to discovered users
- Build social network organically

### Chat Integration
- Friends list populates chat contacts
- Only friends can send messages (depending on privacy settings)
- Friend status shows in chat user list
- Blocked users excluded from chat

## Usage Across Codebase
These DTOs are consumed by:
- **IFriendshipAppService** - Friendship CRUD operations
- **Chat System** - Contact list population
- **User Profile** - Friend list display
- **Notification System** - Friend request notifications
- **Social Features** - Activity feeds, user discovery
- **Mobile Apps** - Contacts and messaging

## Cross-Reference Impact
Changes to these DTOs affect:
- Friend management interfaces
- Friend request notifications
- Chat contact lists
- User blocking functionality
- Social features throughout application
- Cross-tenant communication
- Privacy and security settings