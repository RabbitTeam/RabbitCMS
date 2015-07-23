using Rabbit.Components.Security;
using Rabbit.Components.Security.Web;
using Rabbit.Infrastructures.Security;
using Rabbit.Kernel.Environment.Configuration;
using Rabbit.Kernel.Extensions;
using Rabbit.Kernel.Services;
using Rabbit.Users.Services;
using Rabbit.Web;

namespace Rabbit.Users.Adapter
{
    [SuppressDependency("Rabbit.Components.Security.NullAuthenticationService")]
    internal sealed class FormsAuthenticationService : FormsAuthenticationServiceBase
    {
        private readonly IUserService _userService;

        #region Constructor

        /// <summary>
        /// 初始化一个表单授权服务。
        /// </summary>
        /// <param name="settings">租户设置。</param><param name="clock">时钟服务。</param><param name="httpContextAccessor">HttpContext访问器。</param>
        /// <param name="userService">用户服务。</param>
        public FormsAuthenticationService(ShellSettings settings, IClock clock, IHttpContextAccessor httpContextAccessor, IUserService userService)
            : base(settings, clock, httpContextAccessor)
        {
            _userService = userService;
        }

        #endregion Constructor

        #region Overrides of FormsAuthenticationServiceBase

        /// <summary>
        /// 根据用户标识获取用户信息。
        /// </summary>
        /// <param name="identity">用户标识。</param>
        /// <returns>
        /// 用户模型。
        /// </returns>
        protected override IUser GetUserByIdentity(string identity)
        {
            var user = _userService.GetUserById(identity).Result;
            return user == null ? null : new UserModel
            {
                Identity = user.Id,
                UserName = user.Name
            };
        }

        #endregion Overrides of FormsAuthenticationServiceBase
    }
}