using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Extensions;
using CompanyName.ProjectName.Core.Models.Repositories;
using CompanyName.ProjectName.Core.Models.ResourceParameters;
using CompanyName.ProjectName.Core.Models.Search;
using CompanyName.ProjectName.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.Repository.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository(CompanyNameProjectNameContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAsync(UsersResourceParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(UsersResourceParameters));
            }

            if (parameters.AllPropertiesAreNull())
            {
                return await GetAllAsync();
            }

            return await Search()
                .Apply(parameters, Context.Users as IQueryable<User>)
                .ToListAsync();
        }

        private SearchMutator<User, UsersResourceParameters> Search()
        {
            var searchMutator = new SearchMutator<User, UsersResourceParameters>();

            searchMutator.AddCondition(
                parameters => !string.IsNullOrWhiteSpace(parameters.Email),
                (messages, parameters) => messages.Where(message => message.Email == parameters.Email));

            searchMutator.AddCondition(
                parameters => !string.IsNullOrWhiteSpace(parameters.FirstName),
                (messages, parameters) => messages.Where(message => message.FirstName == parameters.FirstName));

            searchMutator.AddCondition(
                parameters => !string.IsNullOrWhiteSpace(parameters.LastName),
                (messages, parameters) => messages.Where(message => message.LastName == parameters.LastName));

            searchMutator.AddCondition(
                parameters => !string.IsNullOrWhiteSpace(parameters.SearchQuery),
                (messages, parameters) => messages.Where(u =>
                    u.Email.Contains(parameters.SearchQuery) ||
                    u.FirstName.Contains(parameters.SearchQuery) ||
                    u.LastName.Contains(parameters.SearchQuery)));

            return searchMutator;
        }
    }
}
