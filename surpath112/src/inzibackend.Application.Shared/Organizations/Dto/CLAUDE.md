# Organizations DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the Organization Unit system. Organization Units provide hierarchical organizational structure for grouping users, roles, and departments. In the Surpath context, OrganizationUnits represent schools, departments, cohorts, and other structural elements that require permission and access control boundaries.

## Contents

### Files

#### Organization Unit Management
- **OrganizationUnitDto.cs** - Organization unit details:
  - Code - Hierarchical position code
  - DisplayName - User-friendly name
  - ParentId - Parent organization unit
  - MemberCount - Number of members
  - RoleCount - Number of assigned roles
  - Used for tree rendering and unit display

- **CreateOrganizationUnitInput.cs** - Create new unit:
  - ParentId - Parent unit in hierarchy
  - DisplayName - Unit name
  - Creates new organizational node

- **UpdateOrganizationUnitInput.cs** - Update existing unit:
  - Id - Unit to update
  - DisplayName - New name

- **MoveOrganizationUnitInput.cs** - Move unit in hierarchy:
  - Id - Unit to move
  - NewParentId - New parent (null for root level)
  - Maintains hierarchy integrity

#### User Assignment
- **OrganizationUnitUserListDto.cs** - User in organization unit:
  - UserId, UserName - User identification
  - AddedTime - When user added to unit
  - Used in user membership lists

- **GetOrganizationUnitUsersInput.cs** - Query unit members:
  - Id - Organization unit ID
  - Paging, sorting, filtering support

- **FindOrganizationUnitUsersInput.cs** - Search users for assignment:
  - Filter - Search term
  - MaxResultCount - Limit results
  - Used in user picker for adding members

- **UserToOrganizationUnitInput.cs** - Add single user:
  - UserId - User to add
  - OrganizationUnitId - Target unit

- **UsersToOrganizationUnitInput.cs** - Add multiple users:
  - UserIds - Users to add (bulk operation)
  - OrganizationUnitId - Target unit

#### Role Assignment
- **OrganizationUnitRoleListDto.cs** - Role in organization unit:
  - RoleId, RoleName, DisplayName
  - AddedTime - When role added
  - Roles grant permissions within organizational scope

- **GetOrganizationUnitRolesInput.cs** - Query unit roles:
  - Id - Organization unit ID
  - Paging and filtering

- **FindOrganizationUnitRolesInput.cs** - Search roles for assignment

- **RoleToOrganizationUnitInput.cs** - Add single role

- **RolesToOrganizationUnitInput.cs** - Add multiple roles (bulk)

#### Department Assignment (Surpath-Specific)
- **OrganizationUnitDepartmentListDto.cs** - TenantDepartment in unit:
  - TenantDepartmentId - Department reference
  - Department name and details
  - Links Surpath departments to organizational structure

- **GetOrganizationUnitTenantDepartmentInput.cs** - Query unit departments

- **FindOrganizationUnitTenantDepartmentInput.cs** - Search departments for assignment

- **TenantDepartmentToOrganizationUnitInput.cs** - Add single department

- **TenantDepartmentsToOrganizationUnitInput.cs** - Add multiple departments

### Key Components

#### Hierarchical Structure
- Tree-based organization with unlimited depth
- Code-based positioning (e.g., "00001.00003.00012")
- Parent-child relationships
- Move operations preserve subtree integrity

#### Multi-Assignment
- Users can belong to multiple units
- Roles can be assigned to multiple units
- Departments linked to organizational structure
- Cascading permissions

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- **Abp.Organizations** - ABP organization infrastructure
- **inzibackend.Dto** - Paging base DTOs

## Architecture Notes

### Hierarchical Code System
- Each unit has unique code reflecting position
- Parent code prefix (e.g., parent "00001", child "00001.00002")
- Efficient tree queries using code patterns
- Code automatically maintained on moves

### Permission Scoping
- Permissions granted at organizational level
- Users inherit permissions from organization membership
- Role permissions apply within organizational context
- Data access restricted by organization boundaries

### Multi-Tenancy
- Organization units tenant-isolated
- Each tenant has independent hierarchy
- Root units at tenant level

## Business Logic

### Organizational Hierarchy
Typical structure in Surpath:
```
School (Root OU)
├── Nursing Department
│   ├── Fall 2024 Cohort
│   ├── Spring 2025 Cohort
│   └── Administrative Staff
├── Allied Health Department
│   ├── Dental Hygiene Program
│   └── Radiology Program
└── Administration
```

### User Assignment Workflow
1. Admin navigates to organization unit
2. Clicks "Add Users"
3. Uses FindOrganizationUnitUsersInput to search
4. Selects users from results
5. UsersToOrganizationUnitInput adds them to unit
6. Users gain permissions associated with unit

### Role Assignment
- Assign roles to organizational units
- Users in unit automatically get those roles
- Role permissions scoped to organizational context
- Example: "Instructor" role in "Nursing Department" unit

### Department Linking
- TenantDepartments (Surpath concept) linked to OrganizationUnits
- Provides permission boundaries for departments
- Users in department have access to department data
- Cohorts associated with departments via organization units

### Moving Units
1. Select unit to move
2. Choose new parent via MoveOrganizationUnitInput
3. System validates move (can't move to descendant)
4. Code recalculated for unit and all descendants
5. Permissions and memberships preserved

## Usage Across Codebase
These DTOs are consumed by:
- **IOrganizationUnitAppService** - Organization CRUD operations
- **Organization Management UI** - Tree view and editing
- **User Management** - Organizational user assignment
- **Role Management** - Organizational role scoping
- **Permission System** - Organizational permission boundaries
- **Department Management** - Department-organization linking
- **Data Filtering** - Restrict queries to organizational scope

## Cross-Reference Impact
Changes to these DTOs affect:
- Organization tree interface
- User assignment to organizations
- Role scoping within organizations
- Permission checking with organizational context
- Department-organization relationships
- Data access boundaries
- Hierarchical navigation
- Organizational reports and analytics