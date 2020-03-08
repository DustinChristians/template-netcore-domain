using CompanyName.ProjectName.Core.Abstractions.Repositories;

namespace CompanyName.ProjectName.Core.Abstractions.Services
{
    public interface IUsersService
    {
        public IUsersRepository UsersRepository { get; }
    }
}
