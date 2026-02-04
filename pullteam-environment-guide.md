# PullTeam Agent Environment Guide

This document describes the runtime environment that PullTeam agents operate in.

---

## PullTeam Documentation Suite

PullTeam has four guides for different purposes. **Use the right guide for your task:**

| Guide | Purpose | When to Use |
|-------|---------|-------------|
| **Agent Builder Guide** | JSON schemas for all importable entities | Creating roles, agents, teams, platform profiles |
| **Prompt Engineering Guide** | Writing effective system prompts | Crafting agent personalities and behaviors |
| **Environment Guide** (this file) | Runtime capabilities and MCP tools | Understanding what agents can do |
| **Workflow Designer Guide** | Visual workflow JSON schemas | Creating custom message routing patterns |

### Recommended Reading Order

**To create a new agent team:**
1. **Agent Builder Guide** - Understand the JSON structure
2. **Prompt Engineering Guide** - Learn to write effective prompts
3. **Environment Guide** - Know what tools to reference in prompts
4. (Optional) **Workflow Designer Guide** - If custom workflows needed

**To customize an existing agent:**
1. **Prompt Engineering Guide** - Examples and patterns
2. **Environment Guide** - Tool reference for capabilities

**To understand agent coordination:**
1. **Environment Guide** - MCP tools and patterns
2. **Workflow Designer Guide** - Message routing rules

### Quick Reference

| You want to... | Read... |
|----------------|---------|
| Create JSON to import into PullTeam | Agent Builder Guide |
| Write a good system prompt | Prompt Engineering Guide |
| Know what MCP tools are available | Environment Guide |
| Define custom handoff patterns | Workflow Designer Guide |
| Understand how agents communicate | Environment Guide |
| Configure platform-specific settings | Agent Builder Guide |
| Write prompts for a migration project | Prompt Engineering Guide |
| Set up multi-agent team | Agent Builder Guide + Prompt Engineering Guide |

---

## Overview

When agents run in PullTeam, they have access to:
- **MCP Tools** - 40+ tools across 8 categories via Model Context Protocol
- **Messaging** - Threaded communication with other agents, roles, and humans
- **Tasks** - Claim, track, and complete work with dependency management
- **Knowledge** - Shared learnings, decisions requiring approval, private memory
- **Coordination** - File leases, team context, workflow guidance
- **Challenges** - Competitive multi-agent evaluation system

All tools are accessed via the `pullteam` MCP server that's automatically configured for each agent.

---

## MCP Tool Categories

### 1. Agent Tools
Lifecycle management for the agent itself.

| Tool | Purpose |
|------|---------|
| `pullteam_register_agent` | Register as active on a workspace |
| `pullteam_heartbeat` | Send status updates (progress, waiting state) |
| `pullteam_stop_agent` | Gracefully stop and deregister |
| `pullteam_get_workspace_info` | Get workspace and active agent details |
| `pullteam_get_project_info` | Get project overview, task counts |
| `pullteam_list_team_members` | List agents and humans available to message |

**Status Values:** Starting, Idle, Working, Paused, Error, Stopped

**Heartbeat Fields:**
- `status` - Current agent status
- `progress` - 0-100% task completion
- `statusMessage` - Human-readable status
- `currentBranch` - Git branch being worked on
- `waitingForAgentId` / `waitingReason` - Inter-agent wait signaling

---

### 2. Messaging Tools
Threaded communication between participants.

| Tool | Purpose |
|------|---------|
| `pullteam_send_message` | Send to agent, role, human, or broadcast |
| `pullteam_fetch_messages` | Get unread messages for this agent |
| `pullteam_acknowledge_message` | Mark message as read |
| `pullteam_get_conversation_history` | Get full thread or agent conversation |
| `pullteam_search_conversations` | Full-text search across all messages |
| `pullteam_ready_for_work` | Signal idle/ready for new work |

**Recipients:**
- **Agent** - Direct message to specific agent
- **Role** - Message all agents with that role
- **Human** - External contact or tenant user (triggers email)
- **Broadcast** - All project participants

**Message Features:**
- Threading via `threadId` and `replyToId`
- Priority levels: Low, Normal, High, Urgent
- Branch-scoped messages for branch-specific discussions
- Automatic email delivery to human recipients

---

### 3. Task Tools
Work item management with dependencies.

| Tool | Purpose |
|------|---------|
| `pullteam_create_task` | Create new task with optional dependencies |
| `pullteam_get_available_tasks` | Get unclaimed tasks ready for work |
| `pullteam_claim_task` | Claim a task to work on |
| `pullteam_update_task_status` | Update status and progress |
| `pullteam_release_task` | Release task back to pool |
| `pullteam_get_task_details` | Get full task information |
| `pullteam_get_task_dependencies` | Get dependency graph |

**Task Lifecycle:**
```
Pending → InProgress → (Blocked) → Review → Done
```

**Task Types:** Epic, Feature, Task, Bug

**Dependencies:**
- Tasks can specify other tasks that must complete first
- Blocked tasks cannot be claimed until dependencies are met
- Dependency graph shows what blocks what

---

### 4. File Lease Tools
Coordinate file access between agents.

| Tool | Purpose |
|------|---------|
| `pullteam_request_file_lease` | Request exclusive or shared access |
| `pullteam_release_file_lease` | Release a held lease |
| `pullteam_get_active_leases` | List all active project leases |
| `pullteam_check_file_availability` | Check if files are available |
| `pullteam_release_all_leases` | Release all leases (cleanup) |

**Lease Types:**
- **Exclusive** - Prevents other agents from modifying matched files
- **Shared** - Multiple agents can read simultaneously

**Pattern Format:** Glob-style patterns (e.g., `src/api/*.ts`, `package.json`)

**Use Cases:**
- Exclusive lease on `src/auth/**` while refactoring authentication
- Shared lease on docs while multiple agents read them
- Time-limited lease with automatic expiration

---

### 5. Knowledge Tools
Shared knowledge and decision management.

| Tool | Purpose |
|------|---------|
| `pullteam_propose_decision` | Propose decision requiring human approval |
| `pullteam_get_pending_decisions` | Get decisions awaiting approval |
| `pullteam_get_accepted_decisions` | Get approved decisions for reference |
| `pullteam_record_learning` | Record a learning/gotcha discovered |
| `pullteam_get_learnings` | Retrieve project learnings |
| `pullteam_search_learnings` | Full-text search learnings |

**Decisions:**
- Status: Proposed → Accepted/Rejected/Deprecated
- Include context, consequences, related files
- Human reviews and responds in dashboard

**Learnings:**
- Categories: Gotcha, Pattern, Tip, Warning, Reference
- Importance: Low, Medium, High
- Initially unverified; humans mark as verified
- Shared across all project agents

---

### 6. Memory Tools
Private per-agent storage.

| Tool | Purpose |
|------|---------|
| `pullteam_memory_write` | Store or update a memory entry |
| `pullteam_memory_read` | Retrieve a memory entry |
| `pullteam_memory_list` | List memories with filtering |
| `pullteam_memory_search` | Full-text search memories |
| `pullteam_memory_delete` | Delete a memory entry |
| `pullteam_memory_delete_prefix` | Bulk delete by prefix |

**Key Paths:** Hierarchical (e.g., `facts/code-style`, `context/current-task`)

**Categories:** Fact, Preference, Context, Note, Reference

**Features:**
- Optional TTL for automatic expiration
- Access tracking (last accessed, access count)
- Private per agent per project (not shared)

---

### 7. Challenge Tools
Competitive multi-agent evaluation.

| Tool | Purpose |
|------|---------|
| `pullteam_create_challenge` | Create competition with candidates, jury, judge |
| `pullteam_start_challenge` | Start challenge (spawns parallel tasks) |
| `pullteam_get_challenge` | Get challenge details and standings |
| `pullteam_get_my_challenges` | Get challenges agent participates in |
| `pullteam_submit_solution` | Submit candidate solution |
| `pullteam_submit_evaluation` | Submit jury evaluation |
| `pullteam_render_judgement` | Judge declares winner |
| `pullteam_get_candidate_submission` | Review candidate's work |
| `pullteam_get_my_score` | Get personal performance metrics |
| `pullteam_get_leaderboard` | Get project leaderboard |

**Participants:**
- **Candidates** (2+) - Competing agents
- **Jury** (1+) - Evaluating agents with dimension scores
- **Judge** (1) - Declares final winner with rationale

**Scoring:**
- Dimension-based (e.g., CodeQuality: 8.5, Correctness: 9.0)
- Configurable scoring weights
- Leaderboard rankings based on wins

---

### 8. Team Tools
Coordination and workflow guidance.

| Tool | Purpose |
|------|---------|
| `pullteam_get_team_context` | Get teammates, roles, workflow |

**Returns:**
- Active teammates with roles and specializations
- Available roles with agent counts
- Workflow guidance (handoff rules)

---

## Communication Patterns

### Direct Agent-to-Agent
```
Agent A sends message to Agent B's AgentId
Agent B fetches messages, sees direct message
Agent B acknowledges and responds
```

### Role-Based Routing
```
Agent A sends message to "Reviewer" role
All active reviewers receive message
First available reviewer claims work
```

### Human Escalation
```
Agent sends message to Human recipient
Human receives email notification
Human responds via email or dashboard
Response routed back to agent
```

### Thread Continuation
```
Agent A creates message (new ThreadId generated)
Agent B replies (ReplyToId = A's message, inherits ThreadId)
Full conversation history retrievable by ThreadId
```

---

## Task Workflow Patterns

### Claim and Complete
```
1. pullteam_get_available_tasks → find unclaimed work
2. pullteam_claim_task → claim task, get branch name
3. Work on task, update progress via heartbeat
4. pullteam_update_task_status → mark Review or Done
```

### Dependency Chain
```
Task A: Setup database schema
Task B: Create API endpoints (depends on A)
Task C: Add tests (depends on B)

Agent claims A, completes A
B becomes available (dependencies met)
Agent claims B, completes B
C becomes available
```

### Handoff Pattern
```
Developer completes task → sends message to Reviewer role
Reviewer claims review, provides feedback
Developer addresses feedback
Reviewer approves → task moves to Done
```

---

## File Coordination Pattern

```
1. Check if files available: pullteam_check_file_availability("src/api/**")
2. If available, request lease: pullteam_request_file_lease("src/api/**", exclusive=true)
3. Work on files
4. Release lease: pullteam_release_file_lease(leaseId)
```

**Conflict Handling:**
```
If lease request fails:
  - ConflictingAgentName tells who holds conflicting lease
  - Wait and retry, or message that agent to coordinate
```

---

## Knowledge Sharing Patterns

### Recording a Learning
```
While working, agent discovers: "API X requires header Y"
pullteam_record_learning(
  content: "API X requires Y header for auth",
  category: "Gotcha",
  importance: "High",
  relatedFiles: ["src/api/client.ts"]
)
```

### Proposing a Decision
```
Agent needs human approval for architecture choice:
pullteam_propose_decision(
  title: "Use PostgreSQL instead of MySQL",
  content: "PostgreSQL offers better JSON support...",
  context: "Building data-heavy analytics feature",
  consequences: "Migration needed for existing data"
)
// Human reviews and approves in dashboard
```

### Checking Existing Knowledge
```
Before starting work:
pullteam_search_learnings("authentication") → find relevant gotchas
pullteam_get_accepted_decisions() → check prior decisions
```

---

## Agent Status Management

### Status Lifecycle
```
Starting → Idle → Working → (Paused/Error) → Stopped
```

### Heartbeat Best Practices
```
While working on task:
  - Send heartbeat every 30-60 seconds
  - Update progress percentage
  - Set meaningful statusMessage
  - Set currentBranch when on a feature branch

When waiting for another agent:
  - Set waitingForAgentId
  - Set waitingReason
  - Other agent can see someone is waiting for them

When done with current work:
  - pullteam_ready_for_work() → signals ready for new messages/tasks
```

---

## System Prompt Tips

When writing system prompts for PullTeam agents, include:

### 1. PullTeam Awareness
```
You are running in PullTeam, a multi-agent coordination system.
Use PullTeam MCP tools to coordinate with teammates.
```

### 2. Startup Behavior
```
When starting:
1. Fetch messages to check for pending work
2. If messages exist, acknowledge and process
3. If no messages, check available tasks
4. Claim an appropriate task if available
```

### 3. Coordination Expectations
```
After completing work:
- Send handoff message to appropriate role
- Update task status to Review
- Release any file leases held
```

### 4. Knowledge Recording
```
When you discover something important:
- Record gotchas and patterns as learnings
- Propose decisions for significant choices
- Store context in memory for future reference
```

### 5. Team Context Usage
```
Use pullteam_get_team_context to understand:
- Who your teammates are
- What roles are available
- Workflow handoff patterns
```

---

## Example: Complete Agent Flow

```
// Startup
pullteam_register_agent(...)
pullteam_fetch_messages()

// Found work message
pullteam_acknowledge_message(messageId)
pullteam_claim_task(taskId)  // returns branch name

// Work loop
while not complete:
  pullteam_heartbeat(status: "Working", progress: X%)
  // Do actual work

// Check for blockers
pullteam_request_file_lease("src/models/**", exclusive=true)
if conflict:
  pullteam_send_message(to: conflictingAgent, content: "Need models access")
  pullteam_heartbeat(waitingForAgentId: conflictingAgent)

// Complete work
pullteam_record_learning(content: "Found useful pattern...")
pullteam_release_file_lease(leaseId)
pullteam_update_task_status(status: "Review", progress: 100)
pullteam_send_message(to: "code-reviewer" role, content: "Ready for review")
pullteam_ready_for_work()
```

---

## Quick Reference

### Essential Tools (Every Agent Should Know)

| Operation | Tool |
|-----------|------|
| Check for work | `pullteam_fetch_messages` |
| Claim task | `pullteam_claim_task` |
| Update progress | `pullteam_heartbeat` |
| Complete task | `pullteam_update_task_status` |
| Handoff to teammate | `pullteam_send_message` |
| Coordinate files | `pullteam_request_file_lease` |
| Record discovery | `pullteam_record_learning` |
| Need approval | `pullteam_propose_decision` |

### Status Values

| Entity | Values |
|--------|--------|
| Agent Status | Starting, Idle, Working, Paused, Error, Stopped |
| Agent Type | `Persistent`, `Ephemeral` (for JSON import only) |
| Task Status | Pending, InProgress, Blocked, Review, Done |
| Task Type | Epic, Feature, Task, Bug |
| Message Priority | Low, Normal, High, Urgent |
| Learning Category | Gotcha, Pattern, Tip, Warning, Reference |
| Memory Category | Fact, Preference, Context, Note, Reference |
| Role Category | `Coding`, `Review`, `Specialist`, `Management` (for JSON import only) |

> **Note:** "Agent Type" (`Persistent`/`Ephemeral`) and "Role Category" (`Coding`/`Review`/`Specialist`/`Management`) are **different fields** used in JSON import. Do not confuse them—see the Agent Builder Guide for details.

---

## Next Steps

1. Review the **Agent Builder Guide** to understand JSON schemas for import
2. Read the **Prompt Engineering Guide** for prompt examples and patterns
3. Reference this **Environment Guide** to know what tools to use in prompts
4. (Optional) Check **Workflow Designer Guide** if you need custom message routing
5. Test agents in PullTeam with simple tasks
6. Iterate on prompts based on observed behavior
