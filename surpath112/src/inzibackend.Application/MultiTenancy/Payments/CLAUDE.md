# Multi-Tenancy Payment Services Documentation

## Overview
Payment processing services for tenant subscriptions supporting multiple payment gateways: Authorize.Net, PayPal, and Stripe. Handles subscription payments, upgrades, renewals, and payment method management in the SaaS platform.

## Contents

### Files
- **PaymentAppService.cs**: Base payment service coordinating all payment operations
- **AuthorizeNetPaymentAppService.cs**: Authorize.Net credit card processing
- **PayPalPaymentAppService.cs**: PayPal payment integration
- **StripePaymentAppService.cs**: Stripe payment processing

### Key Features
- Multi-gateway support with configurable default
- Subscription creation and renewal
- Edition upgrades/downgrades
- Payment method storage (tokenization)
- Webhook handling for async payment updates
- Invoice generation
- Payment history tracking

## Architecture
- **Pattern**: Strategy pattern for multiple payment gateways
- **Authorization**: Tenant admin or host permissions
- **Transaction Safety**: Unit of Work with rollback on failure
- **Webhooks**: Handle async payment notifications

## Business Logic
1. User selects edition and payment method
2. Service calculates price based on edition and billing period
3. Payment processed through selected gateway
4. Subscription record created/updated
5. Edition features activated for tenant
6. Invoice generated and emailed
7. Payment recorded in accounting ledger

## Usage
Consumed by subscription management UI, upgrade wizards, and webhook endpoints.