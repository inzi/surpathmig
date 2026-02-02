# Logging Documentation

## Overview
This folder contains service interfaces and DTOs for application logging and log file management. The logging system enables administrators to view, download, and analyze log files for troubleshooting and monitoring.

## Contents

### Files
Service interface for log operations (IWebLogAppService.cs or similar)

### Subfolders

#### Dto
Log viewing and retrieval DTOs.
[Full details in Dto/CLAUDE.md](Dto/CLAUDE.md)

## Architecture Notes
- File-based logging (Serilog/NLog)
- Rolling log files
- Log level filtering
- Secure access to logs

## Business Logic
View recent log entries, download log files, search logs, filter by level/date for troubleshooting and monitoring.

## Usage Across Codebase
Admin dashboard log viewer, troubleshooting tools, monitoring systems

## Cross-Reference Impact
Changes affect log viewing UI and log download functionality