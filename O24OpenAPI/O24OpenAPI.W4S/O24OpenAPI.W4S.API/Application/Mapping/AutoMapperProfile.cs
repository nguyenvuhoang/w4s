using O24OpenAPI.Framework.Infrastructure.Mapper;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Mapping
{
    public class AutoMapperProfile : BaseMapperConfiguration
    {
        public AutoMapperProfile()
        {
            CreateMap<WalletCategory, WalletCategoryResponseModel>();
        }
    }
}
