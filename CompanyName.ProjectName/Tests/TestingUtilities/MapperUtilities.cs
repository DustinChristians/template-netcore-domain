using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;

namespace CompanyName.ProjectName.TestUtilities
{
    public static class MapperUtilities
    {
        public static IMapper GetTestMapper()
        {
            var mappingConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddMaps("CompanyName.ProjectName.Infrastructure");
                    cfg.AddMaps("CompanyName.ProjectName.Repository");
                    cfg.AddExpressionMapping();
                });

            var mapper = mappingConfig.CreateMapper();

            return mapper;
        }
    }
}
