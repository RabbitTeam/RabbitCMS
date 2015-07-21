using Rabbit.Kernel.Localization;
using Rabbit.Web.UI.Navigation;

namespace Rabbit.Contents
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
            const string area = "Rabbit.Contents";
            builder.Add(T("系统"),
                menu =>
                    menu
                        .Position("5")
                        .LocalNavigation()
                        .Icon("menu-icon fa fa-tachometer")
                        .Add(T("站点设置"), i => i.Action("Index", "SiteAdmin", new { Area = area }).LocalNavigation())
                );
        }

        #endregion Implementation of INavigationProvider
    }
}