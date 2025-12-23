using LinKit.Core.Mapping;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Grpc.CTH;

namespace O24OpenAPI.CTH.API.Application.Mapping;

[MapperContext]
public class ApplicationMapping : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder.CreateMap<CTHUserNotificationModel, GetUserNotificationReply>();
        builder.CreateMap<CTHUserCommandModel, UserCommandReply>();
    }
}
