# Permission Mapping Guide: MVC/jQuery → React

## Overview

ASPNetZero's permission system is conceptually similar between MVC and React versions, but the implementation differs significantly on the frontend.

---

## MVC/jQuery Permission Pattern (Surpath112)

### Backend Permission Definition
```csharp
// In Authorization/PermissionNames.cs
public static class PermissionNames
{
    public const string Pages_Tenants = "Pages.Tenants";
    public const string Pages_Users = "Pages.Users";
    public const string Pages_Roles = "Pages.Roles";
    // Surpath-specific permissions
    public const string Pages_SurpathModule = "Pages.SurpathModule";
    public const string Pages_SurpathModule_Create = "Pages.SurpathModule.Create";
    public const string Pages_SurpathModule_Edit = "Pages.SurpathModule.Edit";
    public const string Pages_SurpathModule_Delete = "Pages.SurpathModule.Delete";
}
```

### Backend Authorization Provider
```csharp
// In Authorization/SurpathAuthorizationProvider.cs
public override void SetPermissions(IPermissionDefinitionContext context)
{
    var pages = context.GetPermissionOrNull(PermissionNames.Pages) ??
                context.CreatePermission(PermissionNames.Pages, L("Pages"));

    var surpathModule = pages.CreateChildPermission(
        PermissionNames.Pages_SurpathModule,
        L("SurpathModule"));
    
    surpathModule.CreateChildPermission(
        PermissionNames.Pages_SurpathModule_Create,
        L("CreateSurpathModule"));
}
```

### jQuery Frontend Check
```javascript
// In Views/SurpathModule/Index.cshtml or .js files
@if (IsGranted(PermissionNames.Pages_SurpathModule_Create))
{
    <button id="CreateNewButton">Create</button>
}

// Or in JavaScript
if (abp.auth.isGranted('Pages.SurpathModule.Create')) {
    $('#CreateNewButton').show();
}
```

---

## React Permission Pattern (Surpath200)

### Backend (Remains Similar)
The backend permission definitions remain largely the same. Ensure `PermissionNames.cs` and `AuthorizationProvider.cs` are migrated.

### React Frontend Check

#### Using Permission Hook
```tsx
// In React component
import { usePermission } from '@lib/hooks/usePermission';

const SurpathModuleList: React.FC = () => {
    const canCreate = usePermission('Pages.SurpathModule.Create');
    const canEdit = usePermission('Pages.SurpathModule.Edit');
    const canDelete = usePermission('Pages.SurpathModule.Delete');

    return (
        <div>
            {canCreate && (
                <Button onClick={handleCreate}>Create</Button>
            )}
            {/* ... */}
        </div>
    );
};
```

#### Using Permission Component
```tsx
// Alternative: Permission wrapper component
import { PermissionChecker } from '@components/PermissionChecker';

const SurpathModuleList: React.FC = () => {
    return (
        <div>
            <PermissionChecker permission="Pages.SurpathModule.Create">
                <Button onClick={handleCreate}>Create</Button>
            </PermissionChecker>
        </div>
    );
};
```

#### Direct Check via ABP Object
```tsx
// If abp object is available globally
if (abp.auth.isGranted('Pages.SurpathModule.Create')) {
    // Show UI element
}
```

---

## Permission Mapping Table

Complete this table by examining `../surpath150` permission files:

| Surpath112 Permission | React Hook Usage | Component Location |
|-----------------------|------------------|-------------------|
| `Pages.Tenants` | `usePermission('Pages.Tenants')` | Admin/Tenants |
| `Pages.Users` | `usePermission('Pages.Users')` | Admin/Users |
| `Pages.Roles` | `usePermission('Pages.Roles')` | Admin/Roles |
| `Pages.[Module1]` | `usePermission('Pages.[Module1]')` | TBD |
| `Pages.[Module1].Create` | `usePermission('Pages.[Module1].Create')` | TBD |
| `Pages.[Module1].Edit` | `usePermission('Pages.[Module1].Edit')` | TBD |
| `Pages.[Module1].Delete` | `usePermission('Pages.[Module1].Delete')` | TBD |

---

## Migration Steps

### Step 1: Inventory All Permissions
```bash
# Find all permission definitions in Surpath112
grep -r "PermissionNames" ../surpath150 --include="*.cs"
grep -r "IsGranted\|isGranted" ../surpath150 --include="*.cshtml" --include="*.js"
```

### Step 2: Verify Backend Permissions
- Copy `PermissionNames.cs` to Surpath200
- Copy/update `AuthorizationProvider.cs`
- Ensure all permissions are registered

### Step 3: Create Permission Hook (if not exists)
```tsx
// src/lib/hooks/usePermission.ts
import { useAppSession } from './useAppSession';

export const usePermission = (permissionName: string): boolean => {
    const session = useAppSession();
    return session?.auth?.grantedPermissions?.[permissionName] === true;
};

// Or if using abp global object
export const usePermission = (permissionName: string): boolean => {
    return abp.auth.isGranted(permissionName);
};
```

### Step 4: Convert Each View
For each view/component:
1. Find all `IsGranted()` or `isGranted()` calls
2. Replace with `usePermission()` hook at component level
3. Use boolean result for conditional rendering

---

## Common Patterns

### Menu/Navigation Permissions
```tsx
// MVC: In _Layout.cshtml or menu builder
@if (IsGranted("Pages.SurpathModule"))
{
    <li><a href="/SurpathModule">Module</a></li>
}

// React: In navigation config or component
const menuItems = [
    {
        name: 'SurpathModule',
        permission: 'Pages.SurpathModule',
        path: '/app/surpath-module',
        icon: <ModuleIcon />
    }
];

// In menu component
{menuItems.filter(item =>
    !item.permission || usePermission(item.permission)
).map(item => (
    <MenuItem key={item.name} {...item} />
))}
```

### Action Button Permissions
```tsx
// MVC jQuery
<button class="btn-edit" data-permission="Pages.SurpathModule.Edit">Edit</button>

// React
const ActionButtons: React.FC<{ record: SurpathItem }> = ({ record }) => {
    const canEdit = usePermission('Pages.SurpathModule.Edit');
    const canDelete = usePermission('Pages.SurpathModule.Delete');

    return (
        <Space>
            {canEdit && <Button onClick={() => handleEdit(record)}>Edit</Button>}
            {canDelete && <Button danger onClick={() => handleDelete(record)}>Delete</Button>}
        </Space>
    );
};
```

---

## Checklist

- [ ] Inventory all permissions in Surpath112
- [ ] Migrate PermissionNames.cs to Surpath200
- [ ] Migrate AuthorizationProvider.cs to Surpath200
- [ ] Create/verify usePermission hook
- [ ] Convert all view permission checks
- [ ] Convert all JavaScript permission checks
- [ ] Test each permission in isolation
- [ ] Test permission combinations (create + edit, etc.)