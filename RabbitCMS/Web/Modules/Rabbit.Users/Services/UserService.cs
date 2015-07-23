using Rabbit.Components.Data;
using Rabbit.Kernel;
using Rabbit.Users.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Rabbit.Users.Services
{
    public interface IUserService : IDependency
    {
        Task<UserRecord> GetUserByAccount(string account);

        Task<UserRecord> GetUserById(string id);
    }

    internal sealed class UserService : IUserService
    {
        private readonly Lazy<IRepository<UserRecord>> _repository;

        public UserService(Lazy<IRepository<UserRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IUserService

        public Task<UserRecord> GetUserByAccount(string account)
        {
            account = account.ToLower();
            return _repository.Value.Table.FirstOrDefaultAsync(i => i.Account.Account == account);
        }

        public Task<UserRecord> GetUserById(string id)
        {
            return _repository.Value.Table.FirstOrDefaultAsync(i => i.Id == id);
        }

        #endregion Implementation of IUserService
    }
}