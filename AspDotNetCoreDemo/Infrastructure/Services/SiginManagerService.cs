using AspDotNetCoreDemo.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Infrastructure.Services
{
    public class SiginManagerService
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public SiginManagerService(
            SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public async Task Signout()
        {
            await signInManager.SignOutAsync();
        }
    }
}
