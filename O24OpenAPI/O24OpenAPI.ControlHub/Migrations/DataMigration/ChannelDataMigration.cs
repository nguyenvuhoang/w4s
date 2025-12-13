//using O24OpenAPI.ControlHub.Domain;
//using O24OpenAPI.Core.Attributes;
//using O24OpenAPI.Data.Migrations;
//using O24OpenAPI.Data.Utils;

//namespace O24OpenAPI.ControlHub.Migrations.DataMigration
//{
//    /// <summary>
//    /// The user role migration class
//    /// </summary>
//    /// <seealso cref="BaseMigration"/>
//    [O24OpenAPIMigration(
//        "2025/10/15 23:57:36:0000000",
//        "DataMigration",
//        MigrationProcessType.Installation
//    )]
//    [Environment([EnvironmentType.All])]
//    public class ChannelDataMigration : BaseMigration
//    {
//        /// <summary>
//        /// Ups this instance
//        /// </summary>
//        public override void Up()
//        {
//            var pathChannelData = "Migrations/DataJson/Channel/ChannelData.json";
//            var listChannelData = FileUtils.ReadJson<Channel>(pathChannelData).GetAwaiter().GetResult();
//            SeedListData(listChannelData, CTHConditionField.ChannelCondition).Wait();


//            var pathChannelScheduleIntervalData = "Migrations/DataJson/ChannelScheduleInterval/ChannelScheduleIntervalData.json";
//            var listChannelScheduleIntervalData = FileUtils.ReadJson<ChannelScheduleInterval>(pathChannelScheduleIntervalData).GetAwaiter().GetResult();
//            SeedListData(listChannelScheduleIntervalData, CTHConditionField.ChannelConditionScheduleInterval).Wait();

//            var pathChannelScheduleData = "Migrations/DataJson/ChannelSchedule/ChannelScheduleData.json";
//            var listChannelScheduleData = FileUtils.ReadJson<ChannelSchedule>(pathChannelScheduleData).GetAwaiter().GetResult();
//            SeedListData(listChannelScheduleData, CTHConditionField.ChannelScheduleCondition).Wait();


//            var pathBankWorkingCalendarData = "Migrations/DataJson/BankWorkingCalendar/BankWorkingCalendarData.json";
//            var listBankWorkingCalendarData = FileUtils.ReadJson<BankWorkingCalendar>(pathBankWorkingCalendarData).GetAwaiter().GetResult();
//            SeedListData(listBankWorkingCalendarData, CTHConditionField.BankWorkingCalendarDataCondition).Wait();

//        }
//    }
//}
