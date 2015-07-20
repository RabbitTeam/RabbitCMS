using FluentMigrator;
using Rabbit.Blogs.Models;
using Rabbit.Blogs.Services;
using Rabbit.Users.Models;
using System;
using System.Data;
using MigrationBase = Rabbit.Components.Data.Migrators.MigrationBase;

namespace Rabbit.Blogs
{
    [Migration(20150715150330)]
    public class Migrators : MigrationBase
    {
        #region Overrides of MigrationBase

        public override void Up()
        {
            Create
                .Table(TableName<PostCategoryRecord>())
                .WithColumn("Id").AsAnsiString(32).PrimaryKey()
                .WithColumn("Title").AsString(50)
                .WithColumn("Description").AsString(500).Nullable()
                .WithColumn("Visible").AsBoolean()
                .WithColumn("CreateTime").AsDateTime()
                .WithColumn("Seo_Keywords").AsString(255).Nullable()
                .WithColumn("Seo_Description").AsString(255).Nullable()
                .WithColumn("Seo_RoutePath").AsString(50);

            Create
                .Table(TableName<PostRecord>())
                .WithColumn("Id").AsAnsiString(32).PrimaryKey()
                .WithColumn("Title").AsString(200)
                .WithColumn("Content").AsString(int.MaxValue)
                .WithColumn("Summary").AsString(5000).Nullable()
                .WithColumn("User_Id").AsAnsiString(32).ForeignKey(TableName<UserRecord>(), "Id").OnDelete(Rule.Cascade)
                .WithColumn("Status").AsInt32()
                .WithColumn("ShowInIndex").AsBoolean()
                .WithColumn("AllowComment").AsBoolean()
                .WithColumn("Tags").AsString(200).Nullable()
                .WithColumn("RecommendationCount").AsInt32()
                .WithColumn("OppositionCount").AsInt32()
                .WithColumn("ReadingCount").AsInt32()
                .WithColumn("CreateTime").AsDateTime()
                .WithColumn("Seo_Keywords").AsString(255).Nullable()
                .WithColumn("Seo_Description").AsString(255).Nullable()
                .WithColumn("Seo_RoutePath").AsString(50);

            Create.Table("PostCategoryRecordPostRecords")
                .WithColumn("PostCategoryRecord_Id").AsAnsiString(32).PrimaryKey().ForeignKey(TableName<PostCategoryRecord>(), "Id").OnDelete(Rule.Cascade)
                .WithColumn("PostRecord_Id").AsAnsiString(32).PrimaryKey().ForeignKey(TableName<PostRecord>(), "Id").OnDelete(Rule.Cascade);

            Create
                .Table(TableName<PostCommentRecord>())
                .WithColumn("Id").AsAnsiString(32).PrimaryKey()
                .WithColumn("NickName").AsString(20)
                .WithColumn("Content").AsString(8000)
                .WithColumn("Post_Id").AsAnsiString(32).ForeignKey(TableName<PostRecord>(), "Id")
                .WithColumn("CreateTime").AsDateTime();

            Create
                .Table(TableName<SiteSettingsRecord>())
                .WithColumn("Id").AsAnsiString(32).PrimaryKey()
                .WithColumn("Name").AsString(100)
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
                    ThemeName = "Default_TheThemeMachine",
                    Copyright = "Copyright &copy; 2015 RabbitHub. All rights reserved.",
                    Seo_Title = "RabbitCMS",
                    Seo_Keywords = "RabbitCMS,RabbitHub,Rabbit博客,兔子博客",
                    Seo_Description = "这是一个个人博客。"
                });
        }

        public override void Down()
        {
            Delete.Table(TableName("PostCategoryRecordPostRecords"));
            Delete.Table(TableName("PostCommentRecord"));
            Delete.Table(TableName("PostCategoryRecord"));
            Delete.Table(TableName("PostRecord"));
            Delete.Table(TableName("SiteSettingsRecord"));
        }

        #endregion Overrides of MigrationBase
    }
}