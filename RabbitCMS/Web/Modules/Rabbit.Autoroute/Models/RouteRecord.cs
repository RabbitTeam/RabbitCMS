using Rabbit.Components.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.Autoroute.Models
{
    [Entity]
    public class RouteRecord
    {
        /// <summary>
        /// 标识。
        /// </summary>
        public string Id { get; set; }

        [DisplayName("路由路径"), Required, StringLength(500)]
        public string Path { get; set; }

        public static RouteRecord Create(string path = null)
        {
            return new RouteRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                Path = path
            };
        }
    }
}