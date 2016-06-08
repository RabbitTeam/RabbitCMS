using Jkzl.Activitys.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Jkzl.Activitys.Services
{
    public interface ILotteryService : IDependency
    {
        Task<PrizeRecord> Lottery(long userId);

        Task<int> GetLotteryCount(long userId);

        Task<bool> AllowLottery(long userId);

        Task<PrizeRecord> GetHitPrize(long userId);

        Task<PrizeRecord> GetHitPrize(string mobileNumber);
    }

    public class LotteryService : ILotteryService
    {
        private readonly IActivityService _activityService;
        private readonly IPrizeService _prizeService;
        private readonly Lazy<IRepository<OrderRecord>> _orderRepository;
        private readonly IProbabilityService _probabilityService;
        private readonly IUserService _userService;
        private IRepository<OrderRecord> OrdeRepository => _orderRepository.Value;

        public LotteryService(IActivityService activityService, IPrizeService prizeService, Lazy<IRepository<OrderRecord>> orderRepository, IProbabilityService probabilityService, IUserService userService)
        {
            _activityService = activityService;
            _prizeService = prizeService;
            _orderRepository = orderRepository;
            _probabilityService = probabilityService;
            _userService = userService;
        }

        #region Implementation of ILotteryService

        public async Task<PrizeRecord> Lottery(long userId)
        {
            if (!await AllowLottery(userId))
                return null;

            var table = await _prizeService.GetPrizes();
            var prizes = table.Where(i => i.Remain > 0 && i.Total != null).OrderBy(i => i.Probability).ThenByDescending(i => i.Id);

            var user = await _userService.GetUser(userId);
            var hitCount = OrdeRepository.Table.Count();
            var noHitCount = await _userService.GetLotteryCount() - hitCount;
            var random = new Random();

            PrizeRecord prize = null;
            foreach (var prizeRecord in prizes)
            {
                var isHit = _probabilityService.IsHit(prizeRecord.Probability, hitCount, noHitCount, () => random.NextDouble());
                if (!isHit)
                    continue;
                prizeRecord.Remain = prizeRecord.Remain - 1;
                OrdeRepository.Create(new OrderRecord
                {
                    CreateTime = DateTime.Now,
                    IsReceive = false,
                    Prize = prizeRecord,
                    User = user
                });
                prize = prizeRecord;
                break;
            }
            user.LotteryCount = user.LotteryCount + 1;

            return prize;
        }

        public async Task<int> GetLotteryCount(long userId)
        {
            var user = await _userService.GetUser(userId);
            return user.LotteryCount;
        }

        public async Task<bool> AllowLottery(long userId)
        {
            if (await GetHitPrize(userId) != null)
            {
                return false;
            }
            var activity = await _activityService.GetActivity();
            if (!activity.MaxLotteryCount.HasValue)
                return true;
            return activity.MaxLotteryCount.Value - await GetLotteryCount(userId) > 0;
        }

        public async Task<PrizeRecord> GetHitPrize(string mobileNumber)
        {
            mobileNumber = mobileNumber.ToLower();
            var order = await OrdeRepository.Table.FirstOrDefaultAsync(i => i.User.MobileNumber == mobileNumber);
            return order?.Prize;
        }

        public async Task<PrizeRecord> GetHitPrize(long userId)
        {
            var order = await OrdeRepository.Table.FirstOrDefaultAsync(i => i.User.Id == userId);
            return order?.Prize;
        }

        #endregion Implementation of ILotteryService
    }
}