using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

/// <summary>
/// The user account builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{UserAccount}"/>
public class UserAccountBuilder : O24OpenAPIEntityBuilder<UserAccount>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserAccount.ChannelId))
            .AsString(36) // UUID thường dài 36 ký tự
            .NotNullable()
            .WithColumn(nameof(UserAccount.UserId))
            .AsString(36)
            .NotNullable()
            .WithColumn(nameof(UserAccount.UserName))
            .AsString(150) // Giảm từ 255 -> 150
            .Nullable()
            .WithColumn(nameof(UserAccount.UserCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserAccount.LoginName))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserAccount.ContractNumber))
            .AsString(200)
            .Nullable()
            .WithColumn(nameof(UserAccount.FirstName))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(UserAccount.MiddleName))
            .AsString(50) // Giảm từ 100 -> 50
            .Nullable()
            .WithColumn(nameof(UserAccount.LastName))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(UserAccount.RoleChannel))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserAccount.Gender))
            .AsInt16()
            .Nullable()
            .WithColumn(nameof(UserAccount.Address))
            .AsString(500) // Giảm từ 1000 -> 500
            .Nullable()
            .WithColumn(nameof(UserAccount.Email))
            .AsString(150) // Giảm từ 100 -> 150 để tránh lỗi khi dùng email dài
            .Nullable()
            .WithColumn(nameof(UserAccount.Birthday))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserAccount.Status))
            .AsString(20)
            .Nullable()
            .WithColumn(nameof(UserAccount.UserCreated))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserAccount.LastLoginTime))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserAccount.UserModified))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserAccount.IsLogin))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(UserAccount.ExpireTime))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserAccount.BranchID))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserAccount.BranchCode))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserAccount.DepartmentCode))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserAccount.UserLevel))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(UserAccount.UserType))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserAccount.IsShow))
            .AsString(1)
            .Nullable()
            .WithColumn(nameof(UserAccount.PolicyID))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(UserAccount.UUID))
            .AsString(36)
            .Nullable()
            .WithColumn(nameof(UserAccount.Failnumber))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(UserAccount.IsSuperAdmin))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(UserAccount.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserAccount.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserAccount.IsBiometricSupported))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(UserAccount.IsFirstLogin))
            .AsBoolean()
            .Nullable().
            WithColumn(nameof(UserAccount.CurrencyCode))
            .AsString(3)
            .Nullable()
            ;
    }
}
