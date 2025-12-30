using LinKit.Core.Mapping;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Report.API.Application.Models.Common;

namespace O24OpenAPI.Report.API.Application.Mapping;

[MapperContext]
public class MappingConfiguration : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder.CreateMap<CTHUserSessionModel, UserSessions>();
    }
}
