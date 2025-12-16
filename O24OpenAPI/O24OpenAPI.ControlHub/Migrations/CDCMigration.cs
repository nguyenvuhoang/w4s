//using O24OpenAPI.Core.Attributes;
//using O24OpenAPI.Core.Configuration;
//using O24OpenAPI.Core.Domain;
//using O24OpenAPI.Core.Infrastructure;
//using O24OpenAPI.Data.Attributes;
//using O24OpenAPI.Data.Extensions;
//using O24OpenAPI.Data.Migrations;
//using O24OpenAPI.Framework.Domain;
//using O24OpenAPI.Framework.Utils;

//namespace O24OpenAPI.ControlHub.Migrations;

//[O24OpenAPIMigration(
//    "2025/10/02 16:14:01:0000000",
//    "Add CDC migration",
//    MigrationProcessType.Installation
//)]
//[Environment(EnvironmentType.All)]
//[DatabaseType(Data.DataProviderType.SqlServer)]
//public class CDCMigration : BaseMigration
//{
//    public override void Up()
//    {
//        #region enable cdc on database

//        CDCUtils.EnableCDCAsync().GetAwaiter().GetResult();

//        #endregion

//        #region add table LastProcessedLSN

//        string cdcSchema = Singleton<O24OpenAPIConfiguration>.Instance.YourCDCSchema;

//        if (
//            !string.IsNullOrEmpty(cdcSchema)
//            && Schema.Schema(cdcSchema).Exists()
//            && !Schema.Schema(cdcSchema).Table(nameof(LastProcessedLSN)).Exists()
//        )
//        {
//            Create.TableFor<LastProcessedLSN>();

//            Create
//                .Index()
//                .OnTable(nameof(LastProcessedLSN))
//                .InSchema(cdcSchema)
//                .OnColumn(nameof(LastProcessedLSN.TableName));
//        }

//        #endregion

//        #region add schedule job to process cdc

//        if (string.IsNullOrEmpty(Singleton<O24OpenAPIConfiguration>.Instance.YourCDCSchema))
//            throw new Exception("YourCDCSchema is null");

//        var task = new ScheduleTask()
//        {
//            Name = "CheckCDC",
//            Seconds = 10,
//            Type =
//                "O24OpenAPI.Framework.Services.ScheduleTasks.CheckCDCTask, O24OpenAPI.Framework",
//            Enabled = true,
//            StopOnError = false,
//            LastEnabledUtc = DateTime.UtcNow,
//            LastStartUtc = DateTime.UtcNow,
//            LastEndUtc = DateTime.UtcNow
//        };
//        if (DataProvider.GetTable<ScheduleTask>().Any(s => s.Name == task.Name))
//            return;
//        DataProvider.InsertEntity(task).GetAwaiter().GetResult();

//        #endregion
//    }
//}
