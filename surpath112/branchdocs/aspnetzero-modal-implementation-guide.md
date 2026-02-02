# ASP.NET Zero Modal Implementation Guide

## Project Overview
This guide outlines the proper patterns and conventions for implementing modals in ASP.NET Zero 11.4 applications, based on the established framework patterns and best practices.

## Technology Stack
- ASP.NET Zero 11.4 framework
- ASP.NET Core MVC with Razor views
- jQuery and Bootstrap for frontend
- ABP Framework for application services
- JavaScript modal management system

## Modal Architecture Overview

ASP.NET Zero uses a sophisticated modal system that provides:
- Dynamic modal creation and management
- Consistent styling and behavior
- Proper lifecycle management
- Integration with ABP's permission system
- Automatic cleanup and memory management

## Implementation Steps

### 1. Create the ViewModel

**Purpose**: Define the data structure for the modal

**Location**: `src/[Project].Web.Mvc/Areas/App/Models/[Entity]/[ModalName]ViewModel.cs`

```csharp
using System;
using System.Collections.Generic;
using [Project].Surpath.Dtos;

namespace [Project].Web.Areas.App.Models.[Entity]
{
    public class [ModalName]ViewModel
    {
        public Guid EntityId { get; set; }
        public string EntityName { get; set; }
        public List<SomeDto> Items { get; set; } = new List<SomeDto>();
    }
}
```

**Key Points:**
- Place in appropriate Models folder structure
- Use meaningful property names
- Initialize collections to avoid null reference exceptions
- Include all data needed for the modal display

### 2. Create the Controller Action

**Purpose**: Handle modal requests and return the partial view

**Location**: Add to existing controller in `src/[Project].Web.Mvc/Areas/App/Controllers/`

```csharp
[AbpMvcAuthorize(AppPermissions.Pages_[Area]_[Permission])]
public async Task<PartialViewResult> [ModalName]Modal([Parameters])
{
    // Get data for modal
    var data = await _appService.GetDataForModal([parameters]);
    
    var viewModel = new [ModalName]ViewModel
    {
        EntityId = entityId,
        EntityName = data.Name,
        Items = data.Items
    };

    return PartialView("_[ModalName]Modal", viewModel);
}
```

**Key Points:**
- Use `PartialViewResult` return type
- Apply proper authorization attributes
- Fetch all required data before creating ViewModel
- Return PartialView with underscore prefix naming convention

### 3. Create the Modal View

**Purpose**: Define the modal's HTML structure and content

**Location**: `src/[Project].Web.Mvc/Areas/App/Views/[Entity]/_[ModalName]Modal.cshtml`

```html
@using [Project].Authorization
@using [Project].Web.Areas.App.Models.[Entity]
@using [Project].Web.Areas.App.Models.Common.Modals
@model [ModalName]ViewModel

@await Html.PartialAsync("~/Areas/App/Views/Common/Modals/_ModalHeader.cshtml", new ModalHeaderViewModel(L("ModalTitle")))

<div class="modal-body">
    <div class="form-group mb-3">
        <label class="form-label">@L("EntityName")</label>
        <p class="form-control-plaintext fw-bold">@Model.EntityName</p>
        <input type="hidden" id="entityId" value="@Model.EntityId" />
    </div>
    
    <div class="form-group">
        <label class="form-label">@L("Items")</label>
        <div class="table-responsive">
            <table class="table table-hover table-striped" id="ItemsTable">
                <thead>
                    <tr>
                        <th style="width: 40px;">
                            <div class="form-check">
                                <input type="checkbox" class="form-check-input" id="selectAllItems" />
                                <label class="form-check-label" for="selectAllItems"></label>
                            </div>
                        </th>
                        <th>@L("Name")</th>
                        <th>@L("Description")</th>
                    </tr>
                </thead>
                <tbody>
                    @if (Model.Items.Any())
                    {
                        @foreach (var item in Model.Items)
                        {
                            <tr>
                                <td>
                                    <div class="form-check">
                                        <input type="checkbox" class="form-check-input item-checkbox" value="@item.Id" id="item_@item.Id" />
                                        <label class="form-check-label" for="item_@item.Id"></label>
                                    </div>
                                </td>
                                <td>@item.Name</td>
                                <td>@item.Description</td>
                            </tr>
                        }
                    }
                    else
                    {
                        <tr>
                            <td colspan="3" class="text-center text-muted">@L("NoDataAvailable")</td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
    </div>
</div>

<div class="modal-footer">
    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">@L("Cancel")</button>
    @if (IsGranted(AppPermissions.Pages_[Area]_[Permission]))
    {
        <button type="button" class="btn btn-primary" id="processSelectedItems">
            <i class="fa fa-check"></i> @L("Process")
        </button>
    }
</div>
```

**Key Points:**
- Use `_ModalHeader.cshtml` partial for consistent header
- Structure: modal-body and modal-footer as separate divs
- Use permission checks for conditional button display
- Include hidden inputs for IDs that JavaScript will need
- Use Bootstrap classes for consistent styling

### 4. Create the JavaScript File

**Purpose**: Handle modal behavior and user interactions

**Location**: `src/[Project].Web.Mvc/wwwroot/view-resources/Areas/App/Views/[Entity]/_[ModalName]Modal.js`

```javascript
(function ($) {
    app.modals.[ModalName]Modal = function () {
        var _modalManager;
        var _$modal;
        var _$itemsTable;
        var _appService = abp.services.app.[serviceName];

        var _permissions = {
            process: abp.auth.hasPermission('Pages.[Area].[Permission]')
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$modal = _modalManager.getModal();
            _$itemsTable = _$modal.find('#ItemsTable');
            
            initializeEventHandlers();
            updateSelectionInfo();
        };

        function initializeEventHandlers() {
            // Select all checkbox
            _$modal.on('change', '#selectAllItems', function () {
                var isChecked = $(this).prop('checked');
                _$modal.find('.item-checkbox').prop('checked', isChecked);
                updateSelectionInfo();
            });

            // Individual checkbox change
            _$modal.on('change', '.item-checkbox', function () {
                updateSelectAllCheckbox();
                updateSelectionInfo();
            });

            // Process button
            _$modal.on('click', '#processSelectedItems', function () {
                var selectedIds = getSelectedItemIds();
                if (selectedIds.length === 0) {
                    abp.message.warn(app.localize('PleaseSelectAtLeastOneItem'));
                    return;
                }
                
                processItems(selectedIds);
            });
        }

        function updateSelectAllCheckbox() {
            var $itemCheckboxes = _$modal.find('.item-checkbox');
            var checkedCount = $itemCheckboxes.filter(':checked').length;
            var totalCount = $itemCheckboxes.length;
            
            var $selectAllCheckbox = _$modal.find('#selectAllItems');
            
            if (checkedCount === 0) {
                $selectAllCheckbox.prop('checked', false);
                $selectAllCheckbox.prop('indeterminate', false);
            } else if (checkedCount === totalCount) {
                $selectAllCheckbox.prop('checked', true);
                $selectAllCheckbox.prop('indeterminate', false);
            } else {
                $selectAllCheckbox.prop('checked', false);
                $selectAllCheckbox.prop('indeterminate', true);
            }
        }

        function updateSelectionInfo() {
            var selectedCount = _$modal.find('.item-checkbox:checked').length;
            _$modal.find('#processSelectedItems').prop('disabled', selectedCount === 0);
        }

        function getSelectedItemIds() {
            var itemIds = [];
            _$modal.find('.item-checkbox:checked').each(function () {
                itemIds.push($(this).val());
            });
            return itemIds;
        }

        function processItems(itemIds) {
            abp.ui.setBusy(_$modal);
            
            _appService.processItems({
                itemIds: itemIds,
                entityId: _$modal.find('#entityId').val()
            }).done(function (result) {
                if (result.success) {
                    abp.message.success(result.message);
                    _modalManager.close();
                    // Refresh parent page if needed
                    abp.event.trigger('app.[entity]ProcessCompleted');
                } else {
                    abp.message.error(result.message);
                }
            }).always(function () {
                abp.ui.clearBusy(_$modal);
            });
        }

        this.close = function () {
            _modalManager.close();
        };
    };
})(jQuery);
```

**Key Points:**
- Use `_modalManager.getModal()` to get dynamic modal reference
- Scope all selectors to the modal instance
- Use proper ABP service calls with error handling
- Implement loading states with `abp.ui.setBusy()`
- Trigger events for parent page communication

### 5. Register the Modal in Parent Page

**Purpose**: Set up modal manager and integrate with parent page

**Location**: Add to existing page JavaScript file

```javascript
var _[modalName]Modal = new app.ModalManager({
    viewUrl: abp.appPath + 'App/[Entity]/[ModalName]Modal',
    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/[Entity]/_[ModalName]Modal.js',
    modalClass: '[ModalName]Modal',
});
```

**Usage in Actions:**
```javascript
{
    text: app.localize('ModalAction'),
    iconStyle: 'fa fa-icon mr-2',
    visible: function () {
        return _permissions.process;
    },
    action: function (data) {
        _[modalName]Modal.open({ 
            entityId: data.record.entity.id 
        });
    },
}
```

### 6. Include Script Reference

**Purpose**: Load the modal JavaScript file

**Location**: Add to parent page's Scripts section

```html
@section Scripts
{
<script abp-src="/view-resources/Areas/App/Views/[Entity]/_[ModalName]Modal.js" asp-append-version="true"></script>
<script abp-src="/view-resources/Areas/App/Views/[Entity]/Index.js" asp-append-version="true"></script>
}
```

## Best Practices

### 1. **Naming Conventions**
- ViewModels: `[ModalName]ViewModel`
- Views: `_[ModalName]Modal.cshtml`
- JavaScript: `_[ModalName]Modal.js`
- JavaScript Class: `app.modals.[ModalName]Modal`

### 2. **Permission Management**
- Always check permissions in controller actions
- Use conditional rendering in views for buttons
- Verify permissions in JavaScript before actions

### 3. **Error Handling**
- Use try-catch in controller actions
- Implement proper error messages in JavaScript
- Show loading states during operations

### 4. **Memory Management**
- Use modal-scoped selectors to avoid conflicts
- Properly clean up event handlers
- Use `_modalManager.close()` for proper cleanup

### 5. **Localization**
- Use `@L("Key")` in views for all text
- Use `app.localize('Key')` in JavaScript
- Add all strings to localization files

## Common Patterns

### 1. **Selection Management**
```javascript
// Select all functionality
function updateSelectAllCheckbox() {
    var $checkboxes = _$modal.find('.item-checkbox');
    var checkedCount = $checkboxes.filter(':checked').length;
    var totalCount = $checkboxes.length;
    
    var $selectAll = _$modal.find('#selectAll');
    
    if (checkedCount === 0) {
        $selectAll.prop('checked', false).prop('indeterminate', false);
    } else if (checkedCount === totalCount) {
        $selectAll.prop('checked', true).prop('indeterminate', false);
    } else {
        $selectAll.prop('checked', false).prop('indeterminate', true);
    }
}
```

### 2. **Service Integration**
```javascript
// Proper service call pattern
function callService(data) {
    abp.ui.setBusy(_$modal);
    
    _appService.methodName(data)
        .done(function (result) {
            if (result.success) {
                abp.message.success(result.message);
                _modalManager.close();
                refreshParentPage();
            } else {
                abp.message.error(result.message);
            }
        })
        .fail(function (error) {
            abp.message.error(app.localize('AnErrorOccurred'));
        })
        .always(function () {
            abp.ui.clearBusy(_$modal);
        });
}
```

### 3. **Dynamic Content Loading**
```javascript
// Load content after modal opens
this.init = function (modalManager) {
    _modalManager = modalManager;
    _$modal = _modalManager.getModal();
    
    // Load dynamic content
    loadModalContent();
    initializeEventHandlers();
};
```

## Integration Points

### 1. **With DataTables**
- Use modal actions in DataTable row actions
- Refresh DataTable after modal operations
- Pass row data to modal for context

### 2. **With ABP Services**
- Use proper authorization attributes
- Implement comprehensive DTOs
- Handle multi-tenant scenarios

### 3. **With Permission System**
- Check permissions at multiple levels
- Use conditional rendering
- Provide appropriate error messages

## Testing Considerations

### 1. **Unit Testing**
- Test controller actions independently
- Mock application services
- Verify ViewModels are populated correctly

### 2. **Integration Testing**
- Test modal opening and closing
- Verify data loading and display
- Test user interactions and form submissions

### 3. **UI Testing**
- Test responsive behavior
- Verify accessibility features
- Test keyboard navigation

## Troubleshooting

### 1. **Modal Not Opening**
- Check controller action authorization
- Verify script file is loaded
- Check browser console for JavaScript errors

### 2. **JavaScript Errors**
- Ensure modal elements exist before accessing
- Use modal-scoped selectors
- Check for proper modal manager initialization

### 3. **Styling Issues**
- Use Bootstrap classes consistently
- Follow ASP.NET Zero theme patterns
- Test across different screen sizes

## Success Criteria

1. **Functionality**: Modal opens, displays data, and processes user actions correctly
2. **Performance**: Modal loads quickly and responds smoothly to user interactions
3. **Accessibility**: Modal is keyboard navigable and screen reader friendly
4. **Consistency**: Modal follows established ASP.NET Zero patterns and styling
5. **Maintainability**: Code is well-structured and follows naming conventions

This guide provides a comprehensive foundation for implementing modals in ASP.NET Zero applications while maintaining consistency with the framework's established patterns and best practices. 