# Demo Data Builder Documentation

## Overview
Automated demo data generation system for new tenants in demo mode. Creates realistic sample data including organization units, users, friendships, and chat messages to showcase application features.

## Contents

### Files

#### TenantDemoDataBuilder.cs
- **Purpose**: Main service to build demo data for new tenants
- **Key Features**:
  - Conditional execution (only in demo mode)
  - Organization unit hierarchy creation
  - Random user generation (12-26 users)
  - Profile picture assignment (70% probability)
  - Friendship creation between users
  - Sample chat messages
  - Role assignments
  - OU memberships
- **Demo Mode Check**: Reads `App:DemoMode` configuration setting
- **Major Sections**:
  - Organization structure (R&D, Sales, Support departments)
  - User creation with random data
  - Social features (friendships)
  - Chat messages
- **Dependencies**: Extensive use of managers and repositories

#### RandomUserGenerator.cs
- **Purpose**: Generates random user data for demo purposes
- **Features**: (inferred)
  - Random names and emails
  - Diverse user profiles
  - Realistic data patterns
  - Configurable count

#### TenantDemoDataBuilderJob.cs
- **Purpose**: Background job to trigger demo data creation
- **Pattern**: Job pattern for asynchronous execution

### Key Components

- **TenantDemoDataBuilder**: Main builder service
- **RandomUserGenerator**: User data generator
- **TenantDemoDataBuilderJob**: Async job wrapper

### Dependencies

- **External Libraries**:
  - ABP Framework (UoW, dependency injection)

- **Internal Dependencies**:
  - OrganizationUnitManager
  - UserManager
  - FriendshipManager
  - BinaryObjectManager
  - ChatMessage repository
  - User store

## Architecture Notes

- **Pattern**: Builder pattern for complex object creation
- **Conditional**: Only runs in demo mode
- **Transactional**: Wrapped in unit of work
- **Extensibility**: Easy to add more demo data types

## Business Logic

### Demo Organization Structure

#### Producing Department
- Research & Development
  - IVR Related Products
  - Voice Technologies
  - Inhouse Projects
- Quality Management
- Testing

#### Selling Department
- Marketing
- Sales
- Customer Relations

#### Supporting Department
- Buying
- Human Resources

### User Generation
- Random count: 12-26 users
- Each user:
  - Created with random data
  - Assigned to "User" role
  - Added to 0-3 random OUs
  - 70% chance of profile picture
  - Unique username and email

### Admin Setup
- Admin user gets profile picture
- Admin befriends 3 random users
- Bidirectional friendships created
- Sample chat message from one friend

### Profile Pictures
- Selected from sample images (11 available)
- Files: `sample-profile-01.jpg` through `sample-profile-11.jpg`
- Stored in BinaryObject system
- Random assignment

### Friendships
- Admin connected to 3 random users
- Friendships marked as "Accepted"
- Both directions created (A→B and B→A)
- Enables chat feature demonstration

### Chat Messages
- One unread message from random friend to admin
- Demonstrates chat notification feature
- Uses localized message: "Demo_SampleChatMessage"
- Shared message ID for conversation tracking

## Usage Across Codebase

Invoked during:
- New tenant registration (demo mode only)
- Tenant creation workflow
- Background job processing
- Development and testing scenarios

## Configuration

### Demo Mode Setting
```json
{
  "App": {
    "DemoMode": "true"
  }
}
```

### Sample Profile Images Location
Configured via `IAppFolders.SampleProfileImagesFolder`

### Additional Settings
- Auto-activates new registered users for demo tenants
- Setting: `IsNewRegisteredUserActiveByDefault` = true

## Security Considerations

### Demo Mode Only
- Never runs in production with demo mode off
- Clear configuration check before execution
- No sensitive data in demo users

### Sample Data
- Fake names and emails
- Generic organizational structure
- Non-sensitive chat messages
- Sample images only

## Extension Points

- Additional organization structures
- Custom user data patterns
- More sophisticated friendships
- Document samples
- Compliance records (if applicable)
- Payment history samples
- Dashboard data
- Notification samples

## Performance Considerations

- Batched user creation with UoW saves
- Profile picture file I/O in try-catch
- Moderate data volume (12-26 users)
- One-time execution per tenant
- Can be run asynchronously via job