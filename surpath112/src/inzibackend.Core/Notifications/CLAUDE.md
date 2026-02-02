# Notifications Documentation

## Overview
Real-time notification system for user alerts, system messages, and event notifications. Supports multiple notification channels and persistence.

## Contents

### Files

#### AppNotificationProvider.cs
- **Purpose**: Defines all notification types in the application
- **Key Features**:
  - Notification definitions
  - Display names
  - Descriptions
  - Permission requirements
  - Feature dependencies

#### AppNotifier.cs
- **Purpose**: Service for sending notifications to users
- **Key Features**:
  - Send to single user
  - Send to multiple users
  - Send to all users in tenant
  - Notification formatting
  - Real-time delivery via SignalR

#### IAppNotifier.cs
- **Purpose**: Interface for notification service

### Key Components

- **AppNotificationProvider**: Notification definitions
- **AppNotifier**: Notification sending service
- **Notification Types**: Predefined notification categories

### Dependencies

- **External Libraries**:
  - ABP Framework (notifications module)
  - SignalR (real-time delivery)

- **Internal Dependencies**:
  - User system
  - Permission system
  - Feature system
  - Multi-tenancy

## Architecture Notes

- **Pattern**: Provider pattern for definitions, service pattern for sending
- **Delivery**: Real-time via SignalR + database persistence
- **Multi-Channel**: Web UI, email, SMS (configurable)
- **Persistence**: All notifications stored in database

## Business Logic

### Notification Types

#### System Notifications
- Subscription expiring
- Payment received
- System maintenance
- Feature enabled/disabled

#### User Notifications
- Friend request
- New message
- Mention in comment
- Document approved

#### Surpath Notifications
- Compliance deadline approaching
- Document expiring
- Test result available
- Background check complete

### Notification Flow
1. Event occurs in system
2. AppNotifier.SendNotification called
3. Notification persisted to database
4. Real-time push via SignalR (if user online)
5. Badge count updated
6. Optional email/SMS delivery
7. User marks as read (or auto-expires)

### Delivery Channels
- **Web**: Real-time SignalR push
- **Database**: Persistent storage
- **Email**: Optional email notification
- **SMS**: Optional SMS for critical alerts

### Notification States
- **Unread**: New notification
- **Read**: User has viewed
- **Deleted**: User dismissed (soft delete)

## Usage Across Codebase

Triggered by:
- Domain events
- Background workers
- Payment processing
- Subscription changes
- User actions
- Admin operations
- Compliance checks
- Document workflow

## Notification Categories

### Priority Levels
- Information
- Warning
- Error
- Success

### Targeting
- Single user
- Multiple users
- All users in tenant
- All users in role
- Broadcast to all

## Security Considerations

- Permission-based notification access
- Tenant isolation
- No cross-tenant notifications (unless intentional)
- Content sanitization
- Rate limiting on sending

## User Experience

### Notification UI
- Bell icon with badge count
- Dropdown list of recent notifications
- Mark as read/unread
- Delete notifications
- Filter by type/date
- Link to related content

### Settings
- User can enable/disable notification types
- Choose delivery channels per type
- Do not disturb mode
- Email digest frequency

## Extension Points

- Custom notification types
- Additional delivery channels (push notifications, Slack, Teams)
- Notification templates
- Smart notification grouping
- Notification scheduling
- Bulk notification management