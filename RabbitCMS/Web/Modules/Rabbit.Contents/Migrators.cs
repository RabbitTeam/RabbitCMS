using FluentMigrator;
using Rabbit.Contents.Models;
using System;
using MigrationBase = Rabbit.Components.Data.Migrators.MigrationBase;

namespace Rabbit.Contents
{
    [Migration(20150721112830)]
    public class Migrators : MigrationBase
    {
        #region Overrides of MigrationBase

        public override void Up()
        {
            Create
                .Table(TableName<SiteSettingsRecord>())
                .WithColumn("Id").AsAnsiString(32).PrimaryKey()
                .WithColumn("Name").AsString(100)
                .WithColumn("Logo").AsAnsiString(500).Nullable()
                .WithColumn("ThemeName").AsString(50)
                .WithColumn("Copyright").AsString(500).Nullable()
                .WithColumn("Seo_Title").AsString(255).Nullable()
                .WithColumn("Seo_Keywords").AsString(255).Nullable()
                .WithColumn("Seo_Description").AsString(255).Nullable();

            Insert
                .IntoTable(TableName<SiteSettingsRecord>())
                .Row(new
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = "RabbitHub",
                    ThemeName = "TheThemeMachine",
                    Copyright = "Copyright &copy; 2015 RabbitHub. All rights reserved.",
                    Seo_Title = "RabbitCMS",
                    Seo_Keywords = "RabbitCMS,RabbitHub,Rabbit博客,兔子博客",
                    Seo_Description = "这是一个个人博客。"
                });
        }

        public override void Down()
        {
            Delete.Table(TableName("SiteSettingsRecord"));
        }

        #endregion Overrides of MigrationBase
    }
}