using Microsoft.AspNetCore.Mvc;

namespace O24OpenAPI.CMS.API.Models.Zalo
{
    public class ZaloProcessModel : BaseO24OpenAPIModel
    {
        [FromQuery(Name = "oa_id")]
        public string? OaId { get; set; }

        [FromQuery(Name = "code")]
        public string? Code { get; set; }
        [FromQuery(Name = "state")]
        public string? State { get; set; }
    }
}
