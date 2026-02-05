//using Abp.Domain.Entities;
//using Abp.Domain.Repositories;
//using inzibackend.Surpath;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using Abp.Domain.Entities;
//using Abp.Domain.Repositories;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;


//namespace inzibackend.EntityFrameworkCore.MyExtensions
//{

//    public interface IRepositoryExtension<T, TKey> : IRepository<T, TKey> where T : class, IEntity<TKey>
//    {
//        Task<T> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes);
//    }

//    public static class RepositoryExtensions
//    {
//        public static async Task<T> GetByIdAsync<T, TKey>(this IRepository<T, TKey> repository, TKey id, params Expression<Func<T, object>>[] includes) where T : class, IEntity<TKey>
//        {
//            var query = repository.GetAll();

//            // Apply includes
//            foreach (var include in includes)
//            {
//                query = query.Include(include);
//            }

//            return await query.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
//        }
//    }
//}
