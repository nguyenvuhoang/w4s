using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;
using O24OpenAPI.EXT.Domain.AggregatesModel.ExchangeRateAggregate;
using O24OpenAPI.EXT.Infrastructure.Configurations;
using O24OpenAPI.Framework.Extensions;
using System.Globalization;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace O24OpenAPI.EXT.API.Application.BackgroundJobs;

[BackgroundJob("ScanExchangeRate")]
public class ScanExchangeRateJob : BackgroundJobCommand { }


[CqrsHandler]
public class ScanExchangeRateJobHandler(
        IHttpClientFactory httpClientFactory,
        ILogger<ScanExchangeRateJobHandler> logger,
        EXTSetting extSetting,
        IExchangeRateRepository exchangeRateRepository
    ) :
    ICommandHandler<ScanExchangeRateJob>
{
    public async Task<Unit> HandleAsync(
        ScanExchangeRateJob request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {

            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
            );
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[EXT] ScanExchangeRateJob Starting::" + localTime);
            Console.ForegroundColor = ConsoleColor.White;

            var vcbUrl = extSetting.VcbUrl;
            if (string.IsNullOrWhiteSpace(vcbUrl))
                throw new InvalidOperationException("EXTSetting.VcbUrl is empty.");

            var xml = await FetchXmlAsync(vcbUrl, cancellationToken);

            var parsed = ParseVietcombankXml(xml);

            var inserted = 0;
            var updated = 0;

            foreach (var item in parsed.Items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var existing = await exchangeRateRepository.GetByRateDateAndCurrencyAsync(
                    parsed.RateDateUtc,
                    item.CurrencyCode,
                    cancellationToken
                );

                if (existing is null)
                {
                    var entity = new ExchangeRate
                    {
                        RateDateUtc = parsed.RateDateUtc,
                        CurrencyCode = item.CurrencyCode,
                        CurrencyName = item.CurrencyName,
                        Buy = item.Buy,
                        Transfer = item.Transfer,
                        Sell = item.Sell,
                        Source = parsed.Source,
                        CreatedOnUtc = DateTime.UtcNow,
                        UpdatedOnUtc = DateTime.UtcNow
                    };

                    await exchangeRateRepository.Insert(entity);
                    inserted++;
                }
                else
                {
                    existing.CurrencyName = item.CurrencyName;
                    existing.Buy = item.Buy;
                    existing.Transfer = item.Transfer;
                    existing.Sell = item.Sell;
                    existing.Source = parsed.Source;
                    existing.UpdatedOnUtc = DateTime.UtcNow;

                    await exchangeRateRepository.Update(existing);
                    updated++;
                }
            }
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync("ScanExchangeRateJob Exception:: " + ex.Message);
        }
        return Unit.Value;
    }

    private async Task<string> FetchXmlAsync(string url, CancellationToken ct)
    {
        var client = httpClientFactory.CreateClient(nameof(ScanExchangeRateJobHandler));
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        client.Timeout = TimeSpan.FromSeconds(30);

        using var resp = await client.GetAsync(url, ct);
        resp.EnsureSuccessStatusCode();

        var xml = await resp.Content.ReadAsStringAsync(ct);

        if (string.IsNullOrWhiteSpace(xml) ||
            !xml.Contains("<ExrateList", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogWarning("Vietcombank returned unexpected content. Length={Length}", xml?.Length ?? 0);
            throw new InvalidOperationException("Exchange rate XML is empty or invalid.");
        }

        return xml;
    }

    private static VcbParsedResult ParseVietcombankXml(string xml)
    {
        var doc = XDocument.Parse(xml);
        var root = doc.Root ?? throw new InvalidOperationException("XML root not found.");

        var dateText = root.Element("DateTime")?.Value?.Trim();
        var source = root.Element("Source")?.Value?.Trim();

        if (!DateTime.TryParse(dateText, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime rateLocal) &&
            !DateTime.TryParse(dateText, CultureInfo.CurrentCulture, DateTimeStyles.None, out rateLocal))
        {
            rateLocal = DateTime.Now;
        }

        var rateUtc = DateTime.SpecifyKind(rateLocal, DateTimeKind.Local).ToUniversalTime();

        var items = root.Elements("Exrate")
            .Select(x => new VcbRateItem(
                CurrencyCode: (string?)x.Attribute("CurrencyCode") ?? "",
                CurrencyName: (string?)x.Attribute("CurrencyName") ?? "",
                Buy: ParseDecimalNullable((string?)x.Attribute("Buy")),
                Transfer: ParseDecimalNullable((string?)x.Attribute("Transfer")),
                Sell: ParseDecimalNullable((string?)x.Attribute("Sell"))
            ))
            .Where(x => !string.IsNullOrWhiteSpace(x.CurrencyCode))
            .ToList();

        return new VcbParsedResult(rateUtc, source, items);
    }

    private static decimal? ParseDecimalNullable(string? s)
    {
        if (string.IsNullOrWhiteSpace(s) || s.Trim() == "-")
            return null;

        s = s.Trim();

        if (decimal.TryParse(
                s,
                NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out var value))
            return value;

        var normalized = s.Replace(",", "");
        if (decimal.TryParse(
                normalized,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out value))
            return value;

        return null;
    }

    private sealed record VcbParsedResult(DateTime RateDateUtc, string? Source, List<VcbRateItem> Items);
    private sealed record VcbRateItem(string CurrencyCode, string CurrencyName, decimal? Buy, decimal? Transfer, decimal? Sell);
}
