using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using crossblog.Domain;
using Microsoft.EntityFrameworkCore;

namespace crossblog.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : BaseEntity, new()
    {
        protected GenericRepository(CrossBlogDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        protected CrossBlogDbContext DbContext { get; }

        public IQueryable<T> Query()
        {
            return DbContext.Set<T>().AsNoTracking();
        }

        public async Task<T> GetAsync(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task InsertAsync(T t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            t.Created_At = DateTime.UtcNow;
            t.Updated_At = DateTime.UtcNow;

            DbContext.Set<T>().Add(t);

            await SaveChangesAsync();
        }

        public async Task UpdateAsync(T t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            t.Updated_At = DateTime.UtcNow;

            DbContext.Entry(t).State = EntityState.Modified;

            await SaveChangesAsync();
        }

        public async Task DeleteAsync(T t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            DbContext.Entry(t).State = EntityState.Deleted;

            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}