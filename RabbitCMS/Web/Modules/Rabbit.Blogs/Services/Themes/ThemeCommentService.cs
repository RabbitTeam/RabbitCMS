using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Rabbit.Blogs.Services.Themes
{
    public interface IThemeCommentService : IDependency
    {
        IQueryable<PostCommentRecord> GetNewestList(int? count = null);

        void Add(PostCommentRecord record);

        IQueryable<PostCommentRecord> GetListByPost(string postId);

        void Delete(string id);

        Task<bool> Exist(string id);
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

        public void Add(PostCommentRecord record)
        {
            _repository.Value.Create(record);
        }

        public IQueryable<PostCommentRecord> GetListByPost(string postId)
        {
            return Table().OrderBy(i => i.CreateTime).Where(i => i.Post.Id == postId);
        }

        public void Delete(string id)
        {
            var repository = _repository.Value;
            repository.Delete(i => i.Id == id);
        }

        public Task<bool> Exist(string id)
        {
            return _repository.Value.Table.AnyAsync(i => i.Id == id);
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