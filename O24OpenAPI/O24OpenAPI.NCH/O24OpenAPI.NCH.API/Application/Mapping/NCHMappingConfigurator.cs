using LinKit.Core.Mapping;
using O24OpenAPI.NCH.API.Application.Features.MailConfigs;

namespace O24OpenAPI.NCH.API.Application.Mapping;

[MapperContext]
public class NCHMappingConfigurator : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder.CreateMap<UpdateMailConfigCommand, UpdateMailConfigResponse>();
    }
}
