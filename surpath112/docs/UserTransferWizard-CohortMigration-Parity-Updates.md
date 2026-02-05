# User Transfer Wizard - Cohort Migration Wizard Parity Updates

## Overview
This document outlines the updates made to ensure the User Transfer Wizard's mapping experience is identical to the Cohort Migration Wizard.

## Key Changes Implemented

### 1. Row Selection Visibility Control
**Cohort Migration Behavior**: Mapping controls are hidden by default and only shown when a row is selected via checkbox.

**Implementation**:
- Added `handleRowSelection()` function that shows/hides controls based on checkbox state
- Wrapped mapping action select in a container div with `style="display:none;"` by default
- Updated checkbox handlers to call `handleRowSelection()`

```javascript
function handleRowSelection($row, categoryId, isChecked) {
    var $actionContainer = $row.find('.mapping-action-container');
    var $targetMappingSection = $row.find('.target-mapping-section');
    
    if (isChecked) {
        // Show the mapping action dropdown
        $actionContainer.show();
        
        // Check if an action is already selected
        var currentAction = $row.find('.mapping-action-select').val();
        if (currentAction) {
            $targetMappingSection.show();
        }
    } else {
        // Hide the mapping controls
        $actionContainer.hide();
        $targetMappingSection.hide();
    }
}
```

### 2. Validation Scope
**Cohort Migration Behavior**: Only validates selected categories, not all categories.

**Implementation**:
- Updated `validateCategoryMappings()` to only validate checked categories
- Added check for at least one selected category
- Skip validation for unselected categories

```javascript
// Get selected category IDs
var selectedCategoryIds = [];
$('.category-checkbox:checked').each(function() {
    selectedCategoryIds.push($(this).data('category-id'));
});

if (selectedCategoryIds.length === 0) {
    errors.push(app.localize('PleaseSelectAtLeastOneCategory'));
    return errors;
}

// Only validate selected categories
if (selectedCategoryIds.indexOf(categoryId) === -1) {
    return; // Skip unselected categories
}
```

### 3. Progress Tracking
**Cohort Migration Behavior**: Progress shows X/Y where Y is the number of selected categories, not total categories.

**Implementation**:
- Updated `updateMappingProgress()` to count only selected categories
- Progress badge shows "mappedCount/selectedCount" instead of "mappedCount/totalCount"
- Next button enabled when all selected categories are mapped (or none selected)

### 4. Transfer Data Filtering
**Cohort Migration Behavior**: Only selected categories are included in the transfer.

**Implementation**:
- Updated `performTransfer()` to filter mappings based on selected categories
- Only includes categoryMappings, newCategories, and skippedCategories for checked items

```javascript
// Filter mappings to only include selected categories
var selectedCategoryIds = [];
$('.category-checkbox:checked').each(function() {
    selectedCategoryIds.push($(this).data('category-id'));
});

// Filter each mapping type
Object.keys(_transferData.categoryMappings || {}).forEach(function(categoryId) {
    if (selectedCategoryIds.indexOf(categoryId) !== -1) {
        filteredCategoryMappings[categoryId] = _transferData.categoryMappings[categoryId];
    }
});
```

### 5. Select All Functionality
**Cohort Migration Behavior**: Select all checkbox properly shows/hides controls for all rows.

**Implementation**:
- Updated select all handler to iterate through each checkbox
- Calls `handleRowSelection()` for each row to ensure proper visibility

```javascript
modal.off('change', '#selectAllCategories').on('change', '#selectAllCategories', function() {
    var isChecked = $(this).is(':checked');
    $('.category-checkbox').each(function() {
        var $checkbox = $(this);
        var categoryId = $checkbox.data('category-id');
        var $row = $checkbox.closest('tr');
        
        $checkbox.prop('checked', isChecked);
        handleRowSelection($row, categoryId, isChecked);
    });
});
```

### 6. Step Navigation
**Cohort Migration Behavior**: Skip mapping step if no categories need mapping.

**Implementation**:
- Enhanced check to skip step 3 if no mapping required
- Checks for requiresCategoryMapping flag AND non-empty requirementCategories array

## Result
The User Transfer Wizard now provides an identical mapping experience to the Cohort Migration Wizard:

1. **Visual Clarity**: Controls hidden until needed
2. **Selective Mapping**: Users choose which categories to map
3. **Accurate Progress**: Shows progress for selected items only
4. **Efficient Transfer**: Only processes selected mappings
5. **Consistent Validation**: Only validates what user intends to map

## Testing Checklist
- [ ] Mapping controls hidden by default
- [ ] Controls appear when checkbox selected
- [ ] Select all shows/hides all controls
- [ ] Progress updates based on selected items
- [ ] Validation only checks selected categories
- [ ] Transfer only includes selected mappings
- [ ] Skip step 3 when no mapping needed
- [ ] All bulk actions work with selection state