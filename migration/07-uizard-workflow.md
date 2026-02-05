# Uizard → React Style Integration Workflow

## Overview

Since Uizard doesn't export directly to React, this guide provides a workflow for extracting styles element-by-element and integrating them into your React components.

---

## Workflow Overview

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   Uizard    │────▶│   Extract   │────▶│   Create    │────▶│  Integrate  │
│   Design    │     │   Styles    │     │   CSS/SCSS  │     │   React     │
└─────────────┘     └─────────────┘     └─────────────┘     └─────────────┘
```

---

## Phase 1: Design Analysis in Uizard

### 1.1 Identify Component Types
Categorize your Uizard designs into component types:

| Category | Examples | Priority |
|----------|----------|----------|
| Layout | Page containers, sidebars, headers | High |
| Navigation | Menus, breadcrumbs, tabs | High |
| Forms | Inputs, selects, buttons, labels | High |
| Data Display | Tables, cards, lists | High |
| Feedback | Alerts, modals, notifications | Medium |
| Typography | Headings, paragraphs, labels | Medium |

### 1.2 Document Design Tokens
Extract these from your Uizard design:

**Colors**
```
Primary: #[hex]
Secondary: #[hex]
Success: #[hex]
Warning: #[hex]
Error: #[hex]
Background: #[hex]
Text Primary: #[hex]
Text Secondary: #[hex]
Border: #[hex]
```

**Typography**
```
Font Family: [name]
H1: [size]px / [weight] / [line-height]
H2: [size]px / [weight] / [line-height]
Body: [size]px / [weight] / [line-height]
Small: [size]px / [weight] / [line-height]
```

**Spacing**
```
XS: [value]px
SM: [value]px
MD: [value]px
LG: [value]px
XL: [value]px
```

**Border Radius**
```
Small: [value]px
Medium: [value]px
Large: [value]px
```

---

## Phase 2: Element-by-Element Extraction

### 2.1 For Each UI Element in Uizard:

1. **Select the element** in Uizard
2. **Copy style properties** one by one:
   - Background color
   - Border properties
   - Padding/Margin
   - Typography (font, size, weight, color)
   - Shadow
   - Border radius

3. **Document in a consistent format**:
```css
/* [Element Name] */
.element-class {
    /* From Uizard */
    background-color: #value;
    border: 1px solid #value;
    border-radius: 4px;
    padding: 12px 16px;
    font-size: 14px;
    font-weight: 500;
    color: #value;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}
```

### 2.2 Extraction Template

Use this template for each element:

```markdown
## Element: [Name]
**Uizard Location**: [Page/Screen name]
**React Component**: [Component name]

### Extracted Styles
- Background:
- Border:
- Border Radius:
- Padding:
- Margin:
- Font Size:
- Font Weight:
- Font Color:
- Shadow:

### CSS
\`\`\`css
.element-name {
    /* styles */
}
\`\`\`

### Notes
[Any special considerations]
```

---

## Phase 3: Create CSS/SCSS Files

### 3.1 Project Structure

```
src/
├── styles/
│   ├── variables.scss       # Design tokens
│   ├── mixins.scss          # Reusable style patterns
│   ├── global.scss          # Global styles
│   └── components/
│       ├── buttons.scss
│       ├── forms.scss
│       ├── tables.scss
│       ├── modals.scss
│       └── cards.scss
└── scenes/
    └── [Module]/
        └── styles.scss       # Module-specific styles
```

### 3.2 Variables File (Design Tokens)

```scss
// styles/variables.scss

// Colors (from Uizard)
$color-primary: #[from-uizard];
$color-secondary: #[from-uizard];
$color-success: #[from-uizard];
$color-warning: #[from-uizard];
$color-error: #[from-uizard];
$color-background: #[from-uizard];
$color-text-primary: #[from-uizard];
$color-text-secondary: #[from-uizard];
$color-border: #[from-uizard];

// Typography
$font-family: '[from-uizard]', sans-serif;
$font-size-xs: 12px;
$font-size-sm: 14px;
$font-size-md: 16px;
$font-size-lg: 18px;
$font-size-xl: 24px;
$font-size-xxl: 32px;

// Spacing
$spacing-xs: 4px;
$spacing-sm: 8px;
$spacing-md: 16px;
$spacing-lg: 24px;
$spacing-xl: 32px;

// Border Radius
$border-radius-sm: 4px;
$border-radius-md: 8px;
$border-radius-lg: 12px;

// Shadows
$shadow-sm: 0 1px 2px rgba(0, 0, 0, 0.05);
$shadow-md: 0 4px 6px rgba(0, 0, 0, 0.1);
$shadow-lg: 0 10px 15px rgba(0, 0, 0, 0.1);
```

### 3.3 Component-Specific Styles

```scss
// styles/components/buttons.scss
@import '../variables';

.btn-custom {
    padding: $spacing-sm $spacing-md;
    border-radius: $border-radius-sm;
    font-size: $font-size-sm;
    font-weight: 500;
    transition: all 0.2s ease;

    &-primary {
        background-color: $color-primary;
        color: white;
        border: none;

        &:hover {
            background-color: darken($color-primary, 10%);
        }
    }

    &-secondary {
        background-color: transparent;
        color: $color-primary;
        border: 1px solid $color-primary;

        &:hover {
            background-color: rgba($color-primary, 0.1);
        }
    }
}
```

---

## Phase 4: Integration with React Components

### 4.1 Option A: CSS Modules (Recommended)

```tsx
// components/CustomButton/CustomButton.tsx
import React from 'react';
import styles from './CustomButton.module.scss';

interface CustomButtonProps {
    variant?: 'primary' | 'secondary';
    children: React.ReactNode;
    onClick?: () => void;
}

const CustomButton: React.FC<CustomButtonProps> = ({
    variant = 'primary',
    children,
    onClick
}) => {
    return (
        <button
            className={`${styles.btn} ${styles[`btn-${variant}`]}`}
            onClick={onClick}
        >
            {children}
        </button>
    );
};

export default CustomButton;
```

```scss
// components/CustomButton/CustomButton.module.scss
@import '../../styles/variables';

.btn {
    padding: $spacing-sm $spacing-md;
    border-radius: $border-radius-sm;
    font-size: $font-size-sm;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.2s ease;
}

.btn-primary {
    background-color: $color-primary;
    color: white;
    border: none;

    &:hover {
        background-color: darken($color-primary, 10%);
    }
}

.btn-secondary {
    background-color: transparent;
    color: $color-primary;
    border: 1px solid $color-primary;

    &:hover {
        background-color: rgba($color-primary, 0.1);
    }
}
```

### 4.2 Option B: Ant Design Theme Customization

Since ASPNetZero React often uses Ant Design, customize the theme:

```tsx
// src/theme/themeConfig.ts
import { ThemeConfig } from 'antd';

const themeConfig: ThemeConfig = {
    token: {
        // Colors from Uizard
        colorPrimary: '#[from-uizard]',
        colorSuccess: '#[from-uizard]',
        colorWarning: '#[from-uizard]',
        colorError: '#[from-uizard]',
        
        // Typography
        fontFamily: '[from-uizard], sans-serif',
        fontSize: 14,
        
        // Border Radius
        borderRadius: 4,
        
        // Spacing
        padding: 16,
        margin: 16,
    },
    components: {
        Button: {
            borderRadius: 4,
            paddingInline: 16,
        },
        Input: {
            borderRadius: 4,
        },
        Table: {
            borderRadius: 8,
        },
        Modal: {
            borderRadius: 8,
        },
    },
};

export default themeConfig;
```

```tsx
// App.tsx or index.tsx
import { ConfigProvider } from 'antd';
import themeConfig from './theme/themeConfig';

const App: React.FC = () => {
    return (
        <ConfigProvider theme={themeConfig}>
            {/* Your app */}
        </ConfigProvider>
    );
};
```

---

## Phase 5: Quality Checklist

### Per-Element Checklist

- [ ] Style extracted from Uizard
- [ ] CSS/SCSS created
- [ ] React component integrated
- [ ] Responsive behavior verified
- [ ] Hover/active states implemented
- [ ] Consistent with design tokens

### Per-Page/Module Checklist

- [ ] All elements styled
- [ ] Consistent spacing
- [ ] Typography hierarchy correct
- [ ] Color scheme consistent
- [ ] Interactive states working
- [ ] Mobile responsive
- [ ] Cross-browser tested

---

## Tips for Efficient Extraction

### 1. Batch Similar Elements
Extract all buttons at once, then all inputs, etc.

### 2. Use Browser DevTools
If Uizard has a preview mode, inspect elements to get computed styles.

### 3. Create a Style Guide
Document extracted styles in a living style guide for reference.

### 4. Start with Default Theme
As mentioned in the migration plan, stick with ASPNetZero's default theme first. Add Uizard customizations after core functionality is complete.

### 5. Prioritize Functional Components
Focus on components used across multiple modules first.

---

## Recommended Migration Order

1. **Design Tokens** (Colors, Typography, Spacing)
2. **Form Elements** (Inputs, Selects, Buttons)
3. **Data Tables** (Most modules use these)
4. **Modals** (Create/Edit patterns)
5. **Cards/Panels** (Dashboard elements)
6. **Navigation** (Menus, breadcrumbs)
7. **Page Layouts** (Containers, grids)
8. **Specialized Components** (Module-specific)