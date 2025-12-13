using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Web.Report.Models.Common;
using Riok.Mapperly.Abstractions;

namespace O24OpenAPI.Web.Report.Mapper;

[Mapper(
RequiredMappingStrategy = RequiredMappingStrategy.None,
RequiredEnumMappingStrategy = RequiredMappingStrategy.None
)]
public static partial class UserSessionMapper
{
    public static partial UserSessions MapToUserSessions(this CTHUserSessionModel model);
}
