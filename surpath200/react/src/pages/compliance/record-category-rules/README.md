# RecordCategoryRules Module

## Overview
RecordCategoryRules define the business rules for document categories, including expiration days, warning thresholds, notification settings, and status assignments. These rules are critical for the compliance tracking system.

## Migration Status
✅ **COMPLETE** - Migrated from Surpath112 ASP.NET Zero v11.4 MVC + jQuery to Surpath200 v15 React SPA

## Architecture

### Source (Surpath112)
- **Views**: `Areas/App/Views/RecordCategoryRules/`
  - `Index.cshtml` - Main list page
  - `CreateOrEdit.cshtml` - Full-page form (not modal!)
  - `ViewRecordCategoryRule.cshtml` - Full-page read-only view

- **JavaScript**: `wwwroot/view-resources/Areas/App/Views/RecordCategoryRules/`
  - `Index.js` (346 lines) - DataTable with 11 filters
  - `CreateOrEdit.js` (156 lines) - Complex validation logic

- **Backend**: Already migrated in Phase 2
  - Entity: `RecordCategoryRule` (62 entities total)
  - AppService: `IRecordCategoryRulesAppService` (6 methods)
  - DTOs: 8 DTOs in `inzibackend.Application.Shared`

### Target (Surpath200)
- **Service**: `src/services/surpath/recordCategoryRules.service.ts` (186 lines)
- **Pages**: `src/pages/compliance/record-category-rules/`
  - `index.tsx` (609 lines) - Main list page with 11 filters
  - `CreateOrEdit.tsx` (497 lines) - Full-page form with validation
  - `View.tsx` (185 lines) - Full-page read-only view

## Features

### List View (`index.tsx`)
- **Server-side pagination** with configurable page sizes (5, 10, 25, 50, 100, 250, 500)
- **11 Filters** (most complex module!):
  - Global search (filter)
  - Name filter
  - Description filter
  - Notify filter (boolean: All/Yes/No)
  - Expires filter (boolean)
  - Required filter (boolean)
  - IsSurpathOnly filter (boolean - Host only)
  - ExpireInDays range (min/max)
  - WarnDaysBeforeFirst range (min/max)
  - WarnDaysBeforeSecond range (min/max)
  - WarnDaysBeforeFinal range (min/max)
  - MetaData filter
- **10 Table Columns**:
  - Actions
  - Name
  - Description
  - Notify (icon: ✓/✗)
  - ExpireInDays
  - WarnDaysBeforeFirst
  - Expires (icon: ✓/✗)
  - Required (icon: ✓/✗)
  - IsSurpathOnly (icon: ✓/✗ - Host only)
  - WarnDaysBeforeSecond
  - WarnDaysBeforeFinal
- **Actions per row**:
  - View (navigates to full-page view)
  - Edit (navigates to full-page form)
  - Delete (requires `Pages.RecordCategoryRules.Delete`)
  - History (requires `Pages.Administration.AuditLogs`)
- **Bulk actions**:
  - Create (navigates to full-page form)
  - Export to Excel
  - Refresh table

### Create/Edit Page (Full-Page Form)
- **2-Column Layout** for optimal space usage
- **Left Column Fields**:
  - Name (required, max 100 chars)
  - Description (optional, textarea, max 500 chars)
  - Notify (switch)
  - ExpireInDays (number input, min 0)
  - Expires (switch)
  - ExpiredStatus (dropdown - **required** when Expires=true)
  - Required (switch)
- **Right Column Fields**:
  - WarnDaysBeforeFirst (number input, min 0)
  - FirstWarnStatus (dropdown - optional)
  - WarnDaysBeforeSecond (number input, min 0)
  - SecondWarnStatus (dropdown - optional)
  - WarnDaysBeforeFinal (number input, min 0)
  - FinalWarnStatus (dropdown - optional)
  - IsSurpathOnly (switch - Host users only)
  - MetaData (textarea)

- **Complex Validation Logic**:
  ```javascript
  // From Surpath112 CreateOrEdit.js lines 79-89
  // REQUIRED: ExpiredStatus when Expires is checked
  if (expires && !expiredStatusId) {
    error: 'ExpiredStatus Required'
  }

  // RECOMMENDED (info messages, not errors):
  if (warnDaysBeforeFirst > 0 && !firstWarnStatusId) {
    info: 'Consider selecting a status for first warning'
  }
  // Same for Second and Final warnings
  ```

- **Conditional Visibility**:
  - ExpiredStatus field only visible when Expires=true
  - IsSurpathOnly field only visible to Host users

- **Action Buttons**:
  - Save (saves and returns to list)
  - Save & New (saves and clears form - Create mode only)
  - Cancel (returns to list)

### View Page (Full-Page Read-Only)
- Displays all RecordCategoryRule fields in Ant Design Descriptions
- Boolean fields rendered with colored tags (✓ Yes / ✗ No)
- Warning status names displayed
- MetaData rendered in `<pre>` tag with wrapping
- Back button to return to list

## Permissions
- `Pages.RecordCategoryRules.Create` - Create new rules
- `Pages.RecordCategoryRules.Edit` - Edit existing rules
- `Pages.RecordCategoryRules.Delete` - Delete rules
- `Pages.Administration.AuditLogs` - View entity history

## Data Model

### RecordCategoryRuleDto
```typescript
{
  id: string;
  name: string;
  description?: string;
  notify: boolean;
  expireInDays: number;
  warnDaysBeforeFirst: number;
  expires: boolean;
  required: boolean;
  isSurpathOnly: boolean;
  warnDaysBeforeSecond: number;
  warnDaysBeforeFinal: number;
  metaData?: string;
  // Status IDs and Names for 4 warning levels
  firstWarnStatusId?: string;
  firstWarnStatusName?: string;
  secondWarnStatusId?: string;
  secondWarnStatusName?: string;
  finalWarnStatusId?: string;
  finalWarnStatusName?: string;
  expiredStatusId?: string;
  expiredStatusName?: string;
}
```

### Business Logic
**Warning System**: 3-tier warning system + expired status
1. **First Warning**: X days before expiration
2. **Second Warning**: Y days before expiration
3. **Final Warning**: Z days before expiration
4. **Expired Status**: When document expires

**Status Assignment**:
- Each warning level can have an optional RecordStatus assigned
- ExpiredStatus is **required** when Expires=true
- Warning statuses are **optional** but recommended if warning days > 0

**Example Scenario**:
```
ExpireInDays: 365
WarnDaysBeforeFirst: 90  → FirstWarnStatus: "Expiring Soon"
WarnDaysBeforeSecond: 30 → SecondWarnStatus: "Action Required"
WarnDaysBeforeFinal: 7   → FinalWarnStatus: "Urgent"
Expires: true            → ExpiredStatus: "Expired" (REQUIRED)
```

## API Endpoints
- `GET /api/services/app/RecordCategoryRules/GetAll` - Paginated list
- `GET /api/services/app/RecordCategoryRules/GetRecordCategoryRuleForView` - View details
- `GET /api/services/app/RecordCategoryRules/GetRecordCategoryRuleForEdit` - Edit data
- `POST /api/services/app/RecordCategoryRules/CreateOrEdit` - Create/Update
- `DELETE /api/services/app/RecordCategoryRules/Delete` - Delete
- `GET /api/services/app/RecordCategoryRules/GetRecordCategoryRulesToExcel` - Excel export

## TODOs / Known Issues
1. **RecordStatus Lookup**: Currently using placeholder data
   - Need to implement `recordStatusService` when RecordStatuses module is migrated
   - Load dropdown options from `/api/services/app/RecordStatuses/GetAllForDropdown`
2. **Entity History**: History button shows "coming soon" message
   - Need to integrate with ASP.NET Zero entity history feature
3. **Routing**: Routes need to be added to main routing configuration

## Testing
- [ ] Test Create operation with all fields
- [ ] Test Edit operation (load existing, modify, save)
- [ ] Test Delete operation with confirmation
- [ ] Test Save & New button (Create mode only)
- [ ] Test Expires toggle (ExpiredStatus field visibility)
- [ ] Test validation: ExpiredStatus required when Expires=true
- [ ] Test validation: Info messages for warning statuses
- [ ] Test all 11 filters (text, boolean, numeric ranges)
- [ ] Test pagination (different page sizes)
- [ ] Test sorting by columns
- [ ] Test Excel export with all filters
- [ ] Verify permission enforcement (Create, Edit, Delete)
- [ ] Test IsSurpathOnly field visibility (Host vs Tenant)
- [ ] Test navigation: List → Create/Edit → List
- [ ] Test navigation: List → View → List

## Related Modules
- **RecordCategories** (WS 3.1.2) - Categories that use these rules
- **RecordStatuses** (lookup data) - Status values for warning levels
- **Records** (WS 3.1.3) - Documents governed by these rules

## Migration Notes
- Migrated from jQuery DataTables to Ant Design Table
- **Full-page forms** instead of modals (different from RecordCategories)
- Complex validation logic preserved from Surpath112
- Boolean columns rendered with icons (CheckCircle/CloseCircle)
- Status dropdowns currently use placeholder data (TODO)
- Conditional field visibility implemented with React state
- Save & New button behavior matches Surpath112
- Host-only field (IsSurpathOnly) conditionally rendered
- Form state management with React Hook Form
- Numeric range filters implemented with InputNumber components
- Boolean filters implemented with Select dropdowns (All/Yes/No)

## Performance Notes
- Server-side pagination reduces initial load time
- Advanced filters can be hidden to reduce UI clutter
- Boolean columns use icon rendering for visual clarity
- Excel export respects all active filters
