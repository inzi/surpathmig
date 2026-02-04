# Gotcha: GetAllListAsync() vs GetAll()

[2026-02-02|62b86a8]

## The Problem

`GetAllListAsync()` executes the query immediately and returns `List<T>`. You **cannot** use `.Include()` for eager loading.

## The Mistake

```csharp
// ❌ COMPILER ERROR: Include() not available on List<T>
var users = await _userRepository.GetAllListAsync()
    .Include(u => u.Roles)  // ERROR!
    .ToListAsync();
```

## The Fix

```csharp
// ✅ CORRECT: GetAll() returns IQueryable
var users = await _userRepository.GetAll()
    .Include(u => u.Roles)
    .ToListAsync();
```

## Why

| Method | Returns | Use When |
|--------|---------|----------|
| `GetAll()` | `IQueryable<T>` | Need `.Include()`, `.Where()`, `.OrderBy()`, custom LINQ |
| `GetAllListAsync()` | `Task<List<T>>` | Simple "get everything" with no further querying |

`GetAllListAsync()` is equivalent to `GetAll().ToListAsync()` - it immediately executes.

## When to Use Each

**Use `GetAll()`** (most of the time):
```csharp
// ✅ Need eager loading
_repository.GetAll().Include(x => x.Related).ToListAsync();

// ✅ Need filtering
_repository.GetAll().Where(x => x.IsActive).ToListAsync();

// ✅ Need joins
_repository.GetAll().Join(...).ToListAsync();

// ✅ Read-only queries
_repository.GetAll().AsNoTracking().ToListAsync();
```

**Use `GetAllListAsync()`** (rarely):
```csharp
// ✅ Simple dump of all records, no includes
var allStatuses = await _statusRepository.GetAllListAsync();
```

## Performance Note

For read-only queries, add `.AsNoTracking()`:
```csharp
var users = await _userRepository.GetAll()
    .AsNoTracking()  // Faster for read-only
    .Include(u => u.Roles)
    .ToListAsync();
```

## Related

- (see: n-plus-one-queries) Why `.Include()` matters
- Source: `surpath112/conventions/service-layer.md:179`
- Source: `surpath112/conventions/service-layer.md:194-195`
