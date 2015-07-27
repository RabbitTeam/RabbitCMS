using Rabbit.Kernel.Localization;
using Rabbit.Web.UI.Navigation;

namespace Rabbit.Blogs
{
    internal sealed class AdminMenu : INavigationProvider
    {
        public AdminMenu()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        #region Implementation of INavigationProvider

        /// <summary>
        /// 导航菜单名称。
        /// </summary>
        public string MenuName { get { return "admin"; } }

        /// <summary>
        /// 获取导航。
        /// </summary>
        /// <param name="builder">导航建造者。</param>
        public void GetNavigation(NavigationBuilder builder)
        {
            const string area = "Rabbit.Blogs";
            builder.Add(T("博客"),
                menu =>
                    menu
                        .Position("3.1")
                        .LocalNavigation()
                        .Icon("menu-icon fa fa-th-list")
                        .Add(T("分类管理"), i => i.Action("Index", "CategoryAdmin", new { Area = area }).LocalNavigation()
                            .Add(T("添加分类"), z => z.Action("Add", "CategoryAdmin", new { Area = area }))
                            .Add(T("编辑分类"), z => z.Action("Edit", "CategoryAdmin", new { Area = area })))
                        .Add(T("文章管理"), i => i.Action("Index", "PostAdmin", new { Area = area }).LocalNavigation()
                            .Add(T("添加文章"), z => z.Action("Add", "PostAdmin", new { Area = area }))
                            .Add(T("编辑文章"), z => z.Action("Edit", "PostAdmin", new { Area = area })))
                        .Add(T("评论管理"), i => i.Action("Index", "CommentAdmin", new { Area = area }).LocalNavigation())
                );
        }

        #endregion Implementation of INavigationProvider
    }
}