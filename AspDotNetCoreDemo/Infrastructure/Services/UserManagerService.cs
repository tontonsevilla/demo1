using AspDotNetCoreDemo.Domain;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Infrastructure.Services
{
    public class UserManagerService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SigninManagerService siginManagerService;

        public UserManagerService(
            UserManager<IdentityUser> userManager,
            SigninManagerService siginManagerService)
        {
            this.userManager = userManager;
            this.siginManagerService = siginManagerService;
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
    }
}
