using Rabbit.Blogs.Models;
using Rabbit.Blogs.Services;
using Rabbit.Blogs.ViewModels;
using Rabbit.Components.Security;
using Rabbit.Infrastructures.Data;
using Rabbit.Infrastructures.Mvc;
using Rabbit.Users.Services;
using Rabbit.Web.Mvc.UI.Admin;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers
{
    [Admin]
    public class PostAdminController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICategoryService _categoryService;

        public PostAdminController(IPostService postService, IUserService userService, IAuthenticationService authenticationService, ICategoryService categoryService)
        {
            _postService = postService;
            _userService = userService;
            _authenticationService = authenticationService;
            _categoryService = categoryService;
        }

        // GET: PostAdmin
        public ActionResult Index(string category)
        {
            ViewBag.Category = category;
            return View();
        }

        [HttpPost]
        public ActionResult DataSource(int pageIndex, int pageSize, string titleKeywords, string category)
        {
            var table = _postService.GetList(titleKeywords, category);

            var pageParameter = new PageParameter(pageIndex, pageSize);
            table = pageParameter.Paged(table);
            var model = table.Select(i => new PostListViewModel
            {
                Id = i.Id,
                CommentCount = i.Comments.Count(),
                CreateTime = i.CreateTime,
                ReadingCount = i.ReadingCount,
                Status = i.Status,
                Title = i.Title
            }).ToArray();

            return Json(new { pageParameter.PageCount, list = model });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            await _postService.Delete(id);

            return this.Success();
        }

        public ActionResult Add()
        {
            return View("Edit", new PostEditViewModel
            {
                AllowComment = true,
                ShowInIndex = true,
                IsPublish = true
            });
        }

        public async Task<ActionResult> Edit(string id)
        {
            var record = await _postService.Get(id);
            return View((PostEditViewModel)record);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public Task<ActionResult> Add(PostEditViewModel model)
        {
            return AddOrEdit(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public Task<ActionResult> Edit(PostEditViewModel model)
        {
            return AddOrEdit(model);
        }

        [HttpPost]
        private async Task<ActionResult> AddOrEdit(PostEditViewModel model)
        {
            try
            {
                var record = await _postService.Get(model.Id);
                var isAdd = record == null;
                if (record == null)
                    record = PostRecord.Create(await _userService.GetUserById(_authenticationService.GetAuthenticatedUser().Identity));

                model.UpdateRecord(record, _categoryService);

                if (isAdd)
                    await _postService.Add(record);

                return this.Success();
            }
            catch (ValidationException validationException)
            {
                return this.Error(validationException.Message);
            }
        }
    }
}