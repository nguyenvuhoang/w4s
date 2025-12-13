using O24OpenAPI.Core.Infrastructure;

namespace O24OpenAPI.Core.Configuration;

/// <summary>
/// The app settings helper class
/// </summary>
public class AppSettingsHelper
{
    /// <summary>
    /// Saves the app settings using the specified configurations
    /// </summary>
    /// <param name="configurations">The configurations</param>
    /// <param name="fileProvider">The file provider</param>
    /// <param name="overwrite">The overwrite</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>The app settings</returns>
    public static AppSettings SaveAppSettings(
        IList<IConfig> configurations,
        IO24OpenAPIFileProvider fileProvider,
        bool overwrite = true
    )
    {
        ArgumentNullException.ThrowIfNull(configurations);

        AppSettings appSettings = Singleton<AppSettings>.Instance ?? new AppSettings();
        appSettings.Update(configurations);

        Singleton<AppSettings>.Instance = appSettings;
        Singleton<O24OpenAPIConfiguration>.Instance = appSettings.Get<O24OpenAPIConfiguration>();

        //string str = fileProvider.MapPath(O24OpenAPIConfigurationDefaults.AppSettingsFilePath);
        //bool flag = fileProvider.FileExists(str);
        //fileProvider.CreateFile(str);
        //Dictionary<string, JToken> source =
        //    JsonConvert
        //        .DeserializeObject<AppSettings>(fileProvider.ReadAllText(str, Encoding.UTF8))
        //        ?.Configuration ?? new Dictionary<string, JToken>();

        //foreach (IConfig configuration in (IEnumerable<IConfig>)configurations)
        //    source[configuration.Name] = JToken.FromObject(configuration);

        //appSettings.Configuration = source
        //    .SelectMany(
        //        outConfig =>
        //            configurations
        //                .Where<IConfig>(inConfig => inConfig.Name == outConfig.Key)
        //                .DefaultIfEmpty<IConfig>(),
        //        (outConfig, inConfig) => new { OutConfig = outConfig, InConfig = inConfig }
        //    )
        //    .OrderBy(config =>
        //    {
        //        IConfig inConfig = config.InConfig;
        //        return inConfig == null ? int.MaxValue : inConfig.GetOrder();
        //    })
        //    .Select(config => config.OutConfig)
        //    .ToDictionary<KeyValuePair<string, JToken>, string, JToken>(
        //        config => config.Key,
        //        config => config.Value
        //    );

        //if (!flag | overwrite)
        //{
        //    string contents = JsonConvert.SerializeObject(appSettings, (Formatting)1);
        //    fileProvider.WriteAllText(str, contents, Encoding.UTF8);
        //}
        return appSettings;
    }

    /// <summary>
    /// Gets the schema name using the specified type
    /// </summary>
    /// <param name="type">The type</param>
    /// <returns>The schema name</returns>
    public static string GetSchemaName(Type type)
    {
        if (type == null)
        {
            return string.Empty;
        }

        O24OpenAPIConfiguration config = Singleton<O24OpenAPIConfiguration>.Instance;

        if (config == null)
        {
            return string.Empty;
        }

        if (type.Name.Equals("LastProcessedLSN", StringComparison.OrdinalIgnoreCase))
        {
            return config.GetCdcDbSchema();
        }

        string ns = type.Namespace;

        if (ns.StartsWith("O24OpenAPI.DataWarehouse", StringComparison.OrdinalIgnoreCase))
        {
            return config.DWHSchema;
        }

        return string.Empty;
    }
}
