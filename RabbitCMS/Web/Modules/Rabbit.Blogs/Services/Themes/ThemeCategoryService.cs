using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Rabbit.Blogs.Services.Themes
{
    public interface IThemeCategoryService : IDependency
    {
        IQueryable<PostCategoryRecord> GetList();

        Task<PostCategoryRecord> Get(string routePath);
    }

    internal sealed class ThemeCategoryService : IThemeCategoryService
    {
        private readonly Lazy<IRepository<PostCategoryRecord>> _repository;

        public ThemeCategoryService(Lazy<IRepository<PostCategoryRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IThemeCategoryService

        public IQueryable<PostCategoryRecord> GetList()
        {
            return _repository.Value.Table.Where(i => i.Visible).OrderByDescending(i => i.CreateTime);
        }

        public Task<PostCategoryRecord> Get(string routePath)
        {
            routePath = routePath.ToLower();
            return _repository.Value.Table.FirstOrDefaultAsync(i => i.Route.Path == routePath);
        }

        #endregion Implementation of IThemeCategoryService
    }
}