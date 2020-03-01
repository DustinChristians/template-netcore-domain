using CompanyName.ProjectName.Core.Abstractions.Repositories;

namespace CompanyName.ProjectName.Core.Abstractions.Services
{
    public interface IMessagesService
    {
        public IMessagesRepository MessagesRepository { get; }
    }
}
