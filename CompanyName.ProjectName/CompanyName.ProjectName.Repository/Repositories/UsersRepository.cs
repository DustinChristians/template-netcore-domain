using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Core.Models.ResourceParameters;
using CompanyName.ProjectName.Core.Models.Search;
using CompanyName.ProjectName.Repository.Data;
using CompanyName.ProjectName.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.Repository.Repositories
{
    public class UsersRepository : BaseRepository<User, UserEntity>, IUsersRepository
    {
        public UsersRepository(CompanyNameProjectNameContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAsync(UsersResourceParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(UsersResourceParameters));
            }

            var userEntities = await Search()
                .Apply(parameters, Context.Users as IQueryable<UserEntity>)
                .ToListAsync();

            return Mapper.Map<IEnumerable<User>>(userEntities);
        }

        private SearchMutator<UserEntity, UsersResourceParameters> Search()
        {
            var searchMutator = new SearchMutator<UserEntity, UsersResourceParameters>();

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
