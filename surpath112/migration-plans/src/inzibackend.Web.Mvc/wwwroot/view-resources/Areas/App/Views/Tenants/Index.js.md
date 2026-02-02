# Modified
## Filename
Index.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\view-resources\Areas\App\Views\Tenants\Index.js
## Language
JavaScript
## Summary
A JavaScript file that manages the tenant administration interface in an ASP.NET Zero application. It implements a DataTable for displaying tenant information with filtering capabilities including date range filters for creation and subscription end dates, edition dropdown filtering, and text search. The file handles tenant CRUD operations through modal dialogs, provides tenant impersonation functionality, manages tenant features, and includes entity history tracking. It also configures various UI interactions such as date range pickers, refresh buttons, and action buttons for tenant management operations.
## Changes
Two main changes were made: 1) DataTable paging configuration changed from 'paging: true' to 'paging: false' to disable pagination, 2) DataTable responsive control class changed from 'dtr-control responsive' to 'control responsive' in the first column definition, 3) Added a new click event handler for 'NewTenantWizard' button that redirects to '/App/Tenants/NewTenantWizard' page.
## Purpose
This file serves as the client-side JavaScript controller for the tenant management page in an ASP.NET Zero multi-tenant application. It handles the presentation layer logic for tenant administration including data display, filtering, CRUD operations, user impersonation, feature management, and navigation to tenant-related functionality.
