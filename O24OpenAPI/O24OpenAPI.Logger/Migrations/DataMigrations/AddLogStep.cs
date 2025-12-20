using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Logger.Queues;
using O24OpenAPI.Framework.Constants;
using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.Logger.Migrations.DataMigrations;

/// <summary>
/// The add log step class
/// </summary>
/// <seealso cref="BaseMigration"/>
[O24OpenAPIMigration(
    "2025/01/19 14:45:46:0000000",
    "AddLogStep",
    MigrationProcessType.Installation
)]
[Environment([EnvironmentType.Dev, EnvironmentType.Test])]
public class AddLogStep : BaseMigration
{
    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        var list = new List<O24OpenAPIService>
        {
            new()
            {
                StepCode = "LOG_SIMPLE_SEARCH",
                FullClassName = $"{typeof(LogQueue).FullName}",
                MethodName = nameof(LogQueue.SimpleSearch),
                ShouldAwait = true,
                IsInquiry = true,
            },
        };
        SeedListData(list, ConditionField.O24OpenAPIServiceCondition).Wait();
    }
}
