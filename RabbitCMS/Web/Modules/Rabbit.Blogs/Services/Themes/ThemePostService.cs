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

        IQueryable<PostRecord> GetMostReadList(int? count = null);

        PostRecord Read(string routePath, string categoryRoutePath = null);

        IQueryable<PostRecord> GetListByTag(string tag);

        IQueryable<PostRecord> GetListByAuthor(string author);

        IQueryable<PostRecord> GetListByTitleKeywords(string titleKeywords);
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
            return DefaultTable();
        }

        public IQueryable<PostRecord> GetListByCategory(string categoryRoutePath)
        {
            var category = _categoryService.Get(categoryRoutePath);
            return category == null ? null : DefaultTable().Where(i => i.Categorys.Any(z => z.Id == category.Id));
        }

        public IQueryable<PostRecord> GetHomeList()
        {
            return DefaultTable().Where(i => i.ShowInIndex);
        }

        public IQueryable<PostRecord> GetMostReadList(int? count = null)
        {
            IQueryable<PostRecord> table = Table().OrderByDescending(i => i.ReadingCount).ThenByDescending(i => i.CreateTime);
            if (count.HasValue)
                table = table.Take(count.Value);
            return table;
        }

        public PostRecord Read(string routePath, string categoryRoutePath = null)
        {
            var table = DefaultTable();
            if (!string.IsNullOrEmpty(categoryRoutePath))
                table = table.Where(i => i.Categorys.Any(z => z.Seo.RoutePath == categoryRoutePath));
            table = table.Where(i => i.Seo.RoutePath == routePath);

            var record = table.FirstOrDefault();
            if (record == null)
                return null;
            return record.Read();
        }

        public IQueryable<PostRecord> GetListByTag(string tag)
        {
            tag = tag.ToLower();
            return DefaultTable().Where(i => ("," + i.Tags.ToLower() + ",").Contains("," + tag + ","));
        }

        public IQueryable<PostRecord> GetListByAuthor(string author)
        {
            author = author.ToLower();
            return DefaultTable().Where(i => i.User.Name.ToLower() == author);
        }

        public IQueryable<PostRecord> GetListByTitleKeywords(string titleKeywords)
        {
            return DefaultTable().Where(i => i.Title.Contains(titleKeywords));
        }

        #endregion Implementation of IThemePostService

        #region Private Method

        private IQueryable<PostRecord> Table()
        {
            return _repository.Value.Table.Where(i => i.Status == PostStatus.Publish);
        }

        private IQueryable<PostRecord> DefaultTable()
        {
            return Table().OrderByDescending(i => i.CreateTime);
        }

        #endregion Private Method
    }
}