using Rabbit.Autoroute.Services;
using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Rabbit.Blogs.Services
{
    public interface IPostService : IDependency
    {
        IQueryable<PostRecord> GetList(string titleKeywords = null, string category = null);

        Task<PostRecord> Get(string id);

        Task Delete(string id);

        Task Add(PostRecord record);

        Task<bool> Exist(string id);
    }

    internal sealed class PostService : IPostService
    {
        private readonly Lazy<IRepository<PostRecord>> _repository;
        private readonly ICommentService _commentService;
        private readonly IRouteService _routeService;

        public PostService(Lazy<IRepository<PostRecord>> repository, ICommentService commentService, IRouteService routeService)
        {
            _repository = repository;
            _commentService = commentService;
            _routeService = routeService;
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

        public Task<PostRecord> Get(string id)
        {
            return id == null ? Task.FromResult<PostRecord>(null) : _repository.Value.Table.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task Delete(string id)
        {
            var repository = _repository.Value;
            var record = await repository.Table.FirstOrDefaultAsync(i => i.Id == id);
            if (record == null)
                return;
            //删除评论。
            _commentService.DeleteByPost(record.Id);
            //删除路由。
            _routeService.DeleteByPath(record.Route.Path);
            repository.Delete(record);
        }

        public async Task Add(PostRecord record)
        {
            if (await _routeService.ExistByPath(record.Route.Path))
                throw new ValidationException($"路由路径 '{record.Route.Path}' 已经存在！");

            _repository.Value.Create(record);
        }

        public Task<bool> Exist(string id)
        {
            return _repository.Value.Table.AnyAsync(i => i.Id == id);
        }

        #endregion Implementation of ICategoryService
    }
}