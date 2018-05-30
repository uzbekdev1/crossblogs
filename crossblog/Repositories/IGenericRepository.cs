using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace crossblog.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> GetAsync(int? id);

        IQueryable<T> Query();
         
        Task InsertAsync(T t);

        Task UpdateAsync(T t);

        Task DeleteAsync(T t);

        Task SaveChangesAsync();
    }
}