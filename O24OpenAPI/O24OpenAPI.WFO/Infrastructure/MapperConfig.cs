using O24OpenAPI.Web.Framework.Infrastructure.Mapper;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Models;

namespace O24OpenAPI.WFO.Infrastructure;

public class MapperConfig : BaseMapperConfiguration
{
    public MapperConfig()
    {
        CreateModelMap<WorkflowDef, WorkflowDefSearchResponse>();
    }
}
