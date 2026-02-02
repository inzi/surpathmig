# Slot DTOs Documentation

## Overview
This folder contains DTOs for clinical rotation slot management. Slots represent available positions at medical facilities where students can complete clinical rotations. This is a specialized scheduling and capacity management system.

## Contents

### Files
DTOs for slot management including:
- SlotDto - Slot details
- CreateSlotInput - Slot creation
- UpdateSlotInput - Slot updates
- SlotListDto - Slot lists
- SlotAvailabilityDto - Availability tracking
- SlotAssignmentDto - Student assignments

## Key Components
- Slot definition (location, capacity, dates)
- Availability tracking
- Student assignment
- Capacity management
- Schedule conflicts

## Architecture Notes
- Medical unit association
- Capacity constraints
- Date range validation
- Assignment tracking
- Conflict detection

## Business Logic
Create rotation slots at medical facilities, track capacity, assign students to slots, manage conflicts, monitor utilization.

## Usage Across Codebase
Clinical rotation scheduling, slot management UI, student placement, capacity planning

## Cross-Reference Impact
Changes affect clinical rotation scheduling, student placement, and capacity management features