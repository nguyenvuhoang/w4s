using LinKit.Core.Mapping;

namespace O24OpenAPI.AI.API.Application.Mapping
{
    [MapperContext]
    public class ApplicationMapping : IMappingConfigurator
    {
        public void Configure(IMapperConfigurationBuilder builder)
        {
            builder.CreateMap<Domain.AggregatesModel.AskAggreate.AskRequest, Domain.AggregatesModel.AskAggreate.AskRequest>();
        }
    }
}
