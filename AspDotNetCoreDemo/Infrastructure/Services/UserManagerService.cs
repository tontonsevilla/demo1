using AspDotNetCoreDemo.Domain.Interfaces;
using AspDotNetCoreDemo.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Infrastructure.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ISigninManagerService signinManagerService;

        public UserManagerService(
            UserManager<IdentityUser> userManager,
            ISigninManagerService siginManagerService)
        {
            this.userManager = userManager;
            this.signinManagerService = siginManagerService;
        }

        public async Task<CreateUserResult> CreateUserAsync(string userName, string password)
        {
            var user = new IdentityUser
            {
                Email = userName,
                UserName = userName
            };

            var createUserResult = new CreateUserResult();

            createUserResult.RegisterResult = await userManager.CreateAsync(user, password);

            if (createUserResult.RegisterResult.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                createUserResult.ConfirmEmailResult = await userManager.ConfirmEmailAsync(user, token);
            }

            return createUserResult;
        }

        public async Task<bool> IsValidUser(string userName, string password)
        {
            var user = await userManager.FindByEmailAsync(userName);
            return await userManager.CheckPasswordAsync(user, password);
        }
    }
}
