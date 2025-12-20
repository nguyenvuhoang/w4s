using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Response;
using O24OpenAPI.Framework.Infrastructure.Mapper;

namespace O24OpenAPI.O24NCH.Infrastructure.Mapper;

public class NCHMapperConfiguration : BaseMapperConfiguration
{
    public NCHMapperConfiguration()
    {
        CreateMap<MailConfig, MailConfigResponse>();
        CreateMap<MailTemplate, MailTemplateResponse>();
    }
}
