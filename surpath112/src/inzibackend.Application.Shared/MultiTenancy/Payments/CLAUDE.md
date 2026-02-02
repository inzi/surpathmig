# Payments Documentation

## Overview
This folder contains service interfaces and DTOs for subscription payment processing with support for multiple payment gateways.

## Contents

### Files
Service interface for payment operations (IPaymentAppService.cs or similar)

### Subfolders

#### Dto
Core payment DTOs for payment processing, history, and gateway integration.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

#### AuthorizeNet
Authorize.Net payment gateway integration.
[Full details in AuthorizeNet/Dto/CLAUDE.md](AuthorizeNet/Dto/CLAUDE.md)

#### PayPal
PayPal payment gateway integration.
[Full details in PayPal/Dto/CLAUDE.md](PayPal/Dto/CLAUDE.md)

#### Stripe
Stripe payment gateway integration (primary).
[Full details in Stripe/Dto/CLAUDE.md](Stripe/Dto/CLAUDE.md)

## Architecture Notes
- Multi-gateway support
- PCI compliance via tokenization
- Webhook processing
- Retry logic for failed payments

## Business Logic
Payment processing, subscription billing, gateway integration, payment history, failed payment handling.

## Usage Across Codebase
Subscription purchase, payment processing, billing services, webhook controllers

## Cross-Reference Impact
Changes affect payment flows, gateway integrations, and subscription billing