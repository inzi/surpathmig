# Localization Conversion Guide: MVC/jQuery → React

## Overview

ASPNetZero uses ABP's localization system. The backend localization sources remain the same, but frontend access patterns differ between MVC/jQuery and React.

---

## MVC/jQuery Localization Pattern (Surpath112)

### Backend Localization Source
```csharp
// In Localization/SurpathLocalizationConfigurer.cs
public static void Configure(ILocalizationConfiguration localizationConfiguration)
{
    localizationConfiguration.Sources.Add(
        new DictionaryBasedLocalizationSource(
            SurpathConsts.LocalizationSourceName,
            new XmlEmbeddedFileLocalizationDictionaryProvider(
                typeof(SurpathLocalizationConfigurer).GetAssembly(),
                "Surpath.Localization.SourceFiles"
            )
        )
    );
}
```

### XML Localization Files
```xml
<!-- Localization/SourceFiles/Surpath.xml -->
<?xml version="1.0" encoding="utf-8" ?>
<localizationDictionary culture="en">
  <texts>
    <text name="SurpathModule" value="Surpath Module" />
    <text name="CreateNew" value="Create New" />
    <text name="EditRecord" value="Edit Record" />
    <text name="DeleteConfirmation" value="Are you sure you want to delete?" />
  </texts>
</localizationDictionary>
```

### MVC Razor View Usage
```html
<!-- In .cshtml files -->
<h2>@L("SurpathModule")</h2>
<button>@L("CreateNew")</button>

<!-- With format parameters -->
<span>@L("WelcomeMessage", Model.UserName)</span>
```

### jQuery JavaScript Usage
```javascript
// In .js files
var title = abp.localization.localize('SurpathModule', 'Surpath');
var createText = app.localize('CreateNew');

// ASPNetZero shorthand
var message = app.localize('DeleteConfirmation');

// With parameters
var welcome = abp.utils.formatString(
    app.localize('WelcomeMessage'),
    userName
);
```

---

## React Localization Pattern (Surpath200)

### Backend (Remains Same)
The backend localization setup is identical. Ensure localization XML/JSON files are migrated.

### React Localization Hook
```tsx
// Using ASPNetZero's L function
import { L } from '@lib/abpUtility';

const SurpathModule: React.FC = () => {
    return (
        <div>
            <h2>{L('SurpathModule')}</h2>
            <Button>{L('CreateNew')}</Button>
        </div>
    );
};
```

### Alternative: useLocalization Hook
```tsx
// If using a custom hook pattern
import { useLocalization } from '@hooks/useLocalization';

const SurpathModule: React.FC = () => {
    const { L } = useLocalization();
    
    return (
        <div>
            <h2>{L('SurpathModule')}</h2>
        </div>
    );
};
```

### With Parameters
```tsx
// MVC: @L("WelcomeMessage", userName)
// React:
import { L } from '@lib/abpUtility';

const Welcome: React.FC<{ userName: string }> = ({ userName }) => {
    return <span>{L('WelcomeMessage', userName)}</span>;
};

// Or using template literals with the localized base
const message = L('WelcomeMessage').replace('{0}', userName);
```

---

## Conversion Patterns

### Basic Text
```javascript
// jQuery
var text = app.localize('MyKey');
$('#element').text(text);

// React
<span>{L('MyKey')}</span>
```

### Button Labels
```javascript
// jQuery
<button>@L("Save")</button>
// or
$('#btn').text(app.localize('Save'));

// React
<Button>{L('Save')}</Button>
```

### Confirmation Dialogs
```javascript
// jQuery
abp.message.confirm(
    app.localize('DeleteConfirmation'),
    app.localize('AreYouSure'),
    function(confirmed) { ... }
);

// React (with Ant Design or similar)
Modal.confirm({
    title: L('AreYouSure'),
    content: L('DeleteConfirmation'),
    onOk: () => { ... }
});
```

### Form Validation Messages
```javascript
// jQuery (often in validation rules)
rules: {
    name: {
        required: true,
        messages: {
            required: app.localize('NameRequired')
        }
    }
}

// React (with form library like Formik or Ant Design Form)
<Form.Item
    name="name"
    rules={[{ required: true, message: L('NameRequired') }]}
>
    <Input />
</Form.Item>
```

### Notifications
```javascript
// jQuery
abp.notify.success(app.localize('SavedSuccessfully'));

// React
import { message } from 'antd';
message.success(L('SavedSuccessfully'));

// Or using notification service
notification.success({
    message: L('Success'),
    description: L('SavedSuccessfully')
});
```

---

## Localization Key Inventory

Find all localization keys in Surpath112:

```bash
# Find all L() calls in cshtml
grep -r "L(\"" ../surpath150 --include="*.cshtml" | grep -oP 'L\("\K[^"]+' | sort -u

# Find all app.localize calls in JS
grep -r "app.localize\|abp.localization.localize" ../surpath150 --include="*.js" | grep -oP "localize\('\K[^']+" | sort -u
```

### Localization Key Mapping Table

| Key | English Value | Used In |
|-----|---------------|---------|
| `SurpathModule` | Surpath Module | Header, Menu |
| `CreateNew` | Create New | Buttons |
| `Edit` | Edit | Buttons |
| `Delete` | Delete | Buttons |
| `Save` | Save | Forms |
| `Cancel` | Cancel | Forms |
| `DeleteConfirmation` | Are you sure? | Dialogs |
| ... | ... | ... |

---

## Migration Steps

### Step 1: Inventory Localization Keys
1. Extract all keys from XML files
2. Find all L() and app.localize() usage
3. Create mapping table

### Step 2: Migrate Localization Files
```bash
# Copy localization XML files to React project
cp -r ../surpath150/src/Surpath.Core/Localization ./src/Surpath.Core/Localization
```

### Step 3: Verify L() Function in React
```tsx
// Ensure this utility exists in React project
// src/lib/abpUtility.ts
export function L(key: string, ...args: any[]): string {
    return abp.localization.localize(key, 'Surpath', ...args);
}
```

### Step 4: Convert Each Component
For each component:
1. Find all hardcoded strings that should be localized
2. Replace with L('Key') calls
3. Verify key exists in localization files

---

## Common Issues

### Missing Keys
```tsx
// If key doesn't exist, ABP returns the key name
// Check console for warnings about missing localization keys
```

### Parameter Formatting
```tsx
// MVC: L("Hello {0}", name) works directly
// React: May need different approach
const hello = L('Hello {0}').replace('{0}', name);
// Or ensure L() function handles parameters
```

### Dynamic Keys
```javascript
// jQuery
var key = 'Status_' + status;
var text = app.localize(key);

// React - same pattern works
const text = L(`Status_${status}`);
```

---

## Checklist

- [ ] Export all localization keys from Surpath112
- [ ] Migrate XML localization files
- [ ] Verify L() utility function in React project
- [ ] Convert all @L() razor calls
- [ ] Convert all app.localize() JS calls
- [ ] Handle parameterized strings
- [ ] Test each language/culture (if multi-language)