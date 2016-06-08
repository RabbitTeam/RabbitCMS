using FluentMigrator;
using Jkzl.Activitys.Models;
using System.Data;
using MigrationBase = Rabbit.Components.Data.Migrators.MigrationBase;

namespace Jkzl.Activitys
{
    [Migration(20160530215430)]
    public class Migrators : MigrationBase
    {
        #region Overrides of MigrationBase

        public override void Up()
        {
            Create
                .Table(TableName<ActivityRecord>())
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("StartTime").AsDateTime()
                .WithColumn("EndTime").AsDateTime().Nullable()
                .WithColumn("Description").AsString(int.MaxValue)
                .WithColumn("MaxLotteryCount").AsInt32().Nullable();

            Create
                .Table(TableName<PrizeRecord>())
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(20)
                .WithColumn("ImagePath").AsString(500)
                .WithColumn("Probability").AsDouble()
                .WithColumn("Activity_Id")
                .AsInt64()
                .ForeignKey("FK_FK_Pirze_Avtivity_Id_Id", TableName<ActivityRecord>(), "Id")
                .OnDelete(Rule.Cascade)
                .WithColumn("Total").AsInt32().Nullable()
                .WithColumn("Remain").AsInt32();

            Create
                .Table(TableName<AppUserRecord>())
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("MobileNumber").AsString(11)
                .WithColumn("LotteryCount").AsInt32();

            Create
                .Table(TableName<OrderRecord>())
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("User_Id").AsInt64().ForeignKey("FK_UserId_Id", TableName<AppUserRecord>(), "Id")
                .WithColumn("Prize_Id").AsInt64().ForeignKey("FK_PrizeId_Id", TableName<PrizeRecord>(), "Id")
                .WithColumn("CreateTime").AsDateTime()
                .WithColumn("IsReceive").AsBoolean();
        }

        public override void Down()
        {
        }

        #endregion Overrides of MigrationBase
    }
}