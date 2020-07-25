using AutoMapper;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Repository.Entities;

namespace CompanyName.ProjectName.Repository.MappingProfiles
{
    public class SettingMappingProfile : Profile
    {
        public SettingMappingProfile()
        {
            CreateMap<SettingEntity, Setting>().ReverseMap();
        }
    }
}
