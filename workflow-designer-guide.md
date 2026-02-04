# PullTeam Workflow Designer Guide

This guide explains how to create workflow JSON schemas for PullTeam. Use this when designing communication patterns between agents, roles, and humans.

---

## What is a Workflow?

A workflow defines **who communicates with whom** and **what type of messages** flow between participants. Workflows are visual diagrams rendered in the PullTeam dashboard that guide agent behavior.

**Key concepts:**
- **Nodes** = Participants (roles, specific agents, humans, triggers)
- **Edges** = Message patterns between participants
- **Guidance** = Instructions for agents at each step

---

## Schema Structure

```json
{
  "version": "1.0",
  "nodes": [...],
  "edges": [...]
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `version` | string | Yes | Always "1.0" |
| `nodes` | array | Yes | Workflow participants |
| `edges` | array | Yes | Message patterns between nodes |

---

## Node Types

### Role Node

Routes to any active agent with that role. Use for most workflow participants.

```json
{
  "id": "developer",
  "type": "role",
  "position": {"x": 400, "y": 180},
  "data": {
    "roleName": "Backend Coder",
    "label": "Developer",
    "description": "Implements features and fixes",
    "guidance": "When assigned work, claim the task and implement it. Run tests before requesting review."
  }
}
```

| Field | Required | Description |
|-------|----------|-------------|
| `id` | Yes | Unique identifier (used in edges) |
| `type` | Yes | Must be `"role"` |
| `position.x` | Yes | X coordinate for visual layout |
| `position.y` | Yes | Y coordinate for visual layout |
| `data.roleName` | Yes | Exact role name (must match existing role) |
| `data.label` | Yes | Display label in workflow diagram |
| `data.description` | No | What this participant does |
| `data.guidance` | No | Instructions for agents in this role |

**Common role names:**
- `Project Manager` - Coordinates and assigns work
- `Backend Coder` - Server-side implementation
- `Frontend Coder` - UI implementation
- `Code Reviewer` - Reviews and approves PRs
- `Architect` - Technical design and planning
- `Task Manager` - Task decomposition

### Agent Node

Routes to a specific named agent. Use when you need a particular agent.

```json
{
  "id": "lead-dev",
  "type": "agent",
  "position": {"x": 400, "y": 180},
  "data": {
    "agentName": "Senior Developer",
    "label": "Lead",
    "description": "Senior developer for complex tasks",
    "guidance": "Handle complex architectural decisions and mentor junior developers."
  }
}
```

| Field | Required | Description |
|-------|----------|-------------|
| `data.agentName` | Yes | Exact agent name (must match existing agent) |
| `data.label` | Yes | Display label |

### Human Node

Represents human oversight. Messages to humans trigger email notifications.

```json
{
  "id": "human",
  "type": "human",
  "position": {"x": 400, "y": 380},
  "data": {
    "humanLabel": "Human Oversight",
    "label": "Human",
    "description": "Human oversight for critical decisions"
  }
}
```

| Field | Required | Description |
|-------|----------|-------------|
| `data.humanLabel` | Yes | Label identifying the human role |
| `data.label` | Yes | Display label |

### Trigger Node

External event sources like webhooks. Initiates workflows from outside events.

```json
{
  "id": "github-trigger",
  "type": "trigger",
  "position": {"x": 100, "y": 100},
  "data": {
    "triggerType": "webhook",
    "providerId": "github-provider-id",
    "webhookEvents": ["push", "pull_request"],
    "webhookFilters": {
      "branches": ["main", "develop"],
      "actions": ["opened", "synchronize"],
      "paths": ["src/**"]
    },
    "messageTemplate": "New {{event}} on {{branch}} by {{sender}}",
    "label": "GitHub Events"
  }
}
```

| Field | Required | Description |
|-------|----------|-------------|
| `data.triggerType` | Yes | Type of trigger (currently `"webhook"`) |
| `data.providerId` | Yes | Webhook provider GUID |
| `data.webhookEvents` | No | Events to listen for |
| `data.webhookFilters.branches` | No | Branch patterns to match |
| `data.webhookFilters.actions` | No | Actions to match |
| `data.webhookFilters.paths` | No | File path patterns |
| `data.messageTemplate` | No | Template for generating messages |

---

## Edge Structure

Edges define message patterns between nodes.

```json
{
  "id": "e1",
  "source": "pm",
  "target": "developer",
  "data": {
    "messageType": "assigns task",
    "description": "PM assigns work to developer",
    "senderGuidance": "Message a developer to assign work. Include task ID and branch name.",
    "receiverGuidance": "When PM messages you with a task assignment, claim the task and begin work."
  }
}
```

| Field | Required | Description |
|-------|----------|-------------|
| `id` | Yes | Unique edge identifier |
| `source` | Yes | Node ID of message sender |
| `target` | Yes | Node ID of message receiver |
| `data.messageType` | Yes | Type of message (e.g., "assigns task", "PR ready", "approved") |
| `data.description` | No | Human-readable description |
| `data.senderGuidance` | No | Instructions for the sender |
| `data.receiverGuidance` | No | Instructions for the receiver |

---

## Common Message Types

| Message Type | When Used |
|--------------|-----------|
| `assigns task` | PM/Manager assigns work to developer |
| `PR ready` | Developer submits work for review |
| `needs changes` | Reviewer requests modifications |
| `approved` | Reviewer approves the work |
| `needs decision` | Escalation to human for decisions |
| `plan ready` | Architect/Planner completes design |
| `tasks validated` | Task breakdown approved |
| `new request` | Human initiates new work |

---

## Layout Guidelines

### Position Coordinates

- Use multiples of ~100-200 for clean alignment
- Horizontal flow: increment X by 200-300
- Vertical flow: increment Y by 120-160
- Keep nodes spaced for readability

### Common Layout Patterns

**Linear flow (left to right):**
```
PM (100, 200) → Developer (400, 200) → Reviewer (700, 200)
```

**Hierarchical (top to bottom):**
```
            Human (400, 40)
               ↓
          Architect (400, 180)
               ↓
              PM (400, 340)
         ↙    ↓    ↘
     Dev1    Dev2    Dev3
    (200,500) (400,500) (600,500)
```

**Diamond pattern:**
```
              PM (400, 100)
           ↙        ↘
     Developer      Developer
     (200, 250)     (600, 250)
           ↘        ↙
          Reviewer (400, 400)
```

---

## Complete Examples

### Simple Development Workflow

3 participants: PM assigns to Developer, Developer submits to Reviewer.

```json
{
  "version": "1.0",
  "nodes": [
    {
      "id": "pm",
      "type": "role",
      "position": {"x": 120, "y": 180},
      "data": {
        "roleName": "Project Manager",
        "label": "PM",
        "description": "Coordinates work, assigns tasks to developers",
        "guidance": "You assign tasks by messaging developers. Match task complexity to developer capability."
      }
    },
    {
      "id": "developer",
      "type": "role",
      "position": {"x": 400, "y": 180},
      "data": {
        "roleName": "Backend Coder",
        "label": "Developer",
        "description": "Implements features and fixes",
        "guidance": "When assigned work, claim the task and implement it. Run tests before requesting review."
      }
    },
    {
      "id": "reviewer",
      "type": "role",
      "position": {"x": 680, "y": 180},
      "data": {
        "roleName": "Code Reviewer",
        "label": "Reviewer",
        "description": "Reviews PRs and approves merges",
        "guidance": "You are the only one who marks tasks as Done. Provide actionable feedback."
      }
    }
  ],
  "edges": [
    {
      "id": "e1",
      "source": "pm",
      "target": "developer",
      "data": {
        "messageType": "assigns task",
        "senderGuidance": "Message a developer to assign work. Include task ID and branch name.",
        "receiverGuidance": "When PM messages you with a task, claim it and begin work."
      }
    },
    {
      "id": "e2",
      "source": "developer",
      "target": "reviewer",
      "data": {
        "messageType": "PR ready",
        "senderGuidance": "When done and tests pass, message a Reviewer. Set task status to 'Review'.",
        "receiverGuidance": "When developer says PR is ready, review the changes on the branch."
      }
    },
    {
      "id": "e3",
      "source": "reviewer",
      "target": "developer",
      "data": {
        "messageType": "needs changes",
        "senderGuidance": "If changes needed, message developer with specific feedback.",
        "receiverGuidance": "When reviewer requests changes, address feedback and re-submit."
      }
    },
    {
      "id": "e4",
      "source": "reviewer",
      "target": "pm",
      "data": {
        "messageType": "approved",
        "senderGuidance": "When approved, mark task as Done and message PM.",
        "receiverGuidance": "When reviewer confirms completion, assign the next task."
      }
    }
  ]
}
```

### Specialized Team Workflow

Multiple developers with different specializations.

```json
{
  "version": "1.0",
  "nodes": [
    {
      "id": "pm",
      "type": "role",
      "position": {"x": 400, "y": 100},
      "data": {
        "roleName": "Project Manager",
        "label": "PM",
        "guidance": "Route UI tasks to Frontend, API tasks to Backend, DB tasks to Data specialist."
      }
    },
    {
      "id": "frontend",
      "type": "role",
      "position": {"x": 150, "y": 280},
      "data": {
        "roleName": "Frontend Coder",
        "label": "Frontend",
        "guidance": "Handle React components, styling, and UI interactions."
      }
    },
    {
      "id": "backend",
      "type": "role",
      "position": {"x": 400, "y": 280},
      "data": {
        "roleName": "Backend Coder",
        "label": "Backend API",
        "guidance": "Handle REST endpoints, business logic, and services."
      }
    },
    {
      "id": "data",
      "type": "role",
      "position": {"x": 650, "y": 280},
      "data": {
        "roleName": "Backend Coder",
        "label": "Data Layer",
        "guidance": "Handle database schemas, migrations, and data access."
      }
    },
    {
      "id": "reviewer",
      "type": "role",
      "position": {"x": 400, "y": 460},
      "data": {
        "roleName": "Code Reviewer",
        "label": "Reviewer",
        "guidance": "Review all PRs from all three developers."
      }
    }
  ],
  "edges": [
    {
      "id": "e1",
      "source": "pm",
      "target": "frontend",
      "data": {"messageType": "assigns task"}
    },
    {
      "id": "e2",
      "source": "pm",
      "target": "backend",
      "data": {"messageType": "assigns task"}
    },
    {
      "id": "e3",
      "source": "pm",
      "target": "data",
      "data": {"messageType": "assigns task"}
    },
    {
      "id": "e4",
      "source": "frontend",
      "target": "reviewer",
      "data": {"messageType": "PR ready"}
    },
    {
      "id": "e5",
      "source": "backend",
      "target": "reviewer",
      "data": {"messageType": "PR ready"}
    },
    {
      "id": "e6",
      "source": "data",
      "target": "reviewer",
      "data": {"messageType": "PR ready"}
    },
    {
      "id": "e7",
      "source": "reviewer",
      "target": "pm",
      "data": {"messageType": "approved"}
    }
  ]
}
```

### Planning-First Workflow

Human → Architect → PM → Developer → Reviewer chain.

```json
{
  "version": "1.0",
  "nodes": [
    {
      "id": "human",
      "type": "human",
      "position": {"x": 400, "y": 40},
      "data": {
        "humanLabel": "Human",
        "label": "Human",
        "description": "Initiates work requests"
      }
    },
    {
      "id": "architect",
      "type": "role",
      "position": {"x": 400, "y": 180},
      "data": {
        "roleName": "Architect",
        "label": "Architect",
        "guidance": "Receive requests from Human and design technical approach."
      }
    },
    {
      "id": "pm",
      "type": "role",
      "position": {"x": 200, "y": 340},
      "data": {
        "roleName": "Project Manager",
        "label": "PM",
        "guidance": "Break plans into tasks and assign to developers."
      }
    },
    {
      "id": "developer",
      "type": "role",
      "position": {"x": 400, "y": 500},
      "data": {
        "roleName": "Backend Coder",
        "label": "Developer",
        "guidance": "Implement according to the plan."
      }
    },
    {
      "id": "reviewer",
      "type": "role",
      "position": {"x": 600, "y": 340},
      "data": {
        "roleName": "Code Reviewer",
        "label": "Reviewer",
        "guidance": "Review against original plan."
      }
    }
  ],
  "edges": [
    {
      "id": "e1",
      "source": "human",
      "target": "architect",
      "data": {"messageType": "new request"}
    },
    {
      "id": "e2",
      "source": "architect",
      "target": "pm",
      "data": {"messageType": "plan ready"}
    },
    {
      "id": "e3",
      "source": "pm",
      "target": "developer",
      "data": {"messageType": "assigns task"}
    },
    {
      "id": "e4",
      "source": "developer",
      "target": "reviewer",
      "data": {"messageType": "PR ready"}
    },
    {
      "id": "e5",
      "source": "reviewer",
      "target": "developer",
      "data": {"messageType": "needs changes"}
    },
    {
      "id": "e6",
      "source": "reviewer",
      "target": "pm",
      "data": {"messageType": "approved"}
    }
  ]
}
```

---

## Guidance Best Practices

### Node Guidance

Node guidance tells agents **their role** in the workflow:

```json
"guidance": "You are the final quality gate. Review code against the original plan. Provide actionable feedback or approve and mark task as Done."
```

**Good guidance includes:**
- What this participant is responsible for
- Key decisions they make
- What to do when receiving work
- When to escalate

### Edge Guidance

Edge guidance provides **step-by-step handoff instructions**:

```json
"senderGuidance": "When implementation is complete and tests pass, message a Code Reviewer that PR is ready. Set task status to 'Review'.",
"receiverGuidance": "When developer messages you that PR is ready, review the changes on the specified branch."
```

**Good edge guidance includes:**
- **Sender**: When to send, what to include, any status updates
- **Receiver**: What to do when message arrives, expected actions

---

## Validation Rules

### Required Elements

1. Every workflow must have at least 2 nodes
2. Every workflow must have at least 1 edge
3. All edge `source` and `target` must reference existing node IDs
4. Role nodes must have `roleName` matching an existing role
5. Agent nodes must have `agentName` matching an existing agent

### Common Mistakes

| Mistake | Problem | Fix |
|---------|---------|-----|
| Orphan nodes | Node with no edges | Add at least one edge connecting the node |
| Invalid role | `roleName` doesn't exist | Use exact role name from system |
| Missing guidance | No instructions for agents | Add `guidance` to nodes, `senderGuidance`/`receiverGuidance` to edges |
| Disconnected graph | Some nodes can't be reached | Ensure all nodes are connected |
| Circular only | No entry point | Add a trigger, human, or starting role |

---

## Design Checklist

When creating a workflow:

1. **Identify participants** - Who needs to be involved?
2. **Define the flow** - What's the sequence of work?
3. **Add handoffs** - What messages pass between participants?
4. **Include feedback loops** - How do changes get requested?
5. **Add human oversight** - Where do humans need to approve?
6. **Write guidance** - What instructions do agents need?
7. **Validate** - Are all nodes connected? Do roles exist?

---

## Quick Reference

### Node Type Summary

| Type | Use For | Required Data Fields |
|------|---------|---------------------|
| `role` | Any agent with that role | `roleName`, `label` |
| `agent` | Specific named agent | `agentName`, `label` |
| `human` | Human oversight | `humanLabel`, `label` |
| `trigger` | External events | `triggerType`, `label` |

### Minimal Valid Workflow

```json
{
  "version": "1.0",
  "nodes": [
    {"id": "a", "type": "role", "position": {"x": 0, "y": 0}, "data": {"roleName": "Project Manager", "label": "PM"}},
    {"id": "b", "type": "role", "position": {"x": 300, "y": 0}, "data": {"roleName": "Backend Coder", "label": "Dev"}}
  ],
  "edges": [
    {"id": "e1", "source": "a", "target": "b", "data": {"messageType": "assigns task"}}
  ]
}
```
