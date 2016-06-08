using FluentMigrator;
using Rabbit.Users.Models;
using System.Data;
using MigrationBase = Rabbit.Components.Data.Migrators.MigrationBase;

namespace Rabbit.Users
{
    [Migration(20150715120930)]
    public class Migrators : MigrationBase
    {
        #region Overrides of MigrationBase

        public override void Up()
        {
            Create
                .Table(TableName<AccountRecord>())
                .WithColumn("Id").AsAnsiString(32).PrimaryKey()
                .WithColumn("Account").AsString(20)
                .WithColumn("Password").AsString(50);

            Create
                .Table(TableName<UserRecord>())
                .WithColumn("Id").AsAnsiString(32).PrimaryKey()
                .WithColumn("Name").AsString(20)
                .WithColumn("Account_Id").AsAnsiString(32)
                .ForeignKey("FK_UserRecord_Account_Id_AccountRecord_Id", TableName<AccountRecord>(), "Id").OnDelete(Rule.Cascade);

            var account = AccountRecord.Create("admin", "admin");
            var user = UserRecord.Create("admin", account);

            Insert.IntoTable(TableName<AccountRecord>()).Row(new { account.Id, account.Account, account.Password });
            Insert.IntoTable(TableName<UserRecord>()).Row(new { user.Id, user.Name, Account_Id = user.Account.Id });
        }

        public override void Down()
        {
            Delete.Table(TableName("UserRecord"));
            Delete.Table(TableName("AccountRecord"));
        }

        #endregion Overrides of MigrationBase
    }
}