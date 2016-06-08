using Jkzl.Activitys.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Jkzl.Activitys.Services
{
    public interface IActivityService : IDependency
    {
        Task<ActivityRecord> GetActivity();
    }

    public class ActivityService : IActivityService
    {
        private readonly Lazy<IRepository<ActivityRecord>> _repository;
        private IRepository<ActivityRecord> Repository => _repository.Value;

        public ActivityService(Lazy<IRepository<ActivityRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IActivityService

        public async Task<ActivityRecord> GetActivity()
        {
            var record = await Repository.Table.FirstOrDefaultAsync();
            if (record != null)
                return record;
            record = new ActivityRecord
            {
                Description = "活动说明",
                EndTime = new DateTime(2016, 6, 19),
                MaxLotteryCount = 3,
                StartTime = DateTime.Now.Date
            };
            Repository.Create(record);
            await Repository.FlushAsync();
            return record;
        }

        #endregion Implementation of IActivityService
    }
}