using Rabbit.Blogs.Models;
using Rabbit.Blogs.Services;
using Rabbit.Blogs.Services.Themes;
using Rabbit.Infrastructures.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rabbit.Blogs.Controllers.Api
{
    public class CommentController : ApiController
    {
        private readonly IThemeCommentService _commentService;
        private readonly IPostService _postService;

        public CommentController(IThemeCommentService commentService, IPostService postService)
        {
            _commentService = commentService;
            _postService = postService;
        }

        public sealed class CommentViewModel
        {
            /// <summary>
            /// 昵称。
            /// </summary>
            [Required, StringLength(20)]
            public string NickName { get; set; }

            /// <summary>
            /// 内容。
            /// </summary>
            [Required, StringLength(8000)]
            public string Content { get; set; }
        }

        public async Task<HttpResponseMessage> Get(string postId, int pageIndex, int pageSize)
        {
            if (!await _postService.Exist(postId))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "文章不存在！");

            var comments = _commentService.GetListByPost(postId);

            var pageParameter = new PageParameter(pageIndex - 1, pageSize);
            comments = pageParameter.Paged(comments);

            return Request.CreateResponse(HttpStatusCode.OK,
                new
                {
                    pageParameter.PageCount,
                    PageIndex = pageIndex,
                    pageParameter.PageSize,
                    Data = comments.ToArray().Select(i => new { i.Id, i.Content, i.NickName, i.CreateTime })
                });
        }

        public async Task<HttpResponseMessage> Post([FromUri]string postId, CommentViewModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "数据非法。");

            var post = await _postService.Get(postId);
            if (post == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "找不到对应的文章。");

            var record = PostCommentRecord.Create(model.NickName, model.Content, post);
            _commentService.Add(record);

            return Request.CreateResponse(HttpStatusCode.OK, record.Id);
        }

        public async Task<HttpResponseMessage> Delete(string id)
        {
            if (!await _commentService.Exist(id))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "找不到评论信息！");

            _commentService.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}