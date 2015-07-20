using Rabbit.Blogs.Models;
using Rabbit.Blogs.Services;
using Rabbit.Blogs.ViewModels;
using Rabbit.Components.Security;
using Rabbit.Infrastructures.Data;
using Rabbit.Infrastructures.Mvc;
using Rabbit.Users.Services;
using Rabbit.Web.Mvc.UI.Admin;
using System;
using System.Linq;
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
        public ActionResult Delete(string id)
        {
            _postService.Delete(id);

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

        public ActionResult Edit(string id)
        {
            var record = _postService.Get(id);
            return View((PostEditViewModel)record);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Add(PostEditViewModel model)
        {
            return AddOrEdit(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(PostEditViewModel model)
        {
            return AddOrEdit(model);
        }

        [HttpPost]
        private ActionResult AddOrEdit(PostEditViewModel model)
        {
            var record = _postService.Get(model.Id);
            var isAdd = record == null;
            if (record == null)
                record = PostRecord.Create(_userService.GetUserById(_authenticationService.GetAuthenticatedUser().Identity));

            record.Status = model.IsPublish ? PostStatus.Publish : PostStatus.UnPublished;
            record.Content = model.Content;
            record.OppositionCount = model.OppositionCount;
            record.ReadingCount = model.ReadingCount;
            record.RecommendationCount = model.RecommendationCount;
            record.ShowInIndex = model.ShowInIndex;
            record.Summary = model.Summary;
            record.Tags = model.Tags;
            record.Title = model.Title;
            record.AllowComment = model.AllowComment;
            record.Seo = model.Seo;
            var newCategorys = model.Categorys == null ? null :
                _categoryService.GetList(model.Categorys.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            if (record.Categorys.Any())
                record.Categorys.Clear();

            if (newCategorys != null)
                foreach (var category in newCategorys)
                    record.Categorys.Add(category);

            if (isAdd)
                _postService.Add(record);

            return this.Success();
        }
    }
}