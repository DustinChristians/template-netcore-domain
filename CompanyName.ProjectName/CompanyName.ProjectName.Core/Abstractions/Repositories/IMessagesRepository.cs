using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Core.Models.ResourceParameters;

namespace CompanyName.ProjectName.Core.Abstractions.Repositories
{
    public interface IMessagesRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesAsync(MessagesResourceParameters parameters);
    }
}
