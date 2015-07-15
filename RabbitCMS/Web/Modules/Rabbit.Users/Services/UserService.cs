using Rabbit.Components.Data;
using Rabbit.Kernel;
using Rabbit.Users.Models;
using System;
using System.Linq;

namespace Rabbit.Users.Services
{
    public interface IUserService : IDependency
    {
        UserRecord GetUserByAccount(string account);

        UserRecord GetUserById(string id);
    }

    internal sealed class UserService : IUserService
    {
        private readonly Lazy<IRepository<UserRecord>> _repository;

        public UserService(Lazy<IRepository<UserRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IUserService

        public UserRecord GetUserByAccount(string account)
        {
            account = account.ToLower();
            return _repository.Value.Table.FirstOrDefault(i => i.Account.Account == account);
        }

        public UserRecord GetUserById(string id)
        {
            return _repository.Value.Table.FirstOrDefault(i => i.Id == id);
        }

        #endregion Implementation of IUserService
    }
}