using System.Threading.Tasks;
using AspDotNetCoreDemo.Domain.Interfaces;
using AspDotNetCoreDemo.Domain.Models;
using AspDotNetCoreDemo.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNetCoreDemo.Controllers.api
{
    [Route("api/v1/[controller]", Name = "defaultApi")]
    public class AccountController : ControllerBase
    {
        private readonly IUserManagerService userManagerService;

        public AccountController(
                IUserManagerService userManagerService)
        {
            this.userManagerService = userManagerService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            var response = new ApiResponse<Empty>();

            if (!ModelState.IsValid)
            {
                response.AddError(ModelState);
                return BadRequest(response);
            }

            var createResult = await userManagerService.CreateUserAsync(model.Email, model.Password);

            if (createResult.RegisterResult.Succeeded)
            {
                return Ok(response);
            }

            response.AddError(createResult.RegisterResult.Errors);

            return BadRequest(response);
        }

    }
}