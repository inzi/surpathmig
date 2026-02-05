# User Transfer Wizard - Visual Parity Updates

## Overview
This document summarizes the visual and behavioral updates made to ensure the User Transfer Wizard's mapping UI matches the Cohort Migration Wizard exactly.

## Visual Changes Implemented

### 1. Category Name Column
**Before**: Simple bold text
**After**: Wrapped in `<div class="category-info">` with proper structure

### 2. Requirement Name Column with Hierarchy Badges
**Before**: Plain text
**After**: 
- Requirement name with hierarchy level badge below
- Color-coded badges:
  - **Tenant**: Blue badge with building icon
  - **Department**: Info (light blue) badge with sitemap icon  
  - **Cohort**: Green badge with users icon
  - **CohortAndDepartment**: Warning (yellow) badge with both icons

```javascript
var hierarchyClass = 'badge badge-sm ';
switch(hierarchyLevel) {
    case 'Tenant':
        hierarchyClass += 'badge-primary';
        hierarchyIcon = '<i class="fas fa-building"></i> ';
        break;
    case 'Department':
        hierarchyClass += 'badge-info';
        hierarchyIcon = '<i class="fas fa-sitemap"></i> ';
        break;
    // etc...
}
```

### 3. Impact Column
**Before**: Two separate badges (blue for users, gray for records)
**After**: Simple text layout matching migration wizard
- Shows "X Users" and "Y Records" on separate lines
- No badges, just plain text centered

### 4. Status Column
**Before**: Colored circle icon
**After**: Badge with text
- **Pending**: Yellow badge with "Pending" text
- **Mapped**: Green badge with "Mapped" text
- **Skip**: Red badge with "Skip" text

### 5. Bulk Action Buttons
**Before**: Different button styles with all having icons
**After**: 
- Bulk Map: Gray outline with link icon
- Bulk Copy: Gray outline with copy icon
- Bulk Skip: Yellow filled button, no icon
- Buttons always visible (not hidden by default)

### 6. Overall Layout
- Bulk actions section is now always visible
- Consistent spacing and alignment with migration wizard
- Proper badge sizing with `font-size: 0.8rem` for hierarchy badges

## Code Examples

### Updated DataTable Column Rendering
```javascript
// Requirement column with hierarchy badge
{
    data: null,
    render: function(data, type, row) {
        var html = '<div class="requirement-info">';
        html += '<div class="requirement-name">' + row.requirementName + '</div>';
        
        // Show hierarchy level badge
        var hierarchyClass = 'badge badge-sm ';
        var hierarchyIcon = '';
        var hierarchyLevel = row.hierarchyLevel || 'Unknown';
        
        // ... badge logic ...
        
        html += '<span class="' + hierarchyClass + '" style="font-size: 0.8rem;">' + 
                hierarchyIcon + hierarchyLevel + '</span>';
        html += '</div>';
        return html;
    }
}
```

### Updated Status Badge
```javascript
function updateCategoryStatusIcon($row, action) {
    var $badge = $row.find('.mapping-status');
    
    if (!action) {
        $badge.removeClass('badge-success badge-danger').addClass('badge-warning');
        $badge.text('Pending');
    } else if (action === 'skip') {
        $badge.removeClass('badge-success badge-warning').addClass('badge-danger');
        $badge.text('Skip');
    } else {
        $badge.removeClass('badge-warning badge-danger').addClass('badge-success');
        $badge.text('Mapped');
    }
}
```

## Behavioral Changes Implemented (Latest)

### 1. Default Mapping Action
**Issue**: The mapping action dropdown showed "-- Select Action --" as default, requiring an extra click
**Fix**: 
- Removed the empty option from the dropdown
- Set "Map to Existing" as the default selected option
- Categories now initialize with `mappingAction = 'map'`

### 2. Show Both Dropdowns Immediately
**Issue**: Only the mapping action dropdown appeared when selecting a row; the target category dropdown remained hidden
**Fix**: 
- Updated `handleRowSelection` to show both dropdowns immediately
- Target mapping section is now visible as soon as a row is checked
- Target category dropdown is populated immediately for 'map' action

### 3. Code Changes
```javascript
// renderMappingActionSelect - defaults to 'map' with no empty option
html += '<option value="map"' + (row.mappingAction === 'map' || !row.mappingAction ? ' selected' : '') + '>Map to Existing</option>';

// handleRowSelection - shows both dropdowns and populates target categories
if (isChecked) {
    $actionContainer.show();
    $targetMappingSection.show();
    
    var currentAction = $row.find('.mapping-action-select').val() || 'map';
    if (!$row.find('.mapping-action-select').val()) {
        $row.find('.mapping-action-select').val('map');
    }
    
    updateTargetMappingSectionVisibility($row, currentAction);
    if (currentAction === 'map') {
        populateTargetCategoryDropdown(categoryId);
    }
}

// Initialize categories with default mapping action
_transferData.analysisResult.requirementCategories.forEach(function(category) {
    if (!category.mappingAction) {
        category.mappingAction = 'map';
    }
});
```

## Result
The User Transfer Wizard now has identical visual presentation AND behavior to the Cohort Migration Wizard:
- Hierarchy badges clearly show the level of each requirement
- Impact column uses simple text layout
- Status badges provide clear visual feedback
- Bulk action buttons match the style and positioning
- **Mapping action defaults to "Map to Existing"**
- **Both dropdowns appear immediately when selecting a category**
- **Target categories are populated right away**

The UI is now visually and behaviorally consistent between both wizards, providing users with a familiar and efficient experience regardless of which wizard they use.