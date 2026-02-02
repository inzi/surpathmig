# Webhooks Documentation

## Overview
Webhook system for publishing application events to external systems via HTTP callbacks. Enables integration with third-party services and custom automation.

## Contents

### Files

#### AppWebhookDefinitionProvider.cs
- **Purpose**: Defines all webhook events in the application
- **Key Features**:
  - Webhook event definitions
  - Event names and descriptions
  - Feature requirements
  - Payload structure definitions

#### AppWebhookPublisher.cs
- **Purpose**: Service for publishing webhook events
- **Key Features**:
  - Publish events to subscribers
  - Retry failed deliveries
  - Queue management
  - Signature generation

#### IAppWebhookPublisher.cs
- **Purpose**: Interface for webhook publishing

### Key Components

- **AppWebhookDefinitionProvider**: Event definitions
- **AppWebhookPublisher**: Event publishing service
- **Webhook Subscriptions**: Managed in database

### Dependencies

- **External Libraries**:
  - ABP Framework (webhooks module)
  - System.Net.Http

- **Internal Dependencies**:
  - Feature system
  - Multi-tenancy
  - Background jobs
  - Event bus

## Architecture Notes

- **Pattern**: Publisher-subscriber pattern
- **Delivery**: Async HTTP POST to subscriber URLs
- **Reliability**: Retry mechanism with exponential backoff
- **Security**: HMAC signature for payload verification

## Business Logic

### Webhook Lifecycle

#### Subscription
1. Tenant registers webhook URL
2. Selects events to subscribe to
3. Optionally provides secret for signatures
4. Webhook activated

#### Event Publishing
1. Event occurs in application
2. AppWebhookPublisher.PublishAsync called
3. Payload serialized to JSON
4. HMAC signature calculated
5. HTTP POST to all subscribers
6. Response validated
7. Retry if failed

#### Delivery Guarantees
- At-least-once delivery
- Retry failed deliveries (up to N times)
- Exponential backoff between retries
- Dead letter queue for permanent failures
- Webhook disabled after too many failures

### Webhook Events

#### Tenant Events
- Tenant created
- Tenant deleted
- Subscription changed
- Edition upgraded

#### User Events
- User created
- User deleted
- User activated/deactivated

#### Payment Events
- Payment received
- Subscription expired
- Subscription renewed

#### Surpath Events
- Compliance status changed
- Document uploaded
- Test result available
- Background check completed

### Payload Structure
```json
{
  "webhookEvent": "tenant.created",
  "webhookSubscriptionId": "12345",
  "data": {
    "id": 1,
    "tenancyName": "tenant1",
    "name": "Tenant 1"
  },
  "timestamp": "2025-09-29T10:00:00Z"
}
```

### Security

#### HMAC Signature
- Calculated using webhook secret
- Included in HTTP header
- Subscriber verifies signature
- Prevents tampering

#### HTTPS Required
- All webhook URLs must be HTTPS
- Prevents man-in-the-middle attacks
- Certificate validation

## Usage Across Codebase

Triggered by:
- Domain events
- Application services
- Background workers
- Payment processing
- User actions
- Admin operations

## Integration Examples

### CRM Integration
- New user → Create contact in CRM
- Subscription change → Update account in CRM

### Analytics
- Events → Send to analytics platform
- Custom dashboards and reporting

### Automation
- Compliance deadline → Trigger reminder workflow
- Payment failed → Alert accounting system

### Custom Applications
- Build external applications
- React to system events
- Extend functionality without code changes

## Management

### Webhook Subscription UI
- List active webhooks
- Create new subscriptions
- Test webhook delivery
- View delivery logs
- Enable/disable webhooks
- Regenerate secrets

### Monitoring
- Delivery success rate
- Failed deliveries
- Retry queue depth
- Response times
- Error logs

## Best Practices

### Subscriber Implementation
- Respond quickly (< 5 seconds)
- Return 200 OK on success
- Handle idempotency (same event delivered twice)
- Verify HMAC signature
- Log all webhook receipts

### Publisher Configuration
- Reasonable timeout (10-30 seconds)
- Exponential backoff (1s, 2s, 4s, 8s, ...)
- Maximum retries (5-10)
- Circuit breaker for consistently failing webhooks

## Extension Points

- Custom webhook events
- Event filtering (only send if conditions met)
- Payload transformation
- Multiple URLs per subscription
- Webhook templates
- Webhook marketplace/directory