# WebHooks Documentation

## Overview
This folder contains service interfaces and DTOs for the webhook system. Webhooks enable external systems to receive real-time notifications of application events for integration and automation.

## Contents

### Files
Service interface for webhook operations (IWebhookEventAppService.cs, IWebhookSubscriptionAppService.cs or similar)

### Subfolders

#### Dto
Complete webhook DTOs including subscriptions, available events, and delivery tracking.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Async background delivery
- Retry with exponential backoff
- HMAC signature verification
- Delivery attempt logging

## Business Logic
Configure webhook endpoints, subscribe to events, deliver events to external systems, retry failed deliveries, monitor delivery status.

## Usage Across Codebase
Webhook configuration UI, event publishers, background jobs, webhook controllers, integration services

## Cross-Reference Impact
Changes affect webhook configuration, event delivery, integration endpoints, and external system notifications