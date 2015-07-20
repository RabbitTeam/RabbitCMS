using FluentMigrator;
using Rabbit.Blogs.Models;
using Rabbit.Users.Models;
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
                .WithColumn("Seo_Description").AsString(255).Nullable();

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
                .WithColumn("Author").AsString(50).Nullable();

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
        }

        public override void Down()
        {
            Delete.Table(TableName("PostCategoryRecordPostRecords"));
            Delete.Table(TableName("PostCommentRecord"));
            Delete.Table(TableName("PostCategoryRecord"));
            Delete.Table(TableName("PostRecord"));
        }

        #endregion Overrides of MigrationBase
    }
}