using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Repository.Data;
using CompanyName.ProjectName.Repository.Entities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.Repository.Repositories
{
    public class BaseRepository<TDomainModel, TEntity> : IBaseRepository<TDomainModel>
        where TEntity : BaseEntity
        where TDomainModel : BaseModel
    {
        protected CompanyNameProjectNameContext Context;
        protected IMapper Mapper;

        public BaseRepository(CompanyNameProjectNameContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<bool> ExistsAsync(int id) => await GetByIdAsync(id) != null;

        public async Task<TDomainModel> GetByIdAsync(int id) => Mapper.Map<TDomainModel>(await FirstOrDefaultAsync(x => x.Id == id));

        public async Task<TDomainModel> GetByGuidAsync(Guid guid) => await FirstOrDefaultAsync(x => x.Guid == guid);

        public async Task<TDomainModel> FirstOrDefaultAsync(Expression<Func<TDomainModel, bool>> domainPredicate)
        {
            var entityPredicate = Mapper.Map<Expression<Func<TEntity, bool>>>(domainPredicate);

            return Mapper.Map<TDomainModel>(await Context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(entityPredicate));
        }

        public async Task<IEnumerable<TDomainModel>> GetAllAsync() => Mapper.Map<IEnumerable<TDomainModel>>(await Context.Set<TEntity>().AsNoTracking().ToListAsync());

        public async Task<IEnumerable<TDomainModel>> GetWhereAsync(Expression<Func<TDomainModel, bool>> domainPredicate)
        {
            var entityPredicate = Mapper.Map<Expression<Func<TEntity, bool>>>(domainPredicate);

            return Mapper.Map<IEnumerable<TDomainModel>>(await Context.Set<TEntity>().AsNoTracking().Where(entityPredicate).ToListAsync());
        }

        public async Task<IEnumerable<TDomainModel>> GetByIdsAsync(IEnumerable<int> ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            return Mapper.Map<IEnumerable<TDomainModel>>(await Context.Set<TEntity>().AsNoTracking().Where(x => ids.Contains(x.Id))
                .ToListAsync());
        }

        public async Task<IEnumerable<TDomainModel>> GetByGuidsAsync(IEnumerable<Guid> guids)
        {
            if (guids == null)
            {
                throw new ArgumentNullException(nameof(guids));
            }

            return Mapper.Map<IEnumerable<TDomainModel>>(await Context.Set<TEntity>().AsNoTracking().Where(x => guids.Contains(x.Guid))
                .ToListAsync());
        }

        public async Task<int> CountAllAsync() => await Context.Set<TEntity>().AsNoTracking().CountAsync();

        public async Task<int> CountWhereAsync(Expression<Func<TDomainModel, bool>> domainPredicate)
        {
            var entityPredicate = Mapper.Map<Expression<Func<TEntity, bool>>>(domainPredicate);

            return await Context.Set<TEntity>().AsNoTracking().CountAsync(entityPredicate);
        }

        public async Task CreateAsync(TDomainModel domainModel)
        {
            var entity = Mapper.Map<TEntity>(domainModel);

            SetCreateMetadata(entity);
            await Context.Set<TEntity>().AddAsync(entity);
            await SaveChangesAsync();

            Mapper.Map(entity, domainModel);
        }

        public async Task BulkCreateAsync(List<TDomainModel> domainModels)
        {
            if (domainModels == null || !domainModels.Any())
            {
                return;
            }

            var entities = Mapper.Map<List<TEntity>>(domainModels);

            foreach (var entity in entities)
            {
                SetCreateMetadata(entity);
            }

            await Context.BulkInsertAsync(entities);
        }

        public void UpdateAsync(TDomainModel domainModel)
        {
            var entity = Mapper.Map<TEntity>(domainModel);

            SetUpdateMetadata(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task BulkUpdateAsync(List<TDomainModel> domainModels)
        {
            if (domainModels == null || !domainModels.Any())
            {
                return;
            }

            var entities = Mapper.Map<List<TEntity>>(domainModels);

            foreach (var entity in entities)
            {
                SetUpdateMetadata(entity);
            }

            await this.Context.BulkUpdateAsync(entities);
        }

        public void DeleteAsync(TDomainModel domainModel)
        {
            Context.Set<TEntity>().Remove(Mapper.Map<TEntity>(domainModel));
        }

        public async Task BulkDeleteAsync(List<TDomainModel> domainModels)
        {
            if (domainModels == null || !domainModels.Any())
            {
                return;
            }

            var entities = Mapper.Map<List<TEntity>>(domainModels);

            await this.Context.BulkDeleteAsync(entities);
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        private void SetCreateMetadata(TEntity entity)
        {
            entity.CreatedBy = 0;
            entity.CreatedOn = DateTime.Now;
            entity.Guid = Guid.NewGuid();
            SetUpdateMetadata(entity, entity.CreatedOn);
        }

        private void SetUpdateMetadata(TEntity entity, DateTime? modified = null)
        {
            entity.ModifiedBy = 0;
            entity.ModifiedOn = modified.HasValue ? modified.Value : DateTime.Now;
        }
    }
}