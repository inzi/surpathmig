# Corrections Protocol

When a user corrects Claude's understanding, this takes priority.

## Trigger Phrases

- "That's not right"
- "Actually it works like..."
- "No, the reason is..."
- "You misunderstood X"
- "The KB is wrong about..."
- "That's incorrect"
- "Let me clarify..."

## Procedure

1. **Find existing entry** — Search KB for the concept being corrected
2. **Update in place** — Modify the existing content, don't create a duplicate
3. **Update timestamp/SHA** — Mark as freshly updated
4. **Add correction context** (if the misunderstanding was significant):
```markdown
## Corrected
- Previously: {old understanding}
- Actually: {correct understanding}
- Why: {user's explanation, if provided}
```

5. **Check cross-refs** — Does correction affect related concepts? Update them too.

## NEVER

- Argue with the user's correction
- Leave old incorrect entry alongside new one
- Skip the KB update after being corrected
- Assume you know better than the user about their codebase

## After Correction

- Confirm the update: "Updated the KB entry for {concept}"
- If the correction impacts current work, adjust approach immediately
- If correction reveals a pattern of misunderstanding, note it for future reference