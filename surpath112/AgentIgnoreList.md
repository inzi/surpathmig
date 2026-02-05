# Agent Ignore List

This file contains patterns for folders and files that should be ignored by the documentation agent.

## Folders to Ignore

- node_modules/
- bin/
- obj/
- .git/
- dist/
- build/
- temp/
- cache/
- logs/
- packages/
- .vs/
- .vscode/
- .idea/
- TestResults/
- .claudedocs/
- .claude/
- ABP7.3/
- ANZ11.4/
- aspnetzeromvc11.4/
- .taskmaster/
- .cline/
- .clinerules/
- .cursor/
- .roo/
- branchdocs/
- docs/
- migplanner/
- migration-plans/
- misc/
- plans/
- scripts/
- .serena/

## File Patterns to Ignore

- *.min.js
- *.min.css
- *.map
- *.lock
- package-lock.json
- yarn.lock
- *.cache
- *.tmp
- *.temp
- *.log
- *.bak
- *.swp
- .DS_Store
- Thumbs.db
- *.suo
- *.user
- *.sln.docstates

## Extensions to Ignore

- *.dll
- *.exe
- *.pdb
- *.zip
- *.tar
- *.gz
- *.rar
- *.7z
- *.pdf
- *.doc
- *.docx
- *.xls
- *.xlsx
- *.ppt
- *.pptx

## Generated Files

- *.generated.ts
- *.generated.cs
- *.Designer.cs
- *.g.cs
- *.g.i.cs

## Test Coverage

- coverage/
- .coverage/
- *.coverage
- *.coveragexml

---
*This file is used by the codebase-documenter agent to determine which files and folders to skip during documentation generation.*