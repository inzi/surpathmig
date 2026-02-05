# Comparison of Cohort Migration Wizard vs User Transfer Wizard Mapping UI

## Key Differences Found

### 1. **Row Selection and UI Visibility Control**

#### Cohort Migration Wizard (More Complex):
- Uses `handleRowSelection()` function that shows/hides mapping controls based on checkbox state
- When checkbox is checked: Shows mapping action dropdown and target mapping section
- When checkbox is unchecked: Hides mapping action dropdown and target mapping section
- Includes fixed header with column synchronization
- Updates header visibility dynamically based on selected rows

#### User Transfer Wizard (Simpler):
- Does NOT have `handleRowSelection()` function
- All mapping controls are always visible regardless of checkbox state
- No dynamic show/hide of UI elements based on selection
- Simpler checkbox handler that only updates select-all state

### 2. **Visual Implementation Details**

#### Cohort Migration Wizard:
```javascript
function handleRowSelection($row, categoryId, isChecked) {
    var $actionContainer = $row.find('.mapping-action-container');
    var $targetMappingSection = $row.find('.target-mapping-section');
    
    if (isChecked) {
        $actionContainer.show();
        $targetMappingSection.show();
        // Populate dropdowns
    } else {
        $actionContainer.hide();
        $targetMappingSection.hide();
    }
    
    // Update header visibility and sync
    updateHeaderVisibility();
    syncHeaderWithTableColumns();
}
```

#### User Transfer Wizard:
```javascript
// Individual category checkbox handler
modal.off('change', '.category-checkbox').on('change', '.category-checkbox', function() {
    updateSelectAllCheckboxState();
});
// No row visibility changes
```

### 3. **DataTable Column Rendering**

#### Cohort Migration Wizard:
- Mapping action container has `style="display:none;"` by default
- Target mapping section has `style="display:none;"` by default
- These are shown only when row is selected

#### User Transfer Wizard:
- All columns are visible by default
- No dynamic visibility control in render functions

### 4. **Fixed Header Implementation**

#### Cohort Migration Wizard:
- Has complex fixed header with column synchronization
- Updates header column visibility based on which rows are selected
- Includes scroll container with fixed header outside

#### User Transfer Wizard:
- Standard DataTable header
- No fixed header implementation
- No dynamic header visibility updates

## Recommendation

To make the User Transfer Wizard identical to the Cohort Migration Wizard, the following changes are needed:

1. **Add `handleRowSelection()` function** to control visibility of mapping controls
2. **Update DataTable column rendering** to hide mapping controls by default
3. **Modify checkbox change handlers** to call `handleRowSelection()`
4. **Add fixed header implementation** with column synchronization
5. **Update bulk actions and validation** to only consider selected rows
6. **Add header visibility update logic** based on row selections

The main philosophical difference is that the Cohort Migration Wizard only shows mapping controls for selected categories, while the User Transfer Wizard shows all controls all the time, which can be overwhelming when there are many categories.