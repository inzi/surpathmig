# RecordCategories Module

## Overview
RecordCategories define document categories that can be associated with RecordRequirements. Each category can have rules (RecordCategoryRules) that govern document expiration, warnings, and requirements.

## Migration Status
✅ **COMPLETE** - Migrated from Surpath112 ASP.NET Zero v11.4 MVC + jQuery to Surpath200 v15 React SPA

## Architecture

### Source (Surpath112)
- **Views**: `Areas/App/Views/RecordCategories/`
  - `Index.cshtml` - Main list page
  - `_CreateOrEditModal.cshtml` - CRUD modal
  - `_ViewRecordCategoryModal.cshtml` - Read-only view modal
  - `_RecordCategoryRecordRequirementLookupTableModal.cshtml` - Lookup modal

- **JavaScript**: `wwwroot/view-resources/Areas/App/Views/RecordCategories/`
  - `Index.js` (274 lines) - DataTable, filters, actions
  - `_CreateOrEditModal.js` (116 lines) - Modal form logic

- **Backend**: Already migrated in Phase 2
  - Entity: `RecordCategory` (62 entities total)
  - AppService: `IRecordCategoriesAppService` (9 methods)
  - DTOs: 9 DTOs in `inzibackend.Application.Shared`

### Target (Surpath200)
- **Service**: `src/services/surpath/recordCategories.service.ts` (171 lines)
- **Pages**: `src/pages/compliance/record-categories/`
  - `index.tsx` (467 lines) - Main list page with filters
  - `components/CreateOrEditRecordCategoryModal.tsx` (227 lines) - CRUD modal
  - `components/ViewRecordCategoryModal.tsx` (76 lines) - Read-only view

## Features

### List View (`index.tsx`)
- **Server-side pagination** with configurable page sizes (5, 10, 25, 50, 100, 250, 500, 5000)
- **4 Filters**:
  - Global search (filter)
  - Name filter
  - Instructions filter
  - RecordRequirement filter
  - RecordCategoryRule filter
- **Actions per row**:
  - View (all users)
  - Edit (requires `Pages.RecordCategories.Edit`)
  - Delete (requires `Pages.RecordCategories.Delete`)
  - History (requires `Pages.Administration.AuditLogs`)
- **Bulk actions**:
  - Export to Excel (all filtered records)
  - Refresh table
- **Advanced filter toggle** (show/hide)

### Create/Edit Modal
- **Fields**:
  - Name (required, max 255 chars)
  - Instructions (optional, textarea, max 2000 chars)
  - RecordRequirement (lookup - TODO: implement lookup modal)
  - RecordCategoryRule (lookup - TODO: implement lookup modal)
- **Validation**:
  - Name required
  - Max length validation
- **React Hook Form** for form state management
- **Ant Design Modal** for UI

### View Modal
- Read-only display of all RecordCategory fields
- Shows related entity names (RecordRequirement, RecordCategoryRule)
- IsSurpathService flag display

## Permissions
- `Pages.RecordCategories.Create` - Create new records
- `Pages.RecordCategories.Edit` - Edit existing records
- `Pages.RecordCategories.Delete` - Delete records
- `Pages.Administration.AuditLogs` - View entity history

## Data Model

### RecordCategoryDto
```typescript
{
  id: string;
  name: string;
  instructions?: string;
  recordRequirementId?: string;
  recordCategoryRuleId?: string;
  isSurpathService: boolean;
  tenantId?: number;
  recordCategoryRule?: RecordCategoryRuleDto;
}
```

### Relationships
- **RecordRequirement** (Many-to-One): Each RecordCategory belongs to one RecordRequirement
- **RecordCategoryRule** (Many-to-One): Each RecordCategory can have one RecordCategoryRule

## API Endpoints
- `GET /api/services/app/RecordCategories/GetAll` - Paginated list
- `GET /api/services/app/RecordCategories/GetRecordCategoryForView` - View details
- `GET /api/services/app/RecordCategories/GetRecordCategoryForEdit` - Edit data
- `POST /api/services/app/RecordCategories/CreateOrEdit` - Create/Update
- `DELETE /api/services/app/RecordCategories/Delete` - Delete
- `GET /api/services/app/RecordCategories/GetRecordCategoriesToExcel` - Excel export
- `GET /api/services/app/RecordCategories/GetAllRecordRequirementForLookupTable` - Lookup
- `GET /api/services/app/RecordCategories/GetAllRecordCategoryRuleForTableDropdown` - Dropdown
- `GET /api/services/app/RecordCategories/GetRecordCategoryDto` - Custom endpoint

## TODOs / Known Issues
1. **Lookup Modals**: RecordRequirement and RecordCategoryRule lookup modals show placeholder messages
   - Need to implement proper lookup modal components when RecordRequirements module is complete
2. **Entity History**: History button shows "coming soon" message
   - Need to integrate with ASP.NET Zero entity history feature

## Testing
- [ ] Test Create operation with all fields
- [ ] Test Edit operation (load existing, modify, save)
- [ ] Test Delete operation with confirmation
- [ ] Test all filters (global, name, instructions, related entities)
- [ ] Test pagination (different page sizes)
- [ ] Test sorting by columns
- [ ] Test Excel export with filters
- [ ] Verify permission enforcement (Create, Edit, Delete)
- [ ] Test with tenant users vs host users

## Related Modules
- **RecordRequirements** (WS 3.1.1) - Parent requirement structure
- **RecordCategoryRules** (WS 3.1.2) - Rules governing categories
- **Records** (WS 3.1.3) - Actual documents uploaded to categories
- **RecordStates** (WS 3.1.4) - Approval workflow for records

## Migration Notes
- Migrated from jQuery DataTables to Ant Design Table
- Replaced jQuery AJAX with axios-based http service
- Converted jQuery modal pattern to React Modal components
- Form validation migrated from jQuery Validate to React Hook Form
- Permission checks migrated from `abp.auth.hasPermission` to Redux store selectors
- Localization migrated from `app.localize()` to `L()` utility
