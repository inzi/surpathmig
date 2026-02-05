# Health Check Services

## Overview
Application health monitoring services providing health check endpoints and system status information for monitoring tools and load balancers.

## Key Features
- Database connectivity check
- Cache health check
- External service availability (email, storage, payment gateways)
- Background job execution status
- System resource metrics

## Business Logic
Performs health checks on critical system components, returns standardized health check responses (healthy/unhealthy/degraded), and enables automated monitoring and alerting.

## Usage
Consumed by health check middleware, monitoring tools (e.g., Kubernetes, Azure Monitor), load balancers, and operations dashboards.