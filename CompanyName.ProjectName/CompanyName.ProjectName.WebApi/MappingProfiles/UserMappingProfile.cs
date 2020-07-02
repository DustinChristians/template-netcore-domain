using AutoMapper;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.WebApi.Models.User;

namespace CompanyName.ProjectName.WebApi.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ReadUser, User>().ReverseMap();
            CreateMap<CreateUser, User>().ReverseMap();
        }
    }
}
