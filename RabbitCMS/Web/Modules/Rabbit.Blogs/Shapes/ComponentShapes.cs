using Rabbit.Blogs.Services.Themes;
using Rabbit.Kernel.Localization;
using Rabbit.Web.Mvc.DisplayManagement;
using Rabbit.Web.Mvc.DisplayManagement.Descriptors;
using System.Linq;

namespace Rabbit.Blogs.Shapes
{
    internal sealed class ComponentShapes : IShapeTableProvider
    {
        #region Field

        private readonly IShapeFactory _shapeFactory;
        private readonly IThemeCategoryService _categoryService;
        private readonly IThemePostService _postService;
        private readonly IThemeCommentService _commentService;
        private readonly IThemeTagService _tagService;

        #endregion Field

        #region Constructor

        public ComponentShapes(IShapeFactory shapeFactory, IThemeCategoryService categoryService, IThemePostService postService, IThemeCommentService commentService, IThemeTagService tagService)
        {
            _shapeFactory = shapeFactory;
            _categoryService = categoryService;
            _postService = postService;
            _commentService = commentService;
            _tagService = tagService;

            T = NullLocalizer.Instance;
        }

        #endregion Constructor

        #region Property

        public Localizer T { get; set; }

        public dynamic New => _shapeFactory;

        #endregion Property

        #region Implementation of IShapeTableProvider

        /// <summary>
        /// 发现形状表格。
        /// </summary>
        /// <param name="builder">形状表格建造者。</param>
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Blog_Categorys")
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    shape.AddRange(_categoryService.GetList().ToArray());
                });

            //最多阅读
            builder.Describe("Blog_MostReadPosts")
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    int? take = shape.Take ?? 10;
                    shape.AddRange(_postService.GetMostReadList(take));
                });

            //最新回复
            builder.Describe("Blog_NewestComments")
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    int? take = shape.Take ?? 10;
                    shape.AddRange(_commentService.GetNewestList(take));
                });

            //Tags
            builder.Describe("Blog_Tags")
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    int? take = shape.Take ?? 20;
                    shape.AddRange(_tagService.GetTags(take));
                });
        }

        #endregion Implementation of IShapeTableProvider
    }
}