using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.O24ACT.Models;

public class GetCurrencyResponse : BaseO24OpenAPIModel
{
    /// <summary>
    /// GetCurrencyResponse constructor
    /// </summary>
    public GetCurrencyResponse() { }

    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// CurrencyId
    /// </summary>
    public string CurrencyId { get; set; }

    /// <summary>
    /// ShortCurrencyId
    /// </summary>
    public string ShortCurrencyId { get; set; }

    /// <summary>
    /// StatusOfCurrency
    /// </summary>
    public string StatusOfCurrency { get; set; }

    /// <summary>
    /// RoundingDigits
    /// </summary>
    public int RoundingDigits { get; set; }

    /// <summary>
    /// DecimalDigits
    /// </summary>
    public int DecimalDigits { get; set; }

    /// <summary>
    /// DecimalDigits
    /// </summary>
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// CurrencyNumber
    /// </summary>
    public int? CurrencyNumber { get; set; }
}
