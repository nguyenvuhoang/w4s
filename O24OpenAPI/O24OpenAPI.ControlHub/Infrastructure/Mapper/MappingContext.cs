using Linh.JsonKit.Json;
using LinKit.Core.Mapping;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Grpc.CTH;

namespace O24OpenAPI.ControlHub.Infrastructure.Mapper;

[MapperContext]
public partial class MappingContext : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder
            .CreateMap<CTHUserNotificationModel, GetUserNotificationReply>();
        builder.CreateMap<CTHUserCommandModel, UserCommandReply>();
    }
}

public static class MapUtils
{
    public static string SerilizeModel(object model)
    {
        return model.ToJson();
    }
}
