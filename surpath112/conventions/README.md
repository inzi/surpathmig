# Conventions Folder

## Purpose
This folder contains documented coding patterns, architectural conventions, and ASP.NET Zero MVC + jQuery best practices for this codebase.

**Important**: These conventions are specific to **ASP.NET Zero MVC + jQuery** (multi-page application). ASP.NET Zero also supports Angular (SPA), which may have different patterns.

## When to Use These Conventions

### Before Planning
- Review relevant convention files before designing new features
- Check for established patterns that apply to your task
- Use the pre-plan hook to be reminded of available conventions

### During Implementation
- Reference convention files for code examples
- Follow the patterns shown in reference implementations
- Use verification checklists to ensure compliance

### After Learning New Patterns
- Use `/capture-convention` skill to document new patterns
- Update existing conventions when patterns evolve
- Add anti-patterns to "Common Mistakes" sections

## Available Conventions

### [excel-import-export.md](excel-import-export.md)
Complete pattern for bulk data import/export using:
- `NpoiExcelImporterBase` for reading Excel files
- `NpoiExcelExporterBase` for creating Excel files
- Background jobs for async processing
- Service layer for business logic
- Proper error handling and reporting

**When to use**: Bulk user imports, data exports, any Excel-based data transfer

**Key examples**: UserListExcelExporter, TenantUserExporter, ImportUsersToExcelJob

### [service-layer.md](service-layer.md)
Application service pattern with proper separation of concerns:
- Services contain business logic
- Controllers are thin coordinators
- jQuery uses service proxies (`abp.services.app.*`)
- Proper multi-tenancy filter handling
- Authorization at service layer

**When to use**: All business operations, CRUD, queries, any operation needing authorization

### [api.md](api.md)
Dynamic Web API pattern in ASP.NET Zero:
- How ABP auto-generates Web API endpoints from services
- jQuery service proxy usage (`abp.services.app.*`)
- Naming conventions (C# to JavaScript mapping)
- When to use service proxies vs. direct controller calls
- Error handling and authorization flow

**When to use**: Understanding how to call backend services from jQuery, designing new API operations

### [controllers.md](controllers.md)
Controller patterns for ASP.NET Zero MVC:
- Controllers should be thin (under 15 lines)
- Four main patterns: Index, Modal, FileDownload, FileUpload
- What belongs in controllers vs. services
- Authorization patterns
- Common mistakes to avoid

**When to use**: Creating new controllers, reviewing controller actions, refactoring thick controllers

### [importexport.md](importexport.md) - DEPRECATED
**Note**: This file has been superseded by `excel-import-export.md` which has comprehensive coverage. Use `excel-import-export.md` instead.

## How to Add New Conventions

### Using the Skill
```
/capture-convention
```

This will guide you through:
1. Identifying the pattern to document
2. Gathering examples from the codebase
3. Creating or updating convention files
4. Ensuring completeness

### Manual Creation
1. Create new `.md` file in this folder
2. Follow the template structure (see existing files)
3. Include: Purpose, When to Use, Code Examples, Reference Implementations, Common Mistakes, Checklist
4. Add entry to this README

## Convention File Structure

Each convention file should include:

1. **Title & Purpose**: What is this pattern for?
2. **When to Use**: Scenarios where this applies
3. **Key Components**: Files and classes involved
4. **Code Examples**: Minimal but complete examples
5. **Reference Implementations**: Real file paths from codebase
6. **Common Mistakes**: Anti-patterns with corrections
7. **Verification Checklist**: Quick checks for compliance

## Using Conventions During Development

### Pre-Planning Phase
```bash
# List available conventions
ls conventions/

# Read relevant convention
cat conventions/excel-import-export.md
```

### During Implementation
- Keep convention file open for reference
- Copy patterns from reference implementations
- Use checklist to verify completion

### Code Review
- Check implementation against conventions
- Verify all checklist items completed
- Update conventions if new edge cases discovered

## Maintaining Conventions

### When to Update
- Pattern evolves (new requirements, better approach)
- Anti-pattern discovered (add to "Common Mistakes")
- New reference implementation added to codebase
- Clarification needed based on confusion

### Versioning
- Convention files are part of the repository
- Track changes via git
- Include update date in convention files if significant changes

## Integration with Development Workflow

1. **Planning**: Check conventions folder
2. **Implementation**: Follow documented patterns
3. **Review**: Verify against checklists
4. **Learn**: Capture new conventions
5. **Iterate**: Update conventions as patterns evolve

## Questions?

If you're unsure about a pattern:
1. Check convention files first
2. Look at reference implementations
3. Ask for clarification
4. Document the answer in conventions

---

**Remember**: These are living documents. Update them as you learn and as the codebase evolves!
