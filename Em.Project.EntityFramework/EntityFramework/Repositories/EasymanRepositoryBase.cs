using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Easyman.EntityFramework.Repositories
{
    public abstract class EasymanRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<EmProjectDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected EasymanRepositoryBase(IDbContextProvider<EmProjectDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class EasymanRepositoryBase<TEntity> : EasymanRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected EasymanRepositoryBase(IDbContextProvider<EmProjectDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
