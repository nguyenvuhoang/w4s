using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;
using O24OpenAPI.NCH.Domain.AggregatesModel.OtpAggregate;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
using O24OpenAPI.NCH.Domain.AggregatesModel.TelegramAggregate;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Migrations;

/// <summary>
/// The schema migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2026/01/28 17:54:07:0000000",
    "5. Init Table For NCH",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class SchemaMigration : AutoReversingMigration
{
    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(OTP_REQUESTS)).Exists())
        {
            Create.TableFor<OTP_REQUESTS>();
            Create
                .Index("IDX_UserAuthen_UserCode")
                .OnTable(nameof(OTP_REQUESTS))
                .OnColumn(nameof(OTP_REQUESTS.UserCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
            Create
                .Index("IX_OTPREQUESTS_PhoneNumber")
                .OnTable("OTP_REQUESTS")
                .OnColumn("PhoneNumber");

            Create.Index("IX_OTPREQUESTS_OtpCode").OnTable("OTP_REQUESTS").OnColumn("OtpCode");

            Create.Index("IX_OTPREQUESTS_IsUsed").OnTable("OTP_REQUESTS").OnColumn("IsUsed");

            Create
                .Index("IX_OTPREQUESTS_TransactionId")
                .OnTable("OTP_REQUESTS")
                .OnColumn("TransactionId");
        }
        if (!Schema.Table(nameof(SMSTemplate)).Exists())
        {
            Create.TableFor<SMSTemplate>();

            Create
                .Index("IX_SMSTEMPLATE_TemplateCode")
                .OnTable(nameof(SMSTemplate))
                .OnColumn(nameof(SMSTemplate.TemplateCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }
        // ======= SMSProvider =======
        if (!Schema.Table(nameof(SMSProvider)).Exists())
        {
            Create.TableFor<SMSProvider>();

            Create
                .Index("IX_SMSProvider_Name")
                .OnTable(nameof(SMSProvider))
                .OnColumn(nameof(SMSProvider.ProviderName))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        // ======= SMSSendOut =======
        if (!Schema.Table(nameof(SMSSendOut)).Exists())
        {
            Create.TableFor<SMSSendOut>();

            Create
                .Index("IX_SMSSendOut_Phone")
                .OnTable(nameof(SMSSendOut))
                .OnColumn(nameof(SMSSendOut.PhoneNumber));

            Create
                .Index("IX_SMSSendOut_Status")
                .OnTable(nameof(SMSSendOut))
                .OnColumn(nameof(SMSSendOut.Status));
            Create
                .Index("IX_SMSSendOut_OtpRequestId")
                .OnTable(nameof(SMSSendOut))
                .OnColumn(nameof(SMSSendOut.OtpRequestId));
        }

        if (!Schema.Table(nameof(SMSProviderConfig)).Exists())
        {
            Create.TableFor<SMSProviderConfig>();

            Create
                .Index("IX_SMSProviderConfig_SMSProviderId")
                .OnTable(nameof(SMSProviderConfig))
                .OnColumn(nameof(SMSProviderConfig.SMSProviderId));
        }

        if (!Schema.Table(nameof(Notification)).Exists())
        {
            Create.TableFor<Notification>();

            Create
                .Index("IX_Notification_TemplateID")
                .OnTable(nameof(Notification))
                .OnColumn(nameof(Notification.TemplateID));
            Create
                .Index("IX_Notification_UserCode_AppType")
                .OnTable(nameof(Notification))
                .OnColumn(nameof(Notification.UserCode))
                .Ascending()
                .OnColumn(nameof(Notification.AppType))
                .Ascending();
            Create
                .Index("IX_Notification_UserCode_IsRead")
                .OnTable(nameof(Notification))
                .OnColumn(nameof(Notification.UserCode))
                .Ascending()
                .OnColumn(nameof(Notification.IsRead))
                .Ascending();
            Create
                .Index("IX_Notification_UserCode_DateTime")
                .OnTable(nameof(Notification))
                .OnColumn(nameof(Notification.UserCode))
                .Ascending()
                .OnColumn(nameof(Notification.DateTime))
                .Descending();
        }

        if (!Schema.Table(nameof(NotificationTemplate)).Exists())
        {
            Create.TableFor<NotificationTemplate>();

            Create
                .Index("IX_NotificationTemplate_TemplateID")
                .OnTable(nameof(NotificationTemplate))
                .OnColumn(nameof(NotificationTemplate.TemplateID));
        }

        if (!Schema.Table(nameof(PushNotificationLog)).Exists())
        {
            Create.TableFor<PushNotificationLog>();

            Create
                .Index("IX_PushNotificationLog_Token")
                .OnTable(nameof(PushNotificationLog))
                .OnColumn(nameof(PushNotificationLog.Token));

            Create
                .Index("IX_PushNotificationLog_CreatedAt")
                .OnTable(nameof(PushNotificationLog))
                .OnColumn(nameof(PushNotificationLog.CreatedAt));

            Create
                .Index("IX_PushNotificationLog_Status")
                .OnTable(nameof(PushNotificationLog))
                .OnColumn(nameof(PushNotificationLog.Status));

            Create
                .Index("IX_PushNotificationLog_ResponseId")
                .OnTable(nameof(PushNotificationLog))
                .OnColumn(nameof(PushNotificationLog.ResponseId));
        }

        if (!Schema.Table(nameof(MailTemplate)).Exists())
        {
            Create.TableFor<MailTemplate>();

            Create
                .Index("IX_MailTemplate_TemplateId")
                .OnTable(nameof(MailTemplate))
                .OnColumn(nameof(MailTemplate.TemplateId));

            Create
                .Index("IX_MailTemplate_Status")
                .OnTable(nameof(MailTemplate))
                .OnColumn(nameof(MailTemplate.Status));

            Create
                .Index("IX_MailTemplate_SendAsPDF")
                .OnTable(nameof(MailTemplate))
                .OnColumn(nameof(MailTemplate.SendAsPDF));
        }

        if (!Schema.Table(nameof(MailConfig)).Exists())
        {
            Create.TableFor<MailConfig>();

            Create
                .Index("IX_MailConfig_ConfigId")
                .OnTable(nameof(MailConfig))
                .OnColumn(nameof(MailConfig.ConfigId));

            Create
                .Index("IX_MailConfig_EnableTLS")
                .OnTable(nameof(MailConfig))
                .OnColumn(nameof(MailConfig.EnableTLS));
        }

        if (!Schema.Table(nameof(TelegramChatMapping)).Exists())
        {
            Create.TableFor<TelegramChatMapping>();

            Create
                .Index("UX_TelegramChatMapping_ChatId")
                .OnTable(nameof(TelegramChatMapping))
                .OnColumn(nameof(TelegramChatMapping.ChatId))
                .Unique();

            Create
                .Index("IX_TelegramChatMapping_UserCode")
                .OnTable(nameof(TelegramChatMapping))
                .OnColumn(nameof(TelegramChatMapping.UserCode));

            Create
                .Index("IX_TelegramChatMapping_PhoneNumber")
                .OnTable(nameof(TelegramChatMapping))
                .OnColumn(nameof(TelegramChatMapping.PhoneNumber));

            Create
                .Index("IX_TelegramChatMapping_CreatedOnUtc")
                .OnTable(nameof(TelegramChatMapping))
                .OnColumn(nameof(TelegramChatMapping.CreatedOnUtc));
        }

        if (!Schema.Table(nameof(SMSProviderStatus)).Exists())
        {
            Create.TableFor<SMSProviderStatus>();

            Create
                .Index("IX_SMSProviderStatus_ProviderId")
                .OnTable(nameof(SMSProviderStatus))
                .OnColumn(nameof(SMSProviderStatus.ProviderId));

            Create
                .Index("IX_SMSProviderStatus_CheckTime")
                .OnTable(nameof(SMSProviderStatus))
                .OnColumn(nameof(SMSProviderStatus.CheckTime));

            Create
                .Index("IX_SMSProviderStatus_IsOnline")
                .OnTable(nameof(SMSProviderStatus))
                .OnColumn(nameof(SMSProviderStatus.IsOnline));

            Create
                .Index("IX_SMSProviderStatus_ResponseTimeMs")
                .OnTable(nameof(SMSProviderStatus))
                .OnColumn(nameof(SMSProviderStatus.ResponseTimeMs));

            Create
                .Index("IX_SMSProviderStatus_CreatedOnUtc")
                .OnTable(nameof(SMSProviderStatus))
                .OnColumn(nameof(SMSProviderStatus.CreatedOnUtc));
        }

        if (!Schema.Table(nameof(StoreOtp)).Exists())
        {
            Create.TableFor<StoreOtp>();

            Create
                .Index("IX_StoreOtp_PhoneNumber")
                .OnTable(nameof(StoreOtp))
                .OnColumn(nameof(StoreOtp.PhoneNumber));

            Create
                .Index("IX_StoreOtp_Platform")
                .OnTable(nameof(StoreOtp))
                .OnColumn(nameof(StoreOtp.Platform));

            Create
                .Index("IX_StoreOtp_IsActive")
                .OnTable(nameof(StoreOtp))
                .OnColumn(nameof(StoreOtp.IsActive));

            Create
                .Index("IX_StoreOtp_StartAt")
                .OnTable(nameof(StoreOtp))
                .OnColumn(nameof(StoreOtp.StartAt));

            Create
                .Index("IX_StoreOtp_EndAt")
                .OnTable(nameof(StoreOtp))
                .OnColumn(nameof(StoreOtp.EndAt));

            Create
                .Index("IX_StoreOtp_UsedCount")
                .OnTable(nameof(StoreOtp))
                .OnColumn(nameof(StoreOtp.UsedCount));
        }
        if (!Schema.Table(nameof(EmailSendOut)).Exists())
        {
            Create.TableFor<EmailSendOut>();

            Create
                .UniqueConstraint("UQ_EmailSendOut_Receiver_Subject_SentAt")
                .OnTable(nameof(EmailSendOut))
                .Columns(
                    nameof(EmailSendOut.Receiver),
                    nameof(EmailSendOut.Subject),
                    nameof(EmailSendOut.SentAt)
                );

            Create
                .Index("IX_EmailSendOut_ConfigId")
                .OnTable(nameof(EmailSendOut))
                .OnColumn(nameof(EmailSendOut.ConfigId));

            Create
                .Index("IX_EmailSendOut_TemplateId")
                .OnTable(nameof(EmailSendOut))
                .OnColumn(nameof(EmailSendOut.TemplateId));

            Create
                .Index("IX_EmailSendOut_Status")
                .OnTable(nameof(EmailSendOut))
                .OnColumn(nameof(EmailSendOut.Status));
        }

        if (!Schema.Table(nameof(SMSMappingResponse)).Exists())
        {
            Create.TableFor<SMSMappingResponse>();

            Create
                .UniqueConstraint("UQ_SMSMappingResponse_Provider_ResponseCode")
                .OnTable(nameof(SMSMappingResponse))
                .Columns(
                    nameof(SMSMappingResponse.ProviderName),
                    nameof(SMSMappingResponse.ResponseCode)
                );

            // Indexes
            Create
                .Index("IX_SMSMappingResponse_ProviderName")
                .OnTable(nameof(SMSMappingResponse))
                .OnColumn(nameof(SMSMappingResponse.ProviderName));

            Create
                .Index("IX_SMSMappingResponse_ResponseCode")
                .OnTable(nameof(SMSMappingResponse))
                .OnColumn(nameof(SMSMappingResponse.ResponseCode));

            Create
                .Index("IX_SMSMappingResponse_IsSuccess")
                .OnTable(nameof(SMSMappingResponse))
                .OnColumn(nameof(SMSMappingResponse.IsSuccess));

            Create
                .Index("IX_SMSMappingResponse_ServiceType")
                .OnTable(nameof(SMSMappingResponse))
                .OnColumn(nameof(SMSMappingResponse.ServiceType));

            Create
                .Index("IX_SMSMappingResponse_DateUpdated")
                .OnTable(nameof(SMSMappingResponse))
                .OnColumn(nameof(SMSMappingResponse.DateUpdated));
        }

        if (!Schema.Table(nameof(RepaymentRemind)).Exists())
        {
            Create.TableFor<RepaymentRemind>();

            Create
                .Index("UX_RepaymentRemind_Account_Due_Type")
                .OnTable(nameof(RepaymentRemind))
                .OnColumn(nameof(RepaymentRemind.AccountNumber))
                .Ascending()
                .OnColumn(nameof(RepaymentRemind.DueDate))
                .Ascending()
                .OnColumn(nameof(RepaymentRemind.MessageType))
                .Ascending()
                .WithOptions()
                .Unique();
        }

        if (!Schema.Table(nameof(ZaloOAToken)).Exists())
        {
            Create.TableFor<ZaloOAToken>();

            Create
                .Index("UX_ZaloOAToken_OaId_Active")
                .OnTable(nameof(ZaloOAToken))
                .OnColumn(nameof(ZaloOAToken.OaId))
                .Ascending()
                .OnColumn(nameof(ZaloOAToken.IsActive))
                .Ascending()
                .WithOptions()
                .Unique();
        }

        if (!Schema.Table(nameof(ZaloZNSSendout)).Exists())
        {
            Create.TableFor<ZaloZNSSendout>();

            Create
                .Index("UX_ZaloZNSSendout_RefId")
                .OnTable(nameof(ZaloZNSSendout))
                .OnColumn(nameof(ZaloZNSSendout.RefId))
                .Ascending()
                .WithOptions()
                .Unique();

            Create
                .Index("IX_ZaloZNSSendout_OaId_CreatedOnUtc")
                .OnTable(nameof(ZaloZNSSendout))
                .OnColumn(nameof(ZaloZNSSendout.OaId))
                .Ascending()
                .OnColumn(nameof(ZaloZNSSendout.CreatedOnUtc))
                .Descending();

            Create
                .Index("IX_ZaloZNSSendout_Phone_CreatedOnUtc")
                .OnTable(nameof(ZaloZNSSendout))
                .OnColumn(nameof(ZaloZNSSendout.Phone))
                .Ascending()
                .OnColumn(nameof(ZaloZNSSendout.CreatedOnUtc))
                .Descending();
        }


    }
}
