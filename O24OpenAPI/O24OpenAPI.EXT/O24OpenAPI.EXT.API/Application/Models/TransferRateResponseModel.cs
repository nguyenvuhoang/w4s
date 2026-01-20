using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.EXT.API.Application.Models
{
    public class TransferRateResponseModel : BaseO24OpenAPIModel
    {   /// <summary>
        /// Mã tiền tệ (USD, EUR, JPY…)
        /// </summary>
        public string CurrencyCode { get; set; } = default!;

        /// <summary>
        /// Giá chuyển khoản
        /// </summary>
        public decimal? Transfer { get; set; }
    }
}
