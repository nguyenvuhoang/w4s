using LinKit.Core.Mapping;
using LinKit.Json.Runtime;
using O24OpenAPI.ACT.API.Application.Models;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

namespace O24OpenAPI.ACT.API.Application.Mapper;

[MapperContext]
public partial class MappingContext : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder
            .CreateMap<AccountChart, AccountChartCRUDReponseModel>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.MultiValueNameLang, opt => opt.Ignore())
            .ForMember(d => d.IsMasterAccount, opt => opt.Ignore())
            .ForMember(d => d.IsCashAccount, opt => opt.Ignore());
        builder.CreateMap<AccountChartCRUDReponseModel, AccountChart>();
        builder.CreateMap<AccountClearing, AccountClearingViewReponseModel>();
        builder.CreateMap<AccountClearing, AccountClearingCRUDReponseModel>();
    }
}

public static class MapUtils
{
    public static string SerilizeModel(object model)
    {
        return model.ToJson();
    }
}
