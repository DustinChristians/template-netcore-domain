using AutoMapper;
using CompanyName.ProjectName.WebApi.Models;
using RepositoryModel = CompanyName.ProjectName.Core.Models.Repositories.User;
using UserForGetting = CompanyName.ProjectName.WebApi.Models.User;

namespace CompanyName.ProjectName.WebApi.MappingProfiles
{
    class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RepositoryModel, UserForGetting>();
            CreateMap<UserForCreation, RepositoryModel>();
        }
    }
}
