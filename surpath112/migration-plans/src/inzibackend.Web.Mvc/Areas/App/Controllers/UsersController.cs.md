# Modified
## Filename
UsersController.cs
## Relative Path
inzibackend.Web.Mvc\Areas\App\Controllers\UsersController.cs
## Language
C#
## Summary
The modified file adds a permission check before fetching roles and includes retrieving the password complexity setting in one of its methods.
## Changes
Added two lines: a permission check using IsGrantedAsync and retrieving PasswordComplexitySetting from the store. The unmodified version lacks these changes.
## Purpose
The controller class provides API endpoints for user administration functionalities such as viewing roles, creating/editing users, managing permissions, and logging attempts.
