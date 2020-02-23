using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Domain.Interfaces
{
    public interface ISigninManagerService
    {
        Task<SignInResult> SigninAsync(string userName, string password, bool rememberMe = false);
        Task<SignInResult> SigninAsync(IdentityUser user, string password);
        Task SignoutAsync();
    }
}
