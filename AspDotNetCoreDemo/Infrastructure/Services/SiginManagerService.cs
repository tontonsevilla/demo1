using AspDotNetCoreDemo.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Infrastructure.Services
{
    public class SigninManagerService : ISigninManagerService
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public SigninManagerService(
            SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }


        public async Task<SignInResult> SigninAsync(string userName, string password, bool rememberMe = false)
        {
            return await signInManager.PasswordSignInAsync(userName, password, rememberMe, false);
        }

        public async Task<SignInResult> SigninAsync(IdentityUser user, string password)
        {
            return await signInManager.PasswordSignInAsync(user, password, false, false);
        }

        public async Task SignoutAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
