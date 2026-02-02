# Modified
## Filename
_TopUserMenuItem.cshtml
## Relative Path
inzibackend.Web.Mvc\Areas\App\Views\Shared\Components\AppMenu\_TopUserMenuItem.cshtml
## Language
Unknown
## Summary
The modified file implements a navigation menu system with conditional rendering based on menu activity and visibility. It includes theme handling, ordered child menu items, and dynamic class assignments for menu items and submenus.
## Changes
Added conditional rendering of child menu items based on their visibility status when RootLevel is false. Introduced explicit setting of RootLevel to false in Html.PartialAsync calls for each child item.
## Purpose
The file provides functionality for creating a responsive navigation menu with dynamic class assignments and conditional rendering, supporting both root-level and sub-menu items.
