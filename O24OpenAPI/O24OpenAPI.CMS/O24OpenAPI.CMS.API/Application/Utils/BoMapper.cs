using O24OpenAPI.CMS.API.Application.Features.Requests;
using O24OpenAPI.CMS.API.Application.Models.Request;

namespace O24OpenAPI.CMS.API.Application.Utils;

public static class BoMapper
{
    public static BoRequestModel MapToBoRequestModel(BoMappingCoreAPIModel source)
    {
        return new BoRequestModel
        {
            Bo = source
                .Bo.Select(x => new BoRequest
                {
                    Input = new Dictionary<string, object>
                    {
                        { "workflowid", x.Input.WorkFlowId },
                        { "learn_api", x.Input.LearnApi },
                        { "fields", x.Input.Fields },
                    },
                })
                .ToList(),
        };
    }
}
