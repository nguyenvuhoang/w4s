namespace O24OpenAPI.O24Design.Migrations.Builders;

//public class ServiceLogBuilder : O24OpenAPIEntityBuilder<ServiceLog>
//{
//    public override void MapEntity(CreateTableExpressionBuilder table)
//    {
//        table
//            .WithColumn(nameof(ServiceLog.LogLevelId))
//            .AsInt32()
//            .NotNullable()
//            .WithColumn(nameof(ServiceLog.ServiceId))
//            .AsString(100)
//            .NotNullable()
//            .WithColumn(nameof(ServiceLog.ChannelId))
//            .AsString(50)
//            .Nullable()
//            .WithColumn(nameof(ServiceLog.Status))
//            .AsString(50)
//            .Nullable()
//            .WithColumn(nameof(ServiceLog.ShortMessage))
//            .AsString(1000)
//            .Nullable()
//            .WithColumn(nameof(ServiceLog.FullMessage))
//            .AsString(int.MaxValue)
//            .Nullable()
//            .WithColumn(nameof(ServiceLog.Data))
//            .AsString(int.MaxValue)
//            .Nullable()
//            .WithColumn(nameof(ServiceLog.UserId))
//            .AsString(100)
//            .Nullable()
//            .WithColumn(nameof(ServiceLog.Reference))
//            .AsString(100)
//            .Nullable()
//            .WithColumn(nameof(ServiceLog.IpAddress))
//            .AsString(50)
//            .Nullable()
//            .WithColumn(nameof(ServiceLog.UserAgent))
//            .AsString(500)
//            .Nullable()
//            .WithColumn(nameof(ServiceLog.CreatedOnUtc))
//            .AsDateTime2()
//            .Nullable();
//    }
//}
