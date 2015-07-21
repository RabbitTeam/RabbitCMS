using Rabbit.Contents.Services;
using Rabbit.Contents.ViewModels;
using Rabbit.Infrastructures.Mvc;
using Rabbit.Web.Mvc.UI.Admin;
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

        public ActionResult Index()
        {
            var model = (SiteSettingsViewModel)_siteSettingsService.Get();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Index")]
        public ActionResult IndexPost(SiteSettingsViewModel model)
        {
            var record = model.Set(_siteSettingsService.Get());
            _siteSettingsService.Update(record);
            return this.Success();
        }
    }
}