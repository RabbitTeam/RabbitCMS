using System;

namespace Jkzl.Activitys.Services
{
    internal sealed class ProbabilityService : IProbabilityService
    {
        #region Implementation of IProbabilityService

        /// <summary>
        /// 是否命中。
        /// </summary>
        /// <param name="probabilityPercentage">概率百分比（如概率为30%请传入30）。</param>
        /// <param name="getRandomNumber">获取随机数的委托（一般为Random.NextDouble()）。</param>
        /// <returns>是否命中。</returns>
        public bool IsHit(double probabilityPercentage, Func<double> getRandomNumber)
        {
            //如果概率大等于100则每次都命中。
            if (probabilityPercentage >= 100)
                return true;

            //得到概率的百分比。
            probabilityPercentage = probabilityPercentage / 100;

            return InternalIsHit(probabilityPercentage, getRandomNumber);
        }

        /// <summary>
        /// 是否命中。
        /// </summary>
        /// <param name="probabilityPercentage">概率百分比（如概率为30%请传入30）。</param>
        /// <param name="hasHitCount">命中次数。</param>
        /// <param name="noHitCount">没有命中的次数。</param>
        /// <param name="getRandomNumber">获取随机数的委托（一般为Random.NextDouble()）。</param>
        /// <returns>是否命中。</returns>
        public bool IsHit(double probabilityPercentage, int hasHitCount, int noHitCount, Func<double> getRandomNumber)
        {
            //如果概率大等于100则每次都命中。
            if (probabilityPercentage >= 100)
                return true;

            //得到概率的百分比。
            probabilityPercentage = probabilityPercentage / 100;

            //得到总计算次数。
            var totalCount = (double)(hasHitCount + noHitCount);

            //得到当前命中的概率。
            var currentProbability = hasHitCount / totalCount;

            //如果当前命中的概率大于传入的概率则不会命中。
            if (currentProbability > probabilityPercentage)
                return false;

            return InternalIsHit(probabilityPercentage, getRandomNumber);
        }

        #endregion Implementation of IProbabilityService

        #region Private Method

        /// <summary>
        /// 是否命中。
        /// </summary>
        /// <param name="probabilityPercentage">不需要处理的百分比（介于 0.0 和 1.0 之间的数）。</param>
        /// <param name="getRandomNumber">获取随机数的委托（一般为Random.NextDouble()）。</param>
        /// <returns>是否命中。</returns>
        private static bool InternalIsHit(double probabilityPercentage, Func<double> getRandomNumber)
        {
            //得到一个随机数。
            var randomNumber = getRandomNumber();

            if (randomNumber < 0 || randomNumber >= 1)
                throw new ArgumentException("随机数必须是一个介于 0.0 和 1.0 之间的数。");

            //取后15位
            const int places = 15;

            //精简小数位，提升概率准确性。
            randomNumber = GetNumber(randomNumber, places);

            return probabilityPercentage > randomNumber;
        }

        /// <summary>
        /// 精简数字的小数位。
        /// </summary>
        /// <param name="number">数字。</param>
        /// <param name="places">小数位。</param>
        /// <returns>精简小数位后的数字。</returns>
        private static double GetNumber(double number, int places)
        {
            //精简小数位，提升概率准确性。
            return Math.Round(number, places);

            //该方法会提高准确性但会影响性能，适用于高精度场景。
            /*var str = number.ToString(CultureInfo.InvariantCulture);
            if (!str.Contains("."))
                return number;
            var t = str.Split('.');
            number = double.Parse(t[0] + "." + string.Join("", t[1].Take(places)));
            return number;*/
        }

        #endregion Private Method
    }
}