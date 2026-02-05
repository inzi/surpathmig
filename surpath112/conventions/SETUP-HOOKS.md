# Setting Up Convention Hooks for Claude Code

## Overview
Configure **Claude Code CLI** to automatically show available conventions at the start of each session.

**Important**: These are **Claude Code hooks**, not VS Code or Visual Studio hooks. They work in the Claude Code CLI on any platform (Windows, Mac, Linux).

**Hook Used**: `SessionStart` - Runs when you start a new session or resume an existing one. (Note: Claude Code doesn't have a specific "enter plan mode" hook, so this is the best available option.)

## What Are Claude Code Hooks?

Claude Code supports hooks that run shell commands at specific lifecycle points.

**Valid Hook Events** (see [docs](https://code.claude.com/docs/en/hooks)):
- **SessionStart**: When session begins or resumes (✅ **We use this one**)
- **UserPromptSubmit**: When user submits a prompt
- **PreToolUse**: Before tool execution
- **PostToolUse**: After tool succeeds
- **Stop**: When Claude finishes responding
- **SubagentStart/SubagentStop**: When spawning/finishing subagents
- **PermissionRequest**: When permission dialog appears
- **SessionEnd**: Session terminates
- Many more (see documentation)

For conventions reminder, we use **`SessionStart`** which runs at the beginning of each session to remind you about available conventions.

## Hook Configuration

Claude Code hooks are configured in:
- **Global**: `~/.claude/settings.json` (affects all projects)
- **Project**: `<project>/.claude/settings.json` (project-specific)

### Option 1: Project-Specific Hook (Recommended)

Create or edit `I:\surpath150\.claude\settings.local.json`:

**Windows (PowerShell)**:
```json
{
  "hooks": {
    "onEnterPlanMode": {
      "command": "powershell -ExecutionPolicy Bypass -File \".claude\\hooks\\pre-plan.ps1\"",
      "description": "Check conventions before planning"
    }
  }
}
```

**Mac (Bash)**:
```json
{
  "hooks": {
    "onEnterPlanMode": {
      "command": "bash .claude/hooks/pre-plan.sh",
      "description": "Check conventions before planning"
    }
  }
}
```

**Linux (Bash)**:
```json
{
  "hooks": {
    "onEnterPlanMode": {
      "command": "bash .claude/hooks/pre-plan.sh",
      "description": "Check conventions before planning"
    }
  }
}
```

**Cross-Platform (Simple Echo)**:
```json
{
  "hooks": {
    "onEnterPlanMode": {
      "command": "echo \"\n=== REMINDER: Check conventions/ folder before planning ===\nAvailable: excel-import-export.md, service-layer.md\n\"",
      "description": "Convention reminder"
    }
  }
}
```

### Option 2: Global Hook

Edit `~/.claude/settings.json` (or `C:\Users\chris\.claude\settings.json` on Windows):

**Note**: Global hooks need absolute paths since they run outside project context.

**Windows**:
```json
{
  "model": "sonnet[1m]",
  "hooks": {
    "SessionStart": [
      {
        "hooks": [
          {
            "type": "command",
            "command": "powershell -ExecutionPolicy Bypass -File \"I:\\surpath150\\.claude\\hooks\\pre-plan.ps1\""
          }
        ]
      }
    ]
  }
}
```

**Mac/Linux**:
```json
{
  "model": "sonnet[1m]",
  "hooks": {
    "SessionStart": [
      {
        "hooks": [
          {
            "type": "command",
            "command": "bash /path/to/surpath150/.claude/hooks/pre-plan.sh"
          }
        ]
      }
    ]
  }
}
```

## How Claude Code Hooks Work

Claude Code hooks are **CLI-based**, not IDE-based:
- They run in the **Claude Code terminal/CLI**
- They work on **Windows, Mac, and Linux**
- They're configured via **Claude Code's settings.local.json**
- They execute **shell commands** at specific trigger points

**Important**: There is **no "EnterPlanMode" hook** in Claude Code. Instead, we use **`SessionStart`** which runs when you begin a new session or resume an existing one. This reminds you about conventions at the start of your work.

## Why SessionStart Hook?

Since Claude Code doesn't have a specific "plan mode" hook, we use **`SessionStart`** which:
- ✅ Runs at the beginning of each session
- ✅ Reminds you about conventions before you start working
- ✅ Shows available convention files
- ✅ Works on all platforms (Windows, Mac, Linux)

**Alternative Approaches**:
- **UserPromptSubmit hook**: Could check if user is asking to plan something, but adds overhead to every prompt
- **Manual reference**: Simply reference conventions in your prompts when needed
- **CLAUDE.md**: Already includes conventions reminder at the top (works without hooks)

## Available Hook Scripts

Two scripts are provided in `<project>/.claude/hooks/`:

1. **pre-plan.ps1** - For Windows (PowerShell)
2. **pre-plan.sh** - For Mac/Linux (Bash)

Both scripts do the same thing:
- List all `.md` files in `conventions/` folder
- Display reminder to check patterns
- Work from any directory (auto-detect project root)

## What the Hook Does

When you start a new Claude Code session (or resume), you'll see:

```
=== Available Conventions ===
Before planning, review relevant conventions in: /path/to/project/conventions

  - excel-import-export.md
  - service-layer.md

Reminder: Follow ASP.NET Zero MVC + jQuery patterns from conventions/
========================================
```

## Valid Claude Code Hook Events

According to [Claude Code documentation](https://code.claude.com/docs/en/hooks), the valid hook events are:

- `SessionStart` ✅ (We use this one)
- `SessionEnd`
- `UserPromptSubmit`
- `PreToolUse`
- `PostToolUse`
- `PostToolUseFailure`
- `PermissionRequest`
- `SubagentStart`
- `SubagentStop`
- `Stop`
- `PreCompact`
- `Setup`
- `Notification`

**There is no `onEnterPlanMode` or `EnterPlanMode` hook.** We use `SessionStart` as the best available option to remind about conventions when you start working.

## Testing the Hook

### Method 1: Test the Script Directly

**Windows**:
```powershell
cd I:\surpath150
.\.claude\hooks\pre-plan.ps1
```

**Mac/Linux**:
```bash
cd /path/to/surpath150
bash .claude/hooks/pre-plan.sh
```

### Method 2: Enable in Claude Code

1. **Create/Edit Settings File**

   **Project-specific** (recommended): `I:\surpath150\.claude\settings.json`

   **OR Global**: `~/.claude/settings.json` (on Mac) or `C:\Users\<user>\.claude\settings.json` (on Windows)

2. **Add Hook Configuration**

   Choose based on your platform:

   **For Windows**:
   ```json
   {
     "hooks": {
       "SessionStart": [
         {
           "hooks": [
             {
               "type": "command",
               "command": "powershell -ExecutionPolicy Bypass -File .claude/hooks/pre-plan.ps1"
             }
           ]
         }
       ]
     }
   }
   ```

   **For Mac/Linux**:
   ```json
   {
     "hooks": {
       "SessionStart": [
         {
           "hooks": [
             {
               "type": "command",
               "command": "bash .claude/hooks/pre-plan.sh"
             }
           ]
         }
       ]
     }
   }
   ```

   **Cross-Platform (Simple)**:
   ```json
   {
     "hooks": {
       "SessionStart": [
         {
           "hooks": [
             {
               "type": "command",
               "command": "echo \"\n=== REMINDER ===\nCheck conventions/ folder before planning\nFiles: excel-import-export.md, service-layer.md\n\""
             }
           ]
         }
       ]
     }
   }
   ```

3. **Restart Claude Code** (if already running)

4. **Test**: Start a new conversation and ask Claude to plan a feature. You should see the convention reminder.

## Troubleshooting

### Hook Not Running
- **Check settings file syntax**: Valid JSON? Commas correct?
- **Run script manually first**: Does it work when you run it directly?
- **Check Claude Code debug logs**: Run with `--debug hooks` flag
  ```bash
  claude --debug hooks
  ```
- **Verify file permissions**: Mac/Linux: `chmod +x .claude/hooks/pre-plan.sh`

### Platform-Specific Issues

**Windows**:
- PowerShell execution policy: Run `Get-ExecutionPolicy` (should allow scripts)
- Path separators: Use `\\` or `/` in JSON (both work)

**Mac/Linux**:
- Make script executable: `chmod +x .claude/hooks/pre-plan.sh`
- Shebang present: First line should be `#!/bin/bash`

### Testing Hook Configuration
```bash
# Windows
claude --debug hooks

# Mac/Linux
claude --debug hooks
```

This shows hook execution in debug output.

## File Locations

### Hook Scripts (Created)
- ✅ `I:\surpath150\.claude\hooks\pre-plan.ps1` (Windows/PowerShell)
- ✅ `I:\surpath150\.claude\hooks\pre-plan.sh` (Mac/Linux/Bash)

### Configuration Files (You Create)
- **Project-specific**: `I:\surpath150\.claude\settings.local.json` (recommended - works cross-platform per project)
  - **Note**: Use `settings.local.json`, not `settings.json`
  - This file is typically gitignored (user-specific)
- **Global**: `~/.claude/settings.json` (applies to all projects)

### Convention Files (Already Created)
- ✅ `I:\surpath150\conventions\excel-import-export.md`
- ✅ `I:\surpath150\conventions\service-layer.md`
- ✅ `I:\surpath150\conventions\README.md`

## Quick Setup (Recommended)

**Note**: Edit `.claude/settings.local.json` in your project, not `settings.json`

### Windows
```powershell
# Navigate to project
cd I:\surpath150

# Create project-specific settings
'{"hooks":{"SessionStart":[{"hooks":[{"type":"command","command":"powershell -ExecutionPolicy Bypass -File .claude/hooks/pre-plan.ps1"}]}]}}' | Out-File -FilePath ".claude\settings.local.json" -Encoding UTF8
```

### Mac/Linux
```bash
# Navigate to project
cd /path/to/surpath150

# Make script executable
chmod +x .claude/hooks/pre-plan.sh

# Create project-specific settings
echo '{"hooks":{"SessionStart":[{"hooks":[{"type":"command","command":"bash .claude/hooks/pre-plan.sh"}]}]}}' > .claude/settings.local.json
```

### Or Manually Edit

Just create/edit `.claude/settings.local.json` in your project root and paste the appropriate JSON from Option 1 above.

## Quick Copy-Paste Configuration

**For your `.claude/settings.local.json` file on Windows**:
```json
{
  "hooks": {
    "SessionStart": [
      {
        "hooks": [
          {
            "type": "command",
            "command": "powershell -ExecutionPolicy Bypass -File .claude/hooks/pre-plan.ps1"
          }
        ]
      }
    ]
  }
}
```

**For your `.claude/settings.local.json` file on Mac**:
```json
{
  "hooks": {
    "SessionStart": [
      {
        "hooks": [
          {
            "type": "command",
            "command": "bash .claude/hooks/pre-plan.sh"
          }
        ]
      }
    ]
  }
}
```

## Alternative: No Hook Needed

The conventions system works **without hooks**:
- ✅ Convention files are in `conventions/` folder
- ✅ Main CLAUDE.md references conventions
- ✅ Claude will see the reference and check conventions
- ✅ `/capture-convention` skill works without hooks

**Hooks are optional but helpful** - they provide an automatic reminder at session start.

## For Teams

If working with a team:
- ✅ **Commit**: `.claude/hooks/*.sh` and `.claude/hooks/*.ps1` (hook scripts)
- ✅ **Commit**: `conventions/*.md` (convention files)
- ❌ **Don't commit**: `.claude/settings.json` (user-specific configuration)

Each team member configures hooks in their own settings.
