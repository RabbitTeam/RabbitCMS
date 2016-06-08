using Jkzl.Activitys.Services;
using Jkzl.Activitys.ViewModels;
using Newtonsoft.Json;
using Rabbit.Infrastructures.Mvc;
using Rabbit.Web.Mvc.UI.Admin;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Jkzl.Activitys.Controllers
{
    [Admin]
    public class DaZhuanPanAdminController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IPrizeService _prizeService;

        public DaZhuanPanAdminController(IActivityService activityService, IPrizeService prizeService)
        {
            _activityService = activityService;
            _prizeService = prizeService;
        }

        // GET: DaZhuanPnAdmin
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Settings()
        {
            var model = (ActivitySettingsViewModel)await _activityService.GetActivity();
            return View(model);
        }

        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Settings(ActivitySettingsViewModel model)
        {
            if (!ModelState.IsValid)
                return this.Error("数据非法！");

            var record = await _activityService.GetActivity();
            model.Update(record);
            return this.Success();
        }

        public async Task<ActionResult> PrizeSettings()
        {
            var model = (await _prizeService.GetPrizes()).Select(record => new PrizeViewModel
            {
                Id = record.Id,
                ImagePath = record.ImagePath,
                Name = record.Name,
                Probability = record.Probability,
                Total = record.Total,
                Remain = record.Remain
            }).ToArray();
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Update(string json)
        {
            if (!ModelState.IsValid)
                return this.Error("数据非法！");

            var array = await JsonConvert.DeserializeObjectAsync<PrizeViewModel[]>(json);

            foreach (var model in array)
            {
                var record = await _prizeService.Get(model.Id);
                model.Update(record);
            }
            return this.Success();
        }
    }
}