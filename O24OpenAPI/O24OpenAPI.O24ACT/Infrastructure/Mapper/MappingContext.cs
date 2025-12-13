using Linh.JsonKit.Json;
using LinKit.Core.Mapping;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;

namespace O24OpenAPI.O24ACT.Infrastructure.Mapper;

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
