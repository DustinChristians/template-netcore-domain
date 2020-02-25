using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Models.Repositories;
using CompanyName.ProjectName.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.Repository.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : BaseModel
    {
        protected CompanyNameProjectNameContext Context;

        public BaseRepository(CompanyNameProjectNameContext context)
        {
            Context = context;
        }

        public async Task<T> GetByIdAsync(int id) => await Context.Set<T>().FindAsync(id);
        public async Task<T> GetByGuidAsync(Guid guid) => await FirstOrDefaultAsync(x => x.Guid == guid);

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
            => await Context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await Context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
            => await Context.Set<T>().Where(predicate).ToListAsync();

        public async Task<int> CountAllAsync() => await Context.Set<T>().CountAsync();

        public async Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate)
            => await Context.Set<T>().CountAsync(predicate);
    }
}