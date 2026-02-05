# Surpath Tools Documentation

## Overview
Administrative tools for managing the review queue system, allowing administrators to track and process pending items requiring manual review or approval within the compliance system.

## Contents

### Files

#### ISurpathToolReviewQueueAppService.cs
- **Purpose**: Interface defining review queue management operations
- **Key Methods** (inferred from interface):
  - Queue retrieval and filtering
  - Queue item assignment to reviewers
  - Queue item status updates
  - Priority management
  - Batch processing capabilities

#### SurpathToolReviewQueueAppService.cs
- **Purpose**: Implementation of review queue management
- **Key Features** (typical implementation):
  - Retrieve pending review items
  - Assign items to specific reviewers
  - Update review status (in progress, completed, rejected)
  - Filter by type, priority, assignment, age
  - Track reviewer workload
  - Generate review queue reports
- **Use Cases**:
  - Document approval workflows
  - Compliance record verification
  - Drug test result review
  - Background check processing
  - Exception handling

### Key Components

**Review Queue Management:**
- Queue item creation from various sources
- Assignment to reviewers (manual or automatic)
- Status tracking (pending, assigned, in review, completed)
- Priority levels (high, medium, low)
- Age tracking (time in queue)
- SLA monitoring (review within X days)

**Reviewer Operations:**
- View assigned items
- Claim unassigned items
- Complete review with notes
- Escalate items
- Return items to queue

**Administrative Functions:**
- View all queue items
- Reassign items between reviewers
- Monitor queue metrics (items per reviewer, average time)
- Generate queue performance reports

### Dependencies
- **External**:
  - ABP Framework (Application Services, Authorization)
  - Entity Framework Core
- **Internal**:
  - Review queue domain entities
  - User/role management
  - Notification system
  - Audit logging

## Architecture Notes
- **Pattern**: Application Service with interface abstraction
- **Authorization**: Role-based (administrators, reviewers)
- **Queue Management**: FIFO with priority override
- **Assignment Strategy**: Manual, automatic, or round-robin

## Business Logic

### Queue Item Creation
Items added to queue from:
1. Document uploads awaiting approval
2. Compliance records needing verification
3. Drug test results requiring review
4. Exception cases flagged by automated rules
5. User-reported issues

### Assignment Logic
- **Automatic**: Items assigned based on workload balancing
- **Manual**: Admin assigns specific items to specific reviewers
- **Self-Assignment**: Reviewers claim items from unassigned pool
- **Priority-Based**: High-priority items assigned first

### Review Workflow
1. Item enters queue (pending status)
2. Item assigned to reviewer (assigned status)
3. Reviewer opens item (in review status)
4. Reviewer completes review with decision (completed status)
5. Automated actions triggered based on decision
6. Item removed from queue or escalated

### Status Transitions
- **Pending** → **Assigned**: Admin or automatic assignment
- **Assigned** → **In Review**: Reviewer opens item
- **In Review** → **Completed**: Review finalized
- **In Review** → **Pending**: Returned to queue (e.g., missing information)
- **Any** → **Escalated**: Flagged for higher-level review

## Usage Across Codebase

### Primary Consumers
- **Admin Dashboard**: Queue metrics and overview
- **Reviewer Interface**: Assigned items list
- **Document Approval**: Route documents to queue
- **Compliance Services**: Add items needing manual review
- **Exception Handlers**: Queue unusual cases

### Integration Points
- **Notification System**: Alert reviewers of new assignments
- **Audit System**: Log all review decisions
- **Compliance Evaluation**: Trigger recalculation after approval
- **Workflow Engine**: Automate actions based on review outcomes

### Typical Usage
```csharp
// Get items assigned to current reviewer
var myQueue = await _reviewQueueService.GetMyQueueItems();

// Claim an unassigned item
await _reviewQueueService.ClaimItem(itemId);

// Complete review with approval
await _reviewQueueService.CompleteReview(itemId,
    ReviewDecision.Approved,
    "Document verified and approved");
```

## Review Item Types

### Document Reviews
- Student-uploaded compliance documents
- Verification of authenticity
- Expiration date validation
- Quality assurance

### Test Results
- Drug test results from labs
- Medical examination reports
- Background check results
- Verification of provider credentials

### Exception Cases
- Missing information
- Conflicting data
- Policy violations
- System-detected anomalies

## Performance Considerations
- **Queue Size**: Monitor queue depth to prevent backlog
- **Assignment Balance**: Distribute workload evenly
- **Priority Handling**: High-priority items processed quickly
- **Aging Reports**: Identify items stuck in queue too long
- **Caching**: Cache reviewer assignments for performance

## Security Considerations
- **Role-Based Access**: Only authorized reviewers access queue
- **Item Privacy**: Reviewers see only assigned items (or all if admin)
- **Audit Trail**: All review actions logged with timestamp
- **Data Protection**: Sensitive information protected during review
- **Tenant Isolation**: Queue items scoped to tenant

## Metrics and Reporting

### Queue Metrics
- Total items in queue
- Items by status (pending, assigned, in review)
- Average time in queue
- Items per reviewer
- Completion rate by reviewer
- SLA compliance (% reviewed within target time)

### Performance Indicators
- Items reviewed per day
- Average review time
- Items returned to queue (rework rate)
- Escalation rate
- Reviewer workload balance

## Best Practices
- **Clear Assignment**: Ensure reviewers know what they're responsible for
- **SLA Targets**: Define and monitor review time targets
- **Training**: Train reviewers on review criteria
- **Documentation**: Require notes on all review decisions
- **Escalation Path**: Clear process for handling difficult cases
- **Workload Balance**: Monitor and balance reviewer assignments
- **Queue Prioritization**: Use priority levels appropriately

## Extension Points
- Add review templates with checklists
- Implement peer review (second reviewer)
- Add reviewer performance dashboards
- Support review categories/specializations
- Implement automatic routing based on item type
- Add review quality scoring
- Support collaborative reviews (multiple reviewers)
- Integrate with external review systems

## Common Workflows

### Daily Reviewer Workflow
1. Log in and view assigned items
2. Sort by priority or age
3. Open item and review details
4. Make decision (approve, reject, return, escalate)
5. Add notes documenting decision
6. Submit review
7. Item removed from queue or routed appropriately

### Admin Workflow
1. Monitor queue metrics dashboard
2. Identify bottlenecks or backlog
3. Reassign items to balance workload
4. Escalate stuck items
5. Generate reports for management
6. Adjust assignment rules if needed

## Troubleshooting
- **Items Stuck**: Check for missing reviewer assignments
- **Unbalanced Workload**: Review assignment algorithm
- **Slow Reviews**: Analyze average review times
- **High Return Rate**: Review rejection reasons
- **Many Escalations**: May indicate complex policy issues