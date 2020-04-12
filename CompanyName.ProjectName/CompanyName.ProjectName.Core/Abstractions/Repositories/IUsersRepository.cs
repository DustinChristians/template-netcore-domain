using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Core.Models.ResourceParameters;

namespace CompanyName.ProjectName.Core.Abstractions.Repositories
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetUsersAsync(UsersResourceParameters parameters);
    }
}
