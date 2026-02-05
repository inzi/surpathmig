# TenantSurpathService IsEnabled Implementation Plan

## Overview
The TenantSurpathService entity has an `IsEnabled` boolean property that is currently not being honored throughout the system. This implementation plan outlines the changes needed to:
1. Add UI controls to enable/disable services at all hierarchical levels
2. Honor the IsEnabled setting in compliance calculations
3. Honor the IsEnabled setting in pricing calculations for registration and purchases

## Implementation Status
**Last Updated:** 2025-07-24
- **Phase 1**: ✅ COMPLETED - UI Enable/Disable functionality (including fix for disabled services hierarchy display)
- **Phase 2**: ✅ COMPLETED - Compliance calculations respect IsEnabled
- **Phase 3**: ✅ COMPLETED - Pricing calculations filter disabled services
- **Phase 4**: 🔄 PENDING - Testing and validation
- **Phase 5**: 🔄 PENDING - Data migration and cleanup
- **Phase 6**: 🔄 PENDING - Documentation and communication

## Current State Analysis

### Entity Structure
- **TenantSurpathService** entity (src/inzibackend.Core/Surpath/TenantSurpathService.cs)
  - Has `IsEnabled` property (boolean)
  - Supports hierarchical assignment:
    - Tenant level (no department, cohort, or user)
    - Department level
    - Cohort level
    - User level

### UI Components
- **Main List View** (src/inzibackend.Web.Mvc/Areas/App/Views/TenantSurpathServices/Index.cshtml)
  - Already displays IsEnabled status in the grid
  - Has filter for IsEnabled status
- **Create/Edit Modal** (_CreateOrEditModal.cshtml)
  - Already has checkbox for IsEnabled property

### Backend Services
- **SurpathComplianceEvaluator** - Calculates compliance but doesn't check IsEnabled
- **HierarchicalPricingManager** - Manages pricing but doesn't filter by IsEnabled

## Implementation Tasks

### Phase 1: UI Enhancements for Enable/Disable Functionality

#### TODO 1: Add Quick Toggle in Grid View ✅ COMPLETED
- [x] Add toggle switch in the TenantSurpathServices grid for quick enable/disable
- [x] Update Index.js to handle toggle action via AJAX
- [x] Add new endpoint in TenantSurpathServicesAppService for toggle action (using CreateOrEdit)
- [x] Ensure proper permissions are checked (Pages.TenantSurpathServices.Edit)

#### TODO 2: Bulk Operations ✅ COMPLETED
- [x] Add checkbox column for multi-select in grid
- [x] Add bulk enable/disable buttons in the toolbar
- [x] Implement bulk update endpoint in app service
- [x] Add confirmation dialog for bulk operations

#### TODO 3: Visual Indicators ✅ COMPLETED
- [x] Style disabled services differently in the grid (grayed out)
- [x] Add icon or badge to indicate inherited vs. overridden status
- [x] Update row styling based on IsEnabled status

### Phase 2: Compliance Calculation Updates

#### TODO 4: Update SurpathComplianceEvaluator ✅ COMPLETED
- [x] Modified `SetTenantRequirements` method to filter out disabled services
- [x] Added condition: `&& (tss == null || (tss.IsDeleted == false && tss.IsEnabled == true && tss != null))`
- [x] Updated the LINQ query to exclude requirements linked to disabled TenantSurpathServices

#### TODO 5: Update GetComplianceInfo Method ✅ COMPLETED
- [x] Updated `GetTenantSurpathServices` to filter by IsEnabled
- [x] Ensured disabled services don't contribute to compliance requirements
- [x] Added IsEnabled check: `.Where(s => s.IsDeleted == false && s.TenantId == _tenantId && s.IsEnabled == true)`

#### TODO 6: Update Compliance-Related Queries
- [ ] Review all methods in SurpathComplianceEvaluator that query TenantSurpathService
- [ ] Add IsEnabled checks where appropriate
- [ ] Ensure consistent behavior across all compliance calculations

### Phase 3: Pricing Calculation Updates

#### TODO 7: Update HierarchicalPricingManager ✅ COMPLETED
- [x] Updated all service queries to filter by IsEnabled
- [x] Added condition: `.Where(s => s.TenantId == tenantId && !s.IsDeleted && s.IsEnabled)`
- [x] Only enabled services are included in pricing calculations
- [x] Inherited pricing respects parent service's enabled status

#### TODO 8: Update GetPricingForPurchaseAsync ✅ COMPLETED
- [x] Filtered out disabled services when calculating purchase pricing
- [x] Added condition to service queries: `.Where(s => s.TenantId == tenantId && !s.IsDeleted && s.IsEnabled)`
- [x] Ensured users only pay for enabled services

#### TODO 9: Update Registration/Renewal Flow
- [ ] Review TenantRegistrationAppService for pricing calculations
- [ ] Ensure only enabled services are included in registration fees
- [ ] Update any purchase-related calculations

### Phase 4: Testing and Validation

#### TODO 10: Unit Tests
- [ ] Write tests for toggle functionality in app service
- [ ] Test compliance calculations with mixed enabled/disabled services
- [ ] Test pricing calculations with hierarchical enabled/disabled services

#### TODO 11: Integration Tests
- [ ] Test full registration flow with disabled services
- [ ] Test compliance status updates when services are toggled
- [ ] Test bulk operations with proper permission checks

#### TODO 12: UI Testing
- [ ] Test toggle functionality in grid view
- [ ] Test bulk enable/disable operations
- [ ] Verify visual indicators work correctly
- [ ] Test filtering by IsEnabled status

### Phase 5: Data Migration and Cleanup

#### TODO 13: Data Audit
- [ ] Query existing TenantSurpathServices to understand current IsEnabled distribution
- [ ] Identify any inconsistencies in the data
- [ ] Create report of services that may need review

#### TODO 14: Migration Script (if needed)
- [ ] Create migration to set appropriate default values if needed
- [ ] Ensure no breaking changes for existing data
- [ ] Document any data changes made

### Phase 6: Documentation and Communication

#### TODO 15: Update User Documentation
- [ ] Document how to enable/disable services at each level
- [ ] Explain inheritance behavior
- [ ] Add screenshots of new UI features

#### TODO 16: System Documentation
- [ ] Update technical documentation with new behavior
- [ ] Document the impact on compliance and pricing
- [ ] Add architecture diagrams if needed

## Implementation Milestones

### Milestone 1: UI Complete (Phase 1)
**Pause Point**: After completing Phase 1, we'll review:
- Toggle functionality works in the grid
- Bulk operations are functional
- Visual indicators are clear
- No regression in existing functionality

### Milestone 2: Compliance Integration (Phase 2)
**Pause Point**: After Phase 2, we'll verify:
- Disabled services are excluded from compliance calculations
- Existing compliance logic still works correctly
- Performance impact is acceptable
- Logging provides good debugging information

### Milestone 3: Pricing Integration (Phase 3)
**Pause Point**: After Phase 3, we'll confirm:
- Pricing calculations respect IsEnabled flag
- Registration/renewal flows work correctly
- No users are charged for disabled services
- Hierarchical pricing inheritance works as expected

### Milestone 4: Testing Complete (Phase 4)
**Pause Point**: After Phase 4, we'll ensure:
- All tests pass
- Edge cases are covered
- Performance is acceptable
- No security vulnerabilities introduced

## Technical Considerations

### Performance
- Queries already include TenantSurpathService joins, so adding IsEnabled check shouldn't significantly impact performance
- May need to add indexes if performance degrades

### Security
- Ensure proper permission checks for enable/disable operations
- Audit trail already exists via FullAuditedEntity

### Backward Compatibility
- Since IsEnabled already exists and defaults to true for new records, changes should be backward compatible
- Existing services will continue to function as enabled unless explicitly disabled

### Multi-Tenancy
- All queries already respect tenant boundaries
- Ensure tenant isolation is maintained in new endpoints

## Risk Mitigation

1. **Data Integrity**: Before making changes, backup the database and test in a staging environment
2. **Gradual Rollout**: Consider feature flag to enable/disable the new behavior
3. **Monitoring**: Add application insights/logging to track the impact of changes
4. **Rollback Plan**: Ensure we can quickly revert changes if issues arise

## Completed Implementation Details

### Phase 1: UI Enhancements (✅ COMPLETED)
1. **Grid Toggle Switch**: Added switch-style checkbox in TenantSurpathServices grid
2. **Bulk Operations**: Added Select All checkbox and bulk enable/disable buttons
3. **Visual Styling**: Disabled services show with gray background and strikethrough
4. **PricingManager Integration**: Added enable/disable toggle in price edit mode

### Phase 2: Compliance Updates (✅ COMPLETED)
1. **SurpathComplianceEvaluator.cs**:
   - Updated `GetTenantSurpathServices` to filter: `&& s.IsEnabled == true`
   - Updated `SetTenantRequirements` join condition: `&& tss.IsEnabled == true`
   - Disabled services no longer contribute to compliance requirements

### Phase 3: Pricing Updates (✅ COMPLETED)
1. **HierarchicalPricingManager.cs**:
   - Updated all service queries to include: `&& s.IsEnabled`
   - Methods updated:
     - `GetHierarchicalPricingAsync`
     - `GetPricingForPurchaseAsync`
     - `GetHierarchicalPricingV2Async`
     - `GetPricingForPurchaseV2Async`
   - Disabled services excluded from all pricing calculations

### Technical Implementation Notes
- Used existing `CreateOrEdit` method for toggle operations
- Maintained backward compatibility (IsEnabled defaults to true)
- No database schema changes required
- Audit trail maintained through FullAuditedEntity

## Next Steps

1. Phase 4: Testing and validation
2. Phase 5: Data migration audit
3. Phase 6: Update user documentation

## Recent Fixes

### Disabled Service Hierarchy Display (2025-07-24)
**Issue**: When a tenant-wide service was disabled, the PricingManager showed "Service not enabled for this tenant" instead of rendering the hierarchy, preventing re-enabling of the service.

**Solution**:
1. Modified PricingManagementV2.js to always render the service hierarchy, even when the tenant service doesn't exist
2. Added `IncludeDisabled` parameter to GetHierarchicalPricingInputV2 DTO
3. Updated HierarchicalPricingManager.GetHierarchicalPricingV2Async to accept includeDisabled parameter
4. Updated TenantSurpathServicesAppService to pass the includeDisabled flag
5. Modified JavaScript to pass includeDisabled: true when loading pricing data for management UI

**Technical Changes**:
- PricingManagementV2.js: Removed conditional rendering that checked for tenantService existence
- HierarchicalPricingNodeDto.cs: Added IncludeDisabled property to input DTO
- HierarchicalPricingManager.cs: Added optional includeDisabled parameter to GetHierarchicalPricingV2Async
- TenantSurpathServicesAppService.cs: Updated to pass includeDisabled parameter to manager

This ensures that disabled services remain visible in the hierarchy and can be re-enabled at any level.

### Hierarchical Compliance Checking (2025-07-24)
**Issue**: Compliance calculations only checked if the directly linked TenantSurpathService was enabled, not considering hierarchical inheritance. For example, if a requirement was linked to a disabled department-level service but the tenant-level service was enabled, the requirement would be incorrectly excluded.

**Solution**:
1. Added `IsServiceEffectivelyEnabled` helper method to check service enablement through the hierarchy
2. Modified `SetTenantRequirements` to load all requirements initially, then post-process for hierarchical enablement
3. Added caching mechanism to avoid performance issues with multiple database queries
4. Enhanced debug logging to track hierarchical filtering

**Technical Implementation**:
- **IsServiceEffectivelyEnabled**: Checks if a SurpathService is enabled at any applicable level based on requirement assignment:
  - Cohort-level requirements: check cohort → department → tenant
  - Department-level requirements: check department → tenant
  - Tenant-level requirements: check tenant only
- **SetTenantRequirements**: 
  - Removed IsEnabled filter from initial LINQ query
  - Added TenantSurpathService property to TenantRequirement class
  - Build cache of all TenantSurpathServices grouped by SurpathServiceId
  - Post-process requirements to check hierarchical enablement
  - Enhanced logging to show filtering statistics

**Files Modified**:
- SurpathComplianceEvaluator.cs: Added hierarchical checking logic and caching
- TenantRequirement.cs: Added TenantSurpathService property for tracking

This ensures compliance requirements properly respect the hierarchical nature of service enablement, matching the behavior of pricing calculations.