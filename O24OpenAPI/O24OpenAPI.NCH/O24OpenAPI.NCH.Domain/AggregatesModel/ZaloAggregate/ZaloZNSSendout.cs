namespace O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate
{
    using O24OpenAPI.Core.Domain;

    /// <summary>
    /// Defines the <see cref="ZaloZNSSendout" />
    /// </summary>
    public class ZaloZNSSendout : BaseEntity
    {
        /// <summary>
        /// Gets or sets the RefId
        /// Idempotency key - unique per send request
        /// </summary>
        public string RefId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the OaId
        /// OA Id that owns the token / sends the ZNS
        /// </summary>
        public string OaId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Phone
        /// Receiver phone number (E.164 or local format depending on your policy)
        /// </summary>
        public string Phone { get; set; } = default!;

        /// <summary>
        /// Gets or sets the TemplateId
        /// ZNS template id/code
        /// </summary>
        public string TemplateId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the PayloadJson
        /// JSON payload sent to Zalo (optional: mask OTP if needed)
        /// </summary>
        public string? PayloadJson { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// SUCCESS / FAIL / PENDING (if async)
        /// </summary>
        public string Status { get; set; } = "PENDING";

        /// <summary>
        /// Gets or sets the ZaloMsgId
        /// Zalo response message id / request id (if provided)
        /// </summary>
        public string? ZaloMsgId { get; set; }

        /// <summary>
        /// Gets or sets the ErrorCode
        /// Error code returned by Zalo (if any)
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the ErrorMessage
        /// Error message returned by Zalo (if any)
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the AttemptCount
        /// Number of attempts (retry tracking)
        /// </summary>
        public int AttemptCount { get; set; }

        /// <summary>
        /// Gets or sets the TraceId
        /// Correlation / trace id for observability (optional)
        /// </summary>
        public string? TraceId { get; set; }
    }
}
