# Patterns

Development patterns and conventions for the codebase.

## Categories

- [jquery-to-react](jquery-to-react/) - Migration patterns from jQuery to React
- [service-layer](service-layer.md) - Application service pattern
- [excel-import-export](excel-import-export.md) - Bulk data operations
- [asset-bundling](asset-bundling.md) - JavaScript/CSS bundling (surpath112)

## Quick Reference

- **AJAX Calls**: Use `abp.services.app.*` proxies, NOT direct $.ajax (see: architecture/abp-dynamic-web-api)
- **DOM Manipulation**: Use React state, NOT jQuery selectors (see: jquery-to-react/state-management)
- **Multi-tenancy**: Disable filter when Host queries tenant data (see: architecture/multi-tenancy-filter)
