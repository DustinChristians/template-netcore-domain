using AutoMapper;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.WebApi.Models.Message;

namespace CompanyName.ProjectName.WebApi.MappingProfiles
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<ReadMessage, Message>().ReverseMap();
            CreateMap<CreateMessage, Message>().ReverseMap();
            CreateMap<UpdateMessage, Message>().ReverseMap();
        }
    }
}
