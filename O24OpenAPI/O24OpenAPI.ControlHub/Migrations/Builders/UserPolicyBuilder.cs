using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builder;

public class UserPolicyBuilder : O24OpenAPIEntityBuilder<UserPolicy>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserPolicy.PolicyCode)).AsInt32().NotNullable()
            .WithColumn(nameof(UserPolicy.ServiceID)).AsString(20).NotNullable()
            .WithColumn(nameof(UserPolicy.Descr)).AsString(200).Nullable()
            .WithColumn(nameof(UserPolicy.IsDefault)).AsBoolean().Nullable()
            .WithColumn(nameof(UserPolicy.EfFrom)).AsDateTime().NotNullable()
            .WithColumn(nameof(UserPolicy.EfTo)).AsDateTime().Nullable()
            .WithColumn(nameof(UserPolicy.PwdHis)).AsInt32().NotNullable()
            .WithColumn(nameof(UserPolicy.PwdAgeMax)).AsInt32().NotNullable()
            .WithColumn(nameof(UserPolicy.MinPwdLen)).AsInt32().NotNullable()
            .WithColumn(nameof(UserPolicy.PwdCplx)).AsBoolean().NotNullable()
            .WithColumn(nameof(UserPolicy.PwdCplxLc)).AsBoolean().NotNullable()
            .WithColumn(nameof(UserPolicy.PwdCplxUc)).AsBoolean().NotNullable()
            .WithColumn(nameof(UserPolicy.PwdCplxSc)).AsBoolean().NotNullable()
            .WithColumn(nameof(UserPolicy.PwdCplxSn)).AsBoolean().NotNullable()
            .WithColumn(nameof(UserPolicy.TimeLginRequire)).AsBoolean().NotNullable()
            .WithColumn(nameof(UserPolicy.LginFr)).AsString(50).NotNullable()
            .WithColumn(nameof(UserPolicy.LginTo)).AsString(50).NotNullable()
            .WithColumn(nameof(UserPolicy.LlkOutThrs)).AsString(10).NotNullable()
            .WithColumn(nameof(UserPolicy.ResetLkOut)).AsString(10).NotNullable()
            .WithColumn(nameof(UserPolicy.UserCreate)).AsString(50).Nullable()
            .WithColumn(nameof(UserPolicy.DateCreate)).AsDateTime().Nullable()
            .WithColumn(nameof(UserPolicy.UserModify)).AsString(50).Nullable()
            .WithColumn(nameof(UserPolicy.DateModify)).AsDateTime().Nullable()
            .WithColumn(nameof(UserPolicy.ContractID)).AsString(50).Nullable()
            .WithColumn(nameof(UserPolicy.IsBankEdit)).AsBoolean().Nullable().WithDefaultValue(false)
            .WithColumn(nameof(UserPolicy.IsCorpEdit)).AsBoolean().Nullable().WithDefaultValue(false)
            .WithColumn(nameof(UserPolicy.BaseOnPolicy)).AsInt32().Nullable().WithDefaultValue(0);
    }
}
