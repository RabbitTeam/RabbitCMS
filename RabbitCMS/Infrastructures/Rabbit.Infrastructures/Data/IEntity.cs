using System;

namespace Rabbit.Infrastructures.Data
{
    public interface IEntity
    {
        /// <summary>
        /// 标识。
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}