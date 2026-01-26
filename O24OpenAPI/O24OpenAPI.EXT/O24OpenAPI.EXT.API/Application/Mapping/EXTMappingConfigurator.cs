using LinKit.Core.Mapping;
using O24OpenAPI.EXT.API.Application.Models;
using O24OpenAPI.EXT.Domain.AggregatesModel.ExchangeRateAggregate;

namespace O24OpenAPI.EXT.API.Application.Mapping;

[MapperContext]
public class EXTMappingConfigurator : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder.CreateMap<ExchangeRate, ExchangeRateResponseModel>();
    }
}
