using Rabbit.Components.Data;
using Rabbit.Contents.Models;
using Rabbit.Kernel;
using Rabbit.Kernel.Caching;
using System;
using System.Linq;

namespace Rabbit.Contents.Services
{
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