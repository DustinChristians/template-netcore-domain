using AutoMapper;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Repository.Entities;

namespace CompanyName.ProjectName.Repository.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserEntity, User>().ReverseMap();
        }
    }
}
