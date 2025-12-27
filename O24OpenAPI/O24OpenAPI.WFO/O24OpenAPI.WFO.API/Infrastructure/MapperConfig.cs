using O24OpenAPI.Framework.Infrastructure.Mapper;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Infrastructure;

public class MapperConfig : BaseMapperConfiguration
{
    public MapperConfig()
    {
        CreateModelMap<WorkflowDef, WorkflowDefSearchResponse>();
    }
}
