# Testing & Validation Guide: A/B Testing with Production Database Copies

## Overview

This guide covers the testing strategy for validating Surpath200 against Surpath112 using local copies of production databases.

---

## A/B Testing Environment Setup

### Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    Testing Environment                          │
├─────────────────────────────────────┬───────────────────────────┤
│         Surpath112 (Control)        │   Surpath200 (Test)       │
│  ../surpath150 running locally      │   Local React app         │
│                                     │                           │
│  ┌───────────────┐                  │  ┌───────────────┐        │
│  │   MVC/jQuery  │                  │  │     React     │        │
│  │   Frontend    │                  │  │    Frontend   │        │
│  └───────┬───────┘                  │  └───────┬───────┘        │
│          │                          │          │                │
│  ┌───────▼───────┐                  │  ┌───────▼───────┐        │
│  │   Backend     │                  │  │    Backend    │        │
│  │   (MVC)       │                  │  │   (Core)      │        │
│  └───────┬───────┘                  │  └───────┬───────┘        │
│          │                          │          │                │
└──────────┼──────────────────────────┴──────────┼────────────────┘
           │                                     │
           ▼                                     ▼
    ┌─────────────────────────────────────────────────────┐
    │            Production Database Copy (Local)          │
    │                                                      │
    │  ┌──────────────────┐    ┌──────────────────┐       │
    │  │  Surpath112_DB   │    │  Surpath200_DB   │       │
    │  │     (Copy A)     │    │     (Copy B)     │       │
    │  └──────────────────┘    └──────────────────┘       │
    └─────────────────────────────────────────────────────┘
```

### Database Setup

```bash
# 1. Create backup of production database
sqlcmd -S production_server -d SurpathDB -U user -P pass \
    -Q "BACKUP DATABASE SurpathDB TO DISK='surpath_backup.bak'"

# 2. Copy backup to local machine
# (via secure transfer)

# 3. Restore as two copies locally
# Copy A - for Surpath112 testing
sqlcmd -S localhost -U sa -P localpass \
    -Q "RESTORE DATABASE Surpath112_Test FROM DISK='surpath_backup.bak'
        WITH MOVE 'SurpathDB' TO 'C:\Data\Surpath112_Test.mdf',
        MOVE 'SurpathDB_log' TO 'C:\Data\Surpath112_Test_log.ldf'"

# Copy B - for Surpath200 testing
sqlcmd -S localhost -U sa -P localpass \
    -Q "RESTORE DATABASE Surpath200_Test FROM DISK='surpath_backup.bak'
        WITH MOVE 'SurpathDB' TO 'C:\Data\Surpath200_Test.mdf',
        MOVE 'SurpathDB_log' TO 'C:\Data\Surpath200_Test_log.ldf'"
```

### Connection String Configuration

**Surpath112 (appsettings.json or web.config)**
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=Surpath112_Test;User Id=sa;Password=localpass;"
  }
}
```

**Surpath200 (appsettings.json)**
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=Surpath200_Test;User Id=sa;Password=localpass;"
  }
}
```

---

## Testing Methodology

### Test Categories

| Category | Description | Priority |
|----------|-------------|----------|
| **Functional** | Feature works as expected | Critical |
| **Data Integrity** | Data displays correctly | Critical |
| **Business Logic** | Calculations, validations match | Critical |
| **UI/UX** | Layout, interactions appropriate | High |
| **Performance** | Response times acceptable | Medium |
| **Edge Cases** | Boundary conditions handled | Medium |

---

## Test Case Template

### Test Case: [TC-XXX]

| Field | Value |
|-------|-------|
| **Module** | [Module Name] |
| **Feature** | [Feature Name] |
| **Priority** | Critical / High / Medium / Low |
| **Prerequisites** | [Setup needed before test] |

**Steps:**
1. [Step 1]
2. [Step 2]
3. [Step 3]

**Expected Result (Surpath112):**
[What happens in the original system]

**Actual Result (Surpath200):**
[What happens in the new system - fill during testing]

**Status:** ⬜ Pass / ⬜ Fail / ⬜ Blocked

**Notes:**
[Any observations, differences, issues]

---

## Module Test Checklist

### For Each Module:

#### List/Index View Tests

| Test | Surpath112 | Surpath200 | Match? |
|------|------------|------------|--------|
| Page loads without error | ⬜ | ⬜ | ⬜ |
| Data displays in table | ⬜ | ⬜ | ⬜ |
| Column headers correct | ⬜ | ⬜ | ⬜ |
| Data values match | ⬜ | ⬜ | ⬜ |
| Pagination works | ⬜ | ⬜ | ⬜ |
| Page size change works | ⬜ | ⬜ | ⬜ |
| Sorting works (each column) | ⬜ | ⬜ | ⬜ |
| Filtering works | ⬜ | ⬜ | ⬜ |
| Filter results match | ⬜ | ⬜ | ⬜ |
| Create button visible (with permission) | ⬜ | ⬜ | ⬜ |
| Edit button visible (with permission) | ⬜ | ⬜ | ⬜ |
| Delete button visible (with permission) | ⬜ | ⬜ | ⬜ |
| Actions hidden (without permission) | ⬜ | ⬜ | ⬜ |

#### Create Tests

| Test | Surpath112 | Surpath200 | Match? |
|------|------------|------------|--------|
| Create modal/page opens | ⬜ | ⬜ | ⬜ |
| All fields present | ⬜ | ⬜ | ⬜ |
| Required validation works | ⬜ | ⬜ | ⬜ |
| Custom validation works | ⬜ | ⬜ | ⬜ |
| Conditional fields show/hide | ⬜ | ⬜ | ⬜ |
| Dropdowns populate correctly | ⬜ | ⬜ | ⬜ |
| Save creates record | ⬜ | ⬜ | ⬜ |
| Success message shown | ⬜ | ⬜ | ⬜ |
| List refreshes after save | ⬜ | ⬜ | ⬜ |
| Cancel closes without save | ⬜ | ⬜ | ⬜ |

#### Edit Tests

| Test | Surpath112 | Surpath200 | Match? |
|------|------------|------------|--------|
| Edit modal/page opens | ⬜ | ⬜ | ⬜ |
| Existing data populates | ⬜ | ⬜ | ⬜ |
| All fields editable | ⬜ | ⬜ | ⬜ |
| Validation works on edit | ⬜ | ⬜ | ⬜ |
| Save updates record | ⬜ | ⬜ | ⬜ |
| Changes persist correctly | ⬜ | ⬜ | ⬜ |
| List shows updated data | ⬜ | ⬜ | ⬜ |

#### Delete Tests

| Test | Surpath112 | Surpath200 | Match? |
|------|------------|------------|--------|
| Delete confirmation appears | ⬜ | ⬜ | ⬜ |
| Cancel prevents delete | ⬜ | ⬜ | ⬜ |
| Confirm deletes record | ⬜ | ⬜ | ⬜ |
| Record removed from list | ⬜ | ⬜ | ⬜ |
| Related data handled correctly | ⬜ | ⬜ | ⬜ |

#### Business Logic Tests

| Logic | Description | Surpath112 Result | Surpath200 Result | Match? |
|-------|-------------|-------------------|-------------------|--------|
| [Logic 1] | | | | ⬜ |
| [Logic 2] | | | | ⬜ |
| [Logic 3] | | | | ⬜ |

---

## Comparison Testing Process

### Step-by-Step Process

1. **Prepare Test Data**
   - Identify specific records to test
   - Note IDs and values in both databases
   - Take screenshots of Surpath112 as baseline

2. **Side-by-Side Testing**
   - Open both systems in separate browser windows
   - Perform identical actions in both
   - Compare results at each step

3. **Document Differences**
   - Note any behavioral differences
   - Capture screenshots
   - Determine if difference is acceptable or a bug

4. **Regression Check**
   - After fixing issues, re-test from beginning
   - Verify fix didn't break other functionality

### Comparison Points

```
┌─────────────────────────────────────────────────────────────┐
│                  Comparison Checklist                        │
├─────────────────────────────────────────────────────────────┤
│ Visual:                                                      │
│   ⬜ Layout matches design intent                            │
│   ⬜ Data displays in correct format                         │
│   ⬜ Dates formatted consistently                            │
│   ⬜ Numbers/currency formatted correctly                    │
│   ⬜ Status indicators match                                 │
│                                                              │
│ Functional:                                                  │
│   ⬜ Same records returned for same query                    │
│   ⬜ Sorting produces same order                             │
│   ⬜ Pagination returns same page sizes                      │
│   ⬜ Filters produce same results                            │
│   ⬜ CRUD operations have same effect                        │
│                                                              │
│ Data:                                                        │
│   ⬜ All fields present                                      │
│   ⬜ Values match exactly                                    │
│   ⬜ Calculations produce same results                       │
│   ⬜ Aggregations match                                      │
│                                                              │
│ Permissions:                                                 │
│   ⬜ Same buttons/actions available                          │
│   ⬜ Same restrictions enforced                              │
│   ⬜ Error messages similar                                  │
└─────────────────────────────────────────────────────────────┘
```

---

## Issue Tracking

### Issue Template

| Field | Value |
|-------|-------|
| **ID** | ISSUE-XXX |
| **Module** | [Module Name] |
| **Feature** | [Feature Name] |
| **Severity** | Critical / High / Medium / Low |
| **Type** | Bug / Difference / Enhancement |

**Description:**
[What is the issue]

**Steps to Reproduce:**
1. [Step 1]
2. [Step 2]

**Expected (Surpath112):**
[What Surpath112 does]

**Actual (Surpath200):**
[What Surpath200 does]

**Screenshots:**
[Attach comparison screenshots]

**Resolution:**
[How it was fixed]

**Status:** ⬜ Open / ⬜ In Progress / ⬜ Resolved / ⬜ Won't Fix

---

## Automated Testing (Optional)

### API Comparison Tests

```typescript
// tests/api-comparison.test.ts
import { surpath112Api, surpath200Api } from './test-utils';

describe('Module API Comparison', () => {
    test('GetAll returns same data', async () => {
        const params = { filter: '', skipCount: 0, maxResultCount: 10 };
        
        const result112 = await surpath112Api.getAll(params);
        const result200 = await surpath200Api.getAll(params);
        
        expect(result200.totalCount).toBe(result112.totalCount);
        expect(result200.items.length).toBe(result112.items.length);
        
        // Compare each item
        result200.items.forEach((item, index) => {
            expect(item.id).toBe(result112.items[index].id);
            expect(item.name).toBe(result112.items[index].name);
            // ... compare other fields
        });
    });
    
    test('Get single record matches', async () => {
        const testId = 123; // Known test record
        
        const result112 = await surpath112Api.get({ id: testId });
        const result200 = await surpath200Api.get({ id: testId });
        
        expect(result200).toEqual(result112);
    });
});
```

### E2E Comparison Tests

```typescript
// tests/e2e/module.spec.ts
import { test, expect } from '@playwright/test';

test.describe('Module E2E Comparison', () => {
    test('list view displays same data', async ({ page }) => {
        // Test Surpath112
        await page.goto('http://localhost:5112/surpath-module');
        const rows112 = await page.locator('table tbody tr').count();
        
        // Test Surpath200
        await page.goto('http://localhost:5200/app/surpath-module');
        const rows200 = await page.locator('table tbody tr').count();
        
        expect(rows200).toBe(rows112);
    });
});
```

---

## Sign-Off Checklist

### Module Sign-Off

| Module | Dev Complete | QA Complete | Issues Resolved | Sign-Off |
|--------|--------------|-------------|-----------------|----------|
| Module 1 | ⬜ | ⬜ | ⬜ | ⬜ |
| Module 2 | ⬜ | ⬜ | ⬜ | ⬜ |
| Module 3 | ⬜ | ⬜ | ⬜ | ⬜ |
| Module 4 | ⬜ | ⬜ | ⬜ | ⬜ |
| Module 5 | ⬜ | ⬜ | ⬜ | ⬜ |
| Module 6 | ⬜ | ⬜ | ⬜ | ⬜ |
| Module 7 | ⬜ | ⬜ | ⬜ | ⬜ |
| Module 8 | ⬜ | ⬜ | ⬜ | ⬜ |
| Module 9 | ⬜ | ⬜ | ⬜ | ⬜ |
| Module 10 | ⬜ | ⬜ | ⬜ | ⬜ |

### Final Sign-Off

- [ ] All modules pass functional testing
- [ ] All modules pass data integrity testing
- [ ] All business logic validated
- [ ] All critical issues resolved
- [ ] Performance acceptable
- [ ] Ready for production cutover