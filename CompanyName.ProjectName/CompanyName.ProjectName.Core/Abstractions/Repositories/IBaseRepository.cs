using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Models.Repositories;

namespace CompanyName.ProjectName.Core.Abstractions.Repositories
{
    public interface IBaseRepository<T>
        where T : BaseModel
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByGuidAsync(Guid guid);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

        Task<int> CountAllAsync();
        Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate);
    }
}
