using O24OpenAPI.Framework.Infrastructure.Mapper;
using O24OpenAPI.W4S.API.Application.Features.WalletBudgets;
using O24OpenAPI.W4S.API.Application.Helpers;
using O24OpenAPI.W4S.API.Application.Models.Currency;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.CommonAggregate;

namespace O24OpenAPI.W4S.API.Application.Mapping;

public class AutoMapperProfile : BaseMapperConfiguration
{
    public AutoMapperProfile()
    {
        CreateMap<WalletCategory, WalletCategoryResponseModel>()
            .ForMember(
                dest => dest.WebIcon,
                opt => opt.MapFrom(src => IconHelper.ToFaIcon(src.Icon))
            );

        CreateModelMap<WalletBudget, GetWalletBudgetsByWalletModel>();
        CreateModelMap<WalletTransaction, WalletTransactionResponseModel>();
        CreateModelMap<Currency, CurrencyResponseModel>();
    }
}
