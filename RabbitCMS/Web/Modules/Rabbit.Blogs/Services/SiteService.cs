using Rabbit.Components.Data;
using Rabbit.Components.Data.DataAnnotations;
using Rabbit.Kernel;
using Rabbit.Kernel.Caching;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rabbit.Blogs.Services
{
    [Entity]
    public class SiteSettingsRecord
    {
        [Required]
        public string Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string ThemeName { get; set; }

        [StringLength(500)]
        public string Copyright { get; set; }

        public SeoModelFull Seo { get; set; }
    }

    [ComplexType]
    public sealed class SeoModelFull
    {
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Keywords { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }

    public interface ISiteSettingsService : IDependency
    {
        SiteSettingsRecord Get();

        void Update(SiteSettingsRecord settings);
    }

    internal sealed class SiteSettingsService : ISiteSettingsService
    {
        private readonly Lazy<IRepository<SiteSettingsRecord>> _repository;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public SiteSettingsService(Lazy<IRepository<SiteSettingsRecord>> repository, ICacheManager cacheManager, ISignals signals)
        {
            _repository = repository;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        #region Implementation of ISiteSettingsService

        public SiteSettingsRecord Get()
        {
            return _cacheManager.Get("SiteSettings", ctx =>
            {
                ctx.Monitor(_signals.When("SiteSettings.Change"));
                var repository = _repository.Value;
                var record = repository.Table.Single();
                return record;
            });
        }

        public void Update(SiteSettingsRecord settings)
        {
            _signals.Trigger("SiteSettings.Change");
        }

        #endregion Implementation of ISiteSettingsService
    }
}