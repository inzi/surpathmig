# Modified
## Filename
UserFriendCacheSyncronizer.cs
## Relative Path
inzibackend.Core\Friendships\Cache\UserFriendCacheSyncronizer.cs
## Language
C#
## Summary
The modified file introduces a UserFriendCacheSyncronizer class that synchronizes friendship and chat message data with a cache. It handles creation, deletion, and updates of friendships, and tracks unread messages in chats.
## Changes
The unmodified code incorrectly lists 'IEventCreatedData' twice for ChatMessage events. The modified version corrects this typo.
## Purpose
The class synchronizes data between the entity framework and cache, ensuring consistent state across both layers.
