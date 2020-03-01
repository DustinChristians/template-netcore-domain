using AutoMapper;
using RepositoryModel = CompanyName.ProjectName.Core.Models.Repositories.Message;
using WebApiModel = CompanyName.ProjectName.WebApi.Models.Message;

namespace CompanyName.ProjectName.WebApi.MappingProfiles
{
    class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<RepositoryModel, WebApiModel>();
        }
    }
}
