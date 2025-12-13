using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain.Configuration;
using O24OpenAPI.Core.Domain.Localization;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.O24NCH.Migrations;

/// <summary>
/// The required data migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2025/11/25 10:36:01:0000000",
    "NCH data required data SendSMSFailed",
    MigrationProcessType.Update
)]
[Environment(EnvironmentType.Dev)]
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
                        ResourceName = O24NCHResourceCode.Validation.InvalidOTP,
                        ResourceValue = "OTP {0} not found or incorrect",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24NCHResourceCode.Validation.InvalidOTP,
                        ResourceValue = "未找到 OTP {0} 或 OTP {0} 不正確。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24NCHResourceCode.Validation.InvalidOTP,
                        ResourceValue = "ບໍ່ພົບ OTP {0} ຫຼືບໍ່ຖືກຕ້ອງ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24NCHResourceCode.Validation.InvalidOTP,
                        ResourceValue = "OTP {0} không tìm thấy hoặc không hợp lệ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24NCHResourceCode.Validation.UsedOTP,
                        ResourceValue = "OTP {0} has already been used.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24NCHResourceCode.Validation.UsedOTP,
                        ResourceValue = "OTP {0} 已被使用。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24NCHResourceCode.Validation.UsedOTP,
                        ResourceValue = "OTP {0} ໄດ້ຖືກນໍາໃຊ້ແລ້ວ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24NCHResourceCode.Validation.UsedOTP,
                        ResourceValue = "OTP {0} đã được sử dụng.",
                    },
                    // ✅ OTP expired
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24NCHResourceCode.Validation.ExpiredOTP,
                        ResourceValue = "OTP {0} has expired.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24NCHResourceCode.Validation.ExpiredOTP,
                        ResourceValue = "OTP {0} 已過期。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24NCHResourceCode.Validation.ExpiredOTP,
                        ResourceValue = "OTP {0} ໝົດອາຍຸແລ້ວ.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24NCHResourceCode.Validation.ExpiredOTP,
                        ResourceValue = "OTP {0} đã hết hạn.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24NCHResourceCode.Validation.FCMTokenIsNotExist,
                        ResourceValue = "FCM token is not existing.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24NCHResourceCode.Validation.FCMTokenIsNotExist,
                        ResourceValue = "FCM令牌不存在。",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24NCHResourceCode.Validation.FCMTokenIsNotExist,
                        ResourceValue = "ທເຄັນ FCM ບໍ່ມີຢູ່.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24NCHResourceCode.Validation.FCMTokenIsNotExist,
                        ResourceValue = "Token FCM không tồn tại.",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24NCHResourceCode.Validation.MailReceiverNotFound,
                        ResourceValue = "Mail receiver is not found",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24NCHResourceCode.Validation.MailReceiverNotFound,
                        ResourceValue = "郵件接收者未找到",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24NCHResourceCode.Validation.MailReceiverNotFound,
                        ResourceValue = "ບໍ່ພົບຜູ້ຮັບອີເມວ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24NCHResourceCode.Validation.MailReceiverNotFound,
                        ResourceValue = "Không tìm thấy người nhận email",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24NCHResourceCode.Validation.SMSProviderIsExisting,
                        ResourceValue = "The SMS Provider is existing",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24NCHResourceCode.Validation.SMSProviderIsExisting,
                        ResourceValue = "Nhà cung cấp SMS đã tồn tại",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24NCHResourceCode.Validation.SMSProviderIsExisting,
                        ResourceValue = "SMS 提供者已存在",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24NCHResourceCode.Validation.SMSProviderIsExisting,
                        ResourceValue = "ມີຜູ້ໃຫ້ບໍລິການ SMS ຢູ່ກ່ອນແລ້ວ",
                    },
                    new LocaleStringResource()
                    {
                        Language = "en",
                        ResourceName = O24NCHResourceCode.Error.SendSMSFailed,
                        ResourceValue = "SMS send failed. Detail error {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "zh",
                        ResourceName = O24NCHResourceCode.Error.SendSMSFailed,
                        ResourceValue = "短信发送失败。详细错误 {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "lo",
                        ResourceName = O24NCHResourceCode.Error.SendSMSFailed,
                        ResourceValue = "ການສົ່ງ SMS ລົ້ມເຫຼວ. ລາຍລະອຽດຂໍ້ຜິດພາດ {0}",
                    },
                    new LocaleStringResource()
                    {
                        Language = "vi",
                        ResourceName = O24NCHResourceCode.Error.SendSMSFailed,
                        ResourceValue = "Gửi SMS thất bại. Chi tiết lỗi {0}",
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
        this._dataProvider.BulkInsertEntities(
                [new Setting() { Name = "SMSSetting.TimeOTP", Value = "true" }]
            )
            .GetAsyncResult();
    }
}
