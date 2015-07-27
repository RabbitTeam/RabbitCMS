using Rabbit.Blogs.Models;
using Rabbit.Blogs.Services;
using Rabbit.Blogs.ViewModels;
using Rabbit.Infrastructures.Data;
using Rabbit.Infrastructures.Mvc;
using Rabbit.Web.Mvc.UI.Admin;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers
{
    [Admin]
    public class CategoryAdminController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryAdminController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DataSource(int pageIndex, int pageSize, string titleKeywords)
        {
            var table = _categoryService.GetList(titleKeywords);

            var pageParameter = new PageParameter(pageIndex, pageSize);
            table = pageParameter.Paged(table);
            var model = table.Select(i => new CategoryListViewModel
            {
                Id = i.Id,
                PostCount = i.Posts.Count(),
                Title = i.Title,
                Visible = i.Visible,
                Seo = i.Seo,
                RoutePath = i.Route.Path
            }).ToArray();

            return Json(new { pageParameter.PageCount, list = model });
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEdit(CategoryEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return this.Error("数据非法！");

                var record = await _categoryService.Get(model.Id);

                var isAdd = record == null;

                if (record == null)
                    record = PostCategoryRecord.Create(model.Id);

                model.UpdateRecord(record);

                if (isAdd)
                    await _categoryService.Add(record);

                return this.Success();
            }
            catch (ValidationException validationException)
            {
                return this.Error(validationException.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            await _categoryService.Delete(id);

            return this.Success();
        }
    }
}