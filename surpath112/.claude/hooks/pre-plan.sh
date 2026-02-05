#!/bin/bash
# Pre-Plan Hook - Check Conventions
# This hook reminds Claude to check relevant conventions before planning

# Determine project root (script is in .claude/hooks, so go up 2 levels)
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"
CONVENTIONS_PATH="$PROJECT_ROOT/conventions"

if [ -d "$CONVENTIONS_PATH" ]; then
    CONVENTION_FILES=$(find "$CONVENTIONS_PATH" -name "*.md" -type f 2>/dev/null)
    FILE_COUNT=$(echo "$CONVENTION_FILES" | grep -c "\.md$" 2>/dev/null || echo "0")

    if [ "$FILE_COUNT" -gt 0 ]; then
        echo ""
        echo "=== Available Conventions ==="
        echo "Before planning, review relevant conventions in: $CONVENTIONS_PATH"
        echo ""

        find "$CONVENTIONS_PATH" -name "*.md" -type f -exec basename {} \; | while read filename; do
            echo "  - $filename"
        done

        echo ""
        echo "Reminder: Follow ASP.NET Zero MVC + jQuery patterns from conventions/"
        echo "========================================"
        echo ""
    fi
fi
