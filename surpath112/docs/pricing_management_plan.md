# Pricing Management System - Implementation Plan

## Overview
This document outlines the implementation plan for a comprehensive pricing management system that allows administrators to manage service pricing at multiple levels (tenant, department, cohort, and individual user) through a single, intuitive interface.

## Project Goals
1. Create a single-page interface for managing all pricing levels per tenant
2. Minimize popups and modal dialogs (except for entity selection)
3. Reuse existing implementations and endpoints
4. Maintain visual consistency with the existing ASP.NET Zero application
5. Provide an intuitive user experience similar to the pricing prototype

## System Architecture

### Pricing Hierarchy
```
Tenant (Base Price)
├── Department Level Override
│   ├── Cohort Level Override
│   │   └── Individual User Override
│   └── Organization Unit Override
└── Direct User Override
```

Priority Order (Highest to Lowest):
1. Individual User Services
2. Cohort-Level Services
3. Department-Level Services
4. Organization Unit Services
5. Tenant-Level Services

## Implementation Plan

### Phase 1: Backend Infrastructure

#### 1.1 New DTOs (Application.Shared/Surpath/Dtos)
```csharp
// For retrieving hierarchical pricing data
public class GetHierarchicalPricingInput
{
    public int TenantId { get; set; }
    public Guid? SurpathServiceId { get; set; }
}

public class HierarchicalPricingDto
{
    public TenantPricingDto Tenant { get; set; }
    public List<DepartmentPricingDto> Departments { get; set; }
}

public class TenantPricingDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ServicePriceDto> Services { get; set; }
}

public class DepartmentPricingDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<ServicePriceDto> Services { get; set; }
    public List<CohortPricingDto> Cohorts { get; set; }
}

public class CohortPricingDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<ServicePriceDto> Services { get; set; }
    public List<UserPricingDto> Users { get; set; }
}

public class UserPricingDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<ServicePriceDto> Services { get; set; }
}

public class ServicePriceDto
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; }
    public double BasePrice { get; set; }
    public double? OverridePrice { get; set; }
    public double EffectivePrice { get; set; }
    public bool IsInherited { get; set; }
    public Guid? TenantSurpathServiceId { get; set; }
}

// For batch updates
public class BatchUpdatePriceDto
{
    public List<UpdatePriceItemDto> Updates { get; set; }
}

public class UpdatePriceItemDto
{
    public Guid? Id { get; set; } // Existing TenantSurpathService Id
    public Guid SurpathServiceId { get; set; }
    public double? Price { get; set; }
    public Guid? TenantDepartmentId { get; set; }
    public Guid? CohortId { get; set; }
    public long? UserId { get; set; }
}

// For setting all services at once
public class SetAllServicesPriceDto
{
    public double Price { get; set; }
    public string TargetType { get; set; } // "tenant", "department", "cohort", "user"
    public string TargetId { get; set; }
}
```

#### 1.2 Extended Application Service Methods
Add to `ITenantSurpathServicesAppService`:
```csharp
// Get hierarchical pricing data for display
Task<HierarchicalPricingDto> GetHierarchicalPricing(GetHierarchicalPricingInput input);

// Batch update prices
Task BatchUpdatePrices(BatchUpdatePriceDto input);

// Set price for all services at a specific level
Task SetAllServicesPrice(SetAllServicesPriceDto input);

// Get all available SurpathServices for the tenant
Task<List<SurpathServiceDto>> GetAvailableSurpathServices();
```

#### 1.3 Implementation Details
- `GetHierarchicalPricing`: 
  - Retrieves all departments, cohorts, and users for a tenant
  - For each level, gets the TenantSurpathService pricing
  - Calculates effective pricing based on hierarchy
  - Returns structured data for UI display

- `BatchUpdatePrices`:
  - Accepts multiple price updates in a single transaction
  - Creates new TenantSurpathService records where needed
  - Updates existing records
  - Removes records where price is set to null (inherit from parent)

- `SetAllServicesPrice`:
  - Sets the same price for all services at a specific level
  - Useful for bulk operations

### Phase 2: User Interface

#### 2.1 New Page Structure
Create new files:
- `/Areas/App/Views/TenantSurpathServices/PricingManagement.cshtml`
- `/wwwroot/view-resources/Areas/App/Views/TenantSurpathServices/PricingManagement.js`
- `/Areas/App/Views/TenantSurpathServices/_PriceEditModal.cshtml`
- `/wwwroot/view-resources/Areas/App/Views/TenantSurpathServices/_PriceEditModal.js`

#### 2.2 Controller Action
Add to `TenantSurpathServicesController`:
```csharp
public async Task<ActionResult> PricingManagement()
{
    var model = new PricingManagementViewModel
    {
        Tenants = await GetTenantsForDropdown() // For tenant selector
    };
    return View(model);
}
```

#### 2.3 UI Layout Structure

##### Main Page Layout
```html
<div class="content d-flex flex-column flex-column-fluid">
    <!-- Header with Title and Actions -->
    <abp-page-subheader title="Service Pricing Management" 
                        description="Configure pricing across tenants, departments, cohorts, and users">
        <button id="SetAllPricesButton" class="btn btn-primary">
            <i class="fa fa-dollar-sign"></i> Set Pricing for All Services
        </button>
    </abp-page-subheader>

    <div class="container-fluid">
        <!-- Tenant and Service Selection Card -->
        <div class="card card-custom gutter-b">
            <div class="card-header">
                <h3 class="card-title">Select Tenant and Service</h3>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <label>Tenant</label>
                        <select id="TenantSelect" class="form-control">
                            <!-- Populated from server -->
                        </select>
                    </div>
                    <div class="col-md-6">
                        <label>Service</label>
                        <select id="ServiceSelect" class="form-control">
                            <option value="">All Services</option>
                            <!-- Populated via AJAX -->
                        </select>
                    </div>
                </div>
            </div>
        </div>

        <!-- Service Info Display (when single service selected) -->
        <div id="ServiceInfoCard" class="card card-custom gutter-b" style="display:none;">
            <div class="card-body bg-light-primary">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h4 id="ServiceName"></h4>
                        <p id="ServiceDescription" class="text-muted mb-0"></p>
                    </div>
                    <div class="text-right">
                        <small class="text-muted">Base Price</small>
                        <h3 id="ServiceBasePrice" class="text-primary mb-0"></h3>
                    </div>
                </div>
            </div>
        </div>

        <!-- Pricing Tree View -->
        <div id="PricingTreeContainer">
            <!-- Dynamically populated accordion structure -->
        </div>
    </div>
</div>
```

##### Accordion Structure (Bootstrap Collapse Pattern)
```html
<!-- Tenant Level -->
<div class="card card-custom gutter-b">
    <div class="card-header bg-light-primary">
        <div class="card-title collapsed d-flex justify-content-between align-items-center w-100">
            <h3 class="mb-0">
                <span class="svg-icon svg-icon-primary svg-icon-2x"><!--Building icon--></span>
                <span class="ms-2">{{TenantName}}</span>
                <span class="badge badge-primary ms-2">Tenant</span>
            </h3>
            <div class="d-flex align-items-center">
                <h4 class="mb-0 me-3">
                    <span class="text-muted">$</span>
                    <span class="price-display">{{Price}}</span>
                </h4>
                <button class="btn btn-sm btn-light-primary btn-icon edit-price-btn" 
                        data-level="tenant" data-id="{{TenantId}}">
                    <i class="fa fa-edit"></i>
                </button>
            </div>
        </div>
    </div>
    
    <div class="card-body">
        <!-- Departments Accordion -->
        <div class="accordion" id="departmentsAccordion">
            <!-- Department Items -->
            <div class="accordion-item mb-3">
                <div class="accordion-header">
                    <button class="accordion-button collapsed" type="button" 
                            data-bs-toggle="collapse" data-bs-target="#dept-{{DeptId}}">
                        <div class="d-flex justify-content-between align-items-center w-100">
                            <div>
                                <span class="svg-icon svg-icon-2x"><!--Folder icon--></span>
                                <span class="ms-2 fw-bold">{{DepartmentName}}</span>
                                <span class="badge badge-secondary ms-2">Department</span>
                            </div>
                            <div class="d-flex align-items-center me-3">
                                <span class="price-display {{inherited-class}}">
                                    ${{EffectivePrice}}
                                </span>
                                <button class="btn btn-sm btn-light-secondary btn-icon ms-2 edit-price-btn"
                                        data-level="department" data-id="{{DeptId}}">
                                    <i class="fa fa-edit"></i>
                                </button>
                            </div>
                        </div>
                    </button>
                </div>
                
                <div id="dept-{{DeptId}}" class="accordion-collapse collapse">
                    <div class="accordion-body">
                        <!-- Cohorts Nested Accordion -->
                        <div class="ps-4">
                            <!-- Similar structure for cohorts -->
                            <div class="card mb-2">
                                <div class="card-header bg-light-info cursor-pointer" 
                                     data-bs-toggle="collapse" data-bs-target="#cohort-{{CohortId}}">
                                    <div class="d-flex justify-content-between align-items-center">
                                        <div>
                                            <span class="svg-icon"><!--Group icon--></span>
                                            <span class="ms-2">{{CohortName}}</span>
                                            <span class="badge badge-info ms-2">Cohort</span>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <span class="price-display">${{Price}}</span>
                                            <button class="btn btn-sm btn-light-info btn-icon ms-2">
                                                <i class="fa fa-edit"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div id="cohort-{{CohortId}}" class="collapse">
                                    <div class="card-body">
                                        <!-- Users Table -->
                                        <table class="table table-hover">
                                            <thead>
                                                <tr>
                                                    <th>User</th>
                                                    <th>Email</th>
                                                    <th>Price</th>
                                                    <th>Action</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <!-- User rows -->
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
```

#### 2.4 JavaScript Implementation

##### Main Page JavaScript Structure
```javascript
(function () {
    var _tenantSurpathServicesService = abp.services.app.tenantSurpathServices;
    var _$pricingTree = $('#PricingTreeContainer');
    var _selectedTenantId = null;
    var _selectedServiceId = null;
    var _hierarchicalData = null;
    
    // Modal for price editing
    var _priceEditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'App/TenantSurpathServices/PriceEditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TenantSurpathServices/_PriceEditModal.js',
        modalClass: 'PriceEditModal'
    });
    
    // Initialize
    function init() {
        loadTenants();
        bindEvents();
    }
    
    // Event Bindings
    function bindEvents() {
        $('#TenantSelect').on('change', function() {
            _selectedTenantId = $(this).val();
            if (_selectedTenantId) {
                loadServices();
                loadPricingData();
            }
        });
        
        $('#ServiceSelect').on('change', function() {
            _selectedServiceId = $(this).val();
            loadPricingData();
            updateServiceInfo();
        });
        
        // Price edit button click (using delegation)
        _$pricingTree.on('click', '.edit-price-btn', function(e) {
            e.stopPropagation();
            var $btn = $(this);
            var level = $btn.data('level');
            var id = $btn.data('id');
            var currentPrice = $btn.closest('.d-flex').find('.price-display').text().replace('$', '');
            
            openPriceEditModal(level, id, currentPrice);
        });
        
        // Set all prices button
        $('#SetAllPricesButton').on('click', function() {
            openSetAllPricesModal();
        });
    }
    
    // Load hierarchical pricing data
    function loadPricingData() {
        if (!_selectedTenantId) return;
        
        abp.ui.setBusy(_$pricingTree);
        
        _tenantSurpathServicesService.getHierarchicalPricing({
            tenantId: _selectedTenantId,
            surpathServiceId: _selectedServiceId
        }).done(function(result) {
            _hierarchicalData = result;
            renderPricingTree(result);
        }).always(function() {
            abp.ui.clearBusy(_$pricingTree);
        });
    }
    
    // Render the pricing tree
    function renderPricingTree(data) {
        var html = buildTenantSection(data.tenant);
        
        // Add departments
        if (data.departments && data.departments.length > 0) {
            html += '<div class="mt-4">';
            html += buildDepartmentsAccordion(data.departments);
            html += '</div>';
        }
        
        _$pricingTree.html(html);
        
        // Initialize tooltips for inherited prices
        $('[data-bs-toggle="tooltip"]').tooltip();
    }
    
    // Build HTML sections
    function buildTenantSection(tenant) {
        // Implementation of tenant section HTML
    }
    
    function buildDepartmentsAccordion(departments) {
        // Implementation of departments accordion HTML
    }
    
    // Price editing
    function openPriceEditModal(level, id, currentPrice) {
        _priceEditModal.open({
            level: level,
            targetId: id,
            currentPrice: currentPrice,
            serviceId: _selectedServiceId,
            tenantId: _selectedTenantId
        });
    }
    
    // Handle save from modal
    _priceEditModal.onResult(function() {
        loadPricingData(); // Reload the tree
        abp.notify.success(app.localize('SavedSuccessfully'));
    });
    
    // Initialize on document ready
    $(function() {
        init();
    });
})();
```

### Phase 3: Modal Implementation

#### 3.1 Price Edit Modal
Simple modal for editing individual prices:
```html
<!-- _PriceEditModal.cshtml -->
<div class="modal-body">
    <form name="PriceEditForm" role="form" novalidate class="form-validation">
        <input type="hidden" id="Level" value="@Model.Level" />
        <input type="hidden" id="TargetId" value="@Model.TargetId" />
        <input type="hidden" id="ServiceId" value="@Model.ServiceId" />
        
        <div class="mb-3">
            <label class="form-label">Current Price</label>
            <div class="input-group">
                <span class="input-group-text">$</span>
                <input type="text" class="form-control" value="@Model.CurrentPrice" readonly />
            </div>
        </div>
        
        <div class="mb-3">
            <label class="form-label">New Price</label>
            <div class="input-group">
                <span class="input-group-text">$</span>
                <input type="number" id="NewPrice" class="form-control" 
                       min="0" max="999999" step="0.01" required />
            </div>
            <div class="form-text">
                Leave empty to inherit from parent level
            </div>
        </div>
    </form>
</div>
```

#### 3.2 Lookup Modals
Reuse existing lookup modals:
- `_TenantDepartmentLookupTableModal` for department selection
- `_TenantSurpathServiceCohortLookupTableModal` for cohort selection
- `_TenantSurpathServiceUserLookupTableModal` for user selection

### Phase 4: Styling and UX

#### 4.1 Visual Indicators
- **Inherited prices**: Gray text with info icon tooltip
- **Override prices**: Bold black text
- **Price badges**: Color-coded by level
  - Tenant: Primary (blue)
  - Department: Secondary (gray)
  - Cohort: Info (light blue)
  - User: Success (green)

#### 4.2 Responsive Design
- Mobile-first approach
- Collapsible sections for small screens
- Touch-friendly edit buttons
- Horizontal scrolling for tables on mobile

#### 4.3 Performance Optimizations
- Lazy load cohorts and users (only when expanded)
- Implement pagination for large user lists
- Cache pricing data client-side
- Batch updates to minimize server calls

### Phase 5: Testing and Validation

#### 5.1 Unit Tests
- Test batch update logic
- Test pricing hierarchy calculations
- Test permission checks

#### 5.2 Integration Tests
- Test full pricing update flow
- Test concurrent editing scenarios
- Test data integrity

#### 5.3 UI Tests
- Test responsive behavior
- Test keyboard navigation
- Test screen reader compatibility

## Implementation Checklist

### Backend Tasks
- [ ] Create new DTOs for hierarchical pricing
- [ ] Implement GetHierarchicalPricing method
- [ ] Implement BatchUpdatePrices method
- [ ] Implement SetAllServicesPrice method
- [ ] Add unit tests for new methods
- [ ] Add authorization checks

### Frontend Tasks
- [ ] Create PricingManagement.cshtml view
- [ ] Create PricingManagement.js with tree rendering
- [ ] Create PriceEditModal for individual price updates
- [ ] Implement tenant/service selection
- [ ] Implement accordion/collapse functionality
- [ ] Add loading indicators
- [ ] Implement error handling
- [ ] Add success notifications

### Integration Tasks
- [ ] Wire up modal callbacks
- [ ] Implement real-time price updates
- [ ] Add validation for price inputs
- [ ] Test with large datasets
- [ ] Optimize performance

### Documentation Tasks
- [ ] Update user documentation
- [ ] Add code comments
- [ ] Create usage examples
- [ ] Document API endpoints

## Success Criteria
1. Single page manages all pricing levels
2. Minimal use of popups (only for entity selection)
3. Visual consistency with existing application
4. Intuitive navigation through pricing hierarchy
5. Real-time updates without page refresh
6. Mobile-responsive design
7. Performance with large datasets
8. Proper error handling and validation

## Notes
- The implementation follows ASP.NET Zero patterns and conventions
- Reuses existing DTOs and services where possible
- Maintains backward compatibility with existing pricing system
- Uses Bootstrap 5 components for consistency
- Follows DRY principles throughout

## Key Findings During Implementation

### 1. Existing Price Calculation Logic
Found existing price calculation logic in `PaymentController.PurchaseModal` that implements the priority hierarchy:
```csharp
var prioritizedServices = allSurpathServices
    .GroupBy(s => s.SurpathServiceId)
    .Select(group => group
        .OrderByDescending(s => 
            s.UserId != null ? 4 :           // Highest priority
            s.CohortId != null ? 3 :
            s.TenantDepartmentId != null ? 2 :
            1                                // Lowest priority (tenant-wide)
        )
        .First()
    )
    .ToList();
```

This logic should be reused rather than reimplemented to ensure consistency across the application.

### 2. Entity Inheritance Pattern
The DTOs use inheritance to extend base entities:
- `DepartmentPricingDto : TenantDepartmentDto`
- `CohortPricingDto : CohortDto`

This approach reuses existing DTOs and adds pricing-specific properties.

### 3. Object Mapping Considerations
The application uses AutoMapper via `ObjectMapper.Map<>()`. Need to ensure proper mapping configurations for the new DTOs.

### 4. Tenant Context
All service methods must respect the multi-tenant context. Found that:
- Services use `AbpSession.TenantId` for current tenant
- When acting as host, must use `CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant)`

### 5. Existing UI Patterns
- Accordion implementation found in `SurpathServices/Manage.cshtml`
- Uses Bootstrap collapse with `.collapsible` class and `data-bs-toggle="collapse"`
- Rotating chevron icons in `.card-toolbar.rotate-90`

### 6. Performance Considerations
The `GetHierarchicalPricing` method could benefit from:
- Batch loading all data upfront to minimize queries
- Using `.Include()` for eager loading related entities
- Consider pagination for large user lists within cohorts

## Implementation Complete

### Final Implementation Details

1. **Backend Services**
   - Created hierarchical pricing DTOs in `HierarchicalPricingDtos.cs`
   - Implemented all methods in `TenantSurpathServicesAppService`
   - Added AutoMapper configurations for DTO inheritance
   - Reused existing pricing calculation logic

2. **User Interface**
   - Created `PricingManagement.cshtml` with accordion-based hierarchical display
   - Implemented `PricingManagement.js` with dynamic tree rendering
   - Created modals for individual price editing and bulk updates
   - Used existing Bootstrap patterns and ASP.NET Zero conventions

3. **Navigation**
   - Added submenu under TenantSurpathServices
   - "Services List" - existing index page
   - "Pricing Management" - new hierarchical pricing page

4. **Key Features Implemented**
   - Single page for managing all pricing levels
   - Minimal popups (only for price editing)
   - Accordion/collapse UI for easy navigation
   - Visual indicators for inherited vs override prices
   - "Set Pricing for All Services" functionality
   - Responsive design

5. **Code Reuse**
   - Extended existing DTOs (TenantDepartmentDto, CohortDto)
   - Reused existing modals for entity selection
   - Utilized existing pricing priority logic
   - Maintained consistent styling with application

6. **Testing Considerations**
   - All methods respect multi-tenant context
   - Proper authorization checks in place
   - Null checks and error handling implemented
   - Client-side validation for price inputs