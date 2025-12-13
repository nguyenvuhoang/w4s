using FluentMigrator.Runner.Initialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Enums;
using O24OpenAPI.Core.Helper;
using System.Text;

namespace O24OpenAPI.Data.Configuration;

/// <summary>
/// The data config class
/// </summary>
/// <seealso cref="IConfig"/>
/// <seealso cref="IConnectionStringAccessor"/>
public class DataConfig : IConfig, IConnectionStringAccessor
{
    /// <summary>
    /// The empty
    /// </summary>
    private string _connectionString = string.Empty;

    /// <summary>
    /// The decrypted connection string
    /// </summary>
    private string _decryptedConnectionString;

    /// <summary>
    /// The lock
    /// </summary>
    private readonly object _lock = new();

    /// <summary>
    /// Gets or sets the value of the connection string
    /// </summary>
    public string ConnectionString
    {
        get
        {
            if (_decryptedConnectionString != null)
            {
                return _decryptedConnectionString;
            }

            lock (_lock)
            {
                if (_decryptedConnectionString != null)
                {
                    return _decryptedConnectionString;
                }

                string encryptionPrivateKey = "273ece6f97dd844d";
                _decryptedConnectionString = DecryptConnectionString(
                    _connectionString,
                    encryptionPrivateKey
                );
                return _decryptedConnectionString;
            }
        }
        set => this._connectionString = value;
    }

    /// <summary>
    /// Gets or sets the value of the data provider
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public DataProviderType DataProvider { get; set; } = DataProviderType.SqlServer;


    /// <summary>
    /// Load type
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public DbTypeEnum LoadType()
    {
        return DataProvider switch
        {
            DataProviderType.SqlServer => DbTypeEnum.sqlserver,
            DataProviderType.Oracle => DbTypeEnum.oracle,
            DataProviderType.MySql => DbTypeEnum.mysql,
            DataProviderType.PostgreSQL => DbTypeEnum.postgresql,
            _ => throw new Exception($"Unsupported provider: {DataProvider}")
        };
    }
    /// <summary>
    /// Gets or sets the value of the sql command timeout
    /// </summary>
    public int? SQLCommandTimeout { get; set; } = new int?();

    /// <summary>
    /// The empty
    /// </summary>
    private string _defaultConnectionEncrypted = string.Empty;

    /// <summary>
    /// The default connection decrypted
    /// </summary>
    private string _defaultConnectionDecrypted = null;
    /// <summary>
    /// Schema name
    /// </summary>
    public string SchemaName { get; set; } = "dbo";
    /// <summary>
    /// Load timeout
    /// </summary>
    /// <returns></returns>
    public int LoadTimeout() => SQLCommandTimeout ?? 60;
    /// <summary>
    /// Load schema
    /// </summary>
    /// <returns></returns>
    public string LoadSchema() => string.IsNullOrWhiteSpace(SchemaName) ? "dbo" : SchemaName;

    /// <summary>
    /// Gets or sets the value of the default connection
    /// </summary>
    public string DefaultConnection
    {
        get
        {
            if (_defaultConnectionDecrypted == null)
            {
                string encryptionPrivateKey = "273ece6f97dd844d";
                _defaultConnectionDecrypted = DecryptConnectionString(
                    _defaultConnectionEncrypted,
                    encryptionPrivateKey
                );
            }
            return _defaultConnectionDecrypted;
        }
        set
        {
            _defaultConnectionEncrypted = value;
            _defaultConnectionDecrypted = null;
        }
    }

    /// <summary>
    /// Gets the value of the name
    /// </summary>
    [JsonIgnore]
    public string Name => "ConnectionStrings";

    /// <summary>
    /// Gets the order
    /// </summary>
    /// <returns>The int</returns>
    public int GetOrder() => 0;

    /// <summary>
    /// Decrypts the connection string using the specified encrypted connection string
    /// </summary>
    /// <param name="encryptedConnectionString">The encrypted connection string</param>
    /// <param name="encryptionKey">The encryption key</param>
    /// <returns>The string</returns>
    private static string DecryptConnectionString(
        string encryptedConnectionString,
        string encryptionKey
    )
    {
        string[] strArray = encryptedConnectionString.Split(";");
        StringBuilder connectionString = new();

        foreach (string part in strArray)
        {
            string key = part.Split('=')[0];
            string value = part[(part.IndexOf('=') + 1)..];

            if (key.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                value = CommonHelper.DecryptText(value, encryptionKey);
            }

            if (connectionString.Length > 0)
            {
                connectionString.Append(';');
            }

            connectionString.Append($"{key}={value}");
        }
        return connectionString.ToString();
    }
}
