using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Rabbit.Blogs.Services
{
    public interface ICategoryService : IDependency
    {
        IQueryable<PostCategoryRecord> GetList(string titleKeywords = null);

        IQueryable<PostCategoryRecord> GetList(IEnumerable<string> categoryIds);

        Task<PostCategoryRecord> Get(string id);

        void Delete(string id);

        void Add(PostCategoryRecord record);
    }

    internal sealed class CategoryService : ICategoryService
    {
        private readonly Lazy<IRepository<PostCategoryRecord>> _repository;

        public CategoryService(Lazy<IRepository<PostCategoryRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of ICategoryService

        public IQueryable<PostCategoryRecord> GetList(string titleKeywords)
        {
            var table = _repository.Value.Table;

            if (!string.IsNullOrWhiteSpace(titleKeywords))
                table = table.Where(i => i.Title.Contains(titleKeywords));

            return table.OrderByDescending(i => i.CreateTime);
        }

        public IQueryable<PostCategoryRecord> GetList(IEnumerable<string> categoryIds)
        {
            var parameter = "," + string.Join(",", categoryIds) + ",";

            return _repository.Value.Table.Where(i => parameter.Contains("," + i.Id + ","));
        }

        public Task<PostCategoryRecord> Get(string id)
        {
            return id == null ? Task.FromResult<PostCategoryRecord>(null) : _repository.Value.Table.FirstOrDefaultAsync(i => i.Id == id);
        }

        public void Delete(string id)
        {
            _repository.Value.Delete(i => i.Id == id);
        }

        public void Add(PostCategoryRecord record)
        {
            _repository.Value.Create(record);
        }

        #endregion Implementation of ICategoryService
    }
}