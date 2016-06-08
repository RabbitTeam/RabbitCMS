using Jkzl.Activitys.Services;
using Jkzl.Activitys.Tool;
using Jkzl.Activitys.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rabbit.Infrastructures.Mvc;
using Rabbit.Kernel.Environment.Configuration;
using Rabbit.Web.Mvc.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Jkzl.Activitys.Controllers
{
    [Themed(false)]
    public class DaZhuanPanController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IPrizeService _prizeService;
        private readonly ILotteryService _lotteryService;
        private readonly IUserService _userService;
        private readonly ShellSettings _shellSettings;

        public DaZhuanPanController(IActivityService activityService, IPrizeService prizeService, ILotteryService lotteryService, IUserService userService, ShellSettings shellSettings)
        {
            _activityService = activityService;
            _prizeService = prizeService;
            _lotteryService = lotteryService;
            _userService = userService;
            _shellSettings = shellSettings;
        }

        public async Task<ActionResult> Index()
        {
            var prizes = (await _prizeService.GetPrizes()).Select(record => new PrizeViewModel
            {
                Id = record.Id,
                ImagePath = record.ImagePath,
                Name = record.Name,
                Probability = record.Probability,
                Total = record.Total
            }).ToArray();
            var activity = await _activityService.GetActivity();
            var model = new ActivityViewModel
            {
                Prizes = prizes,
                Description = activity.Description,
                StartTime = activity.StartTime,
                EndTime = activity.EndTime
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Lottery(long userId, string mappings)
        {
            var activity = await _activityService.GetActivity();

            if (DateTime.Now.Date < activity.StartTime)
                return this.Error("活动还没有开始，请在等等哦！");
            if (activity.EndTime.HasValue && DateTime.Now.Date > activity.EndTime.Value)
                return this.Error("活动已结束，下次请赶早哦！");

            var hitPrize = await _lotteryService.GetHitPrize(userId) != null;
            if (hitPrize)
                return this.Error("中奖用户不能重复参加！");
            if (!await _lotteryService.AllowLottery(userId))
                return this.Error("抽奖次数已用完，请下次再来哦！");

            var prize = await _lotteryService.Lottery(userId);

            if (prize == null)
                return this.Success(new { isHit = false, index = -1 });

            var dictionary = await JsonConvert.DeserializeObjectAsync<Dictionary<long, int>>(mappings);

            var index = dictionary.Single(i => i.Key == prize.Id);

            return this.Success(new { isHit = true, index = index.Value });
        }

        [HttpPost]
        public async Task<ActionResult> GetHitPrize(string mobileNumber)
        {
            var prize = await _lotteryService.GetHitPrize(mobileNumber);
            return prize == null ? this.Error() : this.Success(new { prize.Name });
        }

        [HttpPost]
        public async Task<ActionResult> SetUser(string mobileNumber)
        {
            if (string.IsNullOrWhiteSpace(mobileNumber) || mobileNumber.Length != 11)
                return this.Error("请输入正确的手机号码！");

            mobileNumber = mobileNumber.ToLower();
            if (!await IsValidUser(mobileNumber))
                return this.Error("该号码不是有效的用户手机号！");

            var user = await _userService.GetOrCreateUser(mobileNumber);
            return this.Success(new { id = user.Id });
        }

        #region Private Method

        private async Task<bool> IsValidUser(string mobileNumber)
        {
            return await Task.Run(() =>
            {
                var mobileNumberRegex = _shellSettings["MobileNumberRegex"];
                if (!string.IsNullOrWhiteSpace(mobileNumberRegex) && !Regex.IsMatch(mobileNumber, mobileNumberRegex))
                    return false;
                var json = JsonConvert.SerializeObject(new Dictionary<string, object>
                {
                    {"Loginid", mobileNumber},
                    {"Logintype", 1},
                    {"Version", "2"}
                });
                var bossUrl = _shellSettings["BossUrl"];
                var retJson = WebRequestHelper.GetHttpBossApiWeb
                    (bossUrl, "{\"ClientId\":9000050}",
                        DateTime.Now.ToString("yyyyMMddHHmmssfff"), "account.UserLogin.getAccLoginInfo",
                        json, 0, 0, "1.0");

                var obj = JObject.Parse(retJson);
                if (obj.Value<int>("Code") != 10000)
                    return false;

                return obj.Value<int>("Totalproperty") > 0;
            });
        }

        #endregion Private Method
    }
}