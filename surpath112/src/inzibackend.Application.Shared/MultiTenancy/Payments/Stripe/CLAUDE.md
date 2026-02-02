# Stripe Payment Gateway Documentation

## Overview
This folder contains service interfaces and DTOs for Stripe payment gateway integration. Stripe is the primary payment processor providing comprehensive payment infrastructure.

## Contents

### Files
Service interface for Stripe operations

### Subfolders

#### Dto
Stripe-specific payment DTOs including Checkout sessions and payment confirmation.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Stripe Checkout
- Stripe.js tokenization
- Webhook events
- Subscription management
- PCI DSS Level 1

## Business Logic
Stripe payment processing, Checkout sessions, webhook handling, subscription management.

## Usage Across Codebase
Payment gateway services (primary), configuration UI, webhook controllers, subscription services

## Cross-Reference Impact
Changes affect primary payment processing, subscription billing, and Stripe integration