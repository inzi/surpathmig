using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;

namespace inzibackend.EntityFrameworkCore.Repositories;

/// <summary>
/// Base class for custom repositories of the application.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
public abstract class inzibackendRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<inzibackendDbContext, TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    protected inzibackendRepositoryBase(IDbContextProvider<inzibackendDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    //add your common methods for all repositories
}

/// <summary>
/// Base class for custom repositories of the application.
/// This is a shortcut of <see cref="inzibackendRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public abstract class inzibackendRepositoryBase<TEntity> : inzibackendRepositoryBase<TEntity, int>
    where TEntity : class, IEntity<int>
{
    protected inzibackendRepositoryBase(IDbContextProvider<inzibackendDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    //do not add any method here, add to the class above (since this inherits it)!!!
}

