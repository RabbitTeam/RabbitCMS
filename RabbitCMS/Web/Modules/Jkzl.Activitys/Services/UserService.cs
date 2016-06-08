using Jkzl.Activitys.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Jkzl.Activitys.Services
{
    public interface IUserService : IDependency
    {
        Task<AppUserRecord> GetOrCreateUser(string mobileNumber);

        Task<AppUserRecord> GetUser(long userId);

        Task<int> GetLotteryCount();
    }

    public class UserService : IUserService
    {
        private readonly Lazy<IRepository<AppUserRecord>> _repository;
        private IRepository<AppUserRecord> Repository => _repository.Value;

        public UserService(Lazy<IRepository<AppUserRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IUserService

        public async Task<AppUserRecord> GetOrCreateUser(string mobileNumber)
        {
            mobileNumber = mobileNumber.ToLower();
            var record = await Repository.Table.SingleOrDefaultAsync(
                i => i.MobileNumber == mobileNumber);
            if (record != null)
            {
                return record;
            }
            var user = new AppUserRecord
            {
                MobileNumber = mobileNumber
            };
            Repository.Create(user);
            await Repository.FlushAsync();
            return user;
        }

        public Task<AppUserRecord> GetUser(long userId)
        {
            return Repository.Table.SingleOrDefaultAsync(i => i.Id == userId);
        }

        public Task<int> GetLotteryCount()
        {
            return Repository.Table.SumAsync(i => i.LotteryCount);
        }

        #endregion Implementation of IUserService
    }
}