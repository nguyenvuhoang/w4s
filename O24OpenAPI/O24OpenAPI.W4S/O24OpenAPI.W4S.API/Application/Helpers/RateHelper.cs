using O24OpenAPI.W4S.API.Application.Models.Currency;

namespace O24OpenAPI.W4S.API.Application.Helpers;

/// <summary>
/// Defines the <see cref="RateHelper" />
/// </summary>
public static class RateHelper
{
    /// <summary>
    /// Build rate map from request.TransferRates.
    /// Expected: amountBase = amount * rate
    /// baseCurrency is always 1
    /// </summary>
    /// <param name="transferRates">The transferRates<see cref="IList{TransferRateResponseModel}?"/></param>
    /// <param name="baseCurrency">The baseCurrency<see cref="string"/></param>
    /// <returns>The <see cref="Dictionary{string, decimal}"/></returns>
    public static Dictionary<string, decimal> BuildRateMapFromRequest(
        IList<TransferRateResponseModel> transferRates,
        string baseCurrency
    )
    {
        Dictionary<string, decimal> map = new(StringComparer.OrdinalIgnoreCase)
        {
            [baseCurrency] = 1m
        };

        if (transferRates == null || transferRates.Count == 0)
            return map;

        foreach (TransferRateResponseModel r in transferRates)
        {
            if (r == null) continue;

            var ccy = r.CurrencyCode?.Trim();
            if (string.IsNullOrWhiteSpace(ccy)) continue;

            if (ccy.Equals(baseCurrency, StringComparison.OrdinalIgnoreCase))
            {
                map[ccy] = 1m;
                continue;
            }

            var rate = r.Transfer;
            if (!rate.HasValue || rate.Value <= 0m)
                continue;

            map[ccy] = rate.Value;
        }

        return map;
    }



    /// <summary>
    /// Find rate from request-provided map.
    /// </summary>
    public static decimal GetRateToBase(
        string fromCurrency,
        string baseCurrency,
        IReadOnlyDictionary<string, decimal> rateMap
    )
    {
        if (fromCurrency.Equals(baseCurrency, StringComparison.OrdinalIgnoreCase))
            return 1m;

        if (!rateMap.TryGetValue(fromCurrency, out var rate) || rate <= 0m)
            throw new InvalidOperationException(
                $"Missing exchange rate for {fromCurrency}->{baseCurrency}"
            );

        return rate;
    }


    // Convert raw VND quote map -> rateToBase map (1 CCY => baseCurrency)
    public static Dictionary<string, decimal> BuildRateMapByBase(
        string baseCurrency,
        IReadOnlyDictionary<string, decimal> rawVndRateMap
    )
    {
        var baseCcy = (baseCurrency ?? "VND").Trim().ToUpperInvariant();

        // normalize keys
        var vndMap = rawVndRateMap.ToDictionary(
            x => x.Key.Trim().ToUpperInvariant(),
            x => x.Value
        );

        if (baseCcy == "VND")
        {
            var result = new Dictionary<string, decimal>(vndMap, StringComparer.OrdinalIgnoreCase)
            {
                ["VND"] = 1m
            };
            return result;
        }

        if (!vndMap.TryGetValue(baseCcy, out var baseToVnd) || baseToVnd <= 0m)
            throw new InvalidOperationException($"Missing exchange rate for {baseCcy}->VND");

        var rateToBase = vndMap.ToDictionary(
            x => x.Key,
            x => x.Key.Equals(baseCcy, StringComparison.OrdinalIgnoreCase) ? 1m : x.Value / baseToVnd,
            StringComparer.OrdinalIgnoreCase
        );

        // allow VND in statements
        rateToBase["VND"] = 1m / baseToVnd;

        return rateToBase;
    }

    /// <summary>
    /// Convert amount from currencyCode to baseCurrency using rateMap.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="currencyCode"></param>
    /// <param name="baseCurrency"></param>
    /// <param name="rateMap"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static decimal ConvertToBase(
        decimal amount,
        string? currencyCode,
        string baseCurrency,
        IReadOnlyDictionary<string, decimal> rateMap
    )
    {
        var baseCcy = (baseCurrency ?? "VND").Trim().ToUpperInvariant();
        var ccy = string.IsNullOrWhiteSpace(currencyCode) ? baseCcy : currencyCode.Trim().ToUpperInvariant();

        if (ccy.Equals(baseCcy, StringComparison.OrdinalIgnoreCase))
            return amount;

        if (!rateMap.TryGetValue(ccy, out var rate) || rate <= 0m)
            throw new InvalidOperationException($"Missing exchange rate for {ccy}->{baseCcy}");

        return amount * rate;
    }
}
