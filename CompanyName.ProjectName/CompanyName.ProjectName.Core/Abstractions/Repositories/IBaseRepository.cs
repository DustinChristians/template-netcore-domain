using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Models.Domain;

namespace CompanyName.ProjectName.Core.Abstractions.Repositories
{
    public interface IBaseRepository<TDomainModel>
        where TDomainModel : BaseModel
    {
        Task<bool> ExistsAsync(int id);
        Task<TDomainModel> GetByIdAsync(int id);
        Task<TDomainModel> GetByGuidAsync(Guid guid);
        Task<TDomainModel> FirstOrDefaultAsync(Expression<Func<TDomainModel, bool>> predicate);

        Task<IEnumerable<TDomainModel>> GetAllAsync();
        Task<IEnumerable<TDomainModel>> GetWhereAsync(Expression<Func<TDomainModel, bool>> predicate);
        Task<IEnumerable<TDomainModel>> GetByIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<TDomainModel>> GetByGuidsAsync(IEnumerable<Guid> guids);

        Task<int> CountAllAsync();
        Task<int> CountWhereAsync(Expression<Func<TDomainModel, bool>> predicate);

        Task CreateAsync(TDomainModel entity);
        Task BulkCreateAsync(List<TDomainModel> entities);

        void UpdateAsync(TDomainModel entity);
        Task BulkUpdateAsync(List<TDomainModel> entities);

        void DeleteAsync(TDomainModel entity);
        Task BulkDeleteAsync(List<TDomainModel> entities);

        Task SaveChangesAsync();
    }
}
