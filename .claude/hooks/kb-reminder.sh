#!/bin/bash
# KB Reminder Hook
# Reminds about querying the Knowledge Base before implementation work
#
# This hook fires on UserPromptSubmit events. When the prompt looks like
# implementation work (contains keywords like "implement", "add", "create", etc.),
# it injects a reminder to query the KB first.

# Read the input JSON from stdin
INPUT=$(cat)

# Extract the prompt text (handle both .prompt and nested structures)
PROMPT=$(echo "$INPUT" | grep -oP '"prompt"\s*:\s*"\K[^"]*' | head -1)

# If we couldn't extract, try a simpler approach
if [ -z "$PROMPT" ]; then
    PROMPT="$INPUT"
fi

# Check if prompt looks like implementation work
# (contains keywords suggesting code changes)
if echo "$PROMPT" | grep -iE "(implement|add feature|create|build|write code|fix bug|refactor|update code|modify|change the|edit the|new feature|add a)" > /dev/null 2>&1; then

    # Output JSON with additionalContext to inject the reminder
    cat << 'HOOKEOF'
{
  "hookSpecificOutput": {
    "hookEventName": "UserPromptSubmit",
    "additionalContext": "**KNOWLEDGE BASE REMINDER**: Before implementing, search the Knowledge Base:\n\n1. Search `.kb/kb.md` for relevant concepts\n2. Follow `(see: x)` cross-references\n3. Load related content files from `.kb/` subdirectories\n\nAfter implementation, update the Knowledge Base with new patterns/concepts.\n\nSee `.claude/skills/search-knowledgebase/SKILL.md` for search instructions."
  }
}
HOOKEOF
    exit 0
fi

# Not an implementation prompt, pass through normally
exit 0
