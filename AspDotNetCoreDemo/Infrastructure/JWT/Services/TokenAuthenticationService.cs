using AspDotNetCoreDemo.Domain.Interfaces;
using AspDotNetCoreDemo.Domain.Jwt.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Infrastructure.JWT.Services
{
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserManagerService userManagerService;
        private readonly TokenManagement tokenManagement;

        public TokenAuthenticationService(
            IUserManagerService userManagerService,
            IOptions<TokenManagement> tokenManagement)
        {
            this.userManagerService = userManagerService;
            this.tokenManagement = tokenManagement.Value;
        }

        public async Task<AuthWithToken> IsAuthenticatedAsync(TokenRequest request)
        {
            var authWithToken = new AuthWithToken();
           
            authWithToken.IsAuthenticated = await userManagerService.IsValidUser(request.Username, request.Password);

            if (!authWithToken.IsAuthenticated)
            {
                return authWithToken;
            }

            var claim = new[]
            {
                new Claim(ClaimTypes.Name, request.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                tokenManagement.Issuer,
                tokenManagement.Audience,
                claim,
                expires: DateTime.Now.AddMinutes(tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );

            authWithToken.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return authWithToken;
        }
    }
}
