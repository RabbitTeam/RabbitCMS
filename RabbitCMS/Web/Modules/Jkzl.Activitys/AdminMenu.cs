using Rabbit.Kernel.Localization;
using Rabbit.Web.UI.Navigation;

namespace Jkzl.Activitys
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
        public string MenuName => "admin";

        /// <summary>
        /// 获取导航。
        /// </summary>
        /// <param name="builder">导航建造者。</param>
        public void GetNavigation(NavigationBuilder builder)
        {
            const string area = "Jkzl.Activitys";
            builder.Add(T("营销活动"),
                menu =>
                    menu
                        .Position("5")
                        .LocalNavigation()
                        .Icon("menu-icon fa fa-th-list")
                        .Add(T("活动设置"), i => i.Action("Settings", "DaZhuanPanAdmin", new { Area = area }).LocalNavigation())
                        .Add(T("奖品设置"), i => i.Action("PrizeSettings", "DaZhuanPanAdmin", new { Area = area }).LocalNavigation())
                        .Add(T("抽奖列表"), i => i.Action("Index", "DaZhuanPanAdmin", new { Area = area }).LocalNavigation())
                );
        }

        #endregion Implementation of INavigationProvider
    }
}