using AutoMapper;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Repository.Entities;

namespace CompanyName.ProjectName.Repository.MappingProfiles
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<MessageEntity, Message>().ReverseMap();
        }
    }
}
