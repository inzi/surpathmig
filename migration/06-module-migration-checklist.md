# Module Migration Checklist Template

Use this template for each of the 8-10 modules being migrated from Surpath112 to Surpath200.

---

## Module: [MODULE NAME]

### Module Overview

| Attribute | Value |
|-----------|-------|
| **Module Name** | [Name] |
| **Priority** | [1-10] |
| **Source Path** | `../surpath150/src/[path]` |
| **Target Path** | `./src/[path]` |
| **Dependencies** | [List other modules this depends on] |
| **Estimated Effort** | [Hours/Days] |
| **Status** | ⬜ Not Started / 🟡 In Progress / ✅ Complete |

---

### Phase 1: Analysis

- [ ] **Inventory Source Files**
  - [ ] List all .cshtml view files
  - [ ] List all .js files
  - [ ] List all .cs backend files
  - [ ] List all DTOs
  
- [ ] **Document Permissions**
  | Permission Name | Purpose |
  |-----------------|---------|
  | Pages.[Module] | Main access |
  | Pages.[Module].Create | Create |
  | Pages.[Module].Edit | Edit |
  | Pages.[Module].Delete | Delete |

- [ ] **Document Localization Keys**
  | Key | English Text | Used In |
  |-----|--------------|---------|
  | | | |

- [ ] **Document API Endpoints**
  | Endpoint | Method | Purpose |
  |----------|--------|---------|
  | /api/services/app/[Module]/GetAll | GET | List |
  | /api/services/app/[Module]/Get | GET | Single |
  | /api/services/app/[Module]/Create | POST | Create |
  | /api/services/app/[Module]/Update | PUT | Update |
  | /api/services/app/[Module]/Delete | DELETE | Delete |

---

### Phase 2: Backend Migration

- [ ] **Application Services**
  - [ ] Copy [Module]AppService.cs
  - [ ] Verify async/await patterns
  - [ ] Update any deprecated ABP methods
  - [ ] Test all endpoints

- [ ] **DTOs**
  - [ ] Copy/update input DTOs
  - [ ] Copy/update output DTOs
  - [ ] Verify AutoMapper configurations

- [ ] **Entities** (if any changes needed)
  - [ ] Review entity definitions
  - [ ] Update if needed
  - [ ] Create migrations if schema changed

- [ ] **Permissions**
  - [ ] Add to PermissionNames.cs
  - [ ] Register in AuthorizationProvider.cs
  - [ ] Test permission enforcement

---

### Phase 3: Frontend Migration

#### 3.1 List/Index Page

- [ ] **Create React Component**
  - [ ] File: `src/scenes/[Module]/index.tsx`
  - [ ] Import required dependencies
  - [ ] Set up component structure

- [ ] **DataTable Conversion**
  - [ ] Define columns
  - [ ] Implement server-side pagination
  - [ ] Implement sorting
  - [ ] Implement filtering
  - [ ] Add action buttons (edit, delete)

- [ ] **Permissions Integration**
  - [ ] usePermission for create button
  - [ ] usePermission for edit action
  - [ ] usePermission for delete action

- [ ] **Custom Business Logic**
  Document and implement any custom logic from jQuery:
  | Logic Description | jQuery Location | React Implementation |
  |-------------------|-----------------|----------------------|
  | | | |

#### 3.2 Create/Edit Modal

- [ ] **Create Modal Component**
  - [ ] File: `src/scenes/[Module]/components/CreateOrEditModal.tsx`
  - [ ] Form structure
  - [ ] Load data for edit mode

- [ ] **Form Fields**
  | Field | Type | Validation | Notes |
  |-------|------|------------|-------|
  | | | | |

- [ ] **Conditional Logic**
  Document field dependencies and conditional display:
  | Trigger | Action |
  |---------|--------|
  | | |

- [ ] **Save Handler**
  - [ ] Form validation
  - [ ] Create vs Update logic
  - [ ] Success notification
  - [ ] Error handling
  - [ ] Modal close and table refresh

#### 3.3 Other Modals/Components

List any additional modals or components:

| Component | Purpose | Status |
|-----------|---------|--------|
| | | |

#### 3.4 Localization

- [ ] All L() calls in place
- [ ] Localization keys exist in XML
- [ ] Test different languages (if applicable)

#### 3.5 Styling

- [ ] Using default theme components
- [ ] No custom styles needed (or documented)
- [ ] Responsive layout verified

---

### Phase 4: Testing

- [ ] **Unit Tests**
  - [ ] Backend service tests
  - [ ] Frontend component tests (if applicable)

- [ ] **Integration Tests**
  - [ ] API endpoint tests
  - [ ] End-to-end tests

- [ ] **A/B Testing with Production DB Copy**
  - [ ] Create operation matches Surpath112
  - [ ] Edit operation matches Surpath112
  - [ ] Delete operation matches Surpath112
  - [ ] List/filter operations match Surpath112
  - [ ] All business logic preserved

- [ ] **Permission Testing**
  - [ ] Test with admin user (all permissions)
  - [ ] Test with limited user (partial permissions)
  - [ ] Test with no permissions

---

### Phase 5: Documentation

- [ ] Code comments for complex logic
- [ ] Update API documentation (if needed)
- [ ] Update user documentation (if needed)

---

### Issues & Notes

| Issue | Resolution | Status |
|-------|------------|--------|
| | | |

---

### Sign-off

| Role | Name | Date | Status |
|------|------|------|--------|
| Developer | | | ⬜ |
| Reviewer | | | ⬜ |
| QA | | | ⬜ |

---

## Quick Commands

```bash
# Find all files for this module in Surpath112
find ../surpath150 -name "*[ModuleName]*" -type f

# Find permission references
grep -r "Pages\.[ModuleName]" ../surpath150

# Find localization keys
grep -r "L(\".*[ModuleName]" ../surpath150 --include="*.cshtml" --include="*.js"

# Find API calls
grep -r "_[moduleName]Service\|[ModuleName]Service" ../surpath150 --include="*.js"
```