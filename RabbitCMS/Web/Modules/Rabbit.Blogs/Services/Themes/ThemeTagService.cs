using Rabbit.Blogs.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Linq;

namespace Rabbit.Blogs.Services.Themes
{
    public interface IThemeTagService : IDependency
    {
        string[] GetTags(int? count);
    }

    internal sealed class ThemeTagService : IThemeTagService
    {
        private readonly Lazy<IRepository<PostRecord>> _repository;

        public ThemeTagService(Lazy<IRepository<PostRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IThemeTagService

        public string[] GetTags(int? count)
        {
            var tags = _repository.Value.Table.Where(i => i.Status == PostStatus.Publish && i.Tags != null && i.Tags != "")
                .Select(i => i.Tags).ToArray().SelectMany(i => i.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();

            tags = tags.Distinct().ToArray();

            if (count.HasValue)
                tags = tags.Take(count.Value).ToArray();
            return tags;
        }

        #endregion Implementation of IThemeTagService
    }
}