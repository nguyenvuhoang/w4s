using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.API.Application.Models.UserCommandModels;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Infrastructure.Mapper;

namespace O24OpenAPI.CTH.API.Application.Mapping;

public class AutoMapperProfile : BaseMapperConfiguration
{
    public AutoMapperProfile()
    {
        CreateMap<UserAccount, UserAccountResponseModel>();
        CreateModelMap<UserRight, UserRightModel>();
        CreateModelMap<CTHUserCommandModel, MenuInfoModel>();

    }
}
