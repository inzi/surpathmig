# Search Knowledge Base Skill

Retrieves relevant context from `.kb/` before starting work. Recursive keyword search → gather files → inject context.

## Query Syntax

| Pattern | Meaning |
|---------|---------|
| `auth` | Match keyword containing "auth" |
| `auth tokens` | Match "auth" AND "tokens" |
| `auth OR caching` | Match either |
| `auth -refresh` | Match "auth" but exclude "refresh" |

Keywords match against: concept names, descriptions, content file names.

## Query Procedure

1. **Parse query** into keywords and operators

2. **Search root kb.md**
```
   .kb/kb.md → find matching (keyword) entries
```

3. **Follow matches recursively**
```
   For each matched concept:
     → Read {concept}/index.md
     → Check {concept}/kb.md for nested concepts
     → Recurse into nested concepts if they match
```

4. **Collect content files**
```
   From each relevant index.md:
     → Gather referenced .md files
     → Note staleness from timestamps
```

5. **Check staleness**
```bash
   git rev-list --count {file-sha}..HEAD
```

6. **Follow cross-references**
```
   For each (see: x) in matched concepts:
     → Check if x is relevant to the query
     → If yes, add to results
     → Especially follow technical ↔ business-logic links
```

7. **Return context**
   - List of relevant files with paths
   - Staleness warnings if any
   - Cross-references followed and why
   - Log "No KB Entry Found" for gaps (triggers gap-filling later)

## Output Format
```markdown
## KB Query: "{keywords}"

### Matched Concepts
- {concept} (X files, current)
- {concept} (X files, ⚠️ stale)

### Cross-References Followed
- → {concept} (linked from {source})

### Files
- .kb/{path} [current]
- .kb/{path} [⚠️ X commits stale]

### No KB Entry Found
- {concept} - need to explore codebase

### Content
{file contents here, or summary if too large}
```