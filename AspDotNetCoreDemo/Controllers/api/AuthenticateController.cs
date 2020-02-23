using System.Threading.Tasks;
using AspDotNetCoreDemo.Domain.Interfaces;
using AspDotNetCoreDemo.Domain.Jwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNetCoreDemo.Controllers.api
{
    [Route("api/v1/[controller]")]
    [ApiController]
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
        public async Task<ActionResult> RequestToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authWithToken = await authneticateService.IsAuthenticatedAsync(request);

            if (authWithToken.IsAuthenticated)
            {
                var credential = new CredentialResponse
                {
                    Token = authWithToken.Token
                };

                return Ok(credential);
            }

            return BadRequest("Invalid Request");
        }
    }
}