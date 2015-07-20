using Rabbit.Blogs.Models;
using Rabbit.Blogs.Services;
using Rabbit.Blogs.ViewModels;
using Rabbit.Infrastructures.Data;
using Rabbit.Infrastructures.Mvc;
using Rabbit.Web.Mvc.UI.Admin;
using System.Linq;
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
                Seo = i.Seo
            }).ToArray();

            return Json(new { pageParameter.PageCount, list = model });
        }

        [HttpPost]
        public ActionResult AddOrEdit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
                return this.Error("数据非法！");

            var record = _categoryService.Get(model.Id);

            var isAdd = record == null;

            if (record == null)
                record = PostCategoryRecord.Create(model.Id);

            record.Title = model.Title;
            record.Visible = model.Visible;
            record.Description = model.Description;
            record.Seo = model.Seo;

            if (isAdd)
                _categoryService.Add(record);

            return this.Success();
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            _categoryService.Delete(id);

            return this.Success();
        }
    }
}