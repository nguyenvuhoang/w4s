using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Infrastructure.Mapper;

namespace O24OpenAPI.CTH.API.Application.Mapping;

public class AutoMapperProfile : BaseMapperConfiguration
{
    public AutoMapperProfile()
    {
        CreateMap<UserAccount, UserAccountResponseModel>();
    }
}
