using LinKit.Core.Mapping;
using O24OpenAPI.ACT.API.Application.Features.AccountCharts;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;
using O24OpenAPI.ACT.Infrastructure.Repositories;

namespace O24OpenAPI.ACT.API.Application.Mapping;

[MapperContext]
public class MappingConfigurator : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder.CreateMap<CreateAccountChartCommand, AccountChart>();
        builder.CreateMap<DeleteAccountChartCommand, AccountChart>();
        builder.CreateMap<AccountBalanceRepository, AccountChart>();
    }
}
