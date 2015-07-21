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

        #endregion Field

        #region Constructor

        public ComponentShapes(IShapeFactory shapeFactory, IThemeCategoryService categoryService)
        {
            _shapeFactory = shapeFactory;
            _categoryService = categoryService;

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
            builder.Describe("BlogMenu")
                .OnCreated(created =>
                {
                    var shape = created.Shape;
                    shape.AddRange(_categoryService.GetList().ToArray());
                });
        }

        #endregion Implementation of IShapeTableProvider

        #region Private Method

        private static string EncodeAlternateElement(string alternateElement)
        {
            return alternateElement.Replace("-", "__").Replace(".", "_");
        }

        #endregion Private Method
    }
}