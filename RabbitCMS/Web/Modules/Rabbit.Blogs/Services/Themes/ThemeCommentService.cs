using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Linq;

namespace Rabbit.Blogs.Services.Themes
{
    public interface IThemeCommentService : IDependency
    {
        IQueryable<PostCommentRecord> GetNewestList(int? count = null);
    }

    internal sealed class ThemeCommentService : IThemeCommentService
    {
        private readonly Lazy<IRepository<PostCommentRecord>> _repository;

        public ThemeCommentService(Lazy<IRepository<PostCommentRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IThemeCommentService

        public IQueryable<PostCommentRecord> GetNewestList(int? count)
        {
            IQueryable<PostCommentRecord> table = Table().OrderByDescending(i => i.CreateTime);
            if (count.HasValue)
                table = table.Take(count.Value);
            return table;
        }

        #endregion Implementation of IThemeCommentService

        #region Private Method

        private IQueryable<PostCommentRecord> Table()
        {
            return _repository.Value.Table.Where(i => i.Post.Status == PostStatus.Publish);
        }

        #endregion Private Method
    }
}