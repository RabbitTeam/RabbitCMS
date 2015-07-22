using Rabbit.Blogs.Services;
using Rabbit.Blogs.ViewModels;
using Rabbit.Infrastructures.Data;
using Rabbit.Web.Mvc.UI.Admin;
using System.Linq;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers
{
    [Admin]
    public class CommentAdminController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentAdminController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public ActionResult Index(string postId)
        {
            ViewBag.PostId = postId;
            return View();
        }

        [HttpPost]
        public ActionResult DataSource(string postId, int pageIndex)
        {
            var list = _commentService.GetListByPost(postId);
            var pageParameter = new PageParameter(pageIndex, 10);
            list = pageParameter.Paged(list);
            var model = list.ToArray().Select(i => (CommentListViewModel)i).ToArray();
            return Json(new { pageParameter.PageCount, list = model });
        }
    }
}