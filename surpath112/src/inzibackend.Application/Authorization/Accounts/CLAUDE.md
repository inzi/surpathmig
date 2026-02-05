# Account Management Services

## Overview
Application services for user account operations including registration, email confirmation, password reset, and account activation.

## Key Features
- User registration (self-service or admin-created)
- Email confirmation and resend
- Password reset request and confirmation
- Account activation/deactivation
- Account switching (impersonation)
- Profile completion checks

## Business Logic
Handles new user registration workflow with email verification, processes password reset requests with token generation, and manages account lifecycle from creation to deletion.

## Usage
Consumed by account controller, registration pages, login flow, and password reset functionality.