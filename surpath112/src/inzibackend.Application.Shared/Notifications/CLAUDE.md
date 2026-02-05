# Notifications Documentation

## Overview
This folder contains service interfaces and DTOs for the notification system. The notification system provides multi-channel alerts (in-app, email, SMS, push) for important events with user-configurable preferences.

## Contents

### Files
Service interface for notification operations (INotificationAppService.cs or similar)

### Subfolders

#### Dto
Complete notification DTOs including retrieval, settings, and subscription management.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Multi-channel delivery (in-app, email, SMS, push)
- Publisher-subscriber pattern
- Background job processing
- SignalR real-time push

## Business Logic
Send notifications, manage notification preferences, mark as read, delivery tracking, subscription management.

## Usage Across Codebase
Notification center UI, event publishers throughout application, email/SMS services, SignalR hubs

## Cross-Reference Impact
Changes affect notification display, preference settings, and notification delivery across all channels