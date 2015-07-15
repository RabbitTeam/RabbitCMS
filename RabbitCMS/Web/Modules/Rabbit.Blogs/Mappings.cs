using Rabbit.Blogs.Models;
using Rabbit.Components.Data.EntityFramework;
using System.Data.Entity.ModelConfiguration;

namespace Rabbit.Blogs
{
    internal sealed class PostRecordMapping : MappingBase<PostRecord>
    {
        #region Overrides of MappingBase<PostRecord>

        /// <summary>
        /// 映射动作。
        /// </summary>
        /// <param name="configuration">实体类型配置对象。</param>
        public override void Mapping(EntityTypeConfiguration<PostRecord> configuration)
        {
            configuration
                .HasMany(i => i.Categorys)
                .WithMany(i => i.Posts)
                .Map(i => i.ToTable("PostCategoryRecordPostRecords"));
        }

        #endregion Overrides of MappingBase<PostRecord>
    }
}