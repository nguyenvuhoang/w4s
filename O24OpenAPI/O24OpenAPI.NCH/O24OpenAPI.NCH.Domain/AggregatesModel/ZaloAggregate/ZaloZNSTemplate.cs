using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate
{
    [Auditable]
    public partial class ZaloZNSTemplate : BaseEntity
    {
        public string OaId { get; set; } = default!;
        public string TemplateId { get; set; } = default!;   // id Zalo trả về
        public string TemplateCode { get; set; } = default!; // internal code (OTP_LOGIN, OTP_TRANSFER…)

        public string TemplateName { get; set; } = default!;
        public string TemplateType { get; set; } = default!; // OTP / TRANSACTION / INFO

        public string Status { get; set; } = default!;       // PENDING / ENABLE / REJECT
        public string? RejectReason { get; set; }

        public string RequestPayload { get; set; } = default!;
        public string? ResponsePayload { get; set; }
    }
}
