# Cohort User Transfer Wizard Enhancement Plan

## Overview
This plan details the enhancement of the existing cohort user transfer wizard to support selective user transfers between cohorts, maintaining the same department or allowing cross-department transfers.

## Current State Analysis
- **Location**: Launched from cohort actions menu via `CohortsController.cs`
- **View**: `/Areas/App/Views/Users/_TransferWizardModal.cshtml`
- **JavaScript**: `/wwwroot/view-resources/Areas/App/Views/Users/_TransferWizardModal.js`
- **Service**: `UserTransferAppService.cs` with interface `IUserTransferAppService.cs`
- **Current Steps**: 3 steps (Department Selection, Category Mapping, Confirmation)
- **Current Behavior**: Transfers ALL users from source cohort

## Required Changes

### Step 1: Target Cohort Selection (Modified)
**Current**: Select target department only
**New**: 
1. First select target department (dropdown)
2. Then select target cohort within that department (dropdown, filtered by department)
3. Show cohort statistics (users count, requirements)
4. Analysis button validates the selection

**Implementation**:
- Modify `_TransferWizardModal.cshtml` step 1 to add cohort selection after department
- Add new method in `UserTransferAppService`: `GetCohortsForDepartment(Guid departmentId)`
- Update JavaScript to handle cascading dropdowns
- Modify analysis to work with cohort-to-cohort transfers

### Step 2: User Selection (New Step)
**Purpose**: Select specific cohort users to transfer
**Features**:
1. List all cohort users with search/filter capability
2. Checkbox selection for individual users
3. Select all/none functionality
4. Show user details (name, email, compliance status)
5. Real-time count of selected users

**Implementation**:
- Add new step in `_TransferWizardModal.cshtml`
- Create user selection UI with DataTable
- Add method: `GetCohortUsersForTransfer(Guid cohortId, string searchFilter)`
- Store selected user IDs in JavaScript state

### Step 3: Requirements Mapping (Existing Step 2)
**Current**: Maps categories between departments
**Modified**: 
- Only shown if transferring between different departments
- Skip if same department transfer
- Identical to current implementation

### Step 4: Confirmation (Existing Step 3)
**Current**: Shows transfer summary
**Modified**:
- List selected users that will be transferred
- Show source cohort → target cohort
- Include user count and names
- Maintain existing confirmation checkbox

## Technical Implementation Details

### 1. Backend Changes

#### UserTransferAppService.cs Modifications:
```csharp
// New methods to add:
Task<List<CohortLookupDto>> GetCohortsForDepartment(Guid departmentId);
Task<PagedResultDto<CohortUserForTransferDto>> GetCohortUsersForTransfer(GetCohortUsersForTransferInput input);
Task<UserTransferAnalysisDto> AnalyzeUserTransfer(AnalyzeUserTransferInput input); // Modified to accept user IDs
Task<UserTransferResultDto> TransferSelectedUsers(TransferSelectedUsersInput input); // New method
```

#### New DTOs (UserTransferDtos.cs):
```csharp
public class GetCohortUsersForTransferInput : PagedAndSortedResultRequestDto
{
    public Guid CohortId { get; set; }
    public string Filter { get; set; }
}

public class CohortUserForTransferDto
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string ComplianceStatus { get; set; }
    public int RecordStatesCount { get; set; }
}

public class TransferSelectedUsersInput
{
    public Guid SourceCohortId { get; set; }
    public Guid TargetCohortId { get; set; }
    public List<Guid> SelectedCohortUserIds { get; set; }
    public Dictionary<Guid, Guid> CategoryMappings { get; set; }
}

public class AnalyzeUserTransferInput
{
    public Guid SourceCohortId { get; set; }
    public Guid TargetCohortId { get; set; }
    public List<Guid> SelectedCohortUserIds { get; set; }
}
```

#### CohortUsersController.cs:
- Add new action methods to support AJAX calls from the wizard
- Methods for getting cohorts by department
- Methods for getting users for selection

### 2. Frontend Changes

#### _TransferWizardModal.cshtml:
1. Update step count from 3 to 4
2. Modify Step 1 to include cohort selection
3. Add new Step 2 for user selection
4. Renumber existing steps 2 and 3 to 3 and 4
5. Update progress bar calculation

#### _TransferWizardModal.js:
1. Update `_maxSteps` from 3 to 4
2. Add user selection management:
   - Track selected users in `_transferData.selectedUsers`
   - Implement DataTable for user list
   - Add select all/none functionality
3. Modify step navigation logic
4. Update analysis and execution methods to include selected users
5. Add cohort dropdown population logic

### 3. Step Navigation Flow:
1. **Step 1**: Select department → Select cohort → Analyze
2. **Step 2**: Select users (with filtering)
3. **Step 3**: Map requirements (skip if same department)
4. **Step 4**: Confirm and execute

### 4. Validation Rules:
- Cannot transfer to the same cohort
- Must select at least one user
- Target cohort must exist and be active
- User must have permission for both cohorts
- Preserve all compliance data during transfer

### 5. Transfer Logic (UserTransferAppService):
When executing transfer:
1. Validate all selected CohortUser records exist
2. For each selected CohortUser:
   - Update `CohortId` to target cohort ID
   - If different department: remap RecordStates to new categories
   - Preserve all RecordNotes
   - Maintain audit trail
3. Do NOT create new CohortUser records
4. Recalculate compliance for affected users

## Testing Considerations
1. Test with users having multiple RecordStates
2. Test same-department transfers (no mapping needed)
3. Test cross-department transfers (mapping required)
4. Test with large number of users (pagination)
5. Test search/filter functionality
6. Verify compliance data integrity after transfer

## UI/UX Considerations
1. Clear indication of selected users count
2. Warning if no users selected
3. Loading states during data fetching
4. Clear distinction between steps
5. Ability to go back and modify selections
6. Show which users will be affected in confirmation

## Migration from Current Implementation
- Existing transfer wizard functionality remains intact
- New functionality is additive, not breaking
- Database schema remains unchanged
- Only CohortUser.CohortId field is updated

## Implementation Order
1. Backend: Add new DTOs and interfaces
2. Backend: Implement new service methods
3. Frontend: Update HTML structure for 4 steps
4. Frontend: Implement user selection UI
5. Frontend: Update JavaScript logic
6. Backend: Implement transfer execution logic
7. Testing and validation
8. Update any related documentation

## Notes
- The wizard should be accessible from cohort actions menu (already implemented)
- Maintain all existing permissions and authorization
- Use existing ASP.NET Zero patterns and conventions
- No direct AJAX calls - use app.services pattern
- Follow existing code style and patterns