using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Linq;

namespace Rabbit.Blogs.Services
{
    public interface IPostService : IDependency
    {
        IQueryable<PostRecord> GetList(string titleKeywords = null, string category = null);

        PostRecord Get(string id);

        void Delete(string id);

        void Add(PostRecord record);
    }

    internal sealed class PostService : IPostService
    {
        private readonly Lazy<IRepository<PostRecord>> _repository;

        public PostService(Lazy<IRepository<PostRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of ICategoryService

        public IQueryable<PostRecord> GetList(string titleKeywords, string category = null)
        {
            var table = _repository.Value.Table;

            if (!string.IsNullOrWhiteSpace(titleKeywords))
                table = table.Where(i => i.Title.Contains(titleKeywords));
            if (!string.IsNullOrWhiteSpace(category))
                table = table.Where(i => i.Categorys.Any(z => z.Id == category));

            return table.OrderByDescending(i => i.CreateTime);
        }

        public PostRecord Get(string id)
        {
            return id == null ? null : _repository.Value.Table.FirstOrDefault(i => i.Id == id);
        }

        public void Delete(string id)
        {
            _repository.Value.Delete(i => i.Id == id);
        }

        public void Add(PostRecord record)
        {
            _repository.Value.Create(record);
        }

        #endregion Implementation of ICategoryService
    }
}