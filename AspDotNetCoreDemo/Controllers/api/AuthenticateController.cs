using System.Threading.Tasks;
using AspDotNetCoreDemo.Domain.Interfaces;
using AspDotNetCoreDemo.Domain.Jwt.Models;
using AspDotNetCoreDemo.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNetCoreDemo.Controllers.api
{
    [Route("api/v1/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService authneticateService;

        public AuthenticateController(
            IAuthenticateService authneticateService)
        {
            this.authneticateService = authneticateService;
        }

        [AllowAnonymous]
        [HttpPost("request")]
        public async Task<IActionResult> RequestToken([FromBody] TokenRequest request)
        {
            var result = new ApiResponse<CredentialResponse>();

            if (!ModelState.IsValid)
            {
                result.AddError(ModelState);
                return Ok(result);
            }

            var authWithToken = await authneticateService.IsAuthenticatedAsync(request);

            if (authWithToken.IsAuthenticated)
            {
                var credentialReponse = new CredentialResponse
                {
                    Token = authWithToken.Token
                };

                result.AddData(credentialReponse);

                return Ok(result);
            }

            result.AddError("Invalid Account");

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> Signin([FromBody] TokenRequest request)
        {
            var result = new ApiResponse<CredentialResponse>();

            if (!ModelState.IsValid)
            {
                result.AddError(ModelState);
                return Ok(result);
            }

            var authWithToken = await authneticateService.IsAuthenticatedAsync(request);

            if (authWithToken.IsAuthenticated)
            {
                var credentialReponse = new CredentialResponse
                {
                    Token = authWithToken.Token
                };

                result.AddData(credentialReponse);

                return Ok(result);
            }

            result.AddError("Invalid Account");

            return Ok(result);
        }
    }
}