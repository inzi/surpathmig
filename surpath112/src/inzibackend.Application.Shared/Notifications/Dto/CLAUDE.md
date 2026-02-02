# Notifications DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the notification system. These DTOs manage user notifications including in-app alerts, email notifications, push notifications, and notification preferences. The system provides real-time and asynchronous notifications for important events like requirement expirations, document approvals, payment reminders, and system alerts.

## Contents

### Files

#### Notification Retrieval
- **GetUserNotificationsInput.cs** - Query user notifications:
  - State - Unread, Read, or All
  - StartDate, EndDate - Date range filtering
  - Paging support for notification lists

- **GetNotificationsOutput.cs** - Notification list:
  - UnreadCount - Count of unread notifications
  - TotalCount - All notifications
  - Items - List of notification DTOs with content

- **SetNotificationAsReadOutput.cs** - Mark notification read result

- **DeleteAllUserNotificationsInput.cs** - Clear notification parameters:
  - State filter - Delete only read/unread
  - Date range for selective deletion

#### Notification Settings
- **GetNotificationSettingsOutput.cs** - User notification preferences:
  - List of NotificationSubscriptionDto
  - Per-notification-type preferences
  - Delivery channel settings (email, SMS, push, in-app)

- **UpdateNotificationSettingsInput.cs** - Update preferences:
  - NotificationName - Type of notification
  - IsSubscribed - Enable/disable notification
  - Used to customize notification delivery

- **NotificationSubscriptionDto.cs** - Notification subscription:
  - Name - Notification type identifier
  - DisplayName - User-friendly name
  - Description - Notification purpose
  - IsSubscribed - User's subscription status

- **NotificationSubscriptionWithDisplayNameDto.cs** - Extended subscription with localized display

### Key Components

#### Notification Types (from Core.Shared)
Common notifications:
- RequirementExpiring - Requirement approaching expiration
- DocumentUploaded - Student uploaded document
- DocumentApproved/Rejected - Document review complete
- PaymentDue - Payment reminder
- SystemAlert - Important system messages
- NewMessage - Chat message received

#### Delivery Channels
- In-App - Real-time browser notifications
- Email - Email delivery
- SMS - Text message alerts
- Push - Mobile push notifications

#### Notification States
- Unread - Not yet viewed
- Read - User has viewed
- Deleted - Removed from inbox

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **Abp.Notifications** - ABP notification infrastructure
- **inzibackend.Dto** - Paging base DTOs

## Architecture Notes

### Notification Architecture
- **Publisher-Subscriber**: Services publish events, subscribers receive notifications
- **Multi-Channel**: Single notification sent to multiple channels
- **Queued Delivery**: Background jobs for email/SMS sending
- **Real-Time**: SignalR for immediate in-app notifications

### Notification Persistence
- In-app notifications stored in database
- Email/SMS tracked for delivery status
- Notification history for audit trail
- Retention policies for old notifications

### Subscription Management
- Users customize which notifications they receive
- Per-channel subscriptions (email vs in-app)
- Tenant-wide default settings
- Critical notifications cannot be unsubscribed

## Business Logic

### Notification Workflow
1. **Event Occurs**: System event (document upload, requirement expiration, etc.)
2. **Publisher Triggered**: NotificationPublisher sends notification
3. **Subscribers Identified**: Find users subscribed to notification type
4. **Multi-Channel Delivery**:
   - In-app: Inserted into database, pushed via SignalR
   - Email: Queued for background email sending
   - SMS: Queued for SMS gateway
   - Push: Sent to mobile devices
5. **User Views**: Notification marked as read
6. **Cleanup**: Old notifications deleted per retention policy

### User Notification Management
1. User opens notification center
2. GetUserNotificationsInput retrieves notifications
3. User clicks notification (marked as read)
4. User configures preferences via GetNotificationSettingsOutput
5. UpdateNotificationSettingsInput saves preferences
6. User deletes notifications via DeleteAllUserNotificationsInput

### Notification Personalization
- Per-user notification preferences
- Timezone-aware delivery scheduling
- Language-specific notification content
- Frequency limiting (digest mode)

### Critical Notifications
- Some notifications mandatory (security alerts)
- Cannot be unsubscribed
- Always delivered to all channels
- Highlighted in UI

## Usage Across Codebase
These DTOs are consumed by:
- **INotificationAppService** - Notification CRUD operations
- **Notification Center UI** - In-app notification display
- **User Settings** - Notification preferences
- **NotificationPublisher** - Event-driven notification sending
- **Background Jobs** - Email/SMS delivery
- **SignalR Hubs** - Real-time push
- **Mobile Apps** - Push notification handling

## Cross-Reference Impact
Changes to these DTOs affect:
- Notification center interface
- Notification preference settings
- Email notification templates
- SMS notification sending
- Push notification delivery
- Real-time notification updates
- Notification subscription management
- Notification history displays