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
    "2026/01/12 10:58:01:0000000",
    "WalletContractNotFound",
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
                    },
                     new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode
                            .Validation
                            .WalletCategoryNotFound,
                        ResourceValue = "The wallet category was not found.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode
                            .Validation
                            .WalletCategoryNotFound,
                        ResourceValue = "Không tìm thấy danh mục ví.",
                    },
                     new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode
                            .Validation
                            .WalletContractNotFound,
                        ResourceValue = "The wallet contract was not found.",
                    },
                     new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode
                            .Validation
                            .WalletContractNotFound,
                        ResourceValue = "Không tìm thấy ví hợp đồng",
                    },

                     new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode
                            .Validation
                            .WalletContractNotFound,
                        ResourceValue = "Không tìm thấy danh mục hợp đồng ví.",
                    },



                ],
                [nameof(LocaleStringResource.Language), nameof(LocaleStringResource.ResourceName)]
            )
            .Wait();
    }
}
