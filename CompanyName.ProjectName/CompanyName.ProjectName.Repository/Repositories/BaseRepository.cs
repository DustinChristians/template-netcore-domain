using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Models.Repositories;
using CompanyName.ProjectName.Repository.Data;
using EFCore.BulkExtensions;
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

        public async Task<bool> ExistsAsync(int id) => await GetByIdAsync(id) != null;

        public async Task<T> GetByIdAsync(int id) => await Context.Set<T>().FindAsync(id);
        public async Task<T> GetByGuidAsync(Guid guid) => await FirstOrDefaultAsync(x => x.Guid == guid);

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
            => await Context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task AddAsync(T entity)
        {
            SetCreateMetadata(entity);
            await Context.Set<T>().AddAsync(entity);
        }

        public async Task BulkAddAsync(List<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                return;
            }

            foreach (var entity in entities)
            {
                SetCreateMetadata(entity);
            }

            await Context.BulkInsertAsync(entities);
        }

        public void UpdateAsync(T entity)
        {
            SetUpdateMetadata(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await Context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
            => await Context.Set<T>().Where(predicate).ToListAsync();

        public async Task<int> CountAllAsync() => await Context.Set<T>().CountAsync();

        public async Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate)
            => await Context.Set<T>().CountAsync(predicate);

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        private void SetCreateMetadata(T entity)
        {
            entity.CreatedBy = 0;
            entity.CreatedOn = DateTime.Now;
            entity.Guid = Guid.NewGuid();
            SetUpdateMetadata(entity);
        }

        private void SetUpdateMetadata(T entity)
        {
            entity.ModifiedBy = 0;
            entity.ModifiedOn = DateTime.Now;
        }
    }
}