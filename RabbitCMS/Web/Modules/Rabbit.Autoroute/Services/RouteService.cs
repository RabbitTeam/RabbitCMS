using Rabbit.Autoroute.Models;
using Rabbit.Components.Data;
using Rabbit.Kernel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Rabbit.Autoroute.Services
{
    public interface IRouteService : IDependency
    {
        Task Add(RouteRecord record);

        void DeleteByPath(string path);

        Task<bool> ExistByPath(string path);
    }

    internal sealed class RouteService : IRouteService
    {
        private readonly Lazy<IRepository<RouteRecord>> _repository;

        public RouteService(Lazy<IRepository<RouteRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IRouteService

        public async Task Add(RouteRecord record)
        {
            record.Path = record.Path.ToLower();
            if (await ExistByPath(record.Path))
                throw new ValidationException($"路由路径 '{record.Path}' 已经存在！");
            _repository.Value.Create(record);
        }

        public void DeleteByPath(string path)
        {
            path = path.ToLower();
            _repository.Value.Delete(i => i.Path == path);
        }

        public Task<bool> ExistByPath(string path)
        {
            path = path.ToLower();
            return _repository.Value.Table.AnyAsync(i => i.Path == path);
        }

        #endregion Implementation of IRouteService
    }
}