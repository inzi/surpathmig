# Authorization/Users Documentation

## Overview
Contains constant definitions related to user data validation and constraints, particularly for phone number fields.

## Contents

### Files

#### UserConsts.cs
- **Purpose**: Defines validation constants for user-related fields
- **Key Functionality**:
  - Sets maximum phone number length to 24 characters
  - Sets minimum phone number length to 10 characters
- **Usage**: Used for data validation in DTOs, entities, and input validation

### Key Components

#### UserConsts Class
- Public class containing validation constants
- **MaxPhoneNumberLength**: 24 - accommodates international formats with extensions
- **MinPhoneNumberLength**: 10 - ensures basic phone number completeness

### Dependencies
- No external dependencies
- Part of the inzibackend.Authorization.Users namespace

## Architecture Notes
- Centralized validation constants promote consistency
- Allows for easy maintenance of validation rules across the application
- Supports international phone number formats

## Business Logic
- Phone numbers must be between 10-24 characters
- Validation rules apply to all user phone number fields
- Accommodates various formats including country codes and extensions

## Usage Across Codebase
These constants are used in:
- User entity definitions
- User DTOs for data transfer
- Input validation attributes
- Database field constraints
- Client-side validation rules