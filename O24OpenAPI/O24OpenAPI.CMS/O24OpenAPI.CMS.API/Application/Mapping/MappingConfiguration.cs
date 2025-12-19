using LinKit.Core.Mapping;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.Domain.AggregateModels.AppAggregate;

namespace O24OpenAPI.CMS.API.Application.Mapping;

[MapperContext]
public class MappingConfiguration : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder.CreateMap<LearnApi, LearnApiModel>();
    }
}
