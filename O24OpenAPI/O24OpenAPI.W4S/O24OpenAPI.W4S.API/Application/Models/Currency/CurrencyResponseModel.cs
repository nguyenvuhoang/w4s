namespace O24OpenAPI.W4S.API.Application.Models.Currency
{
    public class CurrencyResponseModel : BaseO24OpenAPIModel
    {
        /// <summary>
        /// Gets or sets the CurrencyId
        /// </summary>
        public string CurrencyId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ShortCurrencyId
        /// </summary>
        public string ShortCurrencyId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CurrencyName
        /// </summary>
        public string CurrencyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CurrencyNumber
        /// </summary>
        public long? CurrencyNumber { get; set; }

        /// <summary>
        /// Gets or sets the StatusOfCurrency
        /// </summary>
        public string StatusOfCurrency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the DisplayOrder
        /// </summary>
        public int? DisplayOrder { get; set; }
    }
}
