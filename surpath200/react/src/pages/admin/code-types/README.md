# CodeTypes Module - React Frontend

## Overview
Simple CRUD module for managing code type categories (Lab, Quest, Clear Star, FormFox, etc.).

## Status
✅ **Phase 1 (Backend):** Complete - Commit 67f4da31
✅ **Phase 2 (Frontend):** Complete - Commit 06a198f5
⏸️ **Phase 3 (Testing):** Blocked by .NET SDK version mismatch

## Files

### Components
- `index.tsx` - Main list page with Ant Design Table
- `components/CreateOrEditCodeTypeModal.tsx` - Create/Edit modal with form validation
- `codeTypes.service.ts` - **TEMPORARY** service proxy (replace with auto-generated once .NET SDK 10.0 available)

### Features Implemented
- ✅ Server-side pagination
- ✅ Search filter + Advanced filters (name filter)
- ✅ Create/Edit functionality
- ✅ Delete with confirmation
- ✅ Excel export
- ✅ Permission-based UI (Pages.Administration.CodeTypes.*)
- ✅ Localization support
- ✅ Loading states and error handling

## Route
`/app/admin/code-types`

## Navigation
Administration > CodeTypes

## Permissions
- `Pages.Administration.CodeTypes` - View page
- `Pages.Administration.CodeTypes.Create` - Create button
- `Pages.Administration.CodeTypes.Edit` - Edit action
- `Pages.Administration.CodeTypes.Delete` - Delete action

## Known Issues
1. **Cannot test end-to-end** - Backend requires .NET SDK 10.0, system has 9.0
2. **Manual service proxy** - `codeTypes.service.ts` must be replaced with auto-generated version once backend can run

## TODO
- [ ] Resolve .NET SDK issue
- [ ] Run `npm run nswag` to generate proper service proxies
- [ ] Replace `codeTypes.service.ts` with auto-generated proxies
- [ ] Delete temporary service file
- [ ] Update imports in `index.tsx` and `CreateOrEditCodeTypeModal.tsx`
- [ ] Perform E2E testing
- [ ] Hand off to QA Engineer for validation

## Testing Checklist (When Unblocked)
- [ ] List page displays code types
- [ ] Search filter works
- [ ] Advanced name filter works
- [ ] Pagination works
- [ ] Create new code type
- [ ] Edit existing code type
- [ ] Delete code type
- [ ] Excel export generates file
- [ ] Permissions properly gate UI elements
- [ ] Validation: Name required, 1-128 chars
- [ ] Multi-tenancy: Tenant sees only their code types
- [ ] Host sees all code types

## Migration Notes
Migrated from:
- `surpath112/src/inzibackend.Web.Mvc/Areas/App/Views/CodeTypes/`
- jQuery DataTables → Ant Design Table
- MVC forms → React Hook Form
- Server-side rendering → SPA with API calls

Matches surpath112 functionality 1:1.
