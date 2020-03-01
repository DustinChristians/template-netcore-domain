using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Models.Repositories;
using CompanyName.ProjectName.Core.Models.ResourceParameters;
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

            if (string.IsNullOrWhiteSpace(parameters.Category)
                && string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                return await GetAllAsync();
            }

            var collection = Context.Messages as IQueryable<Message>;

            if (!string.IsNullOrWhiteSpace(parameters.Category))
            {
                collection = collection.Where(x => x.Category == parameters.Category.Trim());
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                var searchQuery = parameters.SearchQuery.Trim();

                collection = collection.Where(x => x.Category.Contains(searchQuery)
                    || x.Text.Contains(searchQuery));
            }

            return await collection.ToListAsync();
        }
    }
}
