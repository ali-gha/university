using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.DataLayer.Context;
using University.ServiceLayer.Abstracts;
using System.Linq.Dynamic.Core;

namespace University.ServiceLayer.Concretes
{
    public class GenericService<T> : IGenericService<T> where T : class, new()
    {
        private readonly UniversityContext _ctx;
        private readonly DbSet<T> dbSet;
        public GenericService(UniversityContext ctx)
        {
            _ctx = ctx;
            if (_ctx == null)
                throw new ArgumentNullException(nameof(UniversityContext));
            dbSet = _ctx.Set<T>();
        }
        public virtual async Task<int> CreateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await dbSet.AddAsync(entity);
            return await _ctx.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            dbSet.Remove(entity);
            return await _ctx.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
             if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            dbSet.Update(entity);
            return await _ctx.SaveChangesAsync();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }
        public virtual async Task<T> GetByIdAsyncAsNotTracked(int id)
        {
            return await dbSet.AsNoTracking().Where($"Id = {id}").SingleOrDefaultAsync();
        }
    }
}