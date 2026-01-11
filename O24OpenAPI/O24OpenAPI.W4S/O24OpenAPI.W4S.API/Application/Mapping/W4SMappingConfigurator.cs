using LinKit.Core.Mapping;
using O24OpenAPI.W4S.API.Application.Helpers;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Mapping;

[MapperContext]
public class W4SMappingConfigurator : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder
            .CreateMap<WalletCategory, WalletCategoryResponseModel>()
            .ForMember(
                dest => dest.WebIcon,
                opt => opt.MapFrom(src => IconHelper.ToFaIcon(src.Icon))
            );
        builder
            .CreateMap<WalletBudget, WalletBudgetResponseModel>()
            .ForMember(d => d.BudgetId, o => o.MapFrom(s => s.Id));
    }
}
