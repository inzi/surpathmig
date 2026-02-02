# Surpath112 → Surpath200 Migration Overview

## Project Summary

| Attribute | Value |
|-----------|-------|
| **Source System** | Surpath112 (ASPNetZero MVC/jQuery) |
| **Target System** | Surpath200 (ASPNetZero Core/React) |
| **Source Location** | `../surpath150` |
| **Baseline Reference** | `./aspnetzeromvc11.4` |
| **Module Count** | 8-10 modules |
| **Strategy** | Cut-over (parallel local testing with production DB copies) |
| **Timeline** | ASAP - Immediate start |

---

## Migration Phases

### Phase 1: Foundation Setup (Week 1-2)
- [ ] Set up Surpath200 base project from ASPNetZero React template
- [ ] Configure database connections for A/B testing
- [ ] Map all permissions from Surpath112
- [ ] Set up localization infrastructure
- [ ] Establish Uizard → React style workflow
- [ ] Create shared component library structure

### Phase 2: Backend Alignment (Week 2-3)
- [ ] Verify API compatibility (most backend code translates directly)
- [ ] Update any API signatures that differ between MVC and Core
- [ ] Migrate application services
- [ ] Update DTOs if needed
- [ ] Ensure Entity Framework migrations are compatible

### Phase 3: Module Migration (Week 3-8)
Convert each module following the priority order below. Use `06-module-migration-checklist.md` for each.

| Priority | Module | Complexity | Dependencies | Status |
|----------|--------|------------|--------------|--------|
| 1 | [Module 1] | TBD | None | ⬜ Not Started |
| 2 | [Module 2] | TBD | Module 1 | ⬜ Not Started |
| 3 | [Module 3] | TBD | TBD | ⬜ Not Started |
| 4 | [Module 4] | TBD | TBD | ⬜ Not Started |
| 5 | [Module 5] | TBD | TBD | ⬜ Not Started |
| 6 | [Module 6] | TBD | TBD | ⬜ Not Started |
| 7 | [Module 7] | TBD | TBD | ⬜ Not Started |
| 8 | [Module 8] | TBD | TBD | ⬜ Not Started |
| 9 | [Module 9] | TBD | TBD | ⬜ Not Started |
| 10 | [Module 10] | TBD | TBD | ⬜ Not Started |

### Phase 4: Integration & Testing (Week 8-10)
- [ ] Full integration testing
- [ ] A/B comparison with production database copies
- [ ] Performance benchmarking
- [ ] User acceptance testing
- [ ] Bug fixes and refinements

### Phase 5: Cutover (Week 10-11)
- [ ] Final data migration/sync
- [ ] Production deployment
- [ ] Post-deployment monitoring
- [ ] Documentation updates

---

## Key Migration Guides

| Document | Purpose |
|----------|---------|
| [01-permission-mapping-guide.md](./01-permission-mapping-guide.md) | MVC permissions → React hooks |
| [02-localization-conversion-guide.md](./02-localization-conversion-guide.md) | L() → React localization |
| [03-jquery-to-react-patterns.md](./03-jquery-to-react-patterns.md) | Core transformation patterns |
| [04-datatable-conversion-guide.md](./04-datatable-conversion-guide.md) | DataTables → React tables |
| [05-modal-conversion-guide.md](./05-modal-conversion-guide.md) | Modal conversion patterns |
| [06-module-migration-checklist.md](./06-module-migration-checklist.md) | Per-module checklist template |
| [07-uizard-workflow.md](./07-uizard-workflow.md) | Design → React workflow |
| [08-testing-validation-guide.md](./08-testing-validation-guide.md) | A/B testing approach |

---

## Risk Assessment

| Risk | Impact | Mitigation |
|------|--------|------------|
| Complex jQuery business logic | High | Document all logic before conversion; maintain parity tests |
| Permission misalignment | Medium | Complete mapping document; test each permission |
| DataTable custom features | High | Identify all custom columns/actions; map to React equivalents |
| Modal state management | Medium | Use React state patterns; document modal flows |
| Styling inconsistencies | Low | Use default theme first; add custom theme post-migration |
| Database schema differences | Low | Backend mostly compatible; verify migrations |

---

## Success Criteria

- [ ] All 8-10 modules functional in Surpath200
- [ ] Feature parity with Surpath112 (validated via A/B testing)
- [ ] All permissions working correctly
- [ ] All localization strings displaying properly
- [ ] No regression in existing functionality
- [ ] Performance equal to or better than Surpath112

---

## Notes

- **Keep It Simple and Secure (KISS)** - Per project guidelines
- **Stick with default React theme** - Custom theme added post-migration
- **Backend code translates without issue** - Focus effort on frontend conversion
- **Reference git history** in `../surpath150` for understanding evolution of complex features