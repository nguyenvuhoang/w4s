using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Logger.Services.ScheduleTask;

namespace O24OpenAPI.Logger.Migrations.DataMigrations;

[O24OpenAPIMigration(
    "2025/01/01 21:45:46:0000000",
    "AddLogStep",
    MigrationProcessType.Installation
)]
[Environment([EnvironmentType.All])]
public class ClearLogTaskMigration : BaseMigration
{
    public override void Up()
    {
        var listTask = new List<ScheduleTask>
        {
            new()
            {
                Name = "ClearApplicationLog",
                Seconds = 60 * 60 * 24, // every 24 hours
                Type = $"{typeof(ClearApplicationLogTask).FullName}, O24OpenAPI.Logger",
                LastEnabledUtc = new DateTime(2025, 1, 1, 2, 0, 0, DateTimeKind.Utc),
                Enabled = true,
                StopOnError = false,
                LastStartUtc = new DateTime(2025, 1, 1, 2, 0, 0, DateTimeKind.Utc),
                LastEndUtc = new DateTime(2025, 1, 1, 2, 0, 0, DateTimeKind.Utc),
                LastSuccessUtc = new DateTime(2025, 1, 1, 2, 0, 0, DateTimeKind.Utc),
            },
        };
        SeedListData(listTask, [nameof(ScheduleTask.Name), nameof(ScheduleTask.Type)]).Wait();
    }
}
