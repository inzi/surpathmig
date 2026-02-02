# Workers Documentation

## Overview
Background worker services for periodic maintenance and synchronization tasks in the Surpath system. Currently contains startup and periodic check workers for ensuring data consistency across tenants.

## Contents

### Files

#### StartupCheckWorker.cs
- **Purpose**: Background worker that performs periodic system checks and maintenance
- **Key Functionality**:
  - Runs every hour (configurable)
  - Executes immediately on startup (RunOnStart = true)
  - Placeholder for tenant service synchronization
  - Placeholder for PID type synchronization
- **Technical Details**:
  - Implements singleton pattern via ISingletonDependency
  - Uses ABP's PeriodicBackgroundWorkerBase for scheduling
  - Wraps work in unit of work for transaction support
  - Timer stops during execution to prevent overlapping runs

### Key Components

- **StartupCheckWorker**: Periodic background worker for system maintenance

### Dependencies

- **External Libraries**:
  - ABP Framework (Background Workers, Timers, Unit of Work)

- **Internal Dependencies**:
  - inzibackend.MultiTenancy (Tenant, TenantManager)
  - inzibackend.Surpath (SurpathService, TenantSurpathService)

## Architecture Notes

- **Pattern**: Background Worker pattern with periodic execution
- **Execution**: Runs every hour, with immediate execution on startup
- **Thread Safety**: Singleton pattern ensures single instance
- **Transaction**: Work wrapped in unit of work for consistency
- **Timer Management**: Timer stopped during work to prevent overlap

## Business Logic

### Planned Functionality (TODOs)
1. **Service Synchronization**: Ensure every tenant has TenantSurpathServices matching system-wide SurpathServices
2. **PID Type Synchronization**: Ensure every tenant has required PID types

### Current Implementation
- Framework is in place but actual synchronization logic is not yet implemented
- Worker runs but performs no actual work (placeholder implementation)

## Usage Across Codebase

This worker is:
- Automatically registered and started by the ABP framework on application startup
- Runs independently in the background without user interaction
- Intended to maintain data consistency across the multi-tenant system

## Future Enhancements

Based on the TODO comments, this worker should:
- Synchronize Surpath services from system level to tenant level
- Ensure all tenants have required PID types for compliance tracking
- Potentially handle other periodic maintenance tasks