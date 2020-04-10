using AutoMapper;
using CompanyName.ProjectName.WebApi.Models;
using MessageForGetting = CompanyName.ProjectName.WebApi.Models.Message;
using RepositoryModel = CompanyName.ProjectName.Core.Models.Repositories.Message;

namespace CompanyName.ProjectName.WebApi.MappingProfiles
{
    class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<RepositoryModel, MessageForGetting>();
            CreateMap<MessageForCreation, RepositoryModel>();
            CreateMap<MessageForUpdate, RepositoryModel>();
        }
    }
}
