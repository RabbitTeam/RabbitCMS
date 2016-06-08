using Rabbit.Kernel;
using System;

namespace Jkzl.Activitys.Services
{
    /// <summary>
    /// 一个抽象的概率服务。
    /// </summary>
    public interface IProbabilityService : ISingletonDependency
    {
        /// <summary>
        /// 是否命中。
        /// </summary>
        /// <param name="probabilityPercentage">概率百分比（如概率为30%请传入30）。</param>
        /// <param name="getRandomNumber">获取随机数的委托（一般为Random.NextDouble()）。</param>
        /// <returns>是否命中。</returns>
        bool IsHit(double probabilityPercentage, Func<double> getRandomNumber);

        /// <summary>
        /// 是否命中。
        /// </summary>
        /// <param name="probabilityPercentage">概率百分比（如概率为30%请传入30）。</param>
        /// <param name="hasHitCount">命中次数。</param>
        /// <param name="noHitCount">没有命中的次数。</param>
        /// <param name="getRandomNumber">获取随机数的委托（一般为Random.NextDouble()）。</param>
        /// <returns>是否命中。</returns>
        bool IsHit(double probabilityPercentage, int hasHitCount, int noHitCount, Func<double> getRandomNumber);
    }
}