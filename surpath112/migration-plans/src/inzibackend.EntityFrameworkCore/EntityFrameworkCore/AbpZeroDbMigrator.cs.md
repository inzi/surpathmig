# Modified
## Filename
AbpZeroDbMigrator.cs
## Relative Path
inzibackend.EntityFrameworkCore\EntityFrameworkCore\AbpZeroDbMigrator.cs
## Language
C#
## Summary
The modified file defines a class AbpZeroDbMigrator that extends AbpZeroDbMigrator<inzibackendDbContext>. The constructor takes an IUnitOfWorkManager, an IDbPerTenantConnectionStringResolver, and an dbContextResolver as parameters. It uses the base constructor to initialize these dependencies.
## Changes
The modified file adds the using directive 'using Abp.Domain.Uow;' which was not present in the unmodified version.
## Purpose
The class is part of dependency injection setup for database configurations within an ASP.NET Zero application.
