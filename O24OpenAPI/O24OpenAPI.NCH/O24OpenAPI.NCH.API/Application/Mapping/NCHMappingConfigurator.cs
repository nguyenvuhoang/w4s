using LinKit.Core.Mapping;
using O24OpenAPI.NCH.API.Application.Features.MailConfigs;
using O24OpenAPI.NCH.API.Application.Features.Zalo;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.API.Application.Mapping;

[MapperContext]
public class NCHMappingConfigurator : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder.CreateMap<UpdateMailConfigCommand, UpdateMailConfigResponse>();
        builder.CreateMap<ZaloZNSTemplate, ZaloZNSTemplateResponseModel>();
    }
}
