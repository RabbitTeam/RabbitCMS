using Rabbit.Kernel.Localization;
using Rabbit.Web.UI.Navigation;

namespace Rabbit.Media
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
            const string area = "Rabbit.Media";
            builder.Add(T("媒体"),
                menu =>
                    menu
                        .Position("3.2")
                        .LocalNavigation()
                        .Icon("menu-icon fa fa-image")
                        .Action("Index", "Admin", new { Area = area }).LocalNavigation()
                );
        }

        #endregion Implementation of INavigationProvider
    }
}