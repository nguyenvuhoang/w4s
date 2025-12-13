using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Request;

namespace O24OpenAPI.Web.CMS.Utils;

public static class BoMapper
{
    public static BoRequestModel MapToBoRequestModel(BoMappingCoreAPIModel source)
    {
        return new BoRequestModel
        {
            Bo = source.Bo.Select(x => new BoRequest
            {
                UseMicroservice = x.UseMicroservice,
                Input = new Dictionary<string, object>
                {
                    { "workflowid", x.Input.WorkFlowId },
                    { "learn_api", x.Input.LearnApi },
                    { "fields", x.Input.Fields }
                }
            }).ToList()
        };
    }
}
