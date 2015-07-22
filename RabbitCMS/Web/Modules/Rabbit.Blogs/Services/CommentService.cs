using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Rabbit.Blogs.Services
{
    public interface ICommentService : IDependency
    {
        IQueryable<PostCommentRecord> GetNewestList(int? count = null);

        void Add(PostCommentRecord record);

        IQueryable<PostCommentRecord> GetListByPost(string postId);

        void Delete(string[] ids);

        Task<bool> Exist(string id);
    }

    internal sealed class CommentService : ICommentService
    {
        private readonly Lazy<IRepository<PostCommentRecord>> _repository;

        public CommentService(Lazy<IRepository<PostCommentRecord>> repository)
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
            IQueryable<PostCommentRecord> table = Table().OrderBy(i => i.CreateTime);
            if (!string.IsNullOrEmpty(postId))
                table = table.Where(i => i.Post.Id == postId);
            return table;
        }

        public void Delete(string[] ids)
        {
            var repository = _repository.Value;
            foreach (var id in ids)
            {
                repository.Delete(i => i.Id == id);
            }
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