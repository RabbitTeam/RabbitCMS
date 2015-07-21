using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Linq;

namespace Rabbit.Blogs.Services.Themes
{
    public interface IThemePostService : IDependency
    {
        IQueryable<PostRecord> GetList();

        IQueryable<PostRecord> GetListByCategory(string categoryRoutePath);

        IQueryable<PostRecord> GetHomeList();

        PostRecord Read(string routePath, string categoryRoutePath = null);
    }

    internal sealed class ThemePostService : IThemePostService
    {
        private readonly Lazy<IRepository<PostRecord>> _repository;
        private readonly IThemeCategoryService _categoryService;

        public ThemePostService(Lazy<IRepository<PostRecord>> repository, IThemeCategoryService categoryService)
        {
            _repository = repository;
            _categoryService = categoryService;
        }

        #region Implementation of IThemePostService

        public IQueryable<PostRecord> GetList()
        {
            return Table();
        }

        public IQueryable<PostRecord> GetListByCategory(string categoryRoutePath)
        {
            var category = _categoryService.Get(categoryRoutePath);
            return category == null ? null : Table().Where(i => i.Categorys.Any(z => z.Id == category.Id));
        }

        public IQueryable<PostRecord> GetHomeList()
        {
            return Table().Where(i => i.ShowInIndex);
        }

        public PostRecord Read(string routePath, string categoryRoutePath = null)
        {
            var table = Table();
            if (!string.IsNullOrEmpty(categoryRoutePath))
                table = table.Where(i => i.Categorys.Any(z => z.Seo.RoutePath == categoryRoutePath));
            table = table.Where(i => i.Seo.RoutePath == routePath);

            var record = table.FirstOrDefault();
            if (record == null)
                return null;
            return record.Read();
        }

        #endregion Implementation of IThemePostService

        #region Private Method

        private IQueryable<PostRecord> Table()
        {
            return _repository.Value.Table.Where(i => i.Status == PostStatus.Publish).OrderByDescending(i => i.CreateTime);
        }

        #endregion Private Method
    }
}