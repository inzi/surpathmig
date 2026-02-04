# GWT Format (Given-When-Then)

Captures business logic as behavioral specs. Use for the "why" behind code—rules, workflows, edge cases.

## When to Create GWT Specs

- User explains business rules or requirements
- Code has branching logic driven by business conditions
- Validation rules exist with business reasons
- State machines or workflows with business meaning
- Edge cases that aren't obvious from code alone
- Anything where "why does it work this way?" has a non-technical answer

## Template
```markdown
# {Domain}: {Feature Name}

## Context
{1-3 sentences: what business problem this solves}

## Behaviors

### Happy path: {scenario}
- **Given** {initial state}
- **When** {action}
- **Then** {outcome}

### Edge case: {scenario}
- **Given** {state}
- **When** {action}
- **Then** {outcome}

### Error case: {scenario}
- **Given** {state}
- **When** {invalid action}
- **Then** {error handling}

## Why
{Business reasoning—capture user's words if insightful}

## Exceptions
{Known exceptions to these rules, and why they exist}

## Related
- (see: {technical-concept})
- Source: `src/path/to/file.ts`
```

## Location

GWT specs live in `.kb/business-logic/{domain}/`:
```
.kb/
└── business-logic/
    ├── index.md
    ├── kb.md
    ├── pricing/
    │   ├── index.md
    │   └── discount-rules.md    # GWT spec
    └── orders/
        ├── index.md
        └── order-lifecycle.md   # GWT spec
```

## Cross-Reference Rule

**Every GWT spec must link to its implementation:**
```markdown
## Related
- (see: pricing-service) Technical implementation
- Source: `src/services/pricing.ts`
```

**Every technical concept with business logic must link to its GWT:**
```markdown
## Related
- (see: business-logic/pricing) GWT specs for pricing rules
```

## Capture Verbatim

When the user explains "why" something works a certain way, capture their words directly in the **Why** section. Their phrasing often contains nuance that paraphrasing loses.

## GWT is NOT For

- Pure technical patterns (use standard KB entries)
- Implementation details (belongs in technical docs)
- API contracts (unless there's business meaning)
- Configuration (unless business rules drive it)