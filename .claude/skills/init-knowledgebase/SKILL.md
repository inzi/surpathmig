# Init Knowledge Base Skill

Bootstrap a .kb/ knowledge base for an existing project.

## Prerequisites

- `.claude/skills/update-knowledgebase/SKILL.md` exists
- `.claude/skills/search-knowledgebase/SKILL.md` exists
- `.kb/` folder created with empty `kb.md`
- `.kb/mandatory.md` and includes are in place

## Process

### Phase 1: Technical Scan (autonomous)

Scan the codebase for:
- Architecture patterns and key abstractions
- Auth/security approach
- Data layer and models
- API conventions
- Config/environment setup
- Build/deploy pipeline
- Non-obvious code (complex logic, workarounds, "weird" patterns)

For each, create a candidate concept entry. Do NOT document every file—only knowledge that's hard to derive, easy to forget, or critical for safe changes.

### Phase 2: Business Discovery (interactive)

For code where the "why" is unclear, ASK the user. Batch questions (5-10 at a time). Don't overwhelm.

**Question templates:**

For complex conditionals:
```
This code has branching logic around [X].
- What business rules drive these conditions?
- Are there cases I should capture as GWT specs?
```

For validation rules:
```
I see validation for [X].
- What's the business reason for this constraint?
- What happens downstream if invalid data got through?
```

For integrations:
```
This integrates with [external service].
- What business process depends on this?
- What's the fallback if it's unavailable?
```

For "weird" code:
```
This code [describe pattern] looks intentional but non-obvious.
- Is there a business reason, or is it technical debt?
- Should I document it or flag it for cleanup?
```

For state machines / workflows:
```
This handles [entity] state transitions.
- What are the valid states and transitions?
- What business events trigger each transition?
- Any states that should never be reached?
```

### Phase 3: GWT Capture

For business logic discovered in Phase 2, create GWT specs.

Format and location defined in `.kb/gwt-format.md`.

### Phase 4: Structure

Organize into `.kb/`:
```
.kb/
├── kb.md                    # Technical + business concepts
├── mandatory.md             # Rules (loaded via CLAUDE.md)
├── knowledgebase-rules.md
├── corrections.md
├── gwt-format.md
├── architecture/
│   ├── index.md
│   └── ...
├── business-logic/          # GWT specs live here
│   ├── index.md
│   ├── kb.md
│   └── {domain}/
│       └── ...
└── {concept}/
    └── ...
```

## Output Targets

| Concept Type | Location | Format |
|--------------|----------|--------|
| Technical patterns | `.kb/{concept}/` | Standard content files |
| Business logic | `.kb/business-logic/{domain}/` | GWT specs |
| Decisions/rationale | `.kb/{concept}/decisions.md` | ADR-lite |
| Gotchas/warnings | `.kb/{concept}/gotchas.md` | Bulleted list |

## Limits

- Start with 5-10 top-level concepts max
- Batch user questions, don't ask one at a time
- Don't document the obvious
- Respect file limits (see `knowledgebase-rules.md`)

## Completion

After init:
1. Show summary of what was captured
2. List concepts with cross-references
3. List any areas that need more clarification later
4. Remind user to review `.kb/` for accuracy
```
## KB Init Complete

### Technical Concepts (X)
- auth-pattern (see: business-logic/sessions)
- data-layer (see: business-logic/orders)
- ...

### Business Logic (X GWT specs)
- pricing-rules (see: pricing-service)
- order-lifecycle (see: data-layer/orders)
- ...

### Needs Clarification
- [ ] Complex discount logic in `src/pricing/tiers.ts`
- [ ] Error handling strategy for payment failures

Please review .kb/ for accuracy.
```