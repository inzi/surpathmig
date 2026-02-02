# Fix: Tenant-Wide Requirements Being Archived During User Transfer

## Issue Summary
During the cohort user transfer wizard, tenant-wide requirements (requirements that exist in both source and target) were being incorrectly archived even though they didn't require mapping and should have remained associated with the user.

## Root Cause
In the `TransferSelectedUsers` method of `UserTransferAppService.cs`, the code was checking if a category was in the "analyzed" set to determine if it should be archived. However, the `analyzedCategoryIds` set only included:
- Categories that needed mapping (`CategoryMappings`)
- Categories that needed to be created new (`NewCategories`)
- Categories that were skipped (`SkippedCategories`)

It did NOT include categories that didn't require any action because they already existed in both source and target (like tenant-wide requirements). This caused these categories to be archived at line 938:

```csharp
// If this category wasn't analyzed (meaning it's no longer applicable in target)
else if (!analyzedCategoryIds.Contains(categoryId))
{
    // Archive the record state as it's no longer applicable
    recordState.IsArchived = true;
    ...
}
```

## Solution Applied

### 1. Added Property to DTO
Added `NoTransferRequiredCategoryIds` to `TransferSelectedUsersInput` class:
```csharp
public List<Guid> NoTransferRequiredCategoryIds { get; set; } = new List<Guid>(); 
// Categories that exist in both source and target (e.g., tenant-wide requirements)
```

### 2. Updated Backend Logic
Modified `UserTransferAppService.cs` to include these categories in the analyzed set:
```csharp
// IMPORTANT: Also include categories that didn't require mapping (e.g., tenant-wide requirements)
// These categories exist in both source and target and should NOT be archived
if (input.NoTransferRequiredCategoryIds != null) 
    analyzedCategoryIds.UnionWith(input.NoTransferRequiredCategoryIds);
```

### 3. Updated Frontend JavaScript
Modified `_TransferWizardModal.js` to populate this property:
```javascript
// Extract category IDs from noTransferRequiredCategories
var noTransferCategoryIds = [];
if (_transferData.analysisResult && _transferData.analysisResult.noTransferRequiredCategories) {
    noTransferCategoryIds = _transferData.analysisResult.noTransferRequiredCategories.map(function(cat) {
        return cat.categoryId;
    });
}

var transferDto = {
    // ... other properties ...
    noTransferRequiredCategoryIds: noTransferCategoryIds,
    confirmTransfer: true
};
```

## How It Works Now

1. During analysis, the system identifies categories that exist in both source and target as `noTransferRequiredCategories`
2. These categories are sent to the backend in the `noTransferRequiredCategoryIds` list
3. The backend includes these IDs in the `analyzedCategoryIds` set
4. When processing record states, if a category is in `analyzedCategoryIds` (including tenant-wide requirements), it won't be archived
5. Tenant-wide requirements remain associated with the user after transfer

## Testing
To verify the fix:
1. Create a tenant-wide requirement (not assigned to any department or cohort)
2. Have a user upload a document for this requirement
3. Transfer the user to a different cohort
4. Verify the document remains active (not in archives) after the transfer

## Files Modified
- `/src/inzibackend.Application.Shared/Surpath/Dtos/UserTransferDtos.cs`
- `/src/inzibackend.Application/Surpath/UserTransferAppService.cs`
- `/src/inzibackend.Web.Mvc/wwwroot/view-resources/Areas/App/Views/Users/_TransferWizardModal.js`