# Navigation Documentation

## Overview
This folder contains extension methods for enhancing ABP's navigation menu system with additional functionality for menu item activation detection, URL calculation, and custom ordering.

## Contents

### Files

#### UserMenuItemExtensions.cs
- **Purpose**: Extension methods for UserMenuItem class to enhance navigation features
- **Key Methods**:
  - **IsMenuActive()**: Recursively checks if menu item or any child is active
  - **CalculateUrl()**: Converts relative URLs to absolute based on application path
  - **OrderByCustom()**: Sorts menu items by Order property with DisplayName as tiebreaker
- **Usage**: Used in Razor views and navigation rendering code

### Key Components
- **Active Menu Detection**: Highlights current page in navigation
- **URL Calculation**: Handles relative/absolute URL resolution
- **Custom Ordering**: Consistent menu item sorting logic

### Dependencies
- **Abp.Application.Navigation**: ABP navigation infrastructure (UserMenuItem)
- **Abp.AspNetZeroCore.Web.Url**: URL helper utilities (UrlChecker)

## Architecture Notes

### Extension Method Pattern
Extends ABP's UserMenuItem without modifying core classes:
- Non-invasive enhancement
- Follows open/closed principle
- Easy to test and maintain

### Recursive Menu Structure
Menus can have unlimited depth:
```
Dashboard (level 1)
Administration (level 1)
  ├── Users (level 2)
  ├── Roles (level 2)
  └── Settings (level 2)
      ├── General (level 3)
      └── Email (level 3)
```

IsMenuActive() recursively searches entire tree.

## Business Logic

### IsMenuActive() - Active Menu Highlighting
```csharp
public static bool IsMenuActive(this UserMenuItem menuItem, string currentPageName)
```
- **Purpose**: Determine if menu item should be highlighted as active
- **Logic**:
  1. Check if menu item name matches current page
  2. If no match, recursively check all child items
  3. Return true if any child is active (parent should expand)
- **Use Case**:
  - Highlight current page in menu
  - Expand parent menus to show active child
  - Visual indication of navigation location

**Example**:
```csharp
// In Razor view
@if (menuItem.IsMenuActive(ViewBag.CurrentPageName))
{
    <li class="active">...</li>
}
```

### CalculateUrl() - URL Resolution
```csharp
public static string CalculateUrl(this UserMenuItem menuItem, string applicationPath)
```
- **Purpose**: Convert menu item URLs to absolute URLs
- **Logic**:
  1. If URL is empty, return application root
  2. If URL is rooted (absolute), return as-is
  3. Otherwise, prepend application path to relative URL
- **Use Cases**:
  - Multi-tenant with different paths (/tenant1, /tenant2)
  - Deployed in virtual directory (/myapp)
  - Consistent URL generation across environments

**Examples**:
```csharp
// Absolute URL (unchanged)
menuItem.Url = "https://example.com/page";
menuItem.CalculateUrl("/app") → "https://example.com/page"

// Relative URL (prepended with app path)
menuItem.Url = "/Admin/Users";
menuItem.CalculateUrl("/app") → "/app/Admin/Users"

// Empty URL (returns app root)
menuItem.Url = "";
menuItem.CalculateUrl("/app") → "/app"

// Rooted relative URL (unchanged)
menuItem.Url = "/";
menuItem.CalculateUrl("/app") → "/"
```

### OrderByCustom() - Menu Ordering
```csharp
public static IOrderedEnumerable<UserMenuItem> OrderByCustom(this IList<UserMenuItem> menuItems)
```
- **Purpose**: Sort menu items in display order
- **Logic**:
  1. Primary sort by Order property (lower numbers first)
  2. Secondary sort by DisplayName (alphabetical)
  3. Items with Order=0 sorted before named items (bug or feature?)
- **Use Case**: Consistent menu ordering across application

**Sort Behavior**:
```
Order=1, Name="Dashboard"
Order=2, Name="Users"
Order=2, Name="Reports"     ← Alphabetical within same Order
Order=3, Name="Settings"
Order=0, Name="Help"        ← Zero-order items at end
```

**Note**: The OrderBy condition seems unusual:
```csharp
.ThenBy(menuItem => menuItem.Order == 0 ? null : menuItem.DisplayName)
```
This means Order=0 items don't sort by name (null sorts first). This might be intentional for "Other" or "More" menu items.

## Usage Across Codebase

### Consumed By
- **Layout Views**: _Layout.cshtml, _Sidebar.cshtml
- **Navigation Partials**: _MenuItems.cshtml
- **Menu Rendering**: Loops through UserMenuItem collections
- **Authorization**: Combined with permission checks

### Typical View Usage
```razor
@foreach (var menuItem in Model.Menu.Items.OrderByCustom())
{
    var url = menuItem.CalculateUrl(ApplicationPath);
    var isActive = menuItem.IsMenuActive(CurrentPageName);

    <li class="@(isActive ? "active" : "")">
        <a href="@url">@menuItem.DisplayName</a>

        @if (menuItem.Items.Any())
        {
            <ul class="submenu">
                @foreach (var child in menuItem.Items.OrderByCustom())
                {
                    // Render children...
                }
            </ul>
        }
    </li>
}
```

## Multi-Tenant Considerations

### Tenant-Specific Paths
Different tenants may have different application paths:
- **Subdomain**: tenant1.app.com → applicationPath = "/"
- **Path-based**: app.com/tenant1 → applicationPath = "/tenant1"
- **Custom Domain**: tenant1.com → applicationPath = "/"

CalculateUrl() handles all scenarios correctly.

### Tenant-Specific Menus
Menu items filtered by:
- Tenant permissions (some features disabled per tenant)
- User role/permissions
- Feature availability

Extension methods work with filtered menu list.

## Performance Considerations
- **IsMenuActive()**: Recursive traversal
  - Performance impact: Minimal (menus rarely >3 levels deep)
  - Called during view rendering (acceptable overhead)
- **CalculateUrl()**: String concatenation
  - Performance impact: Negligible
  - Could cache results if needed (likely premature optimization)
- **OrderByCustom()**: Sorting collection
  - Performance impact: O(n log n), acceptable for menu sizes

## Extensibility

### Adding Custom Extension Methods
```csharp
public static class MyMenuExtensions
{
    // Check if menu has badge/notification count
    public static bool HasBadge(this UserMenuItem menuItem)
    {
        return menuItem.CustomData?.ContainsKey("badge") ?? false;
    }

    // Get notification count from custom data
    public static int GetBadgeCount(this UserMenuItem menuItem)
    {
        return menuItem.CustomData?.ContainsKey("badge") == true
            ? (int)menuItem.CustomData["badge"]
            : 0;
    }

    // Check if menu requires specific feature
    public static bool RequiresFeature(this UserMenuItem menuItem, string featureName)
    {
        return menuItem.RequiredFeatures?.Contains(featureName) ?? false;
    }
}
```

### Custom Menu Highlighting
```csharp
// Highlight by URL pattern instead of page name
public static bool IsMenuActiveByUrl(this UserMenuItem menuItem, string currentUrl)
{
    if (currentUrl.StartsWith(menuItem.Url))
        return true;

    return menuItem.Items?.Any(i => i.IsMenuActiveByUrl(currentUrl)) ?? false;
}
```

## Security Considerations
- Menu items already filtered by permissions (ABP framework)
- URL calculation doesn't perform authorization checks
- Extension methods assume UserMenuItem already authorized
- Don't rely on menu visibility for security (enforce in controllers)

## Testing Considerations
- **IsMenuActive()**: Test recursive behavior with nested menus
- **CalculateUrl()**: Test various URL formats (absolute, relative, rooted)
- **OrderByCustom()**: Test edge cases (null DisplayName, duplicate Orders)

**Example Tests**:
```csharp
[Fact]
public void IsMenuActive_Should_Detect_Active_Child()
{
    var parent = new UserMenuItem("Parent", "Parent");
    var child = new UserMenuItem("Child", "Child");
    parent.Items.Add(child);

    Assert.True(parent.IsMenuActive("Child"));
}

[Fact]
public void CalculateUrl_Should_Handle_Absolute_Urls()
{
    var item = new UserMenuItem("Test", "Test", "https://example.com");
    Assert.Equal("https://example.com", item.CalculateUrl("/app"));
}
```