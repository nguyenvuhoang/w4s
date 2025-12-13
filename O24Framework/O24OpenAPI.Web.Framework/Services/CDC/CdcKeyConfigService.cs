using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Logging.Helpers;
using O24OpenAPI.Data;
using O24OpenAPI.Web.Framework.Domain;

namespace O24OpenAPI.Web.Framework.Services.CDC
{
    public class CdcKeyConfigService(
     IRepository<TableKeyConfig> repo,
     IStaticCacheManager cache) : ICdcKeyConfigService
    {
        private readonly IRepository<TableKeyConfig> _repo = repo;
        private readonly IStaticCacheManager _cache = cache;

        /// <summary>
        /// Get CDC key columns for a table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<string[]> GetKeyColumnsAsync(string tableName)
        {
            BusinessLogHelper.Info("======= Getting CDC key columns for table {TableName}", tableName);

            if (string.IsNullOrWhiteSpace(tableName))
                return [];

            try
            {
                string schemaName = "dbo";
                string bareName = tableName;

                if (tableName.Contains('.'))
                {
                    var parts = tableName.Split('.');
                    if (parts.Length == 2)
                    {
                        schemaName = parts[0];
                        bareName = parts[1];
                    }
                }

                var cacheKey = new CacheKey($"cdc:tablekey:{schemaName}.{bareName}".ToLowerInvariant());

                // 1) Check cache
                var cached = await _cache.Get<string[]>(cacheKey);
                if (cached != null)
                    return cached;

                // 2) Query theo schema
                var keys = await _repo.Table
                    .Where(x => x.IsActive
                                && x.SchemaName == schemaName
                                && x.TableName == bareName)
                    .OrderBy(x => x.SortOrder)
                    .Select(x => x.KeyColumn)
                    .ToArrayAsync();

                // 3) Fallback: query không cần schema
                if (keys.Length == 0)
                {
                    BusinessLogHelper.Warning(
                        "CDC KeyConfig: No key found for {Schema}.{Table}, fallback without schema",
                        schemaName, bareName);

                    keys = await _repo.Table
                        .Where(x => x.IsActive
                                    && x.TableName == bareName)
                        .OrderBy(x => x.SortOrder)
                        .Select(x => x.KeyColumn)
                        .ToArrayAsync();
                }

                // 4) Write to cache
                await _cache.Set(cacheKey, keys);

                return keys;
            }
            catch (Exception ex)
            {
                BusinessLogHelper.Error(
                    ex,
                    "CDC GetKeyColumnsAsync failed for table {TableName}. Returning empty key list.",
                    tableName);

                // Always safe fallback
                return [];
            }
        }
    }


}
