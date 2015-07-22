using Rabbit.Blogs.Services.Themes;
using Rabbit.Kernel.Localization;
using Rabbit.Kernel.Works;
using Rabbit.Web.Mvc.DisplayManagement;
using Rabbit.Web.Mvc.DisplayManagement.Descriptors;
using System.Linq;

namespace Rabbit.Blogs.Shapes
{
    internal sealed class ComponentShapes : IShapeTableProvider
    {
        #region Field

        private readonly IShapeFactory _shapeFactory;
        private readonly IWorkContextAccessor _workContextAccessor;

        #endregion Field

        #region Constructor

        public ComponentShapes(IShapeFactory shapeFactory, IWorkContextAccessor workContextAccessor)
        {
            _shapeFactory = shapeFactory;
            _workContextAccessor = workContextAccessor;

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
                    var categoryService = _workContextAccessor.GetContext().Resolve<IThemeCategoryService>();
                    shape.AddRange(categoryService.GetList().ToArray());
                });

            //最多阅读
            builder.Describe("Blog_MostReadPosts")
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    int? take = shape.Take ?? 10;
                    var postService = _workContextAccessor.GetContext().Resolve<IThemePostService>();
                    shape.AddRange(postService.GetMostReadList(take));
                });

            //最新回复
            builder.Describe("Blog_NewestComments")
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    int? take = shape.Take ?? 10;
                    var commentService = _workContextAccessor.GetContext().Resolve<IThemeCommentService>();
                    shape.AddRange(commentService.GetNewestList(take));
                });

            //Tags
            builder.Describe("Blog_Tags")
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    int? take = shape.Take ?? 20;
                    var tagService = _workContextAccessor.GetContext().Resolve<IThemeTagService>();
                    shape.AddRange(tagService.GetTags(take));
                });
        }

        #endregion Implementation of IShapeTableProvider
    }
}