# WebHooks DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the webhook system. Webhooks enable external systems to receive real-time notifications of events in the Surpath application. This allows integration with third-party systems, custom workflows, and automation tools by pushing event data to configured HTTP endpoints.

## Contents

### Files

#### Subscription Management
- **GetAllSubscriptionsOutput.cs** - List webhook subscriptions:
  - Active webhook endpoints
  - Subscribed event types
  - Subscription status

- **ActivateWebhookSubscriptionInput.cs** - Enable/disable subscription:
  - SubscriptionId - Webhook to activate/deactivate
  - IsActive - Enable/disable flag
  - Used for temporarily disabling webhooks

#### Available Webhooks
- **GetAllAvailableWebhooksOutput.cs** - List subscribable events:
  - Event names and descriptions
  - Event payload schemas
  - Used when configuring new webhook

#### Send Attempts & Delivery
- **GetAllSendAttemptsInput.cs** - Query webhook delivery history:
  - Date range filtering
  - Status filtering (success, failed, pending)
  - Subscription filtering
  - Paging support

- **GetAllSendAttemptsOutput.cs** - Webhook delivery attempts:
  - Timestamp
  - HTTP status code
  - Response body
  - Success/failure status

- **GetAllSendAttemptsOfWebhookEventInput.cs** - Query attempts for specific event:
  - WebhookEventId - Specific event
  - See all delivery attempts for one event

- **GetAllSendAttemptsOfWebhookEventOutput.cs** - Event-specific delivery attempts:
  - Multiple attempts if delivery failed
  - Retry history
  - Final delivery status

### Key Components

#### Webhook Events
Common events in Surpath:
- RequirementAdded - New requirement created
- RequirementExpiring - Requirement approaching expiration
- DocumentUploaded - Student uploaded document
- DocumentApproved - Document review approved
- DocumentRejected - Document review rejected
- CohortUserCreated - New student enrolled
- PaymentReceived - Payment completed
- ComplianceStatusChanged - Student compliance status updated

#### Webhook Payload
- Event name
- Event timestamp
- Tenant information
- Event-specific data (entity ID, changes, etc.)
- Signature for verification

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **Abp.Webhooks** - ABP webhook infrastructure
- **inzibackend.Dto** - Paging DTOs

## Architecture Notes

### Webhook Delivery
- **Async Processing**: Background jobs send webhooks
- **Retry Logic**: Failed deliveries retried with exponential backoff
- **Timeout**: 30-second timeout per delivery attempt
- **Signature**: HMAC signature for payload verification

### Security
- HTTPS required for webhook URLs
- Shared secret for signature verification
- IP whitelist option
- Rate limiting per subscription

### Reliability
- Delivery attempts logged
- Failed deliveries retried (3-5 attempts)
- Final failure notifies administrator
- Manual retry option

### Event Filtering
- Subscribe to specific events only
- Filter by entity type
- Filter by tenant (for host webhooks)

## Business Logic

### Webhook Setup Workflow
1. Admin navigates to webhook settings
2. Clicks "Add Webhook"
3. Enters webhook URL (https://example.com/webhook)
4. Selects events from GetAllAvailableWebhooksOutput
5. Enters shared secret for signature
6. Activates webhook
7. System begins sending events to URL

### Webhook Delivery Workflow
1. Event occurs (e.g., document uploaded)
2. Webhook system triggered
3. Finds subscriptions for event type
4. Generates payload with event data
5. Signs payload with HMAC
6. POSTs to webhook URL
7. Logs attempt in GetAllSendAttemptsOutput
8. If delivery fails:
   - Retry with exponential backoff
   - Log each attempt
   - After max retries, mark as failed
   - Notify administrator

### Monitoring Webhooks
1. Admin opens webhook monitoring
2. Views GetAllSendAttemptsOutput for delivery history
3. Filters by status (failed deliveries)
4. Reviews error messages
5. Manually retries failed webhooks
6. Deactivates problematic webhooks

### Integration Examples
- **Slack**: Send notifications to Slack channel
- **Zapier**: Trigger Zapier workflows
- **Custom CRM**: Update CRM when student enrolled
- **Analytics**: Send events to analytics platform
- **Backup System**: Archive events to external storage

## Usage Across Codebase
These DTOs are consumed by:
- **IWebhookEventAppService** - Webhook event operations
- **IWebhookSubscriptionAppService** - Subscription management
- **Webhook Configuration UI** - Setup and management interface
- **Webhook Send Service** - Delivery logic
- **Background Jobs** - Async webhook sending
- **Retry Service** - Failed delivery retries

## Cross-Reference Impact
Changes to these DTOs affect:
- Webhook configuration interface
- Webhook delivery monitoring
- Integration with external systems
- Event payload structure
- Retry logic configuration
- Delivery attempt logging
- Webhook subscription management