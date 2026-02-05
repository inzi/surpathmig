# User Migration Wizard Implementation Document

## Overview
This document provides detailed step-by-step instructions for implementing a separate User Migration Wizard (Member Transfer) feature. This wizard allows users to transfer selected members from one cohort to another within the same department.

## Prerequisites
- Working cohort migration wizard deployed to production
- Branch created from the production release

## Implementation Steps

### Step 1: Create Backend DTOs
**File**: `/src/inzibackend.Application.Shared/Surpath/Dtos/CohortMigrationDtos.cs`

Add the following DTOs at the end of the file (before the closing brace):

```csharp
// Member Transfer DTOs (Separate from Cohort Migration)
public class MemberTransferDto
{
    public Guid SourceCohortId { get; set; }
    public Guid TargetCohortId { get; set; }
    public List<Guid> SelectedCohortUserIds { get; set; } = new List<Guid>();
    public List<long> SelectedUserIds { get; set; } = new List<long>();
    public bool TransferAllMembers { get; set; } = false;
    public string TransferReason { get; set; }
    public bool ConfirmTransfer { get; set; }
}

public class MemberTransferResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int TransferredMembersCount { get; set; }
    public Guid SourceCohortId { get; set; }
    public string SourceCohortName { get; set; }
    public Guid TargetCohortId { get; set; }
    public string TargetCohortName { get; set; }
    public List<string> Warnings { get; set; } = new List<string>();
    public List<string> Errors { get; set; } = new List<string>();
    public DateTime TransferStartTime { get; set; }
    public DateTime TransferEndTime { get; set; }
    public string TransferId { get; set; }
}

public class GetTargetCohortsInput : PagedAndSortedResultRequestDto
{
    public Guid SourceCohortId { get; set; }
    public Guid DepartmentId { get; set; }
    public string Filter { get; set; }
    public bool? IsActive { get; set; }
}

public class ValidateMemberTransferInput
{
    public Guid SourceCohortId { get; set; }
    public Guid TargetCohortId { get; set; }
    public List<Guid> SelectedCohortUserIds { get; set; } = new List<Guid>();
    public List<long> SelectedUserIds { get; set; } = new List<long>();
}

public class MemberTransferValidationResultDto
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public List<string> Warnings { get; set; } = new List<string>();
    public int MembersToTransfer { get; set; }
    public bool HasDuplicateMembers { get; set; }
    public List<string> DuplicateMembers { get; set; } = new List<string>();
}
```

### Step 2: Update Service Interface
**File**: `/src/inzibackend.Application.Shared/Surpath/ICohortMigrationAppService.cs`

Add these methods before the closing braces:

```csharp
// Member Transfer Operations (Separate from Cohort Migration)
Task<MemberTransferResultDto> TransferMembers(MemberTransferDto input);

Task<PagedResultDto<CohortLookupDto>> GetTargetCohortsForTransfer(GetTargetCohortsInput input);

Task<MemberTransferValidationResultDto> ValidateMemberTransfer(ValidateMemberTransferInput input);
```

### Step 3: Create View Model
**File**: `/src/inzibackend.Web.Mvc/Areas/App/Models/Cohorts/MemberTransferViewModel.cs` (NEW FILE)

```csharp
using System;
using System.Collections.Generic;
using inzibackend.Surpath.Dtos;

namespace inzibackend.Web.Areas.App.Models.Cohorts
{
    public class MemberTransferViewModel
    {
        // Source cohort information
        public Guid SourceCohortId { get; set; }
        public string SourceCohortName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TotalMembersCount { get; set; }
        
        // Wizard state
        public int CurrentStep { get; set; } = 1;
        public int MaxSteps { get; set; } = 3; // Target Cohort Selection, Member Selection, Confirmation
        
        // Target cohort selection
        public Guid? TargetCohortId { get; set; }
        public string TargetCohortName { get; set; }
        public List<CohortLookupDto> AvailableCohorts { get; set; } = new List<CohortLookupDto>();
        
        // Member selection
        public string MemberOption { get; set; } = "selected"; // Always "selected" for member transfer
        public bool TransferAllMembers { get; set; } = false;
        public List<CohortMemberDto> CohortMembers { get; set; } = new List<CohortMemberDto>();
        public List<Guid> SelectedCohortUserIds { get; set; } = new List<Guid>();
        public List<long> SelectedUserIds { get; set; } = new List<long>();
        
        // Transfer validation
        public MemberTransferValidationResultDto ValidationResult { get; set; }
        
        // Additional options
        public string TransferReason { get; set; }
        public bool ConfirmTransfer { get; set; }
    }
}
```

### Step 4: Update Controller
**File**: `/src/inzibackend.Web.Mvc/Areas/App/Controllers/CohortsController.cs`

Add this region before the closing braces of the class:

```csharp
#region Member Transfer (Separate Wizard)

[AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
public async Task<PartialViewResult> MemberTransferModal(Guid cohortId)
{
    // Get cohort information
    var cohortForView = await _cohortsAppService.GetCohortForView(cohortId);
    
    // Get total members count
    var membersCount = await _cohortMigrationAppService.GetCohortUsersCount(cohortId);

    // Get available cohorts in the same department
    var targetCohortsInput = new GetTargetCohortsInput
    {
        SourceCohortId = cohortId,
        DepartmentId = cohortForView.Cohort.TenantDepartmentId ?? Guid.Empty,
        MaxResultCount = 100,
        SkipCount = 0
    };
    var availableCohorts = await _cohortMigrationAppService.GetTargetCohortsForTransfer(targetCohortsInput);

    var viewModel = new MemberTransferViewModel
    {
        SourceCohortId = cohortId,
        SourceCohortName = cohortForView.Cohort.Name,
        DepartmentId = cohortForView.Cohort.TenantDepartmentId ?? Guid.Empty,
        DepartmentName = cohortForView.TenantDepartmentName,
        TotalMembersCount = membersCount,
        AvailableCohorts = availableCohorts.Items,
        CurrentStep = 1
    };

    return PartialView("_MemberTransferModal", viewModel);
}

[AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
public async Task<JsonResult> GetCohortMembers(GetCohortMembersInput input)
{
    try
    {
        var members = await _cohortMigrationAppService.GetCohortMembers(input);
        return Json(new { success = true, data = members });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}

[AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
public async Task<JsonResult> GetTargetCohortsForTransfer(GetTargetCohortsInput input)
{
    try
    {
        var cohorts = await _cohortMigrationAppService.GetTargetCohortsForTransfer(input);
        return Json(new { success = true, data = cohorts });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}

[AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
[HttpPost]
public async Task<JsonResult> ValidateMemberTransfer([FromBody] ValidateMemberTransferInput input)
{
    try
    {
        var validation = await _cohortMigrationAppService.ValidateMemberTransfer(input);
        return Json(new { success = true, data = validation });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}

[AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
[HttpPost]
public async Task<JsonResult> ExecuteMemberTransfer([FromBody] MemberTransferDto input)
{
    try
    {
        var result = await _cohortMigrationAppService.TransferMembers(input);
        return Json(new { success = result.Success, data = result });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}

#endregion
```

### Step 5: Create Razor View
**File**: `/src/inzibackend.Web.Mvc/Areas/App/Views/Cohorts/_MemberTransferModal.cshtml` (NEW FILE)

Create this file with the full content from the implementation (2200+ lines). Key sections include:
- Modal header with wizard title
- 3-step wizard progress indicator
- Step 1: Target Cohort Selection
- Step 2: Member Selection with DataTable
- Step 3: Review and Confirmation
- Modal footer with navigation buttons

**Note**: The full Razor view content is too large to include here. Copy from the implementation or request the full content separately.

### Step 6: Create JavaScript File
**File**: `/src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Cohorts/_MemberTransferModal.js` (NEW FILE)

Create this file with the full JavaScript implementation (500+ lines). Key functions include:
- Modal initialization
- Wizard navigation
- Member selection with DataTable
- Transfer validation
- Transfer execution

**Note**: The full JavaScript content is too large to include here. Copy from the implementation or request the full content separately.

### Step 7: Update Index.js
**File**: `/src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Cohorts/Index.js`

1. Add modal manager after the existing `_migrationWizardModal`:

```javascript
var _memberTransferModal = new app.ModalManager({
    viewUrl: abp.appPath + 'App/Cohorts/MemberTransferModal',
    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cohorts/_MemberTransferModal.js',
    modalClass: 'MemberTransferModal',
    modalSize: 'modal-xl'
});
```

2. Update the "Migrate Users" button action (around line 162):

```javascript
{
    text: app.localize('MigrateUsers'),
    iconStyle: 'fas fa-user-friends mr-2',
    visible: function () {
        return _permissions.migrate;
    },
    action: function (data) {
        _memberTransferModal.open({ 
            cohortId: data.record.cohort.id
        });
    },
},
```

3. Add event handler after the existing `cohortUserMoved` event:

```javascript
abp.event.on('app.memberTransferCompleted', function (data) {
    getCohorts();
    abp.notify.success(app.localize('MembersTransferredSuccessfully'));
});
```

### Step 8: Add Localization Keys
**File**: `/src/inzibackend.Core/Localization/inzibackend/inzibackend.xml`

Add before the closing `</texts>` tag:

```xml
<!-- Member Transfer Wizard -->
<text name="TransferCohortMembers">Transfer Cohort Members</text>
<text name="TargetCohort">Target Cohort</text>
<text name="SelectMembers">Select Members</text>
<text name="ChooseCohortToTransferMembersTo">Choose the cohort to transfer members to</text>
<text name="SourceCohortInformation">Source Cohort Information</text>
<text name="SourceCohort">Source Cohort</text>
<text name="MemberTransferSameDepartmentOnly">Members can only be transferred to cohorts within the same department</text>
<text name="SelectTargetCohort">Select Target Cohort</text>
<text name="PleaseSelect">Please Select</text>
<text name="TargetCohortStatistics">Target Cohort Statistics</text>
<text name="CurrentMembers">Current Members</text>
<text name="CreatedOn">Created On</text>
<text name="Status">Status</text>
<text name="SelectMembersToTransfer">Select Members to Transfer</text>
<text name="ChooseWhichMembersToTransfer">Choose which members to transfer to the target cohort</text>
<text name="From">From</text>
<text name="To">To</text>
<text name="SearchMembers">Search members...</text>
<text name="Clear">Clear</text>
<text name="Selected">Selected</text>
<text name="SelectAllVisible">Select All Visible</text>
<text name="DeselectAll">Deselect All</text>
<text name="UserName">Username</text>
<text name="FullName">Full Name</text>
<text name="EmailAddress">Email Address</text>
<text name="RecordStates">Record States</text>
<text name="CompletedReqs">Completed Reqs</text>
<text name="ComplianceRate">Compliance Rate</text>
<text name="LoadingCohortMembers">Loading cohort members...</text>
<text name="Important">Important</text>
<text name="MemberTransferComplianceNote">Members' compliance data and record states will be preserved during the transfer</text>
<text name="ReviewAndConfirmTransfer">Review and Confirm Transfer</text>
<text name="ReviewTransferDetailsBeforeExecution">Review all transfer details before execution</text>
<text name="TransferSummary">Transfer Summary</text>
<text name="MembersToTransfer">Members to Transfer</text>
<text name="SelectedMembers">Selected Members</text>
<text name="TransferReason">Transfer Reason</text>
<text name="EnterReasonForTransfer">Enter the reason for this transfer</text>
<text name="ValidationIssues">Validation Issues</text>
<text name="IConfirmTransferDetails">I confirm that the transfer details are correct</text>
<text name="MembersWillBeRemovedFromSourceCohort">Selected members will be removed from the source cohort</text>
<text name="Information">Information</text>
<text name="MemberTransferInfoMessage">Transferred members will retain all their compliance data and record states in the new cohort</text>
<text name="ExecuteTransfer">Execute Transfer</text>
<text name="ConfirmTransfer">Confirm Transfer</text>
<text name="AreYouSureYouWantToTransferXMembers">Are you sure you want to transfer {0} members?</text>
<text name="MembersTransferredSuccessfully">{0} members transferred successfully</text>
<text name="TransferFailed">Transfer failed</text>
<text name="PleaseSelectTargetCohort">Please select a target cohort</text>
<text name="PleaseSelectAtLeastOneMember">Please select at least one member to transfer</text>
<text name="PleaseConfirmTransferDetails">Please confirm the transfer details</text>
<text name="FailedToLoadMembers">Failed to load cohort members</text>
<text name="Active">Active</text>
<text name="Inactive">Inactive</text>
```

## Service Implementation Notes

The following service methods need to be implemented in `CohortMigrationAppService`:

1. **TransferMembers**: Move selected members from source to target cohort
   - Remove members from source cohort
   - Add members to target cohort
   - Preserve all compliance data
   - Create audit log

2. **GetTargetCohortsForTransfer**: Get cohorts in same department excluding source
   - Filter by department ID
   - Exclude source cohort
   - Include cohort statistics

3. **ValidateMemberTransfer**: Check for duplicate members and other validation rules
   - Check if members already exist in target cohort
   - Validate cohorts are in same department
   - Check permissions

## Important Implementation Details

### DataTable Configuration
The member selection uses DataTables with:
- Client-side processing (serverSide: false)
- Search functionality
- Checkbox selection
- Custom rendering for compliance rate badges

### Member Selection State
- Uses both CohortUser IDs and User IDs for flexibility
- Maintains selection state across pagination
- Updates count in real-time

### Validation Flow
1. Target cohort must be selected
2. At least one member must be selected
3. Server-side validation checks for duplicates
4. Confirmation checkbox required

## Testing Checklist

- [ ] Modal opens correctly from "Migrate Users" button
- [ ] Target cohort dropdown shows only cohorts from same department
- [ ] Source cohort is excluded from target dropdown
- [ ] Member list loads with correct data
- [ ] Search functionality works
- [ ] Select all/deselect all buttons work
- [ ] Individual checkbox selection works
- [ ] Selected count updates correctly
- [ ] Navigation between steps works
- [ ] Previous button works correctly
- [ ] Validation catches empty selection
- [ ] Server validation catches duplicate members
- [ ] Transfer completes successfully
- [ ] Cohort list refreshes after transfer
- [ ] Success notification appears
- [ ] All localization keys work
- [ ] Modal closes after successful transfer

## Build Commands

After implementation:
```bash
cd src/inzibackend.Web.Mvc
yarn
gulp buildDev
```

Then build the .NET solution.

## Rollback Instructions

If issues arise, revert the following files:
1. Remove new files created in Steps 3, 5, and 6
2. Revert changes to `CohortMigrationDtos.cs`
3. Revert changes to `ICohortMigrationAppService.cs`
4. Revert changes to `CohortsController.cs`
5. Revert changes to `Index.js`
6. Revert changes to `inzibackend.xml`

## Future Enhancements

1. Add bulk operations for selecting members by criteria
2. Add preview of what will happen before execution
3. Add progress indicator for large transfers
4. Add ability to transfer to cohorts in different departments (would require category mapping)
5. Add undo/rollback functionality