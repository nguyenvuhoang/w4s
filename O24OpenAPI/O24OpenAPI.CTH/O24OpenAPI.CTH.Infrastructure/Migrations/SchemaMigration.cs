using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Framework.Domain;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.CTH.Domain.AggregatesModel.TransactionAggregate;
using O24OpenAPI.CTH.Domain.AggregatesModel.CalendarAggregate;

namespace O24OpenAPI.CTH.Migrations;

/// <summary>
/// The schema migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2025/12/02 00:01:00:0000000",
    "Table For UserBanner",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
[DatabaseType(DataProviderType.SqlServer)]
public class SchemaMigration : AutoReversingMigration
{
    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(UserAccount)).Exists())
        {
            Create.TableFor<UserAccount>();
            Create
                .UniqueConstraint()
                .OnTable(nameof(UserAccount))
                .Columns(nameof(UserAccount.UserId), nameof(UserAccount.ChannelId));
            Create
                .Index("IX_USERACCOUNT_USERCODE")
                .OnTable(nameof(UserAccount))
                .OnColumn(nameof(UserAccount.UserCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(UserSession)).Exists())
        {
            Create.TableFor<UserSession>();
            Create
                .UniqueConstraint()
                .OnTable(nameof(UserSession))
                .Columns(nameof(UserSession.Token), nameof(UserSession.ChannelId));
            Create
                .Index("IDX_UserSession_LoginName_IsRevoked_ExpiresAt")
                .OnTable(nameof(UserSession))
                .OnColumn(nameof(UserSession.LoginName))
                .Ascending()
                .OnColumn(nameof(UserSession.IsRevoked))
                .Ascending()
                .OnColumn(nameof(UserSession.ExpiresAt))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(UserRightChannel)).Exists())
        {
            Create.TableFor<UserRightChannel>();
            Create
                .UniqueConstraint()
                .OnTable(nameof(UserRightChannel))
                .Columns(nameof(UserRightChannel.RoleId), nameof(UserRightChannel.ChannelId));
        }

        if (!Schema.Table(nameof(UserRoleChannel)).Exists())
        {
            Create.TableFor<UserRoleChannel>();
            Create
                .UniqueConstraint()
                .OnTable(nameof(UserRoleChannel))
                .Columns(nameof(UserRoleChannel.RoleId));
        }

        if (!Schema.Table(nameof(UserPassword)).Exists())
        {
            Create.TableFor<UserPassword>();
            Create
                .UniqueConstraint("")
                .OnTable(nameof(UserPassword))
                .Columns(nameof(UserPassword.UserId));
        }

        if (!Schema.Table(nameof(Channel)).Exists())
        {
            Create.TableFor<Channel>();
            Create.UniqueConstraint().OnTable(nameof(Channel)).Columns(nameof(Channel.ChannelId));
        }

        if (!Schema.Table(nameof(SupperAdmin)).Exists())
        {
            Create.TableFor<SupperAdmin>();
        }

        if (!Schema.Table(nameof(UserCommand)).Exists())
        {
            Create.TableFor<UserCommand>();
            Create
                .UniqueConstraint()
                .OnTable(nameof(UserCommand))
                .Columns(
                    nameof(UserCommand.ApplicationCode),
                    nameof(UserCommand.CommandId),
                    nameof(UserCommand.ParentId)
                );
            Create
                .Index("IX_USERCOMMAND_PARENTID")
                .OnTable(nameof(UserCommand))
                .OnColumn(nameof(UserCommand.ParentId))
                .Ascending()
                .WithOptions()
                .NonClustered();
            Create
                .Index("IX_USERCOMMAND_APP_ENABLED")
                .OnTable(nameof(UserCommand))
                .OnColumn(nameof(UserCommand.ApplicationCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(UserRight)).Exists())
        {
            Create.TableFor<UserRight>();
            Create
                .UniqueConstraint()
                .OnTable(nameof(UserRight))
                .Columns(nameof(UserRight.RoleId), nameof(UserRight.CommandId));
        }

        if (!Schema.Table(nameof(UserRole)).Exists())
        {
            Create.TableFor<UserRole>();
            Create.UniqueConstraint().OnTable(nameof(UserRole)).Columns(nameof(UserRole.RoleId));
        }

        if (!Schema.Table(nameof(UserInRole)).Exists())
        {
            Create.TableFor<UserInRole>();
            Create
                .Index("IX_USERINROLE_USERCODE")
                .OnTable(nameof(UserInRole))
                .OnColumn(nameof(UserInRole.UserCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(UserDevice)).Exists())
        {
            Create.TableFor<UserDevice>();
            Create
                .Index("IDX_UserDevice_UserCode")
                .OnTable(nameof(UserDevice))
                .OnColumn(nameof(UserDevice.UserCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(UserAuthen)).Exists())
        {
            Create.TableFor<UserAuthen>();
            Create
                .Index("IDX_UserAuthen_UserCode")
                .OnTable(nameof(UserAuthen))
                .OnColumn(nameof(UserAuthen.UserCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(UserAvatar)).Exists())
        {
            Create.TableFor<UserAvatar>();
            Create
                .UniqueConstraint()
                .OnTable(nameof(UserAvatar))
                .Column(nameof(UserAvatar.UserCode));
        }

        if (!Schema.Table(nameof(UserLevel)).Exists())
        {
            Create.TableFor<UserLevel>();
            Create
                .Index("IX_USERLEVEL_LEVELNAME")
                .OnTable(nameof(UserLevel))
                .OnColumn(nameof(UserLevel.LevelName))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(UserPolicy)).Exists())
        {
            Create.TableFor<UserPolicy>();

            Create
                .Index("IX_USERPOLICY_SERVICEID")
                .OnTable(nameof(UserPolicy))
                .OnColumn(nameof(UserPolicy.ServiceID))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IX_USERPOLICY_POLICYCODE")
                .OnTable(nameof(UserPolicy))
                .OnColumn(nameof(UserPolicy.PolicyCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(TransactionDefinition)).Exists())
        {
            Create.TableFor<TransactionDefinition>();

            Create
                .Index("IX_TransactionDefinition_Lookup")
                .OnTable(nameof(TransactionDefinition))
                .OnColumn(nameof(TransactionDefinition.TransactionCode))
                .Ascending()
                .OnColumn(nameof(TransactionDefinition.WorkflowId))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(UserLimit)).Exists())
        {
            Create.TableFor<UserLimit>();
            Create
                .Index("IX_UserLimit_Role_Command")
                .OnTable(nameof(UserLimit))
                .OnColumn(nameof(UserLimit.RoleId))
                .Ascending()
                .OnColumn(nameof(UserLimit.CommandId))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }
        if (!Schema.Table(nameof(RoleType)).Exists())
        {
            Create.TableFor<RoleType>();

            Create
                .UniqueConstraint("UQ_RoleType_RoleTypeCode_ServiceID")
                .OnTable(nameof(RoleType))
                .Columns(nameof(RoleType.RoleTypeCode), nameof(RoleType.ServiceID));

            Create
                .Index("IX_RoleType_Service_IsActive")
                .OnTable(nameof(RoleType))
                .OnColumn(nameof(RoleType.ServiceID))
                .Ascending()
                .OnColumn(nameof(RoleType.IsActive))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IX_RoleType_Order")
                .OnTable(nameof(RoleType))
                .OnColumn(nameof(RoleType.SortOrder))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }
        if (!Schema.Table(nameof(C_CODELIST)).Exists())
        {
            Create.TableFor<C_CODELIST>();
            Create
                .UniqueConstraint("UC_C_CODELIST_CodeId_CodeName_CodeGroup")
                .OnTable(nameof(C_CODELIST))
                .Columns(
                    nameof(C_CODELIST.CodeId),
                    nameof(C_CODELIST.CodeName),
                    nameof(C_CODELIST.CodeGroup)
                );
        }

        if (!Schema.Table(nameof(Calendar)).Exists())
        {
            Create.TableFor<Calendar>();
            Create
                .Index("IX_Calendar_Lookup")
                .OnTable(nameof(Calendar))
                .OnColumn(nameof(Calendar.SqnDate))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(BankWorkingCalendar)).Exists())
        {
            Create.TableFor<BankWorkingCalendar>();

            Create
                .Index("IX_BankWorkingCalendar_Lookup")
                .OnTable(nameof(BankWorkingCalendar))
                .OnColumn(nameof(BankWorkingCalendar.WorkingDate))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(ChannelSchedule)).Exists())
        {
            Create.TableFor<ChannelSchedule>();

            Create
                .ForeignKey("FK_ChannelSchedule_Channel")
                .FromTable(nameof(ChannelSchedule))
                .ForeignColumn(nameof(ChannelSchedule.ChannelIdRef))
                .ToTable(nameof(Channel))
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

            Create
                .Index("IX_ChannelSchedule_Channel_Day")
                .OnTable(nameof(ChannelSchedule))
                .OnColumn(nameof(ChannelSchedule.ChannelIdRef))
                .Ascending()
                .OnColumn(nameof(ChannelSchedule.DayOfWeek))
                .Ascending();
        }
        if (!Schema.Table(nameof(ChannelScheduleInterval)).Exists())
        {
            Create.TableFor<ChannelScheduleInterval>();

            Create
                .ForeignKey("FK_ChannelScheduleInterval_Schedule")
                .FromTable(nameof(ChannelScheduleInterval))
                .ForeignColumn(nameof(ChannelScheduleInterval.ChannelScheduleIdRef))
                .ToTable(nameof(ChannelSchedule))
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

            Create
                .Index("IX_ChannelScheduleInterval_Schedule_Sort")
                .OnTable(nameof(ChannelScheduleInterval))
                .OnColumn(nameof(ChannelScheduleInterval.ChannelScheduleIdRef))
                .Ascending()
                .OnColumn(nameof(ChannelScheduleInterval.SortOrder))
                .Ascending();
        }

        if (!Schema.Table(nameof(ChannelOverride)).Exists())
        {
            Create.TableFor<ChannelOverride>();

            Create
                .Index("IX_ChannelOverride_Channel_Date")
                .OnTable(nameof(ChannelOverride))
                .OnColumn(nameof(ChannelOverride.ChannelIdRef))
                .Ascending()
                .OnColumn(nameof(ChannelOverride.Date))
                .Ascending();
        }

        if (!Schema.Table(nameof(ChannelOverrideInterval)).Exists())
        {
            Create.TableFor<ChannelOverrideInterval>();
            Create
                .Index("IX_ChannelOverrideInterval_Override_Sort")
                .OnTable(nameof(ChannelOverrideInterval))
                .OnColumn(nameof(ChannelOverrideInterval.ChannelOverrideIdRef))
                .Ascending()
                .OnColumn(nameof(ChannelOverrideInterval.SortOrder))
                .Ascending();
        }

        if (!Schema.Table(nameof(ChannelUserOverride)).Exists())
        {
            Create.TableFor<ChannelUserOverride>();

            Create
                .Index("IX_ChannelUserOverride_Sort")
                .OnTable(nameof(ChannelUserOverride))
                .OnColumn(nameof(ChannelUserOverride.ChannelIdRef))
                .Ascending()
                .OnColumn(nameof(ChannelUserOverride.UserCode))
                .Ascending();
        }

        if (!Schema.Table(nameof(UserAgreement)).Exists())
        {
            Create.TableFor<UserAgreement>();

            Create
                .Index("IX_USERAGREEMENT_AGREEMENTNUMBER")
                .OnTable(nameof(UserAgreement))
                .OnColumn(nameof(UserAgreement.AgreementNumber))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .UniqueConstraint("UC_USERAGREEMENT_TYPE_TXCODE_NUMBER")
                .OnTable(nameof(UserAgreement))
                .Columns(
                    nameof(UserAgreement.AgreementNumber),
                    nameof(UserAgreement.AgreementType),
                    nameof(UserAgreement.TransactionCode)
                );
        }

        if (!Schema.Table(nameof(UserBanner)).Exists())
        {
            Create.TableFor<UserBanner>();

            Create
                .Index("IX_USERBANNER_USERCODE")
                .OnTable(nameof(UserBanner))
                .OnColumn(nameof(UserBanner.UserCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }
    }
}
