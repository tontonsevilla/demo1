using AspDotNetCoreDemo.Infrastructure.Services;
using AspDotNetCoreDemo.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManagerService userManagerService;
        private readonly SigninManagerService signinManagerService;

        public AccountController(
            UserManagerService userManagerService,
            SigninManagerService signinManagerService)
        {
            this.userManagerService = userManagerService;
            this.signinManagerService = signinManagerService;
        }
             
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return await Task.Run(() => {
                return View();
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,Email,Password")] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registerResult = await userManagerService.CreateUserAsync(model.Email, model.Password);

                if (registerResult.RegisterResult.Succeeded &&
                    registerResult.ConfirmEmailResult.Succeeded)
                {
                    var signinResult = await signinManagerService.SigninAsync(model.Email, model.Password);

                    if (signinResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("UserName, Password")] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await signinManagerService.SigninAsync(model.UserName, model.Password, model.RememberMe);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signinManagerService.SignoutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}