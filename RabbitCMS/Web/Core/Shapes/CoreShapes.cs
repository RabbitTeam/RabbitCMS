using Rabbit.Kernel.Localization;
using Rabbit.Kernel.Utility.Extensions;
using Rabbit.Kernel.Works;
using Rabbit.Web.Mvc.DisplayManagement;
using Rabbit.Web.Mvc.DisplayManagement.Descriptors;
using Rabbit.Web.Mvc.UI;
using Rabbit.Web.Mvc.UI.Navigation;
using Rabbit.Web.Mvc.UI.Zones;
using Rabbit.Web.UI.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Rabbit.Core.Shapes
{
    internal sealed class CoreShapes : IShapeTableProvider
    {
        #region Field

        private readonly Work<IShapeFactory> _shapeFactory;
        private readonly Work<INavigationManager> _navigationManager;

        #endregion Field

        #region Constructor

        public CoreShapes(Work<IShapeFactory> shapeFactory, Work<INavigationManager> navigationManager)
        {
            _shapeFactory = shapeFactory;
            _navigationManager = navigationManager;

            T = NullLocalizer.Instance;
        }

        #endregion Constructor

        #region Property

        public Localizer T { get; set; }

        public dynamic New { get { return _shapeFactory.Value; } }

        #endregion Property

        #region Implementation of IShapeTableProvider

        /// <summary>
        /// 发现形状。
        /// </summary>
        /// <param name="builder">形状表格建造者。</param>
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Layout")
                .Configure(descriptor => descriptor.Wrappers.Add("Document"))
                .OnCreating(creating => creating.Create = () => new ZoneHolding(() => creating.New.Zone()))
                .OnCreated(created =>
                {
                    var layout = created.Shape;

                    layout.Head = created.New.DocumentZone(ZoneName: "Head");
                    layout.Body = created.New.DocumentZone(ZoneName: "Body");
                    layout.Tail = created.New.DocumentZone(ZoneName: "Tail");

                    layout.Body.Add(created.New.PlaceChildContent(Source: layout));

                    layout.Content = created.New.Zone();
                    layout.Content.ZoneName = "Content";
                    layout.Content.Add(created.New.PlaceChildContent(Source: layout));

                    layout.Breadcrumb = created.New.Zone();
                    layout.Breadcrumb.ZoneName = "Breadcrumb";
                    layout.Breadcrumb.Add(created.New.Breadcrumb(GetMenus: new Func<HttpRequestBase, RouteValueDictionary, IEnumerable<MenuItem>>((request, routeValues) => NavigationHelper.SetSelectedPath(_navigationManager.Value.BuildMenu("admin"), request, routeValues))));

                    layout.User = created.New.Zone();
                    layout.User.ZoneName = "User";
                    layout.User.Add(created.New.User());
                });

            builder.Describe("Zone")
                .OnCreating(creating => creating.Create = () => new Zone())
                .OnDisplaying(displaying =>
                {
                    var zone = displaying.Shape;
                    string zoneName = zone.ZoneName;
                    zone.Classes.Add("zone-" + zoneName.HtmlClassify());
                    zone.Classes.Add("zone");

                    zone.Metadata.Alternates.Add("Zone__" + zoneName);
                });

            builder.Describe("Menu")
                .OnDisplaying(displaying =>
                {
                    var menu = displaying.Shape;
                    string menuName = menu.MenuName;
                    menu.Metadata.Alternates.Add("Menu__" + EncodeAlternateElement(menuName));
                });

            builder.Describe("MenuItem")
                .OnDisplaying(displaying =>
                {
                    var menuItem = displaying.Shape;
                    var menu = menuItem.Menu;
                    menuItem.Metadata.Alternates.Add("MenuItem__" + EncodeAlternateElement(menu.MenuName));
                });

            builder.Describe("Breadcrumb")
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    var metadata = shape.Metadata;
                    metadata.Alternates.Add("Breadcrumb");
                });

            builder.Describe("User")
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    var metadata = shape.Metadata;
                    metadata.Alternates.Add("User");
                });
        }

        #endregion Implementation of IShapeTableProvider

        #region Shapes

        [Shape]
        public void Zone(dynamic Display, dynamic Shape, TextWriter Output)
        {
            string id = Shape.Id;
            IEnumerable<string> classes = Shape.Classes;
            IDictionary<string, string> attributes = Shape.Attributes;
            var zoneWrapper = GetTagBuilder("div", id, classes, attributes);
            Output.Write(zoneWrapper.ToString(TagRenderMode.StartTag));
            foreach (var item in ordered_hack(Shape))
                Output.Write(Display(item));
            Output.Write(zoneWrapper.ToString(TagRenderMode.EndTag));
        }

        [Shape]
        public void ContentZone(dynamic Display, dynamic Shape, TextWriter Output)
        {
            foreach (var item in ordered_hack(Shape))
                Output.Write(Display(item));
        }

        [Shape]
        public void DocumentZone(dynamic Display, dynamic Shape, TextWriter Output)
        {
            foreach (var item in ordered_hack(Shape))
                Output.Write(Display(item));
        }

        [Shape]
        public IHtmlString PlaceChildContent(dynamic Source)
        {
            return Source.Metadata.ChildContent;
        }

        [Shape]
        public void EditorTemplate(HtmlHelper Html, TextWriter Output, string TemplateName, object Model, string Prefix)
        {
            RenderInternal(Html, Output, "EditorTemplates/" + TemplateName, Model, Prefix);
        }

        #endregion Shapes

        #region Private Method

        private static void RenderInternal(HtmlHelper Html, TextWriter Output, string TemplateName, object Model, string Prefix)
        {
            var adjustedViewData = new ViewDataDictionary(Html.ViewDataContainer.ViewData)
            {
                Model = DetermineModel(Html, Model),
                TemplateInfo = new TemplateInfo
                {
                    HtmlFieldPrefix = DeterminePrefix(Html, Prefix)
                }
            };
            var adjustedViewContext = new ViewContext(Html.ViewContext, Html.ViewContext.View, adjustedViewData, Html.ViewContext.TempData, Output);
            var adjustedHtml = new HtmlHelper(adjustedViewContext, new ViewDataContainer(adjustedViewData));
            adjustedHtml.RenderPartial(TemplateName);
        }

        private static object DetermineModel(HtmlHelper Html, object Model)
        {
            bool isNull = ((dynamic)Model) == null;
            return isNull ? Html.ViewData.Model : Model;
        }

        private static string DeterminePrefix(HtmlHelper Html, string Prefix)
        {
            var actualPrefix = string.IsNullOrEmpty(Prefix)
                                   ? Html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix
                                   : Html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(Prefix);
            return actualPrefix;
        }

        private class ViewDataContainer : IViewDataContainer
        {
            public ViewDataContainer(ViewDataDictionary viewData)
            {
                ViewData = viewData;
            }

            public ViewDataDictionary ViewData { get; set; }
        }

        #region Ordered Hack

        private static IEnumerable<dynamic> ordered_hack(dynamic shape)
        {
            IEnumerable<dynamic> unordered = shape;
            if (unordered == null || unordered.Count() < 2)
                return shape;

            var i = 1;
            var progress = 1;
            var flatPositionComparer = new FlatPositionComparer();
            var ordering = unordered.Select(item =>
            {
                var position = (item == null || item.GetType().GetProperty("Metadata") == null || item.Metadata.GetType().GetProperty("Position") == null)
                                   ? null
                                   : item.Metadata.Position;
                return new { item, position };
            }).ToList();

            while (i < ordering.Count())
            {
                if (flatPositionComparer.Compare(ordering[i].position, ordering[i - 1].position) > -1)
                {
                    if (i == progress)
                        progress = ++i;
                    else
                        i = progress;
                }
                else
                {
                    var higherThanItShouldBe = ordering[i];
                    ordering[i] = ordering[i - 1];
                    ordering[i - 1] = higherThanItShouldBe;
                    if (i > 1)
                        --i;
                }
            }

            return ordering.Select(ordered => ordered.item).ToList();
        }

        #endregion Ordered Hack

        private static TagBuilder GetTagBuilder(string tagName, string id, IEnumerable<string> classes, IDictionary<string, string> attributes)
        {
            var tagBuilder = new TagBuilder(tagName);
            tagBuilder.MergeAttributes(attributes, false);
            foreach (var cssClass in classes ?? Enumerable.Empty<string>())
                tagBuilder.AddCssClass(cssClass);
            if (!string.IsNullOrWhiteSpace(id))
                tagBuilder.GenerateId(id);
            return tagBuilder;
        }

        private static string EncodeAlternateElement(string alternateElement)
        {
            return alternateElement.Replace("-", "__").Replace(".", "_");
        }

        #endregion Private Method
    }

    /// <summary>
    /// 字符串扩展方法。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Html样式类名称。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <returns>处理后的字符串。</returns>
        public static string HtmlClassify(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var friendlier = text.CamelFriendly();

            var result = new char[friendlier.Length];

            var cursor = 0;
            var previousIsNotLetter = false;
            for (var i = 0; i < friendlier.Length; i++)
            {
                var current = friendlier[i];
                if (current.IsLetter() || (char.IsDigit(current) && cursor > 0))
                {
                    if (previousIsNotLetter && i != 0 && cursor > 0)
                    {
                        result[cursor++] = '-';
                    }

                    result[cursor++] = char.ToLowerInvariant(current);
                    previousIsNotLetter = false;
                }
                else
                {
                    previousIsNotLetter = true;
                }
            }

            return new string(result, 0, cursor);
        }

        public static string Substring(this string source, int take, string separator = "...", bool isRemoveHtml = false)
        {
            if (source == null)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            var content = isRemoveHtml ? source.RemoveHtml() : source;
            if (content.Length > take)
                return content.Substring(0, take) + separator;
            return content;
        }

        public static string RemoveHtml(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            //删除脚本
            source = Regex.Replace(source, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            source = Regex.Replace(source, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            //source = Regex.Replace(source, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"-->", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"<!--.*", "", RegexOptions.IgnoreCase);

            source = Regex.Replace(source, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&ldquo;", "“", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&rdquo;", "”", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            source = source.Replace("<", "&lt;");
            source = source.Replace(">", "&gt;");
            return source;
        }
    }
}