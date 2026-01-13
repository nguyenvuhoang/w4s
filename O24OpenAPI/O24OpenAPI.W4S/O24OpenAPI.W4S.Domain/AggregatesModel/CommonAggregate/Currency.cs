namespace O24OpenAPI.W4S.Domain.AggregatesModel.CommonAggregate
{
    using O24OpenAPI.Core.Domain;

    /// <summary>
    /// Defines the <see cref="Currency" />
    /// </summary>
    public partial class Currency : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        public Currency()
        {
        }

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
        /// Gets or sets the MasterName
        /// </summary>
        public string MasterName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the DecimalName
        /// </summary>
        public string DecimalName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the DecimalDigits
        /// </summary>
        public int DecimalDigits { get; set; }

        /// <summary>
        /// Gets or sets the RoundingDigits
        /// </summary>
        public int RoundingDigits { get; set; }

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
