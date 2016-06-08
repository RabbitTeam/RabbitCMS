using Jkzl.Activitys.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Jkzl.Activitys.Services
{
    public interface IPrizeService : IDependency
    {
        Task<IQueryable<PrizeRecord>> GetPrizes();

        Task<PrizeRecord> Get(long id);
    }

    public class PrizeService : IPrizeService
    {
        private readonly Lazy<IRepository<PrizeRecord>> _repository;
        private readonly IActivityService _activityService;
        private IRepository<PrizeRecord> Repository => _repository.Value;

        public PrizeService(Lazy<IRepository<PrizeRecord>> repository, IActivityService activityService)
        {
            _repository = repository;
            _activityService = activityService;
        }

        #region Implementation of IPrizeService

        public async Task<IQueryable<PrizeRecord>> GetPrizes()
        {
            if (Repository.Table.Any())
                return Repository.Table.Take(8).OrderBy(i => i.Id).AsQueryable();

            var activity = await _activityService.GetActivity();
            var createEmptyPrize = new Func<PrizeRecord>(() => new PrizeRecord
            {
                Activity = activity,
                ImagePath = "~/Modules/Jkzl.Activitys/Content/images/test_xx.png",
                Name = "再接再厉",
                Probability = 100,
                Total = null,
                Remain = 0
            });
            var list = new List<PrizeRecord>
            {
                new PrizeRecord
                {
                    Activity = activity,
                    ImagePath = string.Empty,
                    Name = "一等奖",
                    Probability = 10,
                    Total = 10,
                    Remain = 10
                },
                createEmptyPrize(),
                new PrizeRecord
                {
                    Activity = activity,
                    ImagePath = string.Empty,
                    Name = "二等奖",
                    Probability = 20,
                    Total = 10,
                    Remain = 10
                },
                createEmptyPrize(),
                new PrizeRecord
                {
                    Activity = activity,
                    ImagePath = string.Empty,
                    Name = "三等奖",
                    Probability = 30,
                    Total = 10,
                    Remain = 10
                },
                createEmptyPrize(),
                new PrizeRecord
                {
                    Activity = activity,
                    ImagePath = string.Empty,
                    Name = "幸运奖",
                    Probability = 40,
                    Total = 10,
                    Remain = 10
                },
                createEmptyPrize()
            };
            Repository.CreateRange(list);
            await Repository.FlushAsync();
            return Repository.Table.Take(8).OrderBy(i => i.Id).AsQueryable();
        }

        public Task<PrizeRecord> Get(long id)
        {
            return Repository.Table.SingleOrDefaultAsync(i => i.Id == id);
        }

        #endregion Implementation of IPrizeService
    }
}