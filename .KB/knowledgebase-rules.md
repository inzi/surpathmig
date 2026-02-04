# Knowledge Base Rules

## MANDATORY: Gap Filling

If `search-knowledgebase` logged "No KB Entry Found" items:
- You MUST create entries for these before considering the task complete
- Exploration produces knowledge—capture it even without code changes
- The task is NOT done if identified gaps remain unfilled

## NEVER DO THIS (Anti-patterns)

### ❌ Consolidating to reduce file count
```
WRONG: "This file is 90 lines, let me summarize it to 60 lines"
RIGHT: "This file is 90 lines, I must split it into a concept folder"
```

### ❌ Compressing content to fit limits
```
WRONG: "kb.md has 45 entries, let me combine related ones"
RIGHT: "kb.md has 45 entries, I must create category subfolders"
```

### ❌ Deleting detail to stay under limits
```
WRONG: "Remove older/less important entries to make room"
RIGHT: "Create hierarchy to accommodate all entries"
```

## THE RULE

**Content grows OUTWARD (more structure), never INWARD (compression).**

When limits are hit:
- 80 lines → new folder + split files
- 30 index entries → new nested concepts
- 40 kb entries → new category folders

The KB self-organizes by subdivision, not summarization.

## Cross-Reference Triggers

ALWAYS add `(see: x)` when:
- Technical code implements business logic → link to GWT spec
- GWT spec describes behavior → link to implementation
- Two concepts share data or dependencies
- A change in one concept would require changes in another

## Staleness Awareness

When querying:
- Flag entries >10 commits behind as potentially stale
- After completing work that touched stale areas, update those entries
- Staleness format: `[YYYY-MM-DD|{7-char-sha}]`

When updating:
- Always refresh timestamp and SHA: `git rev-parse --short HEAD`
- If content is unchanged but verified accurate, still update the timestamp