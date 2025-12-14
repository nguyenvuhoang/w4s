using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Domain.Configuration;
using O24OpenAPI.Web.Framework.Domain;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;

namespace O24OpenAPI.Web.Framework.Infrastructure.Mapper;

/// <summary>
/// The fw domain mapper class
/// </summary>
/// <seealso cref="BaseMapperConfiguration"/>
public class FWDomainMapper : BaseMapperConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FWDomainMapper"/> class
    /// </summary>
    public FWDomainMapper()
    {
        CreateModelMap<O24OpenAPIService, O24OpenAPIServiceCreateModel>();
        CreateModelMap<O24OpenAPIService, O24OpenAPIServiceUpdateModel>();
        CreateModelMap<O24OpenAPIService, O24OpenAPIServiceSearchResponse>();
        CreateModelMap<Setting, SettingUpdateModel>();
        CreateModelMap<Setting, SettingCreateModel>();
        CreateModelMap<Setting, SettingSearchResponse>();
        CreateModelMap<Setting, SettingSearchModel>();
        CreateModelMap<StoredCommand, StoredCommandResponse>();
        CreateModelMap<C_CODELIST, CodeListResponseModel>();
    }
}
