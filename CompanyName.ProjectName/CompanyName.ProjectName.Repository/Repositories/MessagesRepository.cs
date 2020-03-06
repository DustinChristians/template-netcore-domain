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
    public class MessagesRepository : BaseRepository<Message>, IMessagesRepository
    {
        public MessagesRepository(CompanyNameProjectNameContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(MessagesResourceParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(MessagesResourceParameters));
            }

            if (parameters.AllPropertiesAreNull())
            {
                return await GetAllAsync();
            }

            return await Search()
                .Apply(parameters, Context.Messages as IQueryable<Message>)
                .ToListAsync();
        }

        private SearchMutator<Message, MessagesResourceParameters> Search()
        {
            var searchMutator = new SearchMutator<Message, MessagesResourceParameters>();

            searchMutator.AddCondition(
                parameters => !string.IsNullOrWhiteSpace(parameters.Category),
                (messages, parameters) => messages.Where(message => message.Category == parameters.Category));

            searchMutator.AddCondition(
                parameters => !string.IsNullOrWhiteSpace(parameters.SearchQuery),
                (messages, parameters) => messages.Where(u => u.Category.Contains(parameters.SearchQuery) || u.Text.Contains(parameters.SearchQuery)));

            return searchMutator;
        }
    }
}
