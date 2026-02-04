## ⚠️ KNOWLEDGE BASE - MANDATORY FIRST STEP

**BEFORE writing ANY code or answering questions about existing code:**
```
→ Invoke skill: search-knowledgebase
```

**AFTER making ANY code changes:**
```
→ Invoke skill: update-knowledgebase
```

| User asks about... | You MUST first... |
|-------------------|-------------------|
| How something works | `search-knowledgebase` |
| Why code does X | `search-knowledgebase` |
| Changing existing code | `search-knowledgebase` |
| Bug or unexpected behavior | `search-knowledgebase` |

**Do NOT:**
- Read `.kb/` files directly (skill handles format)
- Skip KB query because "it's a small change"
- Consolidate KB content when files get large (SPLIT instead)

@knowledgebase-rules.md
@corrections.md
@gwt-format.md