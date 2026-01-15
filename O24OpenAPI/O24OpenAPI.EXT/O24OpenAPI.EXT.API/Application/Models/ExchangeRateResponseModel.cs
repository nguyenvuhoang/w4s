using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.EXT.API.Application.Models
{
    public class ExchangeRateResponseModel : BaseO24OpenAPIModel
    {
        /// <summary>
        /// Thời điểm tỷ giá (theo Vietcombank, convert UTC)
        /// </summary>
        public DateTime RateDateUtc { get; set; }

        /// <summary>
        /// Mã tiền tệ (USD, EUR, JPY…)
        /// </summary>
        public string CurrencyCode { get; set; } = default!;

        /// <summary>
        /// Tên tiền tệ (US DOLLAR, EURO…)
        /// </summary>
        public string CurrencyName { get; set; } = default!;

        /// <summary>
        /// Giá mua tiền mặt
        /// </summary>
        public decimal? Buy { get; set; }

        /// <summary>
        /// Giá chuyển khoản
        /// </summary>
        public decimal? Transfer { get; set; }

        /// <summary>
        /// Giá bán
        /// </summary>
        public decimal? Sell { get; set; }

        /// <summary>
        /// Nguồn dữ liệu (Vietcombank)
        /// </summary>
        public string? Source { get; set; }
    }
}
