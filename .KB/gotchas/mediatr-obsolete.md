# Gotcha: MediatR Pattern No Longer Best Practice

[2026-02-02|62b86a8]

## The Issue

MediatR library is going commercial. It should **not** be considered best practice for MAUI or new projects.

## What This Means

**Don't**:
- Add MediatR to new projects
- Use CQRS pattern via MediatR
- Refactor existing code to use MediatR
- Recommend MediatR in documentation

**Do Instead**:
- Use direct service calls
- Keep application services simple
- Follow existing ABP patterns

## Example

```csharp
// ❌ AVOID: MediatR pattern
var result = await _mediator.Send(new GetUserQuery { UserId = 123 });

// ✅ PREFER: Direct service call
var result = await _userAppService.GetUser(new EntityDto { Id = 123 });
```

## Context

User instruction states:
> "The mediatr pattern / library is going commercial - so it should not be considered best practice for MAUI any more."

## Related

- (see: architecture/clean-architecture-layers) Standard ABP service pattern
- Source: `.KB/mandatory.md` (user instruction)
