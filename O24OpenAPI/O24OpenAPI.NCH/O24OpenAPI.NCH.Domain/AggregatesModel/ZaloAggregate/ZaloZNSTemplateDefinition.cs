using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate
{
    public class ZaloZNSTemplateDefinition : BaseEntity
    {
        public string OaId { get; set; } = default!;
        public string TemplateCode { get; set; } = default!;
        public string TemplateName { get; set; } = default!;
        public int TemplateType { get; set; }       // 1..5
        public string Tag { get; set; } = default!; // "1".."3"
        public string LayoutJson { get; set; } = default!;
        public string? ParamsJson { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
