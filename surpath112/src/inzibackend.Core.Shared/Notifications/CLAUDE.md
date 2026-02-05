# Notifications Documentation

## Overview
Centralized notification name constants for the application's notification system, supporting both system-wide and domain-specific notifications.

## Contents

### Files

#### AppNotificationNames.cs
- **Purpose**: Constants for all notification types in the application
- **Categories**:
  - **System Messages**: SimpleMessage, WelcomeToTheApplication
  - **User Events**: NewUserRegistered, DownloadInvalidImportUsers
  - **Tenant Events**: NewTenantRegistered, TenantsMovedToEdition
  - **Compliance/GDPR**: GdprDataPrepared
  - **Record Management**: Multiple record state notifications
  - **Support**: TicketUpdated
  - **Meta**: YouHaveUnreadNotifications
- **Naming Convention**: "App." prefix for all notifications

### Key Components

#### System Notifications
- SimpleMessage: Generic notification type
- WelcomeToTheApplication: New user onboarding
- NewUserRegistered: Admin notification for new users
- NewTenantRegistered: Admin notification for new tenants

#### Record State Notifications
- RecordStateChanged: Generic state change notification
- RecordStateExpirationWarning: General expiration warning
- RecordStateExpirationFirstWarning: First expiration alert
- RecordStateExpirationSecondWarning: Second expiration alert
- RecordStateExpirationFinalWarning: Final expiration alert
- RecordStateExpirationExpired: Expiration occurred
- NewRecordUploaded: New document/record uploaded

### Dependencies
- No external dependencies
- Part of the inzibackend.Notifications namespace

## Architecture Notes
- Consistent "App." prefix for namespace organization
- Progressive warning system for expirations
- Supports both real-time and queued notifications
- Clear categorization of notification types

## Business Logic
- **Expiration Warnings**: Multi-stage warning system (First → Second → Final → Expired)
- **User Journey**: Welcome → Registration → Ongoing notifications
- **Compliance**: GDPR data preparation notifications
- **Record Lifecycle**: Upload → State changes → Expiration tracking

## Usage Across Codebase
These notification names are used in:
- Notification service implementations
- Real-time SignalR hubs
- Email notification templates
- In-app notification displays
- Notification preference settings
- Event handlers and domain events
- Background jobs for scheduled notifications
- Notification history tracking