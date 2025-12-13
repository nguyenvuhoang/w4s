using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.ControlHub.Migrations.Scripts;

[O24OpenAPIMigration(
    "2025/03/24 10:55:55:0000000",
    "Init Function",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class InitFunction : Migration
{
    public override void Down() { }

    public override void Up()
    {
        //var functionFolderPath = "Migrations/Scripts/SqlServer/Functions";
        //var storedFolderPath = "Migrations/Scripts/SqlServer/StoredProcedures";
        //ExecuteSqlScriptsFromFolder(functionFolderPath);
        //ExecuteSqlScriptsFromFolder(storedFolderPath);
    }

    private void ExecuteSqlScriptsFromFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Folder not found: {folderPath}");
            return;
        }

        var sqlFiles = Directory.GetFiles(folderPath, "dbo.__*.sql");

        foreach (var file in sqlFiles)
        {
            string sql = File.ReadAllText(file);
            Console.WriteLine($"Executing SQL file: {file}");
            Execute.Sql(sql);
        }
    }
}
