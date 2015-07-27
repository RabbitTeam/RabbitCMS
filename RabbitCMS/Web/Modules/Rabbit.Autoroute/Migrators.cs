using FluentMigrator;
using Rabbit.Autoroute.Models;
using MigrationBase = Rabbit.Components.Data.Migrators.MigrationBase;

namespace Rabbit.Autoroute
{
    [Migration(20150715150328)]
    public class Migrators : MigrationBase
    {
        #region Overrides of MigrationBase

        public override void Up()
        {
            Create
                .Table(TableName<RouteRecord>())
                .WithColumn("Id").AsAnsiString(32).PrimaryKey()
                .WithColumn("Path").AsString(500);
        }

        public override void Down()
        {
            Delete.Table(TableName("RouteRecord"));
        }

        #endregion Overrides of MigrationBase
    }
}