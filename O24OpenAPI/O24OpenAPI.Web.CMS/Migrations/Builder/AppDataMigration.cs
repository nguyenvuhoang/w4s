namespace O24OpenAPI.Web.CMS.Migrations;

// /// <summary>
// /// Bo Schema Migration
// /// </summary>
// [O24OpenAPIMigration("2024/01/01 14:33:39:0000000", "App-CMS data", UpdateMigrationType.Data,
//     MigrationProcessType.Installation)]
// public class AppDataMigration : Migration
// {
//     /// <summary>
//     /// The data provider
//     /// </summary>
//     private readonly IO24OpenAPIDataProvider _dataProvider;
//
//     /// <summary>
//     /// Localization migrations
//     /// </summary>
//     /// <param name="dataProvider">The data provider.</param>
//     public AppDataMigration(IO24OpenAPIDataProvider dataProvider)
//     {
//         _dataProvider = dataProvider;
//     }
//
//     /// <summary>
//     /// Up migration
//     /// </summary>
//     public override async void Up()
//     {
//         var data = await Utils.Utils.ReadData<App>("AppData.json");
//
//         await _dataProvider.Truncate<App>(true);
//         await _dataProvider.BulkInsertEntities(data.ToArray());
//     }
//
//     /// <summary>
//     /// Down migration
//     /// </summary>
//     public override void Down()
//     {
//     }
// }
