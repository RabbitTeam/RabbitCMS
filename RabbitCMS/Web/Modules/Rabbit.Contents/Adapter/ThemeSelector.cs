using Rabbit.Contents.Services;
using Rabbit.Web.Themes;
using System.Web.Routing;

namespace Rabbit.Contents.Adapter
{
    internal sealed class ThemeSelector : IThemeSelector
    {
        private readonly ISiteSettingsService _siteSettingsService;

        public ThemeSelector(ISiteSettingsService siteSettingsService)
        {
            _siteSettingsService = siteSettingsService;
        }

        #region Implementation of IThemeSelector

        /// <summary>
        /// 根据当前请求获取主题。
        /// </summary>
        /// <param name="context">请求上下文。</param>
        /// <returns>
        /// 主题选择结果。
        /// </returns>
        public ThemeSelectorResult GetTheme(RequestContext context)
        {
            return new ThemeSelectorResult
            {
                ThemeName = _siteSettingsService.Get().ThemeName
            };
        }

        #endregion Implementation of IThemeSelector
    }
}