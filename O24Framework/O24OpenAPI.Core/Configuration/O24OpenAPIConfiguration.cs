namespace O24OpenAPI.Core.Configuration;

/// <summary>
/// The 24 open api configuration class
/// </summary>
/// <seealso cref="IConfig"/>
public class O24OpenAPIConfiguration : IConfig
{
    /// <summary>
    /// Gets or sets the value of the environment
    /// </summary>
    public string Environment { get; set; } = "Dev";

    /// <summary>
    /// Gets or sets the value of the run migration
    /// </summary>
    public bool RunMigration { get; set; } = true;

    /// <summary>
    /// Gets or sets the value of the assembly migration
    /// </summary>
    public string AssemblyMigration { get; set; } = "";

    /// <summary>
    /// Gets or sets the value of the your service id
    /// </summary>
    public string YourServiceID { get; set; } = "MS1";

    /// <summary>
    /// Gets or sets the value of the your instance id
    /// </summary>
    public string YourInstanceID { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the value of the your database
    /// </summary>
    public string YourDatabase { get; set; } = "";

    /// <summary>
    /// Gets or sets the value of the your schema
    /// </summary>
    public string YourSchema { get; set; } = "dbo";

    /// <summary>
    /// Gets or sets the value of the your cdc schema
    /// </summary>
    public string YourCDCSchema { get; set; } = "cdc";

    /// <summary>
    /// Gets or sets the value of the connect to wfo
    /// </summary>
    public bool ConnectToWFO { get; set; } = true;

    /// <summary>
    /// Gets or sets the value of the o 24 open api grpc token
    /// </summary>
    public string O24OpenAPIGrpcToken { get; set; } = "";

    /// <summary>
    /// Gets or sets the value of the wfo http url
    /// </summary>
    public string WFOHttpURL { get; set; } = "";

    /// <summary>
    /// Gets or sets the value of the wfo grpc url
    /// </summary>
    public required string WFOGrpcURL { get; set; }

    /// <summary>
    /// Gets or sets the value of the your grpc url
    /// </summary>
    public required string YourGrpcURL { get; set; }

    /// <summary>
    /// Gets or sets the value of the urlsms
    /// </summary>
    public string? URLSMS { get; set; }

    /// <summary>
    /// Gets or sets the value of the log event message
    /// </summary>
    public bool LogEventMessage { get; set; }

    /// <summary>
    /// Gets or sets the value of the data warehouse entities
    /// </summary>
    public HashSet<string> DataWarehouseEntities { get; set; } = [];

    /// <summary>
    /// Gets or sets the value of the dwh schema
    /// </summary>
    public string? DWHSchema { get; set; }

    /// <summary>
    /// Gets or sets the value of the create database script path
    /// </summary>
    public string CreateDatabaseScriptPath { get; set; } = "App_Data/CreateDatabase.sql";
    public string CreateSchemaScriptPath { get; set; } = "App_Data/CreateSchema.sql";

    /// <summary>
    /// Gets or sets the value of the enable cdc script path
    /// </summary>
    public string EnableCDCScriptPath { get; set; } = "App_Data/EnableCDC.sql";

    /// <summary>
    /// Gets or sets the value of the open apiuri
    /// </summary>
    public string OpenAPIURI { get; set; } = "";

    public List<string> FanoutExchanges { get; set; } = [];

    public string FileLogPath { get; set; } = "App_Data/LogFile.txt";

    public bool AutoDeleteCommandQueue { get; set; } = true;
    public bool AutoDeleteEventQueue { get; set; } = false;
    public string OpenAPICMSURI { get; set; } = "";
    public string AllowedCorsOrigins { get; set; } = "";

    /// <summary>
    /// Gets the cdc db schema
    /// </summary>
    /// <returns>The string</returns>
    public string GetCdcDbSchema()
    {
        return $"{YourDatabase}.{YourCDCSchema}";
    }

    /// <summary>
    /// Gets the db schema
    /// </summary>
    /// <returns>The string</returns>
    public string GetDbSchema()
    {
        return $"{YourDatabase}.{YourSchema}";
    }
}
