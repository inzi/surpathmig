# Timing Documentation

## Overview
This folder contains service interfaces and DTOs for timezone management. The timing system enables proper timezone handling across the multi-tenant application with user-specific timezone preferences.

## Contents

### Files
Service interface for timing operations (ITimingAppService.cs or similar)

### Subfolders

#### Dto
Timezone selection and management DTOs.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- UTC server storage
- User timezone conversion
- Timezone inheritance (user → tenant → application)
- DST handling

## Business Logic
Timezone selection, timezone lists, date/time display in user timezone, timezone-aware scheduling.

## Usage Across Codebase
User profile settings, date/time display throughout application, scheduling services, email timestamps

## Cross-Reference Impact
Changes affect timezone selection, time display, and scheduling across the application