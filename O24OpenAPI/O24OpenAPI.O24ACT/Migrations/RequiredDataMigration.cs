using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Domain.Localization;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.O24ACT.Common;

namespace O24OpenAPI.O24ACT.Migrations;

/// <summary>
/// The required data migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2025/10/05 22:01:01:0000000",
    "AccountNumberIsExisting",
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
                        ResourceName = O24ActResourceCode.Account.AccountNumberIsExisting,
                        ResourceValue = "Account chart number {0} is existing",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24ActResourceCode.Account.AccountNumberIsExisting,
                        ResourceValue = "Số tài khoản {0} đã tồn tại",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24ActResourceCode.Account.AccountNumberIsExisting,
                        ResourceValue = "ໝາຍເລກແຜນຜັງບັນຊີ {0} ມີຢູ່ແລ້ວ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24ActResourceCode.Account.AccountNumberIsExisting,
                        ResourceValue = "帐户图表编号 {0} 已存在",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24ActResourceCode.Validation.AccountCommonNotDefined,
                        ResourceValue = "AccountCommon {0} is not defined",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24ActResourceCode.Validation.AccountCommonNotDefined,
                        ResourceValue = "AccountCommon {0} chưa được định nghĩa",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24ActResourceCode.Validation.AccountCommonNotDefined,
                        ResourceValue = "AccountCommon {0} ບໍ່ໄດ້ຖືກກຳນົດ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24ActResourceCode.Validation.AccountCommonNotDefined,
                        ResourceValue = "AccountCommon {0} 未定义",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24ActResourceCode.Validation.UnBalancePosting,
                        ResourceValue = "UnBalance Posting {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24ActResourceCode.Validation.UnBalancePosting,
                        ResourceValue = "Hoạch toán không cân {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24ActResourceCode.Validation.UnBalancePosting,
                        ResourceValue = "UnBalance Posting {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24ActResourceCode.Validation.UnBalancePosting,
                        ResourceValue = "UnBalance Posting {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24ActResourceCode.Validation.AccountLevelNotAllow,
                        ResourceValue = "Level of account {0} not allow posting",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24ActResourceCode.Validation.AccountLevelNotAllow,
                        ResourceValue = "Mức tài khoản {0} không cho phép hoạch toán",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24ActResourceCode.Validation.AccountLevelNotAllow,
                        ResourceValue = "ລະດັບບັນຊີ {0} ບໍ່ອະນຸຍາດໃຫ້ໂພສ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24ActResourceCode.Validation.AccountLevelNotAllow,
                        ResourceValue = "帐户级别 {0} 不允许发帖",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24ActResourceCode.Validation.ACT_NOT_ALLOW_DEBIT_BAL,
                        ResourceValue = "Account {0} is not allowed to be debit balance",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24ActResourceCode.Validation.ACT_NOT_ALLOW_DEBIT_BAL,
                        ResourceValue = "Tài khoản {0} không được phép có số dư nợ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24ActResourceCode.Validation.ACT_NOT_ALLOW_DEBIT_BAL,
                        ResourceValue = "ບັນຊີ {0} ບໍ່ໄດ້ຮັບອະນຸຍາດໃຫ້ເປັນຍອດເງິນເດບິດ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24ActResourceCode.Validation.ACT_NOT_ALLOW_DEBIT_BAL,
                        ResourceValue = "账户 {0} 不允许有借方余额",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24ActResourceCode.Validation.ACT_NOT_ALLOW_CREDIT_BAL,
                        ResourceValue = "Account {0} is not allowed to be credit balance",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24ActResourceCode.Validation.ACT_NOT_ALLOW_CREDIT_BAL,
                        ResourceValue = "Tài khoản {0} không được phép có số dư có",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24ActResourceCode.Validation.ACT_NOT_ALLOW_CREDIT_BAL,
                        ResourceValue = "ບັນຊີ {0} ບໍ່ໄດ້ຖືກອະນຸຍາດໃຫ້ເປັນຍອດເງິນສິນເຊື່ອ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24ActResourceCode.Validation.ACT_NOT_ALLOW_CREDIT_BAL,
                        ResourceValue = "帐户 {0} 不允许有贷方余额",
                    },
                ],
                [nameof(LocaleStringResource.Language), nameof(LocaleStringResource.ResourceName)]
            )
            .Wait();
    }

    /// <summary>
    /// Installs the settings
    /// </summary>
    protected void InstallSettings()
    {
        this._dataProvider.BulkInsertEntities([new Setting() { Name = "", Value = "" }])
            .GetAsyncResult();
    }
}
