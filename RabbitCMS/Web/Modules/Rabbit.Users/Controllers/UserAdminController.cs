using Rabbit.Components.Security;
using Rabbit.Infrastructures.Mvc;
using Rabbit.Infrastructures.Security;
using Rabbit.Users.Services;
using Rabbit.Users.ViewModels;
using Rabbit.Web.Mvc.UI.Admin;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Rabbit.Users.Controllers
{
    [Admin]
    public class UserAdminController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public UserAdminController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = _userService.GetUserById(id ?? _authenticationService.GetAuthenticatedUser().Identity);
            if (user == null)
                return HttpNotFound();

            var model = (UserEditViewModel)await user;
            return View(model);
        }

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<ActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return this.Error("数据非法！");

            var user = await _userService.GetUserById(model.Id);
            if (user == null)
                return this.Error("数据非法！");

            model.UpdateRecord(user);

            var userModel = _authenticationService.GetAuthenticatedUser() as UserModel;
            if (userModel != null && userModel.Identity == user.Id)
            {
                userModel.UserName = user.Name;
                _authenticationService.SignIn(userModel, userModel.CreatePersistentCookie);
            }

            return this.Success();
        }
    }
}