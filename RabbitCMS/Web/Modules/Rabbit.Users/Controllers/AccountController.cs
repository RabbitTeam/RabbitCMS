using Rabbit.Components.Security;
using Rabbit.Infrastructures.Security;
using Rabbit.Users.Services;
using Rabbit.Users.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Rabbit.Users.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAccountService accountService, IUserService userService, IAuthenticationService authenticationService)
        {
            _accountService = accountService;
            _userService = userService;
            _authenticationService = authenticationService;
        }

        // GET: Account
        public ActionResult SignIn(string returnUrl)
        {
            return View(new SignInViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //验证失败。
            if (!await _accountService.Exist(model.Account, model.Password))
            {
                ModelState.AddModelError(string.Empty, "用户名或密码错误。");
                return View(model);
            }

            var user = await _userService.GetUserByAccount(model.Account);

            _authenticationService.SignIn(new UserModel { Identity = user.Id, UserName = user.Name, CreatePersistentCookie = model.Remember }, model.Remember);

            if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                return RedirectToAction("Index", "Admin");
            return Redirect(model.ReturnUrl);
        }

        public ActionResult SignOut(string returnUrl)
        {
            _authenticationService.SignOut();
            return RedirectToAction("SignIn", new { returnUrl });
        }
    }
}