using O24OpenAPI.Framework.Infrastructure.Mapper;
using O24OpenAPI.NCH.API.Application.Models.Response;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.API.Application.Mapping;

public class AutoMapperProfile : BaseMapperConfiguration
{
    public AutoMapperProfile()
    {
        CreateMap<MailConfig, MailConfigResponse>();
        CreateMap<MailTemplate, MailTemplateResponse>();
    }
}
