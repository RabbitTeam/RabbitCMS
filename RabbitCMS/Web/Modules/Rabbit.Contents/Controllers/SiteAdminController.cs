using Rabbit.Contents.Services;
using Rabbit.Contents.ViewModels;
using Rabbit.Infrastructures.Mvc;
using Rabbit.Web.Mvc.UI.Admin;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Rabbit.Contents.Controllers
{
    [Admin]
    public class SiteAdminController : Controller
    {
        private readonly ISiteSettingsService _siteSettingsService;

        public SiteAdminController(ISiteSettingsService siteSettingsService)
        {
            _siteSettingsService = siteSettingsService;
        }

        public async Task<ActionResult> Index()
        {
            var model = (SiteSettingsViewModel)await _siteSettingsService.Get();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Index")]
        public async Task<ActionResult> IndexPost(SiteSettingsViewModel model)
        {
            var record = model.Set(await _siteSettingsService.Get());
            _siteSettingsService.Update(record);
            return this.Success();
        }
    }
}