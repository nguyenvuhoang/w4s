using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Models.Roles;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper;

namespace O24OpenAPI.ControlHub.Infrastructure.Mapper;

/// <summary>
/// The auto mapper configuration class
/// </summary>
/// <seealso cref="BaseMapperConfiguration"/>
public class AutoMapperConfiguration : BaseMapperConfiguration
{
    /// <summary>
    /// Deserializes the roles using the specified roles
    /// </summary>
    /// <param name="roles">The roles</param>
    /// <returns>A list of string</returns>
    private static List<string> DeserializeRoles(string roles)
    {
        return System.Text.Json.JsonSerializer.Deserialize<List<string>>(roles);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMapperConfiguration"/> class
    /// </summary>
    public AutoMapperConfiguration()
    {
        CreateMap<UserSession, UserSessionModel>()
            .ForMember(
                dest => dest.ChannelRoles,
                opt => opt.MapFrom(src => DeserializeRoles(src.ChannelRoles))
            );
        CreateModelMap<UserRight, UserRightModel>();
        var map = CreateMap<UpdateUserRequestModel, UserAccount>();
        map.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateModelMap<Calendar, CalendarSearchResponseModel>();
        CreateModelMap<UserAccount, UserAccountResponseModel>();
    }
}
