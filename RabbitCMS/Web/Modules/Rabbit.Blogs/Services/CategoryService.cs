using Rabbit.Autoroute.Services;
using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        Task Delete(string id);

        Task Add(PostCategoryRecord record);
    }

    internal sealed class CategoryService : ICategoryService
    {
        private readonly Lazy<IRepository<PostCategoryRecord>> _repository;
        private readonly IRouteService _routeService;

        public CategoryService(Lazy<IRepository<PostCategoryRecord>> repository, IRouteService routeService)
        {
            _repository = repository;
            _routeService = routeService;
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

        public async Task Delete(string id)
        {
            var repository = _repository.Value;
            var record = await repository.Table.FirstOrDefaultAsync(i => i.Id == id);
            if (record == null)
                return;
            _routeService.DeleteByPath(record.Route.Path);
            repository.Delete(record);
        }

        public async Task Add(PostCategoryRecord record)
        {
            if (await _routeService.ExistByPath(record.Route.Path))
                throw new ValidationException($"路由路径 '{record.Route.Path}' 已经存在！");

            _repository.Value.Create(record);
        }

        #endregion Implementation of ICategoryService
    }
}