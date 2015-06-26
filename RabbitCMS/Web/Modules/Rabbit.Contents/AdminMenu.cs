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
            builder.Add(T("会员卡"),
                menu =>
                    menu
                        .Position("5")
                        .LocalNavigation()
                        .Icon("menu-icon fa fa-tachometer")
                        .Add(T("会员注册设置"), i => i.Action("RegisterSettings", "MemberAdmin", new { Area = area }).LocalNavigation())
                        .Add(T("会员CRM"), i => i.Action("Index", "MemberCrm", new { Area = area }).LocalNavigation()
                            .Add(T("编辑会员资料"), z => z.Action("Edit", "MemberCrm", new { Area = area })))
                        .Add(T("会员卡设置"), i => i.Action("Index", "Home", new { Area = area }).LocalNavigation()
                            .Add(T("添加特权"), z => z.Action("AddPrivilege", "MemberCardSetting", new { Area = area }))
                            .Add(T("编辑特权"), z => z.Action("EditPrivilege", "MemberCardSetting", new { Area = area })))
                        .Add(T("会员卡管理"), i => i.Action("Index", "MemberCardAdmin", new { Area = area }).LocalNavigation()
                            .Add(T("编辑会员卡资料"), z => z.Action("Edit", "MemberCardAdmin", new { Area = area })))
                        .Add(T("客户关怀"), i => i.LocalNavigation()
                            .Add(T("节日关怀"), z => z.Action("Index", "Caring", new { Area = area }).LocalNavigation()
                                .Add(T("添加节日关怀"), x => x.Action("Add", "Caring"))
                                .Add(T("编辑节日关怀"), x => x.Action("Edit", "Caring")))
                            .Add(T("到期提醒"), z => z.Action("Index", "RemindSetting", new { Area = area }).LocalNavigation()
                                .Add(T("添加新提醒"), x => x.Action("Add", "RemindSetting"))
                                .Add(T("编辑提醒"), x => x.Action("Edit", "RemindSetting")))
                            .Add(T("消息通知"), z => z.Action("Index", "Notice", new { Area = area }).LocalNavigation()
                                .Add(T("添加消息通知"), x => x.Action("Add", "Notice"))
                                .Add(T("编辑消息通知"), x => x.Action("Edit", "Notice")))
                        )
                        .Add(T("营销管理"), i => i.LocalNavigation()
                            .Add(T("优惠券"), z => z.Action("Index", "CouponAdmin", new { Area = area }).LocalNavigation()
                                .Add(T("添加优惠券"), x => x.Action("Add", "CouponAdmin"))
                                .Add(T("编辑优惠券"), x => x.Action("Edit", "CouponAdmin"))
                                .Add(T("SN码管理"), x => x.Action("Sn", "CouponAdmin")))
                            .Add(T("开卡送"), z => z.Action("Index2", "Home", new { Area = area }).LocalNavigation()
                                .Add(T("添加开卡送活动"), x => x.Action("Add", "OpenCardGift"))
                                .Add(T("编辑开卡送活动"), x => x.Action("Edit", "OpenCardGift")))
                            .Add(T("积分攻略"), z => z.Action("Index", "PointRaider", new { Area = area }).LocalNavigation())
                            .Add(T("积分兑换记录"),
                                z => z.Action("Index", "PointExchange", new { Area = area }).LocalNavigation())
                            .Add(T("预存赠送"), z => z.Action("Index", "DepositSend", new { Area = area }).LocalNavigation()
                                .Add(T("添加预存赠送活动"), x => x.Action("Add", "DepositSend"))
                                .Add(T("编辑预存赠送活动"), x => x.Action("Edit", "DepositSend")))
                            .Add(T("消费赠送"), z => z.Action("Index", "ConsumerSend", new { Area = area }).LocalNavigation()
                                .Add(T("添加消费赠送活动"), x => x.Action("Add", "ConsumerSend"))
                                .Add(T("编辑消费赠送活动"), x => x.Action("Edit", "ConsumerSend")))
                            .Add(T("分享赠送"), z => z.Action("Index", "ShareLinkAdmin", new { Area = area }).LocalNavigation()
                                .Add(T("添加分享赠送活动"), x => x.Action("Add", "ShareLinkAdmin"))
                                .Add(T("编辑分享赠送活动"), x => x.Action("Edit", "ShareLinkAdmin")))
                        )
                );
        }

        #endregion Implementation of INavigationProvider
    }
}