using LinKit.Core.Mapping;
using O24OpenAPI.AI.API.Application.Features.ChatClients;
using O24OpenAPI.AI.Domain.AggregatesModel.ChatHistoryAggregate;

namespace O24OpenAPI.AI.API.Application.Mapping;

[MapperContext]
public class ApplicationMapping : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder.CreateMap<
            Domain.AggregatesModel.AskAggreate.AskRequest,
            Domain.AggregatesModel.AskAggreate.AskRequest
        >();
        builder.CreateMap<SaveChatCommand, ChatHistory>();
    }
}
