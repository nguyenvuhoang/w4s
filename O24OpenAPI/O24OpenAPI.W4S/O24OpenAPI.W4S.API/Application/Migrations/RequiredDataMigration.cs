using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain.Localization;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.W4S.API.Application.Constants;

namespace O24OpenAPI.W4S.API.Application.Migrations;

/// <summary>
/// The required data migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2026/01/01 10:46:01:0000000",
    "ContractPhoneExists",
    MigrationProcessType.Update
)]
[Environment(EnvironmentType.All)]
public class RequiredDataMigration(IO24OpenAPIDataProvider dataProvider) : BaseMigration
{
    /// <summary>
    /// The data provider
    /// </summary>
    private readonly IO24OpenAPIDataProvider _dataProvider = dataProvider;

    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        this.InstallInitialData();
        //this.InstallSettings();
    }

    /// <summary>
    /// Installs the initial data
    /// </summary>
    protected void InstallInitialData()
    {
        SeedListData(
                [
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode
                            .Validation
                            .ContractPhoneExists,
                        ResourceValue = "The phone number {0} already exists.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode
                            .Validation
                            .ContractPhoneExists,
                        ResourceValue = "Số điện thoại {0} đã tồn tại.",
                    }


                ],
                [nameof(LocaleStringResource.Language), nameof(LocaleStringResource.ResourceName)]
            )
            .Wait();
    }
}
