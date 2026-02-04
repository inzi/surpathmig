# Update Knowledge Base Skill

Maintains a hierarchical markdown knowledge base in `.kb/` that tracks project concepts, decisions, and patterns.

## Structure
```
.kb/
├── kb.md              # Root concept index
├── mandatory.md       # Rules (loaded via CLAUDE.md)
└── {concept}/
    ├── index.md       # File manifest
    ├── kb.md          # Optional: nested concepts
    └── *.md           # Content files
```

## Hard Limits

| Element | Max Size | On Overflow |
|---------|----------|-------------|
| Content file | 80 lines | Convert to concept folder |
| index.md | 30 entries | Split into nested concepts |
| kb.md | 40 entries | Create category groupings |

Rules for handling limits are in `knowledgebase-rules.md`.

## File Formats

### kb.md (concept index)
```markdown
(keyword) Ultra-concise description [YYYY-MM-DD|{short-sha}]
(see: other-keyword)
```

### index.md (file manifest)
```markdown
# {Concept Title}

(filename.md) Ultra-concise description [YYYY-MM-DD|{short-sha}]
```

### Content file
```markdown
# {Title}

## Context
Why this exists. 1-3 sentences.

## Detail
The actual content. Concise.

## Related
- (see: other-concept)
- Source: `src/path/to/file.ts`
```

## Staleness Format

`[YYYY-MM-DD|{7-char-sha}]`

Get current SHA: `git rev-parse --short HEAD`

## Update Procedure

After ANY code change:

1. **Identify affected concepts**

2. **Update or create content files**
   - One concept per file
   - Last-write-wins for conflicts
   - **Check line count** → if >80 lines, trigger overflow

3. **Update index.md** in concept folder
   - Add/update entry with timestamp + SHA
   - **Check entry count** → if >30, trigger overflow

4. **Update kb.md** (walk up tree)
   - Update timestamp/SHA for modified concepts
   - Add `(see: x)` cross-refs where relevant
   - **Check entry count** → if >40, trigger overflow

5. **Verify chain** - Content → index.md → kb.md in sync

6. **Fill gaps** - Check for "No KB Entry Found" items from search phase

## Overflow Procedure

When a file exceeds limits, it MUST be restructured (never consolidated—see `knowledgebase-rules.md`):

### Content file overflow (>80 lines)
```
Before:
  auth-pattern/
    jwt-flow.md        # 120 lines - TOO BIG

After:
  auth-pattern/
    index.md           # Updated: remove jwt-flow.md, add (jwt-flow) concept
    kb.md              # Updated: add (jwt-flow) entry
    jwt-flow/          # NEW: concept folder
      index.md         # NEW: manifest for split files
      kb.md            # NEW: can be empty initially
      generation.md    # Split content
      validation.md    # Split content
      claims.md        # Split content
```

### index.md overflow (>30 entries)
```
Before:
  styling/
    index.md           # 35 entries - TOO BIG

After:
  styling/
    index.md           # Reduced: references concepts, not files
    kb.md              # Updated: new nested concepts
    buttons/
      index.md
      kb.md
    forms/
      index.md
      kb.md
```

### kb.md overflow (>40 entries)
```
Before:
  .kb/
    kb.md              # 45 entries - TOO BIG

After:
  .kb/
    kb.md              # Reduced: category entries only
    auth/
      kb.md            # Auth-related concepts moved here
    data/
      kb.md            # Data-related concepts moved here
```

## Commands
```bash
# Current commit SHA
git rev-parse --short HEAD

# Line count check
wc -l .kb/concept/file.md

# Entry count check
grep -c "^(" .kb/kb.md
```