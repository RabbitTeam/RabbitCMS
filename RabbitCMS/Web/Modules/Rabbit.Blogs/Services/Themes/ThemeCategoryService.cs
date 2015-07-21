using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Linq;

namespace Rabbit.Blogs.Services.Themes
{
    public interface IThemeCategoryService : IDependency
    {
        IQueryable<PostCategoryRecord> GetList();

        PostCategoryRecord Get(string routePath);
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

        public PostCategoryRecord Get(string routePath)
        {
            return _repository.Value.Table.FirstOrDefault(i => i.Seo.RoutePath == routePath);
        }

        #endregion Implementation of IThemeCategoryService
    }
}