using AspDotNetCoreDemo.Infrastructure.Services;
using AspDotNetCoreDemo.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signinManager;
        private readonly SiginManagerService signinManagerService;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signinManager,
            SiginManagerService signinManagerService)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
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
                var user = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.Email
                };

                var registerResult = await userManager.CreateAsync(user, model.Password);

                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmEmailResult = await userManager.ConfirmEmailAsync(user, token);

                if (registerResult.Succeeded && confirmEmailResult.Succeeded)
                {
                    var signinResult = await signinManager.PasswordSignInAsync(user, model.Password, false, false);
                    
                    if (signinResult.Succeeded)
                    {
                        return Redirect(Url.Action("Index", "Home"));
                    }
                }
                
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signinManagerService.Signout();

            return Redirect(Url.Action("Login", "Account"));
        }
    }
}