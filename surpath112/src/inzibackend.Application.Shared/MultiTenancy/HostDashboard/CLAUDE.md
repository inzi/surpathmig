# Host Dashboard Documentation

## Overview
This folder contains service interfaces and DTOs for the host (SaaS provider) dashboard. Provides analytics and monitoring for the entire multi-tenant platform.

## Contents

### Files
Service interface for host dashboard operations (IHostDashboardAppService.cs or similar)

### Subfolders

#### Dto
Complete host dashboard DTOs for revenue analytics, tenant tracking, and platform metrics.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- Platform-wide analytics
- Revenue tracking
- Tenant growth monitoring
- Edition statistics

## Business Logic
Platform KPIs, revenue trends, tenant signups, expiring subscriptions, edition distribution.

## Usage Across Codebase
Host dashboard UI, business intelligence, monitoring

## Cross-Reference Impact
Changes affect host dashboard displays and platform analytics