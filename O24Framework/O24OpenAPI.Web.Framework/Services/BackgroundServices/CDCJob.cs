using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;
using O24OpenAPI.Contracts.Events;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.Logging.Extensions;
using O24OpenAPI.Core.Logging.Helpers;
using O24OpenAPI.EventBus.Abstractions;
using O24OpenAPI.Web.Framework.DBContext;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Utils;

namespace O24OpenAPI.Web.Framework.Services.BackgroundServices;

[BackgroundJob("CDCJOb")]
public class CDCJob : BackgroundJobCommand { }

[CqrsHandler]
internal class CDCJobHandler(ServiceDBContext serviceDBContext) : ICommandHandler<CDCJob>
{
    private readonly ServiceDBContext _serviceDBContext = serviceDBContext;

    public async Task HandleAsync(CDCJob command, CancellationToken cancellationToken = default)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                "=====================[CDC] Starting CDC Job...======================="
            );
            Console.ForegroundColor = ConsoleColor.White;
            var config = Singleton<O24OpenAPIConfiguration>.Instance;

            var cdcChanges = await _serviceDBContext.CallServiceStoredProcedure(
                storedProcedureName: "sp_get_all_cdc_changes",
                null,
                config.YourDatabase,
                config.YourCDCSchema
            );
            if (cdcChanges is null)
            {
                return;
            }

            var data = cdcChanges.ToString();
            if (string.IsNullOrWhiteSpace(data) || data == "{}")
            {
                return;
            }

            List<CDCData> listCDCData;
            try
            {
                listCDCData = System.Text.Json.JsonSerializer.Deserialize<List<CDCData>>(
                    data,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    }
                );
            }
            catch (Exception deserEx)
            {
                deserEx.WriteError();
                return;
            }

            if (listCDCData == null || listCDCData.Count == 0)
            {
                return;
            }

            var invalids = listCDCData
                .Where(x =>
                    x == null
                    || string.IsNullOrWhiteSpace(x.TableName)
                    || x.Data == null
                    || string.IsNullOrWhiteSpace(x.LSN)
                )
                .ToList();

            if (invalids.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    $"================[CDC] Found {invalids.Count} invalid CDC records (null fields). Skipped.===================="
                );
                Console.ForegroundColor = ConsoleColor.White;
            }

            var cleaned = listCDCData
                .Where(x =>
                    x != null
                    && !string.IsNullOrWhiteSpace(x.TableName)
                    && x.Data != null
                    && !string.IsNullOrWhiteSpace(x.LSN)
                )
                .ToList();

            if (cleaned.Count == 0)
            {
                return;
            }

            cleaned =
            [
                .. cleaned.OrderBy(
                    x => TryParseLsn(x.LSN, out var lsn) ? lsn : [],
                    ByteArrayComparer.Instance
                ),
            ];

            var eventBus = EngineContext.Current.Resolve<IEventBus>();
            using var cts = new CancellationTokenSource();

            foreach (var cdcData in cleaned)
            {
                var (sql, action) = CDCUtils.GenDML(cdcData);

                var eventData = new CDCEvent
                {
                    table_name = cdcData.TableName,
                    action = action,
                    source = config.YourServiceID,
                    sql = sql,
                };
                BusinessLogHelper.Info(
                    $"[CDC:800] Publishing CDC Event for Table: {cdcData.TableName}, Action: {action}, LSN: {cdcData.LSN}, SQL:{sql}"
                );
                await eventBus.PublishAsync(eventData, cts.Token);
                await Task.Delay(10, cts.Token);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            _ = ex.LogErrorAsync();
            _ = ex.LogError();
            ex.WriteError();
        }
    }

    /// <summary>So sánh mảng byte theo thứ tự lexicographic.</summary>
    sealed class ByteArrayComparer : IComparer<byte[]>
    {
        public static readonly ByteArrayComparer Instance = new();

        public int Compare(byte[]? x, byte[]? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            var len = Math.Min(x.Length, y.Length);
            for (int i = 0; i < len; i++)
            {
                int cmp = x[i].CompareTo(y[i]);
                if (cmp != 0)
                {
                    return cmp;
                }
            }
            return x.Length.CompareTo(y.Length);
        }
    }

    /// <summary>
    /// Parse LSN/SeqVal từ chuỗi base64 hoặc hex (có/không có "0x") sang byte[].
    /// </summary>
    static bool TryParseLsn(string? s, out byte[] bytes)
    {
        bytes = [];
        if (string.IsNullOrWhiteSpace(s))
        {
            return false;
        }

        s = s.Trim();

        static bool IsHexChar(char c) =>
            (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');

        string? hex = null;
        if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            hex = s[2..];
        }
        else if (s.All(IsHexChar) && s.Length % 2 == 0)
        {
            hex = s;
        }

        if (hex != null)
        {
            try
            {
                int len = hex.Length / 2;
                var buf = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    buf[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }

                bytes = buf;
                return true;
            }
            catch { }
        }

        try
        {
            bytes = Convert.FromBase64String(s);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
