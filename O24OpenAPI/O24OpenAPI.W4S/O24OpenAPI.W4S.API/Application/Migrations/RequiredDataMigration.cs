using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain.Localization;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.W4S.API.Application.Constants;

namespace O24OpenAPI.W4S.API.Application.Migrations;

/// <summary>
/// The required data migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2026/01/20 15:58:01:0000000",
    "ClosingBalance",
    MigrationProcessType.Update
)]
[Environment(EnvironmentType.All)]
public class RequiredDataMigration() : BaseMigration
{
    public override void Up()
    {
        this.InstallInitialData();
    }

    protected void InstallInitialData()
    {
        SeedListData(
                [
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode.Validation.ContractPhoneExists,
                        ResourceValue = "The phone number {0} already exists.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode.Validation.ContractPhoneExists,
                        ResourceValue = "Số điện thoại {0} đã tồn tại.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode.Validation.WalletCategoryNotFound,
                        ResourceValue = "The wallet category was not found.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode.Validation.WalletCategoryNotFound,
                        ResourceValue = "Không tìm thấy danh mục ví.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode.Validation.WalletContractNotFound,
                        ResourceValue = "The wallet contract was not found.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode.Validation.WalletContractNotFound,
                        ResourceValue = "Không tìm thấy ví hợp đồng",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode.Validation.WalletContractNotFound,
                        ResourceValue = "Không tìm thấy danh mục hợp đồng ví.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode.Accounting.AccountNumberIsNotDefined,
                        ResourceValue = "The account {0} is not defined accounting yet",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode.Accounting.AccountNumberIsNotDefined,
                        ResourceValue = "Tài khoản {0} chưa được định nghĩa kế toán",
                    },
                      new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode.Accounting.CurrencyIsNotDefined,
                        ResourceValue = "The currency {0} is not defined accounting yet",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode.Accounting.CurrencyIsNotDefined,
                        ResourceValue = "Mã tiền tệ {0} chưa được định nghĩa kế toán",
                    },
                     new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode.Validation.ExchangeRateNotFound,
                        ResourceValue = "The exchange rate for currency {0} was not found.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode.Validation.ExchangeRateNotFound,
                        ResourceValue = "Không tìm thấy tỷ giá hối đoái.",
                    },
                       new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode.WalletStatement.OpeningBalance,
                        ResourceValue = "The opening balance",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode.WalletStatement.OpeningBalance,
                        ResourceValue = "Số dư đầu kỳ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24W4SResourceCode.WalletStatement.ClosingBalance,
                        ResourceValue = "The closing balance",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24W4SResourceCode.WalletStatement.ClosingBalance,
                        ResourceValue = "Số dư cuối kỳ",
                    },

                ],
                [nameof(LocaleStringResource.Language), nameof(LocaleStringResource.ResourceName)]
            )
            .Wait();
    }
}
