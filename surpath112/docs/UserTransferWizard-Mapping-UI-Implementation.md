# User Transfer Wizard - Mapping UI Implementation Guide

## Overview
The User Transfer Wizard's category mapping UI (Step 3) should be identical to the Cohort Migration Wizard's mapping interface, providing the same functionality for mapping requirements and creating new ones during user transfers.

## Current State vs. Desired State

### Current State (User Transfer Wizard)
```javascript
function loadCategoryMappings() {
    // TODO: Load category mappings for step 2
    var container = $('#categoryMappingContainer');
    container.html('<p class="text-muted">' + app.localize('LoadingCategoryMappings') + '...</p>');
    
    // This would typically load the actual category mappings
    // For now, just show a placeholder
    setTimeout(function() {
        container.html('<p>' + app.localize('NoCategoryMappingsRequired') + '</p>');
    }, 500);
}
```

### Desired State (From Cohort Migration Wizard)
A comprehensive mapping interface with:
- DataTable showing all categories requiring migration
- Three mapping actions per category: Map, Copy, Skip
- Bulk actions for efficiency
- Auto-suggest functionality
- Validation before proceeding

## Implementation Requirements

### 1. UI Structure
Copy the HTML structure from `_MigrationWizardModal.cshtml`:

```html
<!-- Step 3: Category Mapping -->
<div id="step3" class="wizard-step">
    <div class="card">
        <div class="card-header">
            <h5 class="card-title mb-0">@L("MapRequirementCategories")</h5>
            <p class="text-muted mb-0">@L("ChooseHowToHandleCategories")</p>
        </div>
        <div class="card-body">
            <!-- Migration Analysis Summary -->
            <div id="migrationSummary" class="alert alert-info mb-4">
                <!-- Summary statistics -->
            </div>

            <!-- No Migration Required Section -->
            <div id="noMigrationRequiredSection" class="mb-4">
                <!-- Requirements that exist in both cohorts -->
            </div>

            <!-- Bulk Actions -->
            <div class="row mb-3">
                <div class="col-md-6">
                    <div class="btn-group" role="group">
                        <button type="button" class="btn btn-outline-primary btn-sm" id="bulkMapBtn">
                            <i class="fa fa-link"></i> @L("BulkMap")
                        </button>
                        <button type="button" class="btn btn-outline-secondary btn-sm" id="bulkCopyBtn">
                            <i class="fa fa-copy"></i> @L("BulkCopy")
                        </button>
                        <button type="button" class="btn btn-outline-warning btn-sm" id="bulkSkipBtn">
                            <i class="fa fa-skip-forward"></i> @L("BulkSkip")
                        </button>
                    </div>
                </div>
                <div class="col-md-6 text-end">
                    <button type="button" class="btn btn-outline-info btn-sm" id="autoSuggestBtn">
                        <i class="fa fa-magic"></i> @L("AutoSuggest")
                    </button>
                    <button type="button" class="btn btn-outline-success btn-sm" id="validateMappingsBtn">
                        <i class="fa fa-check"></i> @L("ValidateMappings")
                    </button>
                </div>
            </div>

            <!-- Category Mapping Table -->
            <div class="table-responsive">
                <table id="categoryMappingTable" class="table table-striped table-bordered">
                    <thead>
                        <tr>
                            <th width="5%">
                                <input type="checkbox" id="selectAllCategories">
                            </th>
                            <th width="20%">@L("SourceCategory")</th>
                            <th width="15%">@L("Requirement")</th>
                            <th width="10%">@L("Impact")</th>
                            <th width="15%">@L("MappingAction")</th>
                            <th width="20%">@L("TargetMapping")</th>
                            <th width="10%">@L("Suggestions")</th>
                            <th width="5%">@L("Status")</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Data loaded via DataTable -->
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>
```

### 2. JavaScript Implementation

Replace the placeholder `loadCategoryMappings()` function with:

```javascript
function loadCategoryMappings() {
    var container = $('#categoryMappingContainer');
    
    // Show loading state
    showCategoryMappingLoading();
    
    // Initialize DataTable for category mapping
    _categoryMappingTable = $('#categoryMappingTable').DataTable({
        paging: false,
        serverSide: false,
        processing: false,
        searching: true,
        ordering: true,
        data: _transferData.analysisResult.requirementCategories,
        columns: [
            {
                data: null,
                render: function(data, type, row) {
                    return '<input type="checkbox" class="category-checkbox" data-category-id="' + row.categoryId + '">';
                }
            },
            {
                data: 'categoryName',
                render: function(data, type, row) {
                    return '<strong>' + data + '</strong>';
                }
            },
            {
                data: 'requirementName'
            },
            {
                data: null,
                render: function(data, type, row) {
                    return '<span class="badge badge-info">' + row.affectedUsersCount + ' users</span><br>' +
                           '<span class="badge badge-secondary">' + row.recordStatesCount + ' records</span>';
                }
            },
            {
                data: null,
                render: function(data, type, row) {
                    return renderMappingActionSelect(row);
                }
            },
            {
                data: null,
                render: function(data, type, row) {
                    return renderTargetMappingSection(row);
                }
            },
            {
                data: null,
                render: function(data, type, row) {
                    return '<button class="btn btn-sm btn-outline-info suggestions-btn" data-category-id="' + row.categoryId + '">' +
                           '<i class="fa fa-lightbulb"></i></button>';
                }
            },
            {
                data: null,
                render: function(data, type, row) {
                    return '<i class="fa fa-circle text-warning" title="Pending"></i>';
                }
            }
        ],
        drawCallback: function() {
            initializeCategoryMappingEventHandlers();
            updateMappingProgress();
        }
    });
    
    // Load target categories for mapping
    loadTargetCategories();
    
    // Display requirements that don't need mapping
    displayNoMigrationRequiredSection();
}

function renderMappingActionSelect(row) {
    var html = '<select class="form-control mapping-action-select" data-category-id="' + row.categoryId + '">';
    html += '<option value="">-- Select Action --</option>';
    html += '<option value="map">Map to Existing</option>';
    html += '<option value="copy">Copy to New</option>';
    html += '<option value="skip">Skip</option>';
    html += '</select>';
    return html;
}

function renderTargetMappingSection(row) {
    var html = '<div class="target-mapping-section" style="display:none;">';
    
    // Map to existing section
    html += '<div class="target-category-section" style="display:none;">';
    html += '<select class="form-control target-category-select" data-category-id="' + row.categoryId + '">';
    html += '<option value="">Select Target Category</option>';
    html += '</select>';
    html += '</div>';
    
    // Copy to new section
    html += '<div class="new-category-section" style="display:none;">';
    html += '<input type="text" class="form-control new-category-input mb-1" placeholder="New Requirement Name" data-category-id="' + row.categoryId + '" data-field="requirement">';
    html += '<input type="text" class="form-control new-category-input" placeholder="New Category Name" data-category-id="' + row.categoryId + '" data-field="category">';
    html += '</div>';
    
    // Skip warning section
    html += '<div class="skip-warning-section" style="display:none;">';
    html += '<div class="alert alert-warning mb-0 py-1 px-2"><i class="fa fa-exclamation-triangle"></i> Data will be lost</div>';
    html += '</div>';
    
    html += '</div>';
    return html;
}
```

### 3. Event Handlers

Add comprehensive event handlers for the mapping interface:

```javascript
function initializeCategoryMappingEventHandlers() {
    var modal = _modalManager.getModal();
    
    // Mapping action change
    modal.off('change', '.mapping-action-select').on('change', '.mapping-action-select', function() {
        var categoryId = $(this).data('category-id');
        var action = $(this).val();
        var $row = $(this).closest('tr');
        
        // Update UI based on selected action
        updateTargetMappingSectionVisibility($row, action);
        
        // Store mapping in transfer data
        if (!_transferData.categoryMappings) {
            _transferData.categoryMappings = {};
        }
        
        if (action === 'skip') {
            delete _transferData.categoryMappings[categoryId];
        }
        
        updateMappingProgress();
    });
    
    // Target category selection
    modal.off('change', '.target-category-select').on('change', '.target-category-select', function() {
        var sourceCategoryId = $(this).data('category-id');
        var targetCategoryId = $(this).val();
        
        if (targetCategoryId) {
            _transferData.categoryMappings[sourceCategoryId] = targetCategoryId;
        }
        
        updateMappingProgress();
    });
    
    // New category inputs
    modal.off('input', '.new-category-input').on('input', '.new-category-input', function() {
        var categoryId = $(this).data('category-id');
        var field = $(this).data('field');
        var value = $(this).val();
        
        // Store new category data
        if (!_transferData.newCategories) {
            _transferData.newCategories = {};
        }
        
        if (!_transferData.newCategories[categoryId]) {
            _transferData.newCategories[categoryId] = {};
        }
        
        _transferData.newCategories[categoryId][field] = value;
        
        updateMappingProgress();
    });
}

function updateTargetMappingSectionVisibility($row, action) {
    var $targetSection = $row.find('.target-mapping-section');
    var $mapSection = $row.find('.target-category-section');
    var $copySection = $row.find('.new-category-section');
    var $skipSection = $row.find('.skip-warning-section');
    
    // Hide all sections first
    $mapSection.hide();
    $copySection.hide();
    $skipSection.hide();
    
    // Show relevant section
    if (action === 'map') {
        $targetSection.show();
        $mapSection.show();
    } else if (action === 'copy') {
        $targetSection.show();
        $copySection.show();
    } else if (action === 'skip') {
        $targetSection.show();
        $skipSection.show();
    } else {
        $targetSection.hide();
    }
}
```

### 4. Bulk Actions Implementation

```javascript
function applyBulkAction(action) {
    var selectedCategories = [];
    
    $('.category-checkbox:checked').each(function() {
        selectedCategories.push($(this).data('category-id'));
    });
    
    if (selectedCategories.length === 0) {
        abp.message.warn(app.localize('PleaseSelectCategories'));
        return;
    }
    
    selectedCategories.forEach(function(categoryId) {
        var $select = $('.mapping-action-select[data-category-id="' + categoryId + '"]');
        $select.val(action).trigger('change');
    });
    
    abp.message.success(app.localize('BulkActionApplied'));
}
```

### 5. Auto-Suggest Feature

```javascript
function autoSuggestMappings() {
    abp.ui.setBusy($('#categoryMappingTable'));
    
    // Get target categories
    var targetCategories = _targetCategoriesCache || [];
    
    _transferData.analysisResult.requirementCategories.forEach(function(sourceCategory) {
        var bestMatch = findBestMatch(sourceCategory, targetCategories);
        
        if (bestMatch && bestMatch.confidence > 0.9) {
            // Auto-select mapping for high confidence matches
            var $select = $('.mapping-action-select[data-category-id="' + sourceCategory.categoryId + '"]');
            $select.val('map').trigger('change');
            
            var $targetSelect = $('.target-category-select[data-category-id="' + sourceCategory.categoryId + '"]');
            $targetSelect.val(bestMatch.categoryId);
        }
    });
    
    abp.ui.clearBusy($('#categoryMappingTable'));
    abp.message.success(app.localize('AutoSuggestComplete'));
}

function findBestMatch(sourceCategory, targetCategories) {
    var bestMatch = null;
    var highestScore = 0;
    
    targetCategories.forEach(function(targetCategory) {
        var score = calculateSimilarityScore(
            sourceCategory.categoryName + ' ' + sourceCategory.requirementName,
            targetCategory.categoryName + ' ' + targetCategory.requirementName
        );
        
        if (score > highestScore) {
            highestScore = score;
            bestMatch = {
                categoryId: targetCategory.categoryId,
                confidence: score
            };
        }
    });
    
    return bestMatch;
}
```

### 6. Validation

```javascript
function validateCategoryMappings() {
    var isValid = true;
    var errors = [];
    
    _transferData.analysisResult.requirementCategories.forEach(function(category) {
        var $actionSelect = $('.mapping-action-select[data-category-id="' + category.categoryId + '"]');
        var action = $actionSelect.val();
        
        if (!action) {
            isValid = false;
            errors.push('Category "' + category.categoryName + '" has no mapping action selected');
        } else if (action === 'map') {
            var targetId = $('.target-category-select[data-category-id="' + category.categoryId + '"]').val();
            if (!targetId) {
                isValid = false;
                errors.push('Category "' + category.categoryName + '" needs a target category selected');
            }
        } else if (action === 'copy') {
            var newReq = $('input[data-category-id="' + category.categoryId + '"][data-field="requirement"]').val();
            var newCat = $('input[data-category-id="' + category.categoryId + '"][data-field="category"]').val();
            if (!newReq || !newCat) {
                isValid = false;
                errors.push('Category "' + category.categoryName + '" needs new requirement and category names');
            }
        }
    });
    
    if (!isValid) {
        showValidationErrors(errors);
    }
    
    return isValid;
}
```

## Integration with User Transfer Service

### 1. Update TransferSelectedUsersInput DTO
Add properties to handle the mapping data:
```csharp
public Dictionary<Guid, Guid> CategoryMappings { get; set; }
public Dictionary<Guid, NewCategoryDto> NewCategories { get; set; }
public List<Guid> SkippedCategories { get; set; }
```

### 2. Update Transfer Logic
Enhance the `TransferSelectedUsers` method to handle:
- Creating new requirements/categories when "Copy to New" is selected
- Mapping record states to new categories
- Skipping categories marked for skip

## Benefits of This Implementation

1. **Consistency**: Users familiar with cohort migration will immediately understand the interface
2. **Flexibility**: Supports all three mapping scenarios (map, copy, skip)
3. **Efficiency**: Bulk actions and auto-suggest save time
4. **Safety**: Validation ensures no data loss without user awareness
5. **Transparency**: Shows impact (users/records) for each category
6. **User Control**: Allows creating new requirements during transfer

## Testing Checklist

- [ ] All three mapping actions work correctly
- [ ] Bulk actions apply to selected categories
- [ ] Auto-suggest provides reasonable matches
- [ ] Validation catches all error cases
- [ ] New requirements are created in target cohort
- [ ] Record states are properly mapped/migrated
- [ ] Progress tracking updates accurately
- [ ] UI is responsive and user-friendly

## Conclusion

By implementing the same mapping UI from the cohort migration wizard, the user transfer wizard will provide a complete, professional solution for handling complex requirement mappings during user transfers between cohorts.