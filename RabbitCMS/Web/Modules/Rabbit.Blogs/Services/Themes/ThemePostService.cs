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

        IQueryable<PostRecord> GetHomeList();

        PostRecord Read(string routePath, string categoryRoutePath = null);
    }

    internal sealed class ThemePostService : IThemePostService
    {
        private readonly Lazy<IRepository<PostRecord>> _repository;

        public ThemePostService(Lazy<IRepository<PostRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IThemePostService

        public IQueryable<PostRecord> GetList()
        {
            return Table();
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