using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Abstractions.Services;

namespace CompanyName.ProjectName.Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        public UsersService(IUsersRepository usersRepository)
        {
            this.UsersRepository = usersRepository;
        }

        public IUsersRepository UsersRepository { get; }

        // Add business logic here
    }
}
