using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Abstractions.Services;

namespace CompanyName.ProjectName.Infrastructure.Services
{
    public class MessagesService : IMessagesService
    {
        public MessagesService(IMessagesRepository messagesRepository)
        {
            this.MessagesRepository = messagesRepository;
        }

        public IMessagesRepository MessagesRepository { get; }

        // Add business logic here
    }
}
